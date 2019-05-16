using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KJU = Arciviz.KJUtility.KJUtility;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SWF = System.Windows.Forms;

namespace Elec_Test
{
  public partial class InputData :  SWF.Form
  {
    public enum option { BySingle , ByFloor, ByProject}
    public IList<object> Data = new List<object>();
    public InputData()
    {
      try
      {
        InitializeComponent();
      }
      catch(Exception ex)
      {
        TaskDialog.Show("Error Found","Error Found ! ! ! " + ex.ToString());
      }
    }
    private IList<object> GetData()
    {
      option opt;
      if(Floor.Checked)
      { opt = option.ByFloor; }
      else if(Element.Checked)
      { opt = option.BySingle; }
      else
      { opt = option.ByProject; }
      Data.Add(opt);
      Data.Add(Convert.ToDouble(LeftOffset.Text));
      Data.Add(Convert.ToDouble(RightOffset.Text));
      Data.Add(Convert.ToDouble(TopOffset.Text));
      Data.Add(Convert.ToDouble(BottomOffset.Text));
      Data.Add(Convert.ToDouble(NearClip.Text));
      Data.Add(Convert.ToDouble(FarClip.Text));
      return Data;
    }
    private void button1_Click(object sender,EventArgs e)
    {
      GetData();
    }
  } // public partial class OptionForm :  SWF.Form
}//namespace Elec_Test
