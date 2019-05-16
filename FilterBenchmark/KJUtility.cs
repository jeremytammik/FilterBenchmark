// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Class1.cs
 * https://www.buildtech4u.com/
 * © Arciviz, 2017
 *
 * This updater is used to create an updater capable of reacting
 * to changes in the Revit model.
 */
#region Namespaces
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using WPF = System.Windows;
using System.Linq;
using SWF = System.Windows.Forms;
using Arciviz.KJUtility;
#endregion
namespace Arciviz.KJUtility
{

    public class KJUtility
    {
        public enum NumberType
        {
            Positive_Zero_and_Negative,
            Positive_Only,
            Positive_and_Zero,
            Zero_Only,
            Negative_Only,
            Negative_And_Zero,
        }
        public enum IsZoroAllowed
        {
            Allow, Deny
        }

        public class Converter
        {
            public static double R2D(double Radian)
            { return Radian * 180 / Math.PI; }
            public static double D2R(double Degree)
            { return Degree * Math.PI / 180; }
            public static string Point2String(XYZ pt)
            {
                if (pt == null)
                    return "";
                else
                    return string.Format("({0},{1},{2})",
                              pt.X.ToString("F4"), pt.Y.ToString("F4"), pt.Z.ToString("F4"));
            }
            public static string Feet2FtIn(double feet)
            {
                double inch = feet * 12;
                return ((Convert.ToInt32(inch)) / 12).ToString() + "' " + Math.Round((inch % 12), 8).ToString() + "\"";
            }
            public static string Feet2Inch(double feet)
            {
                double inch = feet * 12;
                return (inch.ToString() + "\"");
            }
            public static string Feet2Inch(double feet, int Precision)
            {
                double inch = Math.Round(feet * 12, Precision);
                return (inch.ToString() + "\"");
            }
            public static double Feet2Inch(int pre, double feet)
            {
                return (feet * 12);
            }
            public static double Feet2MM(double feet)
            {
                double inch = feet * 12;
                return (inch * 25.4);
            }
            public static double String_FeetandInch2Double(Document Document, string FeetandInchs)
            {
                double dParsedLength = 0;
                Units units = Document.GetUnits();
                UnitFormatUtils.TryParse(units, UnitType.UT_Length, FeetandInchs, out dParsedLength);
                return dParsedLength;
            }

        }

        public class Draw
        {
            public static void DrawCircle(UIDocument ui_doc, XYZ CenterPoint, double radius, XYZ Normal)
            {
                Plane plane = Plane.CreateByNormalAndOrigin(Normal, CenterPoint);
                SketchPlane skplane = SketchPlane.Create(ui_doc.Document, plane);
                Arc arc = Arc.Create(plane, radius, 0.0F, 2 * 22 / 7.0);
                ModelCurve mc = ui_doc.Document.Create.NewModelCurve(arc, skplane);
            }
            public static ViewSection CreateSectionAtBottomAndRight(Document doc, Element StructuralFrame, double bottomorleftoffset, double ToporRightoffset)
            {

                IList<Parameter> param1 = new List<Parameter>();
                IList<XYZ> points = (StructuralFrame.Location as LocationCurve).Curve.Tessellate();
                XYZ p1 = points[0], p2 = points[1];
                // XYZ v = p1 - p2;
                double vec_ang1 = 360 - Math.Round(Converter.R2D((p2 - p1).AngleOnPlaneTo(XYZ.BasisX, XYZ.BasisZ)));
                vec_ang1 = (vec_ang1 == 360) ? 0.0 : vec_ang1;
                XYZ pt1 = StructuralFrame.get_BoundingBox(doc.ActiveView).Min;
                XYZ pt2 = StructuralFrame.get_BoundingBox(doc.ActiveView).Max;
                double len = (p2 - p1).GetLength() * 0.5;
                ElementId BeamSymid = StructuralFrame.GetTypeId();
                Element BeamSym = doc.GetElement(BeamSymid);
                double wid = BeamSym.GetParameters("Beam Width")[0].AsDouble();
                double ht = pt2.Z - pt1.Z;

                ViewFamilyType view = Filter.ElementFiltering(doc, typeof(ViewFamilyType), "Section",
                     "Building Section", null) as ViewFamilyType;


                XYZ min = new XYZ(-len - bottomorleftoffset, pt1.Z - 1, -wid);
                XYZ max = new XYZ(len + ToporRightoffset, pt2.Z + 1, wid);
                XYZ midpt = p1 + (p2 - p1) / 2;
                XYZ BeamDir = null;

                if (((vec_ang1 >= 0) && (vec_ang1 <= 135)) || ((vec_ang1 >= 315) && (vec_ang1 <= 360)))
                {
                    BeamDir = (p1 - p2).Normalize();
                }
                else
                {
                    BeamDir = (p2 - p1).Normalize();
                }
                XYZ up = XYZ.BasisZ;
                XYZ viewdir = BeamDir.CrossProduct(up);
                Transform t = Transform.Identity;
                t.Origin = midpt;
                t.BasisX = BeamDir;
                t.BasisY = up;
                t.BasisZ = viewdir;

                BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
                sectionBox.Transform = t;
                sectionBox.Min = min;
                sectionBox.Max = max;

                ViewSection vsec = ViewSection.CreateSection(doc, view.Id, sectionBox);
                return vsec;
            }

            public static ViewSection CreateSectionAtBottomAndRight(Document doc, XYZ Start_Point, XYZ End_Point,
                 BoundingBoxXYZ bb, double MaxBeamWidth, double MaxBeamDepth, double bottomorleftoffset, double ToporRightoffset)
            {

                IList<Parameter> param1 = new List<Parameter>();


                // XYZ v = Start_Point - End_Point;
                double vec_ang1 = 360 - Math.Round(Converter.R2D((End_Point - Start_Point).AngleOnPlaneTo(XYZ.BasisX, XYZ.BasisZ)));
                vec_ang1 = (vec_ang1 == 360) ? 0.0 : vec_ang1;
                XYZ pt1 = bb.Min;
                XYZ pt2 = bb.Max;
                double len = (End_Point - Start_Point).GetLength() * 0.5;



                ViewFamilyType view = Filter.ElementFiltering(doc, typeof(ViewFamilyType), "Section",
                     "Building Section", null) as ViewFamilyType;


                XYZ min = new XYZ(-len - bottomorleftoffset, pt1.Z - 1, -MaxBeamWidth);
                XYZ max = new XYZ(len + ToporRightoffset, pt2.Z + 1, MaxBeamWidth);
                XYZ midpt = Start_Point + (End_Point - Start_Point) / 2;
                XYZ BeamDir = null;

                if (((vec_ang1 >= 0) && (vec_ang1 <= 135)) || ((vec_ang1 >= 315) && (vec_ang1 <= 360)))
                {
                    BeamDir = (Start_Point - End_Point).Normalize();
                }
                else
                {
                    BeamDir = (End_Point - Start_Point).Normalize();
                }
                XYZ up = XYZ.BasisZ;
                XYZ viewdir = BeamDir.CrossProduct(up);
                Transform t = Transform.Identity;
                t.Origin = midpt;
                t.BasisX = BeamDir;
                t.BasisY = up;
                t.BasisZ = viewdir;

                BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
                sectionBox.Transform = t;
                sectionBox.Min = min;
                sectionBox.Max = max;

                ViewSection vsec = ViewSection.CreateSection(doc, view.Id, sectionBox);
                return vsec;
            }
        }
        public class Filter
        {
            /// <summary>
            /// Get Type and Category of Element in the document 
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="targetType"></param>
            /// <param name="targetFamilyName"></param>
            /// <param name="targetTypeName"></param>
            /// <param name="targetCategory"></param>
            /// <returns></returns>
            public static Element ElementFiltering(Document doc, Type TargetClass, string TargetFamilyName,
                           string TargetTypeName, BuiltInCategory? TargetCategory)

            {
                FilteredElementCollector FECol = new FilteredElementCollector(doc).OfClass(TargetClass);
                if (TargetCategory.HasValue)
                {
                    FECol.OfCategory(TargetCategory.Value);
                }
                var TarElem = from Elem in FECol
                              where Elem.Name.Equals(TargetTypeName) &&
                      Elem.get_Parameter(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM)
                     .AsString().Equals(TargetFamilyName)
                              select Elem;
                IList<Element> TargetElems = TarElem.ToList();
                if (TargetElems.Count > 0)
                    return TargetElems[0];
                else
                    return null;
            }
            public static Element ElementFiltering(Document doc, Type TargetClass, string TargetTypeName)
            {
                FilteredElementCollector FECol = new FilteredElementCollector(doc).OfClass(TargetClass);
                IList<Element> TargetElement = (from Elem in FECol
                                                where (Elem.Name.Equals(TargetTypeName))
                                                select Elem).ToList();
                if (TargetElement.Count > 0)
                    return TargetElement.FirstOrDefault();
                else
                    return null;
            }

            public static IList<Element> CategoryNameFilterByRectangle(UIDocument uidoc, string CategoryName)
            // Filter Elements by Catagory Name using cross/window Rectrangle
            {
                Document doc = uidoc?.Document;
                ISelectionFilter filter1 = new CategoryNamefilter(CategoryName);
                ICollection<Element> Elements = uidoc.Selection.PickElementsByRectangle(filter1, "Select " + CategoryName + " by Rectangle : ");
                return Elements as IList<Element>;
            }
            public static IList<Element> SelectColumnandBeambyRectangle(UIDocument uidoc)
            // Filter Elements by Catagory Name using cross/window Rectrangle
            {
                Document doc = uidoc?.Document;
                IList<string> catnames = new List<string>();
                catnames.Add("Structural Framing"); catnames.Add("Structural Columns");
                IList<Element> elemftr = new List<Element>();
                ISelectionFilter filter1 = new CategoriesNamefilter(catnames);
                ICollection<Element> Elements = uidoc.Selection.PickElementsByRectangle(filter1, "Select by Window Rectangle : ");
                return Elements as IList<Element>;
            }

            public static IList<string> FilteredElementCollector2IList(FilteredElementCollector lst, Type typ)
            {
                IList<string> ItemList = new List<string>();
                if (null == lst || lst.Count() < 1)
                {
                    TaskDialog.Show(typ.Name, "No " + typ.Name + " Found ! . . ." + "\n Please Create " + typ.Name);
                    return null;
                }
                else
                {
                    foreach (object item in lst)
                    {
                        if (typ == typeof(RebarCoverType))
                        {

                            ItemList.Add(((RebarCoverType)item).Name + " < " + Converter.Feet2Inch(((RebarCoverType)item)
                               .CoverDistance, 2) + " >");

                        }
                        else if (typ == typeof(RebarBarType))
                        {

                            ItemList.Add(((RebarBarType)item).Name);
                        }
                        else if (typ == typeof(RebarHookType))
                        {
                            ItemList.Add(((RebarHookType)item).Name);
                        }
                    }
                    return ItemList;
                }
            }




            public class CategoryNamefilter : ISelectionFilter
            {
                string CategoryName = "";
                public CategoryNamefilter(string Str)
                {
                    CategoryName = Str;
                }
                public bool AllowElement(Element elem)
                {
                    if (elem.Category.Name.Equals(CategoryName))
                    {
                        return true;
                    }
                    return false;
                }
                public bool AllowReference(Reference refer, XYZ point)
                {
                    return false;
                }

            }

            public class CategoriesNamefilter : ISelectionFilter
            {
                IList<string> CategoriesName = new List<string>();
                public CategoriesNamefilter(IList<string> Str)
                {
                    CategoriesName = Str;
                }
                public bool AllowElement(Element elem)
                {

                    if (elem.Category.Name.Equals(CategoriesName[0]) || elem.Category.Name.Equals(CategoriesName[1]))
                    {
                        return true;
                    }
                    return false;


                }
                public bool AllowReference(Reference refer, XYZ point)
                {
                    return false;
                }

            }
        }
        public class EStorage_Schema
        {
            public class ExtensibleStorage
            {
                Schema schema;
                string Schema_Name;
                string Field_Name;
                Type Key_Type, Value_Type;
                SchemaBuilder schemaBuilder;
                FieldBuilder fieldBuilder;
                string FieldDocumentation;
                public ExtensibleStorage(string guid, string SchemaName, string FieldName, Type KeyType, Type ValueType, string Fielddocumentation)
                {
                    this.Schema_Name = SchemaName;
                    this.Field_Name = FieldName;
                    this.Key_Type = KeyType;
                    this.Value_Type = ValueType;
                    this.schemaBuilder = new SchemaBuilder(new Guid(guid));
                    schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
                    schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);
                    // create a field to store a string map
                    this.FieldDocumentation = Fielddocumentation;
                    this.fieldBuilder = schemaBuilder.AddMapField(Field_Name, Key_Type, Value_Type);
                    fieldBuilder.SetDocumentation(FieldDocumentation);
                    schemaBuilder.SetSchemaName(Schema_Name);
                    schema = schemaBuilder.Finish();
                }
                public ExtensibleStorage(string guid, string SchemaName, string FieldName, Type ValueType, string Fielddocumentation)
                {
                    this.Schema_Name = SchemaName;
                    this.Field_Name = FieldName;
                    this.Value_Type = ValueType;
                    this.schemaBuilder = new SchemaBuilder(new Guid(guid));
                    schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
                    schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);
                    // create a field to store a string map
                    this.FieldDocumentation = Fielddocumentation;
                    this.fieldBuilder = schemaBuilder.AddArrayField(Field_Name, ValueType);
                    fieldBuilder.SetDocumentation(FieldDocumentation);
                    schemaBuilder.SetSchemaName(Schema_Name);
                    schema = schemaBuilder.Finish();
                }


                public void SetData2Element(Element elem, IDictionary<string, string> DataMap)
                {

                    Entity entity = new Entity(schema);
                    Field field = schema.GetField(Field_Name);
                    entity.Set<IDictionary<string, string>>(field, DataMap);
                    elem.SetEntity(entity);
                }
                public void SetData2Element(Element elem, IList<string> DataMap)
                {
                    Entity entity = new Entity(schema);
                    Field field = schema.GetField(Field_Name);
                    entity.Set<IList<string>>(field, DataMap);
                    elem.SetEntity(entity);
                }

                public void SetData2Element(Element elem, IDictionary<string, int> DataMap)
                {
                    Entity entity = new Entity(schema);
                    Field field = schema.GetField(Field_Name);
                    entity.Set<IDictionary<string, int>>(field, DataMap);
                    elem.SetEntity(entity);
                }
                public IDictionary<string, string> GetDatafromElementasString(Element elem)
                {
                    Entity retrievedEntity = elem.GetEntity(schema);
                    if (retrievedEntity.Schema != null)
                    { return retrievedEntity.Get<IDictionary<string, string>>(schema.GetField(Field_Name)); }
                    else { return null; }
                }
                public IList<string> GetDatafromElementasList(Element elem)
                {
                    Entity retrievedEntity = elem.GetEntity(schema);
                    if (retrievedEntity.Schema != null)
                    { return retrievedEntity.Get<IList<string>>(schema.GetField(Field_Name)); }
                    else { return null; }
                }
                /// <summary>
                /// Get Data from Element as ListViewItem.ListViewSubItemCollection
                /// </summary>
                /// <param name="elem"></param>
                /// <returns></returns>
                public IDictionary<string, IList<SWF.ListViewItem.ListViewSubItemCollection>> GetDatafromElementasLVSubItemColl(Element elem)
                {
                    Entity retrievedEntity = elem.GetEntity(schema);
                    if (retrievedEntity.Schema != null)
                    {
                        return retrievedEntity.Get<IDictionary<string, IList<SWF.ListViewItem.ListViewSubItemCollection>>>
                            (schema.GetField(Field_Name));
                    }
                    else { return null; }
                }
                public void DeleteSchema(Element elem)
                {
                    Entity retrievedEntity = elem.GetEntity(schema);
                    retrievedEntity.Clear(Field_Name);
                    retrievedEntity.Dispose();
                    retrievedEntity.Schema.Dispose();
                }

            }
            public static void DeleteEntity(UIDocument ui_doc, String Guid)
            {
                Reference picked = ui_doc.Selection.PickObject(ObjectType.Element, "Pick the element to remove the data from");
                Schema sch = Schema.Lookup(new Guid(Guid));
                ui_doc.Document.GetElement(picked).DeleteEntity(sch);

            }
            public static void DeleteEntity(String guid)
            {
                SchemaBuilder schemaBuilder = new SchemaBuilder(new Guid(guid));
                schemaBuilder.Dispose();

            }

            public static void PrintcCurrentSchemaList()
            {
                IList<Schema> list = Schema.ListSchemas();
                System.Diagnostics.Debug.WriteLine("All Schema returned by the Schema.ListSchemas():");
                string str = null;
                foreach (Schema sch in list)
                {
                    str = str + sch.GUID.ToString() + " - " + sch.SchemaName + " -" + sch.Documentation + "\n";
                }
                TaskDialog.Show("   GUID  ", str);
            }
        }
        public class GeometryUtils
        {
            public static double GetAngleinDegree(XYZ StartPoint, XYZ EndPoint)
            {
                double degree = 360 - Math.Round(Converter.R2D((EndPoint - StartPoint).AngleOnPlaneTo(XYZ.BasisX, XYZ.BasisZ)), 12);
                if (degree == 360) { degree = 0.0; }
                return Math.Round(degree, 8);
            }


        }

        public class WindowsForm
        {
            public static bool GetLengthInput(Document document, String userInputLength, out double dParsedLength)
            {
                dParsedLength = 0;
                Units units = document.GetUnits();
                // try to parse a user entered string (i.e. 100 mm, 1'6")
                return UnitFormatUtils.TryParse(units, UnitType.UT_Length, userInputLength, out dParsedLength);

            }

            public static string Validate_Length_TextBox(Document document, System.Windows.Forms.TextBox Tbox, IsZoroAllowed Zero)
            {
                double value;
                Units units = document.GetUnits();
                if (GetLengthInput(document, Tbox.Text, out value) == false)
                {
                    TaskDialog.Show("Length is Wrong Input", "Type ? to Help");
                    Tbox.Focus();
                }
                if (Zero == IsZoroAllowed.Deny)
                {
                    if (value <= 0)
                    {
                        TaskDialog.Show("No Zero Length is Allowed", "No Zero Length is Allowed \n Type ? to Help");
                        Tbox.Focus();
                    }
                }
                return Tbox.Text = UnitFormatUtils.Format(units, UnitType.UT_Length, value, false, false);

            }
            public static string Validate_Number_TextBox(System.Windows.Forms.TextBox Tbox, NumberType NumberType)
            {
                double value;
                value = Convert.ToDouble(Tbox.Text);
                switch (NumberType)
                {
                    case NumberType.Positive_Only:
                        if (value <= 0)
                        { TaskDialog.Show("Error in Number:", "Should be Positive value "); Tbox.Focus(); }
                        break;
                    case NumberType.Positive_and_Zero:
                        if (value < 0)
                        { TaskDialog.Show("Error in Number:", "Should be pasitive value and Non Zero"); Tbox.Focus(); }
                        break;
                    case NumberType.Negative_Only:
                        if (value >= 0)
                        { TaskDialog.Show("Error in Number:", "Should be Negative value"); Tbox.Focus(); }
                        break;
                    case NumberType.Negative_And_Zero:
                        if (value > 0)
                        { TaskDialog.Show("Error in Number:", "Should be Negative value and Non Zero"); Tbox.Focus(); }
                        break;
                    case NumberType.Zero_Only:
                        if (value != 0)
                        { TaskDialog.Show("Error in Number:", "Should be Zero only"); Tbox.Focus(); }
                        break;
                }
                return value.ToString();

            }

            public static void AddItemsInComboBox(FilteredElementCollector lst, Type typ, System.Windows.Forms.ComboBox CBox)
            {
                if (null == lst || lst.Count() < 1)
                {
                    TaskDialog.Show(typ.Name, "No " + typ.Name + " Found ! . . ." + "\n Please Create " + typ.Name);
                }
                else
                {

                    foreach (object item in lst)
                    {
                        if (typ == typeof(RebarCoverType))
                        {

                            CBox.Items.Add(((RebarCoverType)item).Name + " < " + Converter.Feet2Inch(((RebarCoverType)item)
                               .CoverDistance, 2) + " >");

                        }
                        else if (typ == typeof(RebarBarType))
                        {

                            CBox.Items.Add(((RebarBarType)item).Name);
                        }
                        else if (typ == typeof(RebarHookType))
                        {
                            CBox.Items.Add(((RebarHookType)item).Name);
                        }



                    }
                    CBox.SelectedIndex = 0;
                }
            }
            public static void AddItemsInComboBox(IList<Element> lst, Type typ, System.Windows.Forms.ComboBox CBox)
            {
                if (null == lst || lst.Count() < 1)
                {
                    TaskDialog.Show(typ.Name, "No " + typ.Name + " Found ! . . ." + "\n Please Create " + typ.Name);
                }
                else
                {

                    foreach (object item in lst)
                    {
                        if (typ == typeof(RebarCoverType))
                        {

                            CBox.Items.Add(((RebarCoverType)item).Name + " < " + Converter.Feet2Inch(((RebarCoverType)item)
                               .CoverDistance, 2) + " >");

                        }
                        else if (typ == typeof(RebarBarType))
                        {

                            CBox.Items.Add(((RebarBarType)item).Name);
                        }
                        else if (typ == typeof(RebarHookType))
                        {
                            CBox.Items.Add(((RebarHookType)item).Name);
                        }



                    }
                    CBox.SelectedIndex = 0;
                }
            }
            public static void AddItemsInComboBox(FilteredElementCollector lst, Type typ, System.Windows.Forms.ComboBox CBox, string Value)
            {
                if (null == lst || lst.Count() < 1)
                {
                    TaskDialog.Show(typ.Name, "No " + typ.Name + " Found ! . . ." + "\n Please Create " + typ.Name);
                }
                else
                {

                    foreach (object item in lst)
                    {
                        if (typ == typeof(RebarCoverType))
                        {

                            CBox.Items.Add(((RebarCoverType)item).Name + " < " + Converter.Feet2Inch(((RebarCoverType)item)
                               .CoverDistance, 2) + " >");

                        }
                        else if (typ == typeof(RebarBarType))
                        {

                            CBox.Items.Add(((RebarBarType)item).Name);
                        }
                        else if (typ == typeof(RebarHookType))
                        {
                            CBox.Items.Add(((RebarHookType)item).Name);
                        }

                    }
                    if (Value != "")
                    {
                        CBox.Text = Value;
                    }
                    else { CBox.SelectedIndex = 0; }
                }
            }
            public static void AddItemsInComboBox(FilteredElementCollector lst, Type typ, IList<System.Windows.Forms.ComboBox> CBoxs)
            {
                if (null == lst || lst.Count() < 1)
                {
                    TaskDialog.Show(typ.Name, "No " + typ.Name + " Found ! . . ." + "\n Please Create " + typ.Name);
                }
                else
                {
                    foreach (System.Windows.Forms.ComboBox CBox in CBoxs)
                    {
                        foreach (object item in lst)
                        {
                            if (typ == typeof(RebarCoverType))
                            {
                                CBox.Items.Add(((RebarCoverType)item).Name + " < " + Converter.Feet2Inch(((RebarCoverType)item)
                                   .CoverDistance, 2) + " >");

                            }
                            else if (typ == typeof(RebarBarType))
                            {
                                CBox.Items.Add(((RebarBarType)item).Name);
                            }
                            else if (typ == typeof(RebarHookType))
                            {
                                CBox.Items.Add(((RebarHookType)item).Name);
                            }
                            else if (typ == typeof(RebarShape))
                            {
                                CBox.Items.Add(((RebarShape)item).Name);
                            }

                        }
                        CBox.SelectedIndex = 0;
                    }
                }
            }
            public static void AddItemsInComboBox(IList<Element> lst, Type typ, IList<System.Windows.Forms.ComboBox> CBoxs)
            {
                if (null == lst || lst.Count() < 1)
                {
                    TaskDialog.Show(typ.Name, "No " + typ.Name + " Found ! . . ." + "\n Please Create " + typ.Name);
                }
                else
                {
                    foreach (System.Windows.Forms.ComboBox CBox in CBoxs)
                    {
                        foreach (object item in lst)
                        {
                            if (typ == typeof(RebarCoverType))
                            {
                                CBox.Items.Add(((RebarCoverType)item).Name + " < " + Converter.Feet2Inch(((RebarCoverType)item)
                                   .CoverDistance, 2) + " >");

                            }
                            else if (typ == typeof(RebarBarType))
                            {
                                CBox.Items.Add(((RebarBarType)item).Name);
                            }
                            else if (typ == typeof(RebarHookType))
                            {
                                CBox.Items.Add(((RebarHookType)item).Name);
                            }
                            else if (typ == typeof(RebarShape))
                            {
                                CBox.Items.Add(((RebarShape)item).Name);
                            }

                        }
                        CBox.SelectedIndex = 0;
                    }
                }
            }

        }

        public class MathExtensions
        {

            public static string ToOrdinal(long number)
            {
                if (number < 0) return number.ToString();
                long rem = number % 100;
                if (rem >= 11 && rem <= 13) return number + "th";

                switch (number % 10)
                {
                    case 1:
                        return number + "st";
                    case 2:
                        return number + "nd";
                    case 3:
                        return number + "rd";
                    default:
                        return number + "th";
                }
            }

            public static string ToOrdinal(int number)
            {
                if (number < 0) return number.ToString();
                int rem = number % 100;
                if (rem >= 11 && rem <= 13) return number + "th";

                switch (number % 10)
                {
                    case 1:
                        return number + "st";
                    case 2:
                        return number + "nd";
                    case 3:
                        return number + "rd";
                    default:
                        return number + "th";
                }
            }

            public static string ToOrdinal(string number)
            {
                if (String.IsNullOrEmpty(number)) return number;

                var dict = new Dictionary<string, string>();
                dict.Add("zero", "zeroth");
                dict.Add("nought", "noughth");
                dict.Add("one", "first");
                dict.Add("two", "second");
                dict.Add("three", "third");
                dict.Add("four", "fourth");
                dict.Add("five", "fifth");
                dict.Add("six", "sixth");
                dict.Add("seven", "seventh");
                dict.Add("eight", "eighth");
                dict.Add("nine", "ninth");
                dict.Add("ten", "tenth");
                dict.Add("eleven", "eleventh");
                dict.Add("twelve", "twelfth");
                dict.Add("thirteen", "thirteenth");
                dict.Add("fourteen", "fourteenth");
                dict.Add("fifteen", "fifteenth");
                dict.Add("sixteen", "sixteenth");
                dict.Add("seventeen", "seventeenth");
                dict.Add("eighteen", "eighteenth");
                dict.Add("nineteen", "nineteenth");
                dict.Add("twenty", "twentieth");
                dict.Add("thirty", "thirtieth");
                dict.Add("forty", "fortieth");
                dict.Add("fifty", "fiftieth");
                dict.Add("sixty", "sixtieth");
                dict.Add("seventy", "seventieth");
                dict.Add("eighty", "eightieth");
                dict.Add("ninety", "ninetieth");
                dict.Add("hundred", "hundredth");
                dict.Add("thousand", "thousandth");
                dict.Add("million", "millionth");

                dict.Add("billion", "billionth");
                dict.Add("trillion", "trillionth");
                dict.Add("quadrillion", "quadrillionth");
                dict.Add("quintillion", "quintillionth");

                // rough check whether it's a valid number
                string temp = number.ToLower().Trim().Replace(" and ", " ");
                string[] words = temp.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    if (!dict.ContainsKey(word)) return number;
                }

                // extract last word
                number = number.TrimEnd().TrimEnd('-');
                int index = number.LastIndexOfAny(new char[] { ' ', '-' });
                string last = number.Substring(index + 1);

                // make replacement and maintain original capitalization
                if (last == last.ToLower())
                {
                    last = dict[last];
                }
                else if (last == last.ToUpper())
                {
                    last = dict[last.ToLower()].ToUpper();
                }
                else
                {
                    last = last.ToLower();
                    last = Char.ToUpper(dict[last][0]) + dict[last].Substring(1);
                }

                return number.Substring(0, index + 1) + last;
            }

        }
    }


}

