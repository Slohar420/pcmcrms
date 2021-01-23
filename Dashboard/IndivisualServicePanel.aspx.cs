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
using System.Data;

public partial class Dashboard_IndivisualServicePanel : System.Web.UI.Page
{
    public static string serviceID = "";
    public static DataSet objds;
    static DataTable dTable = new DataTable();
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
            else
            {
                Response.Redirect("~/Default.aspx");
            }
            bindKioskDetails();
            bindServiceGrid();
        }
        EssentialInfo.GetMacAddress();


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
                dTable.Columns.Clear();
                dTable = new DataTable();
                dTable.Columns.Add("ServiceId");
                dTable.Columns.Add("Service");
                dTable.Columns.Add("Status");

                for (int i = 0; i < objResponse.DS.Tables[0].Rows.Count; i++)
                {
                    DataRow drow = dTable.NewRow();
                    drow["serviceId"] = objResponse.DS.Tables[0].Rows[i]["ID"].ToString();
                    drow["Service"] = objResponse.DS.Tables[0].Rows[i]["SERVICE"].ToString();
                    drow["Status"] = "Enable";
                    dTable.Rows.Add(drow);
                }

                servicesGrid.DataSource = dTable;
                servicesGrid.DataBind();
                lblGridError.Visible = false;
                gridErrorDiv.Visible = true;

                int Ivisiablerows = 0;

                string partnerRole = Session["PartnerRole"].ToString().ToLower();

                if (Session["PartnerRole"].ToString().ToLower() != "rms" && Session["PartnerRole"].ToString().ToLower() != "superadmin")
                {
                    for (int i = 0; i < servicesGrid.Rows.Count; i++)
                    {
                        if (partnerRole != servicesGrid.Rows[i].Cells[1].Text.ToLower())
                        {
                            servicesGrid.Rows[i].Visible = false;
                        }
                        else
                        {
                            Ivisiablerows++;
                        }

                        servicesGrid.Rows[i].Cells[4].Visible = false;
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

    public void bindKioskDetails()
    {
        try
        {
            if (objds == null)
                objds = new DataSet();

            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";

                string JsonString = JsonConvert.SerializeObject("KioskList");
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(url + "/GetKioskMasterList", "POST", dataEncrypted);

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<Reply>(reader);

                if (objRes.res == true)
                {

                    ddlkiosklist.Items.Clear();
                    ddlkiosklist.Items.Add("Select Kiosk ID");
                    for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                    {
                        ddlkiosklist.Items.Add(objRes.DS.Tables[0].Rows[i]["kiosk ID"].ToString());
                    }
                }
                else
                {
                    Response.Write("<script type='text/javascript'>alert('" + objRes.strError + "')</script>");
                }
            }
        }
        catch (Exception excp)
        {

        }
    }

    protected void GV_Kiosk_Details_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView grid = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.Cells[i].Text == "&nbsp;" || e.Row.Cells[i].Text == "")
                    {
                        e.Row.Cells[i].Text = "N/A";
                    }
                }
            }

        }
        catch (Exception ex)
        {

            throw;
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
            addService.ServiceStatus = radioOperational.Checked ? "Operational" : radioDisable.Checked ? "Disable" : radioInPipeline.Checked ? "InPipeline" : "Disable";
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


    public class ReqServiceChange
    {
        public string status { get; set; }
        public string serviceid { get; set; }

        // public string[] KioskMac { get; set; }
        public string Kioskid { get; set; }
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

                if (e.Row.Cells[2].Text.ToLower().ToString().Contains("enable") || e.Row.Cells[2].Text.ToLower().ToString().Contains("operational"))
                {
                    btn.Text = "DISABLE";
                }
                else if (e.Row.Cells[2].Text.ToLower().ToString().Contains("disable"))
                {
                    btn.Text = "ENABLE";
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
            serviceID = gvr.Cells[0].Text;


            switch (e.CommandName)
            {
                case "delete_service":
                    {
                        gridDiv.Visible = false;
                        confirmmodal.Style.Add("display", "block");

                        break;
                    }
                case "changestatus":
                    {
                        string updatedstatus = "";
                        if (ddlkiosklist.SelectedIndex != 0)
                        {
                            ReqServiceChange objreqServiceChange = new ReqServiceChange();

                            //string currentStatus = gvr.Cells[2].Text.ToString().ToLower() == "disable" ? "Operational" : gvr.Cells[2].Text.ToString().ToLower() == "operational" ? "Disable" : gvr.Cells[2].Text.ToString().ToLower() == "inpipeline" ? "Operational" : "Operational";
                            string currentStatus = gvr.Cells[2].Text.ToString();
                            if (currentStatus.ToLower().ToString().Contains("enable"))
                            {
                                updatedstatus = "Disable";
                            }
                            else
                            {
                                updatedstatus = "Enable";
                            }
                            objreqServiceChange.serviceid = serviceID;
                            objreqServiceChange.status = updatedstatus; // radioOperational.Checked ? "Operational" : radioDisable.Checked ? "Disable" : radioInPipeline.Checked ? "InPipeline" : "Disable";
                            objreqServiceChange.Kioskid = ddlkiosklist.SelectedItem.Text;

                            string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(objreqServiceChange);
                            // string jsonstring = JsonConvert.SerializeObject(serviceID + "#" + currentStatus);
                            string res = client.UploadString(url + "/ChangeInduvisualServiceStatus", "POST", jsonstring);

                            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
                            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(string));
                            string response = (string)jsonObj.ReadObject(ms);

                            //ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "customalert(" + res + ");", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "customalert('Please Select Kiosk ID');", true);
                        }

                        break;
                    }
                //case "edit_service":
                //    {

                //        string res = client.UploadString(url + "/EditService", "POST", jsonstring);

                //        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
                //        DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(string));
                //        string response = (string)jsonObj.ReadObject(ms);

                //        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "customalert(" + res + ");", true);

                //        break;
                //    }
                default:
                    break;
            }

            //bindServiceGrid();
            getServiceIndivualKiosk(ddlkiosklist.SelectedItem.Text);

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

    protected void getKioskList()
    {
        try
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            string jsonstring = JsonConvert.SerializeObject(serviceID);
            string res = client.UploadString(url + "/GetKioskMasterList", "POST", jsonstring);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(Reply));
            Reply objReply = (Reply)jsonObj.ReadObject(ms);
            if (objReply.res)
            {
                if (objReply.DS.Tables[0].Rows.Count != 0)
                {
                    ddlkiosklist.Items.Clear();
                    ddlkiosklist.Items.Add("Select Kiosk Ip");
                    for (int i = 0; i < objReply.DS.Tables[0].Rows.Count; i++)
                    {
                        ddlkiosklist.Items.Add(objReply.DS.Tables[0].Rows[i]["kiosk_ip"].ToString());
                    }
                }
            }



        }
        catch (Exception ex)
        {
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

    protected void ddlkiosklist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getServiceIndivualKiosk(ddlkiosklist.SelectedItem.Text);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

    private void bindGrid()
    {
        int Ivisiablerows = 0;
        string partnerRole = Session["PartnerRole"].ToString().ToLower();

        if (Session["PartnerRole"].ToString().ToLower() != "rms" && Session["PartnerRole"].ToString().ToLower() != "superadmin")
        {
            for (int i = 0; i < servicesGrid.Rows.Count; i++)
            {
                if (partnerRole != servicesGrid.Rows[i].Cells[1].Text.ToLower())
                {
                    servicesGrid.Rows[i].Visible = false;
                }
                else
                {
                    Ivisiablerows++;
                }

                servicesGrid.Rows[i].Cells[4].Visible = false;
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
    protected void getServiceIndivualKiosk(string data)
    {
        try
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            string jsonstring = JsonConvert.SerializeObject(data);
            string res = client.UploadString(url + "/GetServicesListIndivusalKiosk", "POST", jsonstring);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(Reply));
            Reply objReply = (Reply)jsonObj.ReadObject(ms);

            if (objReply.res)
            {
                if (objReply.DS.Tables[0].Rows.Count != 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("Serviceid");
                    table.Columns.Add("Service");
                    table.Columns.Add("Status");
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        DataRow row = table.NewRow();

                        row["Serviceid"] = dTable.Rows[i]["Serviceid"].ToString();
                        row["Service"] = dTable.Rows[i]["SERVICE"].ToString();

                        for (int j = 0; j < objReply.DS.Tables[0].Rows.Count; j++)
                        {
                            if (dTable.Rows[i]["Serviceid"].ToString() == objReply.DS.Tables[0].Rows[j]["ID"].ToString())
                            {
                                if (objReply.DS.Tables[0].Rows[j]["Status"].ToString().Trim() == "")
                                {
                                    row["Status"] = "ENABLE";
                                }
                                else
                                    row["Status"] = objReply.DS.Tables[0].Rows[j]["Status"].ToString();
                            }

                        }
                        table.Rows.Add(row);
                    }
                    servicesGrid.DataSource = table;
                    servicesGrid.DataBind();

                    bindGrid();
                }

            }
        }
        catch (Exception ex)
        {

        }
    }
}