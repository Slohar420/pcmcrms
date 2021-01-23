using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;

public partial class Replenishment : System.Web.UI.Page
{


    public static string serviceID = "";

    public string url { get { return ConfigurationManager.AppSettings["ServiceURL1"].ToString(); } set { value = ConfigurationManager.AppSettings["ServiceURL1"].ToString(); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }

    }

   
    


    public class AddServiceRequest
    {
        public string ServiceName { get; set; }
        public string ServiceStatus { get; set; }
        public string KioskMac { get; set; }
        public string Username { get; set; }
    }

    protected void servicesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {



                Button btn = e.Row.Cells[3].Controls[0] as Button;
                btn.CssClass = "btn btn-primary";
                btn.ToolTip = "Click to change status";

                Button btnDelete = e.Row.Cells[4].Controls[0] as Button;
                btnDelete.ToolTip = "Click to delete record";

                btnDelete.CssClass = "btn btn-danger";

                if (e.Row.Cells[2].Text.ToLower() == "operational")
                {
                    btn.Text = "DISABLE";
                }
                else if (e.Row.Cells[2].Text.ToLower() == "disable")
                {
                    btn.Text = "OPERATIONAL";
                }
                else if (e.Row.Cells[2].Text.ToLower() == "inpipeline")
                {
                    btn.Text = "OPERATIONAL";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

  



    protected void btnno_Click(object sender, EventArgs e)
    {
        try
        {
            gridDiv.Visible = true; // Showing gridview div
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            string jsonstring = JsonConvert.SerializeObject(serviceID);
            string res = client.UploadString(url + "/DeleteService", "POST", jsonstring);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(string));
            string response = (string)jsonObj.ReadObject(ms);

            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "customalert(" + res + ");", true);
        
           
            gridDiv.Visible = true; // Showing gridview div
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}