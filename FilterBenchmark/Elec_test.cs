#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using KJU = Arciviz.KJUtility.KJUtility;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;



#endregion

namespace Elec_Test
{
  [Transaction(TransactionMode.Manual)]
  public class Elec_test : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements)
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;
      Selection sel = uidoc.Selection;
      TaskDialog.Show("BuilDTecH Architects","BuilDTecH Architects by Sudhan");

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
      InputData InputData = new InputData();
      InputData.ShowDialog();
      IList<object> data = InputData.Data;
      
      /////////////////////////////// Input Values
      double LeftOffset = Convert.ToDouble(data[1]),
        RightOffset = Convert.ToDouble(data[2]),
        TopOffset = Convert.ToDouble(data[3]),
             BottomOffset = Convert.ToDouble(data[4]),
             NearClipOffset = -Convert.ToDouble(data[5]),
             FarClipOffset = Convert.ToDouble(data[6]);
      string Section_Name = "Electrical GangBox",
         ElectricalEquipment = "Modular Gang Box";
      //Input  /////////////////////////////// Input Values
      
      if(data[0].Equals(InputData.option.ByFloor))
      {
        Timer floortimeLinq1 = new Timer();
        floortimeLinq1.Start();
        IEnumerable<Element> elems = Linq1(doc,BuiltInCategory.OST_ElectricalEquipment,ElectricalEquipment);
        floortimeLinq1.Stop();
        TaskDialog.Show("time","LINQ1 Method Time = " + floortimeLinq1.Duration.ToString() + 
          " No. of Elements =  " + elems.Count().ToString() );

        elems = null;
        Timer floortimeLinq2 = new Timer();
        floortimeLinq2.Start();
        elems = Linq2(doc,BuiltInCategory.OST_ElectricalEquipment,ElectricalEquipment);
        floortimeLinq2.Stop();
        TaskDialog.Show("time","LINQ2 Method Time = " + floortimeLinq2.Duration.ToString() +
          " No. of Elements =  " + elems.Count().ToString());
        elems = null;

        Timer floortimeFilterRule = new Timer();
        floortimeFilterRule.Start();
        elems = FilterRule(
          doc, 
//          uidoc.ActiveView.Id,
          BuiltInCategory.OST_ElectricalEquipment,ElectricalEquipment);
        floortimeFilterRule.Stop();
        TaskDialog.Show("time","Filter Rule Method Time = " + floortimeFilterRule.Duration.ToString() +
          " No. of Elements =  " + elems.Count().ToString());
        elems = null;

        Timer floortimeFactoryRule = new Timer();
        floortimeFactoryRule.Start();
        elems = Factory(doc,BuiltInCategory.OST_ElectricalEquipment,ElectricalEquipment);
        floortimeFactoryRule.Stop();
        TaskDialog.Show("time"," Factory Rule Method Time = " + floortimeFactoryRule.Duration.ToString() +
          " No. of Elements =  " + elems.Count().ToString());



        //foreach(Element ele in elems)
        //{
        //  Draw_Section.Draw(doc,ele,Section_Name,LeftOffset,RightOffset,TopOffset,BottomOffset,NearClipOffset,FarClipOffset);
        //}



        
      }
      else if(data[0].Equals(InputData.option.BySingle))
      {
        TaskDialog td = new TaskDialog("Element By Element");
        td.Title = "Want to Continue";
        td.MainInstruction = "Do you want to create a new section";
        td.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
        td.DefaultButton = TaskDialogResult.Yes;
        bool next = true;
        while(next)
        {
          ISelectionFilter selFilter = new FamilySelectionFilter(
            doc,
            BuiltInCategory.OST_ElectricalEquipment,
            ElectricalEquipment);
          Reference refe = sel.PickObject(ObjectType.Element, selFilter, "Select Object");
          Element ele = doc.GetElement(refe);
          Draw_Section.Draw(doc,ele,Section_Name,LeftOffset,RightOffset,TopOffset,BottomOffset,NearClipOffset,FarClipOffset);
          TaskDialogResult tdRes = td.Show();
          if(tdRes == TaskDialogResult.No)
          { next = false; }
        }
      }
      else if(data[0].Equals(InputData.option.ByProject))
      {
        Timer floortimeFactoryRule = new Timer();
        floortimeFactoryRule.Start();
        IEnumerable<Element> elems = Factory(doc,BuiltInCategory.OST_ElectricalEquipment,ElectricalEquipment);
        floortimeFactoryRule.Stop();
        TaskDialog.Show("time"," Factory Rule Method Time = " + floortimeFactoryRule.Duration.ToString() +
          " No. of Elements =  " + elems.Count().ToString());
        foreach(Element ele in elems)
        {
          Draw_Section.Draw(doc,ele,Section_Name,LeftOffset,RightOffset,TopOffset,BottomOffset,NearClipOffset,FarClipOffset);
        }
        
      }
      return Result.Succeeded;
    }
    public class FamilySelectionFilter : ISelectionFilter
    {
      Document Doc;
      string FmlyName = "";
      int BultCatId;
      public FamilySelectionFilter(Document doc,BuiltInCategory BuiltInCat,string familyTypeName)
      {
        Doc = doc;
        FmlyName = familyTypeName;
        BultCatId = (int)BuiltInCat;
        
      }
      public bool AllowElement(Element elem)
      {
        //IEnumerable < Element > FtlElem = new FilteredElementCollector(Doc)
        //  .OfCategory(BultCat)
        //  .OfClass(typeof(FamilyInstance))
        //  .WherePasses(
        //      new ElementParameterFilter(
        //       ParameterFilterRuleFactory.CreateEqualsRule(
        //         new ElementId(BuiltInParameter.ELEM_FAMILY_PARAM),FmlyName,true)));
        if(elem.Category.Id.IntegerValue == BultCatId)
        {
          return true;
        }
        return false;
      }
      public bool AllowReference(Reference refer,XYZ point)
      {
        Element e = Doc.GetElement(refer);
        if(e.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString().Equals(FmlyName))
          return true;
        return false;
      }
    }

    #region Retrieve named family type using either LINQ or a parameter filter 
    static IEnumerable<Element> Linq1(
    Document doc,
    BuiltInCategory BultCat,
    string familyTypeName)
    {

      return new FilteredElementCollector(doc).OfCategory(BultCat).OfClass(typeof(FamilyInstance))
        .Cast<FamilyInstance>()
        .Where(x => x .Symbol.Family.Name.Equals(familyTypeName));
    }

    static IEnumerable<Element> Linq2(
    Document doc,
    BuiltInCategory BultCat,
    string familyTypeName)
    {
      return new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>()
                .Where(x => x.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM)
                .AsValueString() == familyTypeName);
    }
    static IEnumerable<Element> FilterRule(
    Document doc,
   // ElementId ActiveViewId,
    BuiltInCategory BultCat,
    string familyTypeName)
    {
      return new FilteredElementCollector(doc)//,ActiveViewId)
      .OfCategory(BultCat)
      .OfClass(typeof(FamilyInstance))
        .WherePasses(
          new ElementParameterFilter(
            new FilterStringRule(
              new ParameterValueProvider(
                new ElementId(BuiltInParameter.ELEM_FAMILY_PARAM)),
              new FilterStringEquals(),familyTypeName,true)));
    }
    static IEnumerable<Element> Factory(
   Document doc,
   BuiltInCategory BultCat,
   string familyTypeName)
    {
      return new FilteredElementCollector(doc)
      .OfCategory(BultCat)
      .OfClass(typeof(FamilyInstance))
      .WherePasses(
        new ElementParameterFilter(
          ParameterFilterRuleFactory.CreateEqualsRule(
            new ElementId(BuiltInParameter.ELEM_FAMILY_PARAM),familyTypeName,true)));
    }
    #endregion // Retrieve named family symbols using either LINQ or a parameter filter
    public class Timer
    {
      [DllImport("Kernel32.dll")]
      private static extern bool QueryPerformanceCounter(
        out long lpPerformanceCount);

      [DllImport("Kernel32.dll")]
      private static extern bool QueryPerformanceFrequency(
        out long lpFrequency);

      private long startTime, stopTime;
      private long freq;

      /// <summary>
      /// Constructor
      /// </summary>
      public Timer()
      {
        startTime = 0;
        stopTime = 0;
        if(!QueryPerformanceFrequency(out freq))
        {
          throw new Win32Exception(
            "high-performance counter not supported");
        }
      }

      /// <summary>
      /// Start the timer
      /// </summary>
      public void Start()
      {
        Thread.Sleep(0); // let waiting threads work
        QueryPerformanceCounter(out startTime);
      }

      /// <summary>
      ///Stop the timer 
      /// </summary>
      public void Stop()
      {
        QueryPerformanceCounter(out stopTime);
      }

      /// <summary>
      /// Return the duration of the timer in seconds
      /// </summary>
      public double Duration
      {
        get
        {
          return (double)(stopTime - startTime)
            / (double)freq;
        }
      }
    }

  }
}