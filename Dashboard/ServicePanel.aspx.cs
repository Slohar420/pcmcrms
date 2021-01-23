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

public partial class Dashboard_ServicePanel : System.Web.UI.Page
{


    public static string serviceName = "";

    public string url { get { return ConfigurationManager.AppSettings["ServiceURL1"].ToString(); } set { value = ConfigurationManager.AppSettings["ServiceURL1"].ToString(); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!object.ReferenceEquals(Session["PartnerRole"], null))
            {
                if (Session["PartnerRole"].ToString() == "")
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
            else {
                Response.Redirect("~/Default.aspx");
            }

          
            bindServiceGrid();
        }
        
    }

    private void bindServiceGrid()
    {
        try
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            string res = client.UploadString(url + "/GetServicesList", "POST", "");

            MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(res));
            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(Reply));
            Reply objResponse = (Reply)jsonObj.ReadObject(ms);


            if (objResponse.res && objResponse.DS.Tables[0].Rows.Count > 0)
            {
                servicesGrid.DataSource = objResponse.DS.Tables[0];
                servicesGrid.DataBind();
                lblGridError.Visible = false;
                gridErrorDiv.Visible = true;

                int Ivisiablerows = 0;

                string partnerRole = Session["PartnerRole"].ToString().ToLower();

                if (Session["PartnerRole"].ToString().ToLower() != "rms" && Session["PartnerRole"].ToString().ToLower() != "superadmin")
                {
                    for (int i = 0; i < servicesGrid.Rows.Count; i++)
                    {
                        if (partnerRole!= servicesGrid.Rows[i].Cells[1].Text.ToLower())
                        {
                            servicesGrid.Rows[i].Visible = false;
                        }
                        else
                        {
                            Ivisiablerows++;
                        }

                        servicesGrid.Rows[i].Cells[2].Visible = false;
                        clientBtnAddService.Visible = false;
                    }

                    if (Ivisiablerows > 0)
                    {
                        servicesGrid.Visible = true;
                    }
                    else
                    {
                        servicesGrid.Visible = false;
                    }
                }
            }
            else
            {
                gridDiv.Visible = false;
                lblGridError.Visible = true;
                lblGridError.InnerText = "No service could be found";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnAddService_Click(object sender, EventArgs e)
    {
        try
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            AddServiceRequest addService = new AddServiceRequest();

            addService.ServiceName = txtServiceName.Text;
            addService.ServiceStatus = "";//radioOperational.Checked ? "Operational" : radioDisable.Checked ? "Disable" : radioInPipeline.Checked ? "InPipeline" : "Disable";
            addService.KioskMac = EssentialInfo.MACADDRESS;
            addService.Username = Session["Username"].ToString();

            string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(addService);
            string res = client.UploadString(url + "/AddService", "POST", jsonstring);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(Reply));
            Reply objReply = (Reply)jsonObj.ReadObject(ms);


            if (objReply.res)
            {
                servicesGrid.DataSource = objReply.DS.Tables[0];
                servicesGrid.DataBind();
                lblerror.Text = objReply.strError;
                bindServiceGrid();
            }
            else
            {
                divError.Visible = true;
                lblerror.Text = objReply.strError;
            }
        }
        catch (Exception ex)
        {

            throw;
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
                    btn.Text = "Enable";
                }
                else if (e.Row.Cells[2].Text.ToLower() == "inpipeline")
                {
                    btn.Text = "Enable";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void servicesGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            GridViewRow gvr = servicesGrid.Rows[Convert.ToInt32(e.CommandArgument)];
            serviceName = gvr.Cells[1].Text;


            switch (e.CommandName)
            {
                case "delete_service":
                    {
                        gridDiv.Visible = false;
                        confirmmodal.Style.Add("display", "block");

                        break;
                    }            
                   
                default:
                    break;
            }

            bindServiceGrid();

        }
        catch (Exception ex)
        {

            throw;
        }
    }



    protected void btnno_Click(object sender, EventArgs e)
    {
        try
        {
            confirmmodal.Style.Add("display", "none");//Hiding confirm modal 
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

            string jsonstring = JsonConvert.SerializeObject(serviceName);
            string res = client.UploadString(url + "/DeleteService", "POST", jsonstring);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(string));
            string response = (string)jsonObj.ReadObject(ms);

            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "customalert(" + res + ");", true);
            bindServiceGrid();
            confirmmodal.Style.Add("display", "none");  //Hiding confirm modal 
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