﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard_ReportDetail : System.Web.UI.Page
{
    private DataSet objds;
    public static Reply objGlobal = new Reply();
    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    public static string type = "";


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Username"] == null)
        {
            Response.Redirect("../Default.aspx");
            return;
        }


        if (!IsPostBack)
        {

            bindKioskHealth();
        }
        
    }

    public void bindKioskHealth()
    {
        try
        {
            int connected = 0;
            int Disconnected = 0;


            if (objds == null)
                objds = new DataSet();

            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                string type = "";
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                //URL = "http://localhost:50462/Service1.svc";
                
                string JsonString = JsonConvert.SerializeObject(type);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/getServiceDataExchange", "POST", dataEncrypted);

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<Reply>(reader);

                if (objRes.res == true)
                {
                    
                  
                    
                    objGlobal.DS = objRes.DS;
                    //if (Request.QueryString["type"] == "WaterTax")
                    //{


                    DataTable dTable = new DataTable();

                    dTable.Columns.Add("S No.");
                    dTable.Columns.Add("Service Type");
                    dTable.Columns.Add("API Consume");
                    dTable.Columns.Add("Formate");
                    dTable.Columns.Add("Input");

                    for (int i = 0; i < objGlobal.DS.Tables[0].Rows.Count; i++)
                    {
                        DataRow dRow = dTable.NewRow();

                        dRow["S No."] = objGlobal.DS.Tables[0].Rows[i]["S No."].ToString();
                        dRow["Service Type"] = objGlobal.DS.Tables[0].Rows[i]["Service Type"].ToString();
                        dRow["API Consume"] = objGlobal.DS.Tables[0].Rows[i]["API Consume"].ToString();
                        dRow["Formate"] ="JSON";
                        dRow["Input"] = objGlobal.DS.Tables[0].Rows[i]["Request Parameter"].ToString();
                        dTable.Rows.Add(dRow);
                    }

                    gv_waterTaxServiceData.Visible = true;
                        gv_PropertyTax.Visible = false;
                        gv_SlumBilling.Visible = false;
                        gv_Nagarvasti.Visible = false;
                        gv_SlumBilling.Visible = false;
                        gv_BirthCertificate.Visible = false;
                        gv_DeathCertificate.Visible = false;
                        gv_waterTaxServiceData.DataSource = dTable;
                        gv_waterTaxServiceData.DataBind();
                    //}
                    //else if (Request.QueryString["type"] == "PropertyTax")
                    //{
                    //    gv_waterTaxServiceData.Visible = false;
                    //    gv_PropertyTax.Visible = true;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_Nagarvasti.Visible = false;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_BirthCertificate.Visible = false;
                    //    gv_DeathCertificate.Visible = false;
                    //    gv_PropertyTax.DataSource = objRes.DS;
                    //    gv_PropertyTax.DataBind();
                    //}
                    //else if (Request.QueryString["type"] == "SlumBilling")
                    //{
                    //    gv_waterTaxServiceData.Visible = false;
                    //    gv_PropertyTax.Visible = false;
                    //    gv_SlumBilling.Visible = true;
                    //    gv_Nagarvasti.Visible = false;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_BirthCertificate.Visible = false;
                    //    gv_DeathCertificate.Visible = false;
                    //    gv_SlumBilling.DataSource = objRes.DS;
                    //    gv_SlumBilling.DataBind();

                    //}
                    //else if (Request.QueryString["type"] == "Nagarvasti")
                    //{
                    //    gv_waterTaxServiceData.Visible = false;
                    //    gv_PropertyTax.Visible = false;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_Nagarvasti.Visible = true;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_BirthCertificate.Visible = false;
                    //    gv_DeathCertificate.Visible = false;
                    //    gv_Nagarvasti.DataSource = objRes.DS;
                    //    gv_Nagarvasti.DataBind();
                    //}
                    //else if (Request.QueryString["type"] == "BirthCertificate")
                    //{
                    //    gv_waterTaxServiceData.Visible = false;
                    //    gv_PropertyTax.Visible = false;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_Nagarvasti.Visible = false;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_BirthCertificate.Visible = true;
                    //    gv_DeathCertificate.Visible = false;
                    //    gv_BirthCertificate.DataSource = objRes.DS;
                    //    gv_BirthCertificate.DataBind();
                    //}
                    //else if (Request.QueryString["type"] == "DeathCertificate")
                    //{
                    //    gv_waterTaxServiceData.Visible = false;
                    //    gv_PropertyTax.Visible = false;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_Nagarvasti.Visible = false;
                    //    gv_SlumBilling.Visible = false;
                    //    gv_BirthCertificate.Visible = false;
                    //    gv_DeathCertificate.Visible = true;
                    //    gv_DeathCertificate.DataSource = objRes.DS;
                    //    gv_DeathCertificate.DataBind();
                    //}

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
            // Response.Write("<script type='text/javascript'>alert( 'catch error : '" + excp.Message + "' )</script>");

            Response.Write("<script type='text/javascript'>alert( 'Service Error Occured Machine Not Connected:-" + excp.Message + "' )</script>");
            //lblTotalConnected.Text = "0";
            //lblTotalDisConnected.Text = "0";
        }


    }


    private PdfPCell GetCell(string text, int i)
    {
        return GetCell(text, 1, i);
    }



    private PdfPCell GetCell(string text, int colSpan, int i)
    {
        var whitefont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 14, Color.BLACK);//"Times New Roman"
        var blackfont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 14, Color.BLACK);//"Times New Roman"

        if (i < 3)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, whitefont));
            cell.HorizontalAlignment = 1;
            //cell.Rowspan = rowSpan;
            cell.Colspan = colSpan;
            //Header colour
            if (i == 1 || i == 2)
            {
                cell.BackgroundColor =  Color.LIGHT_GRAY;
            }
            //column name colour
            if (i == 3)
                cell.BackgroundColor = Color.CYAN;
            return cell;
        }
        else if (i == 3)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, blackfont));
            cell.HorizontalAlignment = 1;
            //cell.Rowspan = rowSpan;
            cell.Colspan = colSpan;
            //Header colour
            if (i == 1 || i == 2)
            {
                cell.BackgroundColor = Color.BLUE;
            }
            //column name colour
            if (i == 3)
                cell.BackgroundColor = Color.CYAN;
            return cell;
        }
        else
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));
            cell.HorizontalAlignment = 1;
            //cell.Rowspan = rowSpan;
            cell.Colspan = colSpan;
            //Header colour
            if (i == 1 || i == 2)
            {
                cell.BackgroundColor = Color.BLUE;
            }
            //column name colour
            if (i == 3)
                cell.BackgroundColor = Color.CYAN;
            string value = text.ToLower();
            if (value == "disconnected" || value.ToLower().Contains("jam") || value.ToLower().Contains("low") || value.ToLower().Contains("out") || value.ToLower().Contains("printererror") || value == "faulty")
            {
                cell.BackgroundColor = Color.RED;
            }
            else if (value.ToLower() == "connected") { cell.BackgroundColor = Color.GREEN; }
            return cell;
        }
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    //UpdateGrid();
        //    //GV_Kiosk_Health.DataSource = objGlobal.DS;
        //    //GV_Kiosk_Health.DataBind();

        //    DataTable dt = objGlobal.DS.Tables[0].Clone();


        //    string searchText = txtSearch.Text.Trim();
        //    bool addRow = false;
        //    if (searchText != "")
        //    {
        //        foreach (DataRow item in objGlobal.DS.Tables[0].Rows)
        //        {
        //            var str = item.ToString();

        //            for (int i = 0; i < objGlobal.DS.Tables[0].Columns.Count; i++)
        //            {
        //                if (item[i].ToString().Contains(searchText))
        //                {
        //                    addRow = true;
        //                    break;
        //                }
        //            }

        //            if (addRow)
        //            {
        //                addRow = false;
        //                dt.Rows.Add(item.ItemArray);
        //            }
        //        }
        //        GV_Kiosk_Health.DataSource = dt;
        //        GV_Kiosk_Health.DataBind();
        //    }
        //    else
        //    {
        //        dt = objGlobal.DS.Tables[0];
        //        GV_Kiosk_Health.DataSource = objGlobal.DS;
        //        GV_Kiosk_Health.DataBind();
        //    }



        //    ////
        //    Response.ContentType = "application/pdf";
        //    //Response.AddHeader("content-disposition", "attachment;filename=Cash_Recent_Health_Report_" + DateTime.Now.ToString("dd-MM-yy_HH:mm") + ".xlsx");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    float[] widArr = new float[] { 200f, 220f, 220f, 220f, 220f, 220f, 220f, 220f, 250f, 280f, 220f, 250f, };


        //    //creating pdf document      
        //    Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 10f, 5f);
        //    // getting writer for pdf
        //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    Paragraph para = new Paragraph();
        //    Paragraph para1 = new Paragraph();
        //    Paragraph para2 = new Paragraph();
        //    para.Alignment = 1;
        //    para.Font = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 30f, Color.BLACK);

        //    para.SpacingBefore = 50;
        //    para.SpacingAfter = 50;
        //    para.Add("PCMC");



        //    PdfPTable table = new PdfPTable(dt.Columns.Count);  // -2 new
        //    //spacing before and after table
        //    table.SetWidths(widArr);
        //    table.TotalWidth = 823f;
        //    table.LockedWidth = true;
        //    table.SpacingBefore = 5f;
        //    table.SpacingAfter = 5f;
        //    table.HorizontalAlignment = 0;


        //    PdfPCell cell = new PdfPCell(new Phrase());
        //    int columnscount = dt.Columns.Count;

        //    // getting bank name
        //    // string login_ini_path = "C:\\inetpub\\wwwroot\\RMS\\Configuration\\kiosk_type.ini";

        //    string login_ini_path = HttpRuntime.AppDomainAppPath + "Configuration\\machines_type.ini";

        //    string bank = null;

        //    string header1 = null;
        //    string header2 = null;
        //    string headertop = " ";


        //    header1 = bank + " PCMC -HEALTH REPORT GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, HH:mm tt");
        //    header2 = " ";
        //    Response.AddHeader("content-disposition", "attachment;filename=PCMC_Health_Report_" + DateTime.Now.ToString("dd-MM-yy_HH:mm") + ".pdf");

        //   // table.AddCell(GetCell(headertop, columnscount, 2));
        //    table.AddCell(GetCell(header1, columnscount, 1));
        //    table.AddCell(GetCell(header2, columnscount, 2));


        //    for (int j = 1; j <= columnscount; j++)
        //    {
        //        table.AddCell(GetCell(dt.Columns[j - 1].ColumnName.ToString(), 3));  //
        //    }


        //    foreach (DataRow row in dt.Rows)
        //    {
        //        //char b = 'A';
        //        //string str = b + "" + k;
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            table.AddCell(GetCell(row[i].ToString(), 4));
        //        }
        //    }
        //    string imageURL = Server.MapPath(".") + "\\images\\logo.png";
        //    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);

        //    jpg.ScaleToFit(140f, 120f);
        //    jpg.SpacingBefore = 10f;
        //    jpg.SpacingAfter = 1f;
        //    jpg.Alignment = Element.ALIGN_MIDDLE;



        //    pdfDoc.Open();
        //    pdfDoc.Add(para);
        //    pdfDoc.Add(jpg);
        //    pdfDoc.Add(table);


        //    pdfDoc.Close();
        //    Response.Write(pdfDoc);
        //    Response.End();
        //}

        //catch (Exception ex)
        //{
        //    Response.Write(ex.Message);
        //}
    }




    protected void GV_Kiosk_Health_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        {

            throw;
        }
    }
}