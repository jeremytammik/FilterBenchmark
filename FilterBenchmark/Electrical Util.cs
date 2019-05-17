using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
//using KJU = Arciviz.KJUtility.KJUtility;
using KJU = FilterBenchmark.KJUtility;
using System.Linq;
using System.Collections;

namespace FilterBenchmark
{
  public class Draw_Section
  {
    public static void Draw(Document doc,Element elem,String Section_Name,
      double LeftOffset,double RightOffset, double TopOffset,double BottomOffset,double NearClipOffset,double FarClipOffset)
    {
      ElementId elemid = elem.Id; //= refe.ElementId;
      XYZ Cen_pt = ((LocationPoint)elem.Location).Point;
      XYZ Y_Plane = new XYZ(0,-1,0);
      ViewFamilyType view = KJU.Filter.ElementFiltering(doc,typeof(ViewFamilyType),
        "Section",Section_Name,null) as ViewFamilyType;
      FamilyInstance inst = elem as FamilyInstance;
      Transform tr = inst.GetTransform();
      Parameter Elec_PanelName = inst.get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_NAME);
      string Elec_Equipment_Name = null;
      Room room = inst.Room;
      if(null != room)
      {
        Elec_Equipment_Name =
          room.Name.Substring(0,room.Name.Length - room.Number.Length - 1)
          + "-"
          + inst.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
      }
      else
      {
        Elec_Equipment_Name = "Exterior" + "-"
        + inst.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
      }

      double Half_Width = inst.GetParameters("##Width")[0].AsDouble() / 2.0,
             Half_Height = inst.GetParameters("##Height")[0].AsDouble() / 2.0;
      XYZ Right_Dir = tr.BasisX;
      XYZ St_Pt = tr.Origin - Right_Dir * Half_Width,
               End_Pt = tr.Origin + Right_Dir * Half_Width;
      XYZ v = St_Pt - End_Pt;
      double X_Angle = (End_Pt - St_Pt).AngleOnPlaneTo(XYZ.BasisX,XYZ.BasisZ);
      //TaskDialog.Show("Angle","Angle = " + Math.Round(X_Angle * 180 / Math.PI,5).ToString()
      //  + " --- " + Elec_Equipment_Name);
      XYZ Min = new XYZ(-(Half_Width + LeftOffset),-(Half_Height + BottomOffset),NearClipOffset);
      XYZ Max = new XYZ(Half_Width + RightOffset,Half_Height + TopOffset,FarClipOffset);
      XYZ Orgin = End_Pt + 0.5 * v;
      XYZ X_Dir = v.Normalize();
      XYZ Up_Dir = XYZ.BasisZ;
      XYZ View_Dir = X_Dir.CrossProduct(Up_Dir);
      Transform Sec_Tr = Transform.Identity;
      Sec_Tr.Origin = Orgin;
      Sec_Tr.BasisX = X_Dir;
      Sec_Tr.BasisY = Up_Dir;
      Sec_Tr.BasisZ = View_Dir;
      BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
      sectionBox.Transform = Sec_Tr;
      sectionBox.Min = Min;
      sectionBox.Max = Max;
      ViewSection vs = null;
      using(Transaction tn = new Transaction(doc))
      {
        tn.Start("Section");
        try
        {
          vs = ViewSection.CreateSection(doc,view.Id,sectionBox);
          Elec_PanelName.Set(Elec_Equipment_Name);
          vs.Name = inst.get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_NAME).AsString();
          tn.Commit();
        }
        catch(Exception)
        { return; }
      }

    }
    /// <summary>
    /// Converts Level to BuilDTecH Floor ID as Integer
    /// </summary>
    //public static int Level2Int(Document doc,ElementId lvl)
    //{
    //  string str = (doc.GetElement(lvl) as Level).get_Parameter(BuiltInParameter.DATUM_TEXT).AsString();
    //  int start = str.IndexOf("("), end = str.IndexOf(")"), len = end - start;
    //  return Convert.ToInt16(str.Substring(start + 1,len - 1));
    //}
    //public static string Level2String(Document doc,ElementId lvl)
    //{
    //  string str = (doc.GetElement(lvl) as Level).get_Parameter(BuiltInParameter.DATUM_TEXT).AsString();
    //  int start = str.IndexOf("("), end = str.IndexOf(")"), len = end - start;
    //  return str.Substring(start + 1,len - 1);
    //}
  }
}

