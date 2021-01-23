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
using System.Threading;

public partial class Dashboard_ScreenSDelation : System.Web.UI.Page
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

            if (servicesGrid.Rows.Count == 0) {
                gridErrorDiv.Visible = true;
                lblGridError.InnerText = "Please select an ID to get screensaver details";
            }

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
                        ddlkiosklist.Items.Add(new ListItem { Text = objRes.DS.Tables[0].Rows[i]["kiosk ID"].ToString(), Value= objRes.DS.Tables[0].Rows[i]["kiosk IP"].ToString() });
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
                //if (e.Row.Cells[0].Text != "")
                //{
                //    string[] files = e.Row.Cells[1].Text.ToString().Split(':');
                //    DropDownList list = new DropDownList();

                //    list.Items.Add("Image Files");
                //    foreach (var item in files)
                //    {
                //        if (item.Trim() != "")
                //            list.Items.Add(new ListItem { Text = item, Value = item });
                //    }

                //    Panel panel = new Panel();
                //    list.CssClass = "dropdown";
                //    panel.Controls.Add(list);
                //    e.Row.Cells[1].Controls.Add(panel);
                //}
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

            if (e.Row.RowType == DataControlRowType.DataRow)
            {


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
                case "_delete":
                    {
                        PatchUpdateINI objReq = new PatchUpdateINI();

                        objReq.KioskIP = new string[1];
                        objReq.PatchName = gvr.Cells[1].Text;
                        objReq.patch = "deletescreensaver";
                        objReq.Instant = true;

                        if (objds == null)
                            objds = new DataSet();

                        Reply objRes = new Reply();

                        // send request
                        using (WebClient client1 = new WebClient())
                        {
                            client.Headers[HttpRequestHeader.ContentType] = "text/json";

                            objReq.KioskIP[0] = ddlkiosklist.SelectedItem.Value;

                            string JsonString = JsonConvert.SerializeObject(objReq);
                            EncRequest objEncRequest = new EncRequest();
                            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                            string result = client.UploadString(url + "/PatchSave", "POST", dataEncrypted);

                            EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                            objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                            json.NullValueHandling = NullValueHandling.Ignore;
                            StringReader sr = new StringReader(objResponse.ResponseData);
                            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                            result = json.Deserialize<string>(reader);

                            if (result.ToLower().Contains("delete"))
                            {
                                //Data Source
                                Response.Write("<script type='text/javascript'>alert('ScreenSaver Image Deleted SuccessFully')</script>");
                                bindKioskDetails();
                                Thread.Sleep(1000);

                                if (ddlkiosklist.SelectedIndex != 0)
                                {
                                    getServiceIndivualKiosk(objReq.KioskIP[0]);
                                }
                            }

                            else
                            {
                                Response.Write("<script type='text/javascript'>alert('Error Occurs While Updation ')</script>");
                            }
                        }
                    }
                        break;
                   
                
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
       // bindServiceGrid();
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
            if (ddlkiosklist.SelectedIndex != 0)
            {
                getServiceIndivualKiosk(ddlkiosklist.SelectedItem.Text);
            }
            else {
                gridErrorDiv.Visible = true;
                lblGridError.InnerText = "Please select an ID to get screensaver details";
            }
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
        string res = client.UploadString(url + "/GetCurrentScreenSaverFiles", "POST", jsonstring);

        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
        DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(Reply));
        Reply objReply = (Reply)jsonObj.ReadObject(ms);

        int index = 1;

            if (objReply.res)
            {
                gridErrorDiv.Visible = false;
                DataTable newdt = new DataTable();
                newdt.Columns.Add("SR");
                newdt.Columns.Add("File");
                newdt.Columns.Add("Datetime");

                if (objReply.DS.Tables[0].Rows.Count != 0)
                {

                    foreach (DataRow item in objReply.DS.Tables[0].Rows)
                    {

                        string[] files = item[0].ToString().Split(':');

                        foreach (var file in files)
                        {
                            DataRow row = newdt.NewRow();

                            if (file.Trim() != "")
                            {
                                row["SR"] = Convert.ToString(index++);
                                row["File"] = file;
                                row["Datetime"] = item[1];
                                newdt.Rows.Add(row);
                            }
                           
                        }

                    }

                    if (newdt.Rows.Count != 0)
                    {
                        servicesGrid.DataSource = newdt;
                        servicesGrid.DataBind();
                    }
                    else
                    {
                        servicesGrid.DataSource = null;
                        servicesGrid.DataBind();
                        gridErrorDiv.Visible = true;
                        lblGridError.InnerText = "Data not found!";
                    }

                    // bindGrid();
                }

            }
            else {
                servicesGrid.DataSource = null;
                servicesGrid.DataBind();
                gridErrorDiv.Visible = true;
                lblGridError.InnerText = "Data not found!";
            }
    }
    catch (Exception ex)
    {

    }
}
}