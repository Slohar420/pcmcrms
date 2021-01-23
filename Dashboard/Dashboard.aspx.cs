﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Data;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

public partial class Dashboard_Dashboard : System.Web.UI.Page
{

    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    public static string Interval = System.Configuration.ConfigurationManager.AppSettings["TimerInterval"].ToString();
    public static DataSet objds;
    Reply objGlobal = new Reply();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Username"] == null)
        {
            Response.Redirect("../Default.aspx");
            return;
        }

        if (!IsPostBack)
        {
            Timer1.Interval = Convert.ToInt32(Interval);
            bindKioskHealth();
        }
    }
    public void ObjTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            bindKioskHealth();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void View_UPSDetails(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {

            int index = Convert.ToInt32(e.CommandArgument.ToString());
            //Session["AppID"] = GridView1.Rows[index].Cells[7].Text;
            // CurrentRowTokenNumber = GridView1.Rows[index].Cells[2].Text;
            // string KioskIp = GV_Kiosk_Health.Rows[index].Cells[3].Text;
            string KioskIp = "172.16.4.82";

            if (KioskIp != null)
            {
                string url = "http://" + KioskIp + ":15178/ViewPower";

                Response.Redirect("http://" + KioskIp + ":15178/ViewPower/");

            }





            // string url = "EODViewDetails.aspx";
            // Response.Write("<script type='text/javascript'>window.open('" + url + "');</script>");
            // Response.Redirect("GenerateVisitorPass.aspx");
        }
    }
    public void bindKioskHealth()
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

                string result = client.UploadString(URL + "/GetKioskHealth", "POST", dataEncrypted);

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                JsonSerializer json = new JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<Reply>(reader);

                if (objRes.res == true)
                {
                    objGlobal.DS = objRes.DS;
                    GV_Kiosk_Health.DataSource = objRes.DS;
                    GV_Kiosk_Health.DataBind();

                    for (int i = 1; i <= objRes.DeviceCount; ++i)
                    {
                        switch (i)
                        {
                            case 1:
                                div1.Visible = false;
                                l1.Visible = false;
                                break;
                            case 2:
                                l2.Visible = false;
                                break;
                            case 3:
                                l3.Visible = false;
                                break;
                            case 4:
                                l4.Visible = false;
                                break;
                            case 5:
                                div2.Visible = false;
                                l5.Visible = false;
                                break;
                            case 6:
                                l6.Visible = false;
                                break;
                            case 7:
                                l7.Visible = false;
                                break;
                            case 8:
                                l8.Visible = false;
                                break;
                            case 9:
                                div3.Visible = false;
                                l9.Visible = false;
                                break;
                            case 10:
                                l10.Visible = false;
                                break;
                            case 11:
                                l11.Visible = false;
                                break;
                            case 12:
                                l12.Visible = false;
                                break;
                            default:
                                break;
                        }
                    }

                    for (int i = 0; i < GV_Kiosk_Health.Rows.Count; i++)
                    {
                        if (Convert.ToDateTime(GV_Kiosk_Health.Rows[i].Cells[16].Text) < DateTime.Now.AddMinutes(-3)) // Checking if health data was updated in previews 120 min 
                        {
                            GV_Kiosk_Health.Rows[i].Cells[5].Controls.Clear();
                            Image img1 = new Image();
                            img1.ImageUrl = "images/cross.png";
                            GV_Kiosk_Health.Rows[i].Cells[5].Controls.Add(img1);
                            img1.ToolTip = "DISCONNECTED";
                            img1.Attributes.Add("data-toggle", "popover");
                            img1.Attributes.Add("data-content", "kiosk is not healthy");

                            //i++;
                        }
                        else
                        {
                            GV_Kiosk_Health.Rows[i].Cells[5].Controls.Clear();
                            Image img1 = new Image();
                            img1.ImageUrl = "images/right.png";
                            GV_Kiosk_Health.Rows[i].Cells[5].Controls.Add(img1);
                            img1.ToolTip = "CONNECTED";
                            img1.Attributes.Add("data-toggle", "popover");
                            img1.Attributes.Add("data-content", "kiosk is healthy");
                            //  i++;
                        }
                    }
                    //lblTotalConnected.Text = connected.ToString();
                    //lblTotalDisConnected.Text = Disconnected.ToString();
                }
                else
                {
                    // lblTotalRows.Text = "0";
                    Response.Write("<script type='text/javascript'>alert('" + objRes.strError + "')</script>");
                }
            }
        }
        catch (Exception excp)
        {

            Response.Write("<script type='text/javascript'>alert( 'Service Error Occured Machine Not Connected:-" + excp.Message + "' )</script>");
            //lblTotalConnected.Text = "0";
            //lblTotalDisConnected.Text = "0";
        }
    }




    protected void GVHealth_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (!(sender is GridView))
            return;
        var colCount = e.Row.Cells.Count;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                for (int i = 0; i <= colCount - 1; i++)
                {



                    if (e.Row.Cells[i].Text.ToLower() == "healthy")
                    {
                        e.Row.Cells[i].Controls.Clear();
                        Image img1 = new Image();

                        img1.ImageUrl = "images/right.png";
                        e.Row.Cells[i].Controls.Add(img1);
                        img1.ToolTip = "Healthy";
                        img1.Attributes.Add("data-toggle", "tooltip");

                    }
                    if (e.Row.Cells[i].Text.ToLower() == "faulty")
                    {
                        e.Row.Cells[i].Controls.Clear();
                        Image img1 = new Image();
                        img1.ImageUrl = "images/faulty.png";
                        e.Row.Cells[i].Controls.Add(img1);
                        img1.ToolTip = "Faulty";
                        img1.Attributes.Add("data-toggle", "tooltip");

                    }

                    if (e.Row.Cells[i].Text.ToLower().Contains("connected"))
                    {
                        e.Row.Cells[i].Controls.Clear();
                        Image img1 = new Image();
                        // img1.ImageUrl = GetImage(cell.Text);
                        img1.ImageUrl = "images/right.png";
                        e.Row.Cells[i].Controls.Add(img1);
                        img1.ToolTip = "Connected";
                        img1.Attributes.Add("data-toggle", "tooltip");
                    }
                    if (e.Row.Cells[i].Text.ToLower().Contains("disconnected"))
                    {
                        e.Row.Cells[i].Controls.Clear();
                        Image img1 = new Image();
                        // img1.ImageUrl = GetImage(cell.Text);
                        img1.ImageUrl = "images/cross.png";
                        e.Row.Cells[i].Controls.Add(img1);
                        img1.ToolTip = "Disconnected";
                        img1.Attributes.Add("data-toggle", "tooltip");
                    }
                    if (e.Row.Cells[i].Text.ToLower().Contains("error"))
                    {
                        e.Row.Cells[i].Controls.Clear();
                        Image img1 = new Image();
                        // img1.ImageUrl = GetImage(cell.Text);
                        img1.ImageUrl = "images/printererror.png";
                        e.Row.Cells[i].Controls.Add(img1);
                        img1.ToolTip = "Printer Error";
                        img1.Attributes.Add("data-toggle", "tooltip");
                    }
                    if (e.Row.Cells[i].Text.ToLower().Contains("unknown"))
                    {
                        e.Row.Cells[i].Controls.Clear();
                        Image img1 = new Image();
                        // img1.ImageUrl = GetImage(cell.Text);
                        img1.ImageUrl = "images/unknown.png";
                        e.Row.Cells[i].Controls.Add(img1);
                        img1.ToolTip = "Unknown Error";
                        img1.Attributes.Add("data-toggle", "tooltip");
                    }
                    if (e.Row.Cells[i].Text.ToLower().Contains("out"))
                    {
                        e.Row.Cells[i].Controls.Clear();
                        Image img1 = new Image();
                        // img1.ImageUrl = GetImage(cell.Text);
                        img1.ImageUrl = "images/paperout.png";
                        e.Row.Cells[i].Controls.Add(img1);
                        img1.ToolTip = "Paper Out";
                        img1.Attributes.Add("data-toggle", "tooltip");
                    }
                    if (e.Row.Cells[i].Text.ToLower().Contains("low"))
                    {
                        e.Row.Cells[i].Controls.Clear();
                        Image img1 = new Image();
                        // img1.ImageUrl = GetImage(cell.Text);
                        img1.ImageUrl = "images/paperlow.png";
                        e.Row.Cells[i].Controls.Add(img1);
                        img1.ToolTip = "Paper Low";
                        img1.Attributes.Add("data-toggle", "tooltip");
                    }

                    e.Row.Cells[15].Controls.Clear();
                    Image img = new Image();
                    // img1.ImageUrl = GetImage(cell.Text);
                    img.ImageUrl = "images/status.png";
                    img.Height = 30;
                    img.Width = 45;



                    e.Row.Cells[15].Controls.Add(img);
                    img.ToolTip = "View UPS Status";
                    e.Row.Cells[15].Attributes.Add("class", "upsviewer");

                    img.Attributes.Add("data-toggle", "tooltip");
                    img.Attributes.Add("data-content", "And here's some amazing content. It's very engaging. Right?");
                    e.Row.Cells[2].Attributes.Add("class", "kioskip");


                }
            }
        }
    }
}