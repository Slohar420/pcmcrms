using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class Dashboard_TxnDetail : System.Web.UI.Page
{
    private DataSet objds;
    public static Reply objGlobal = new Reply();
    public string selectedKioskID = "";
    public string selectedTxnID = "";
    private DataTable dtFiltered = new DataTable();
    private static int visibleRowCount = 0;

    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Username"] == null || Session["PartnerRole"] == null || Session["PartnerRole"].ToString() == String.Empty)
        {
            Response.Redirect("../Default.aspx");
            return;
        }


        if (!IsPostBack)
        {

            string isExcelVisible = System.Configuration.ConfigurationManager.AppSettings["TXNEXCELBTN"].ToString();
            string isPdfVisible = System.Configuration.ConfigurationManager.AppSettings["TXNPDFBTN"].ToString();

            if (isExcelVisible.ToLower() == "false" && isPdfVisible.ToLower() == "false")
                btnExport.Visible = btn1.Visible = false;
            else if (isExcelVisible.ToLower() == "false")
                btnExport.Visible = false;
            else if (isPdfVisible.ToLower() == "false")
                btn1.Visible = false;


            bindKioskTxnReport();
            bindServices();
           // divServiceStatus.Visible = false;
            //bindTransactionIDs();

        }
        else
        {

        }




    }

    /// <summary>
    /// It fetch transaction records on load
    /// </summary>
    private void bindKioskTxnReport() //Load transaction report
    {
        try
        {

            if (objds == null)
                objds = new DataSet();

            Reply objRes = new Reply();
            // send request

            string parameters = "";



            GetFilteredTXN filteredTXN = new GetFilteredTXN()
            {
                FromDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd hh:mm:ss"),
                ToDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                txntype = Session["PartnerRole"].ToString().ToLower()
            };

            parameters += "01#" + Session["PartnerRole"].ToString().ToLower() + "#" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " 00:00:00" + "#" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:00";

            if (selectedKioskID != "")
            {
                filteredTXN.KioskID = selectedKioskID;
            }

            string jsondata = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";

                string result = client.UploadString(URL + "/GetAllTxn", "POST", jsondata);

                EncResponse response = JsonConvert.DeserializeObject<EncResponse>(result);


                response.ResponseData = AesGcm256.Decrypt(response.ResponseData);
                //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                //objRes = (Reply)objDCS.ReadObject(objMS);
                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(response.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<Reply>(reader);

                if (objRes.res == true)
                {


                    btn1.Attributes.Remove("disabled");
                    btnExport.Attributes.Remove("disabled");
                    txtSearch.Attributes.Remove("disabled");
                    divError.Visible = false;
                    GV_Kiosk_Health.Visible = true;

                    for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                    {
                        objRes.DS.Tables[0].Rows[i][4] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(objRes.DS.Tables[0].Rows[i][4].ToString()));
                    }

                    objGlobal.DS = objRes.DS;
                    GV_Kiosk_Health.DataSource = objRes.DS;
                    GV_Kiosk_Health.DataBind();
                }
                else
                {
                    this.GV_Kiosk_Health.DataSource = null;
                    GV_Kiosk_Health.DataBind();
                    txtSearch.Attributes.Add("disabled", "disabled");
                    GV_Kiosk_Health.Visible = false;
                    btn1.Attributes.Add("disabled", "disabled");
                    btnExport.Attributes.Add("disabled", "disabled");
                    lblError.InnerText = "NO TRANSACTION OCCURRED IN THE PREVIOUS WEEK";
                    divError.Visible = true;
                    // Response.Write("<script type='text/javascript'>alert('" + objRes.strError + "')</script>");
                }
            }

        }
        catch (Exception excp)
        {
            // Response.Write("<script type='text/javascript'>alert( 'catch error : '" + excp.Message + "' )</script>");

            // Response.Write("<script type='text/javascript'>alert( 'Service Error Occured Machine Not Connected:-" + excp.Message + "' )</script>");
            //lblTotalConnected.Text = "0";
            //lblTotalDisConnected.Text = "0";
        }


    }

    /// <summary>
    /// It fetch transaction records on load
    /// </summary>
    private void bindServices() //binding KioskID 
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
                string jsondata = JsonConvert.SerializeObject("servicename"); //json input to send

                string result = client.UploadString(URL + "/GetColumnOfSM", "POST", jsondata);

                EncResponse response = JsonConvert.DeserializeObject<EncResponse>(result);


                response.ResponseData = AesGcm256.Decrypt(response.ResponseData);
                //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                //objRes = (Reply)objDCS.ReadObject(objMS);
                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(response.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<Reply>(reader);


                ddlServices.Items.Clear();
                ddlServices.Items.Add("Select Service");
                string partnerRole = Session["PartnerRole"].ToString().ToLower();
                if (objRes.res == true)
                {
                    foreach (DataRow item in objRes.DS.Tables[0].Rows)
                    {
                        if (Session["PartnerRole"].ToString().ToLower() != "rms" && Session["PartnerRole"].ToString().ToLower() != "superadmin")
                        {
                            if (item[0].ToString().ToLower() == partnerRole)
                            {
                                ddlServices.Items.Add(item[0].ToString());
                                ddlServices.SelectedIndex = 1;
                                ddlServices.Enabled = false;
                            }
                        }
                        else
                        {
                            ddlServices.Items.Add(item[0].ToString());
                        }
                    }
                }
                else
                {
                    //Response.Write("<script type='text/javascript'>alert('" + objRes.strError + "')</script>");
                }
            }

        }
        catch (Exception excp)
        {
            // Response.Write("<script type='text/javascript'>alert( 'catch error : '" + excp.Message + "' )</script>");

            // Response.Write("<script type='text/javascript'>alert( 'Service Error Occured Machine Not Connected:-" + excp.Message + "' )</script>");
            //lblTotalConnected.Text = "0";
            //lblTotalDisConnected.Text = "0";
        }


    }

    protected void ddTxnFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }

   


    private PdfPCell GetCell(string text, int i, bool bHindiText = false)
    {
        return GetCell(text, 1, i, bHindiText);
    }



    private PdfPCell GetCell(string text, int colSpan, int i, bool bHindiText)
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
                cell.BackgroundColor = Color.LIGHT_GRAY;
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
            PdfPCell cell;
            string value;
            if (bHindiText)  //If text contains hindi data 
            {
                BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.TTF", BaseFont.IDENTITY_H, true);
                Font font = new Font(bf, 10, Font.NORMAL);
                cell = new PdfPCell(new Phrase(14, text, font));

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


                value = text.ToLower();
                if (value == "disconnected" || value.ToLower().Contains("jam") || value.ToLower().Contains("low") || value.ToLower().Contains("out") || value.ToLower().Contains("printererror") || value == "faulty")
                {
                    cell.BackgroundColor = Color.RED;
                }
                return cell;
            }
            else
            {


                cell = new PdfPCell(new Phrase(text));
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


                value = text.ToLower();
                if (value == "disconnected" || value.ToLower().Contains("jam") || value.ToLower().Contains("low") || value.ToLower().Contains("out") || value.ToLower().Contains("printererror") || value == "faulty")
                {
                    cell.BackgroundColor = Color.RED;
                }
                else if (value.ToLower() == "connected") { cell.BackgroundColor = Color.GREEN; }
            }
            return cell;
        }
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        try
        {
            //UpdateGrid();
            //GV_Kiosk_Health.DataSource = objGlobal.DS;
            //GV_Kiosk_Health.DataBind();

            DataTable filteredTable = objGlobal.DS.Tables[0].Clone();


            string searchText = txtSearch.Text.Trim();
            bool addRow = false;
            if (searchText != "")
            {
                foreach (DataRow item in objGlobal.DS.Tables[0].Rows)
                {
                    var str = item.ToString();

                    for (int i = 0; i < objGlobal.DS.Tables[0].Columns.Count; i++)
                    {
                        if (item[i].ToString().Contains(searchText))
                        {
                            addRow = true;
                            break;
                        }
                    }

                    if (addRow)
                    {
                        addRow = false;
                        filteredTable.Rows.Add(item.ItemArray);
                    }
                }
                GV_Kiosk_Health.DataSource = filteredTable;
                GV_Kiosk_Health.DataBind();
            }
            else
            {
                filteredTable = objGlobal.DS.Tables[0];
                GV_Kiosk_Health.DataSource = filteredTable;
                GV_Kiosk_Health.DataBind();
            }


            ////
            Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=Cash_Recent_Health_Report_" + DateTime.Now.ToString("dd-MM-yy_HH:mm") + ".xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            float[] widArr = new float[] { 100, 200f, 200f, 200f, 200f, 220f, 220f, 220f, 220f, 220f, 220f };


            //creating pdf document      
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 10f, 5f);
            // getting writer for pdf
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            Paragraph para = new Paragraph();
            Paragraph para1 = new Paragraph();
            Paragraph para2 = new Paragraph();
            para.Alignment = 1;
            para.Font = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 30f, Color.BLACK);

            para.SpacingBefore = 50;
            para.SpacingAfter = 50;
            para.Add("PCMC");



            PdfPTable table = new PdfPTable(filteredTable.Columns.Count);  // -2 new
            //spacing before and after table
            table.SetWidths(widArr);
            table.TotalWidth = 823f;
            table.LockedWidth = true;
            table.SpacingBefore = 5f;
            table.SpacingAfter = 5f;
            table.HorizontalAlignment = 0;


            PdfPCell cell = new PdfPCell(new Phrase());
            int columnscount = filteredTable.Columns.Count;

            // getting bank name
            // string login_ini_path = "C:\\inetpub\\wwwroot\\RMS\\Configuration\\kiosk_type.ini";

            string login_ini_path = HttpRuntime.AppDomainAppPath + "Configuration\\machines_type.ini";

            string bank = null;

            string header1 = null;
            string header2 = null;

            if (Session["PartnerRole"].ToString().ToLower() == "watertax")
            {
                header1 = bank + " PCMC KIOSK WATERTAX TRANSACTION REPORT GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, HH:mm tt");
                header2 = " ";
                Response.AddHeader("content-disposition", "attachment;filename=PCMC_WaterTax_Transaction_Report_" + DateTime.Now.ToString("dd-MM-yy_HH:mm") + ".pdf");
            }
            else if (Session["PartnerRole"].ToString().ToLower() == "propertytax")
            {
                header1 = bank + " PCMC KIOSK PROPERTYTAX TRANSACTION REPORT GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, HH:mm tt");
                header2 = " ";
                Response.AddHeader("content-disposition", "attachment;filename=PCMC_PropertyTax_Transaction_Report_" + DateTime.Now.ToString("dd-MM-yy_HH:mm") + ".pdf");
            }
            else
            {
                header1 = bank + " PCMC KIOSK TRANSACTION REPORT GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, HH:mm tt");
                header2 = " ";
                Response.AddHeader("content-disposition", "attachment;filename=PCMC_WaterTax_Transaction_Report_" + DateTime.Now.ToString("dd-MM-yy_HH:mm") + ".pdf");
            }
            // table.AddCell(GetCell(headertop, columnscount, 2));
            table.AddCell(GetCell(header1, columnscount, 1, false));
            table.AddCell(GetCell(header2, columnscount, 2, false));


            for (int j = 1; j <= columnscount; j++)
            {
                table.AddCell(GetCell(filteredTable.Columns[j - 1].ColumnName.ToString(), 3));  //
            }

            foreach (DataRow row in filteredTable.Rows)
            {
                //char b = 'A';
                //string str = b + "" + k;
                for (int i = 0; i < filteredTable.Columns.Count; i++)
                {
                    if (i == 4)
                        table.AddCell(GetCell(row[i].ToString(), 4, true));
                    else
                        table.AddCell(GetCell(row[i].ToString(), 4));
                }
            }
            string imageURL = Server.MapPath(".") + "\\images\\logo.png";
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);

            jpg.ScaleToFit(140f, 120f);
            jpg.SpacingBefore = 10f;
            jpg.SpacingAfter = 1f;
            jpg.Alignment = Element.ALIGN_MIDDLE;



            pdfDoc.Open();
            pdfDoc.Add(para);
            pdfDoc.Add(jpg);
            pdfDoc.Add(table);


            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }


    static string Decode(string input)
    {
        var sb = new StringBuilder();
        int position = 0;
        var bytes = new List<byte>();
        while (position < input.Length)
        {
            char c = input[position++];
            if (c == '\\')
            {
                if (position < input.Length)
                {
                    c = input[position++];
                    if (c == 'x' && position <= input.Length - 2)
                    {
                        var b = Convert.ToByte(input.Substring(position, 2), 16);
                        position += 2;
                        bytes.Add(b);
                    }
                    else
                    {
                        AppendBytes(sb, bytes);
                        sb.Append('\\');
                        sb.Append(c);
                    }
                    continue;
                }
            }
            AppendBytes(sb, bytes);
            sb.Append(c);
        }
        AppendBytes(sb, bytes);
        return sb.ToString();
    }

    private static void AppendBytes(StringBuilder sb, List<byte> bytes)
    {
        if (bytes.Count != 0)
        {
            var str = (bytes.ToArray());
            sb.Append(str);
            bytes.Clear();
        }
    }
    static string UnicodeToUTF8(string from)
    {
        var bytes = Encoding.UTF8.GetBytes(from);
        return new string(bytes.Select(b => (char)b).ToArray());
    }

    protected void GV_Kiosk_Health_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            var userrole = Session["Role"].ToString().ToLower();

            if (userrole.Contains("location") && !userrole.Contains("administrator"))
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string uLocation = Session["Location"].ToString().Trim().ToLower();

                    if (Session["Location"].ToString() != "" && e.Row.Cells[6].Text.Trim().ToLower().Contains(uLocation))
                    {
                        e.Row.Visible = true;
                        visibleRowCount++;
                    }
                    else
                    {
                        e.Row.Visible = false;
                    }
                }
            }
          
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void GV_Kiosk_Health_PageIndexChanging(object sender, GridViewPageEventArgs e)  // Page indexing on Grid Transaction Report
    {
        try
        {
            GV_Kiosk_Health.PageIndex = e.NewPageIndex;
            bindKioskTxnReport();
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    protected void ddlKioskIDs_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bool bDDLTxnVisiable = false;

            if (objds == null)
                objds = new DataSet();

            Reply objRes = new Reply();
            string parameters = "02#" + Session["PartnerRole"].ToString().ToLower() + "#";

            if (ddlKioskIDs.SelectedIndex > 0)
            {
                parameters += ddlKioskIDs.SelectedValue.ToString();

                if (datefrom.Value.Length > 0 && dateto.Value.Length > 0)
                {
                    DateTime dfrom = DateTime.ParseExact(datefrom.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    DateTime dto = DateTime.ParseExact(dateto.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    parameters += "#" + dfrom.ToString("yyyy-MM-dd") + " 00:00:00";
                    parameters += "#" + dto.ToString("yyyy-MM-dd") + " 23:59:00";

                }

                bDDLTxnVisiable = true;
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "text/json";
                    string jsondata = JsonConvert.SerializeObject(parameters); //json input to send

                    string result = client.UploadString(URL + "/GetAllTxn", "POST", jsondata);

                    EncResponse response = JsonConvert.DeserializeObject<EncResponse>(result);

                    //  bindTransactionIDs();

                    response.ResponseData = AesGcm256.Decrypt(response.ResponseData);
                    Newtonsoft.Json.JsonSerializer json = new JsonSerializer();
                    json.NullValueHandling = NullValueHandling.Ignore;
                    StringReader sr = new StringReader(response.ResponseData);
                    Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                    objRes = json.Deserialize<Reply>(reader);

                    if (objRes.res == true)
                    {
                        btn1.Attributes.Remove("disabled");
                        btnExport.Attributes.Remove("disabled");
                        txtSearch.Attributes.Remove("disabled");
                        //   divTransactions.Visible = true;
                        divError.Visible = false;
                        GV_Kiosk_Health.Visible = true;

                        //  divTransactions.Visible = bDDLTxnVisiable;
                        objGlobal.DS = objRes.DS;
                        GV_Kiosk_Health.DataSource = objRes.DS;
                        GV_Kiosk_Health.DataBind();


                    }
                    else
                    {
                        btn1.Attributes.Add("disabled", "disabled");
                        txtSearch.Attributes.Add("disabled", "disabled");
                        btnExport.Attributes.Add("disabled", "disabled");
                        divError.Visible = true;
                        //    divTransactions.Visible = false;
                        GV_Kiosk_Health.Visible = false;
                        GV_Kiosk_Health.DataSource = null;
                        GV_Kiosk_Health.DataBind();

                        //     divTransactions.Visible = bDDLTxnVisiable;
                        GV_Kiosk_Health.DataSource = null;
                        GV_Kiosk_Health.DataBind();
                        //Response.Write("<script type='text/javascript'>alert('" + objRes.strError + "')</script>");
                    }
                }
            }          
            else
            {
                bDDLTxnVisiable = false;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnFetchRecords_Click(object sender, EventArgs e) //Fetching records using filters
    {
        try
        {
            if (objds == null)
                objds = new DataSet();

            Reply objRes = new Reply();
            string parameters = "";

            GV_Kiosk_Health.DataSource = null;
            GV_Kiosk_Health.DataBind();
            divError.Visible = false;
            lblError.InnerText = "";

            parameters = "02#" + Session["PartnerRole"].ToString().ToLower() + "";

            if (datefrom.Value.Length > 0 && dateto.Value.Length > 0)
            {
                DateTime dfrom = DateTime.ParseExact(datefrom.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dto = DateTime.ParseExact(dateto.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                parameters += "#" + dfrom.ToString("yyyy-MM-dd") + " 00:00:01";
                parameters += "#" + dto.ToString("yyyy-MM-dd") + " 23:59:00";
            }

            if (ddlServices.SelectedIndex > 0)
            {
                parameters += "#" + ddlServices.SelectedItem.Text.ToLower().ToString();
            }
            else {
                parameters += "#";
            }

            if (ddlServiceStatus.SelectedIndex > 0 && ddlServiceStatus.Visible)
            {
                parameters += "#" + ddlServiceStatus.SelectedItem.Text.ToLower().ToString();
            }
            else {
                parameters += "#";
            }


            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                string jsondata = JsonConvert.SerializeObject(parameters); //json input to send

                string result = client.UploadString(URL + "/GetAllTxn", "POST", jsondata);

                EncResponse response = JsonConvert.DeserializeObject<EncResponse>(result);


                response.ResponseData = AesGcm256.Decrypt(response.ResponseData);
                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(response.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<Reply>(reader);


                if (objRes.res == true)
                {

                    foreach (DataRow item in objRes.DS.Tables[0].Rows)
                    {

                        //  item[] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(objRes.DS.Tables[0].Rows[i][4].ToString()));
                        string name = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(objRes.DS.Tables[0].Rows[0][4].ToString()));
                        item[4] = name;
                    }

                    btn1.Attributes.Remove("disabled");
                    btnExport.Attributes.Remove("disabled");
                    txtSearch.Attributes.Remove("disabled");
                    //divError.Visible = false;
                    GV_Kiosk_Health.Visible = true;
                    objGlobal.DS = objRes.DS;
                    GV_Kiosk_Health.DataSource = objRes.DS;
                    GV_Kiosk_Health.DataBind();
                }
                else
                {
                    btn1.Attributes.Add("disabled", "disabled");
                    txtSearch.Attributes.Add("disabled", "disabled");
                    btnExport.Attributes.Add("disabled", "disabled");
                    divError.Visible = true;
                    lblError.InnerText = "NO RECORD COULD BE FOUND!";
                    GV_Kiosk_Health.Visible = false;
                    //GV_Kiosk_Health.DataSource = null;
                    //GV_Kiosk_Health.DataBind();
                    // Response.Write("<script type='text/javascript'>alert('" + objRes.strError + "')</script>");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public class GetFilteredTXN
    {
        public bool bDateFilter { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string KioskID { get; set; }
        public string TransactionID { get; set; }
        public string txntype { get; set; }
    }

    //protected void ddlKioskIDs_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bool bDDLTxnVisiable = false;

    //        if (objds == null)
    //            objds = new DataSet();

    //        Reply objRes = new Reply();
    //        string parameters = "02#" + Session["PartnerRole"].ToString().ToLower() + "#";

    //        if (ddlKioskIDs.SelectedIndex > 0)
    //        {
    //            parameters += ddlKioskIDs.SelectedValue.ToString();

    //            if (datefrom.Value.Length > 0 && dateto.Value.Length > 0)
    //            {
    //                DateTime dfrom = DateTime.ParseExact(datefrom.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
    //                DateTime dto = DateTime.ParseExact(dateto.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture);

    //                parameters += "#" + dfrom.ToString("yyyy-MM-dd") + " 00:00:00";
    //                parameters += "#" + dto.ToString("yyyy-MM-dd") + " 23:59:00";

    //            }

    //            bDDLTxnVisiable = true;
    //            using (WebClient client = new WebClient())
    //            {
    //                client.Headers[HttpRequestHeader.ContentType] = "text/json";
    //                string jsondata = JsonConvert.SerializeObject(parameters); //json input to send

    //                string result = client.UploadString(URL + "/GetAllTxn", "POST", jsondata);

    //                EncResponse response = JsonConvert.DeserializeObject<EncResponse>(result);

    //                bindTransactionIDs();

    //                response.ResponseData = AesGcm256.Decrypt(response.ResponseData);
    //                Newtonsoft.Json.JsonSerializer json = new JsonSerializer();
    //                json.NullValueHandling = NullValueHandling.Ignore;
    //                StringReader sr = new StringReader(response.ResponseData);
    //                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
    //                objRes = json.Deserialize<Reply>(reader);

    //                if (objRes.res == true)
    //                {
    //                    btn1.Attributes.Remove("disabled");
    //                    btnExport.Attributes.Remove("disabled");
    //                    txtSearch.Attributes.Remove("disabled");
    //                    divTransactions.Visible = true;
    //                    divError.Visible = false;
    //                    GV_Kiosk_Health.Visible = true;

    //                    divTransactions.Visible = bDDLTxnVisiable;
    //                    objGlobal.DS = objRes.DS;
    //                    GV_Kiosk_Health.DataSource = objRes.DS;
    //                    GV_Kiosk_Health.DataBind();


    //                }
    //                else
    //                {
    //                    btn1.Attributes.Add("disabled", "disabled");
    //                    txtSearch.Attributes.Add("disabled", "disabled");
    //                    btnExport.Attributes.Add("disabled", "disabled");
    //                    divError.Visible = true;
    //                    divTransactions.Visible = false;
    //                    GV_Kiosk_Health.Visible = false;
    //                    GV_Kiosk_Health.DataSource = null;
    //                    GV_Kiosk_Health.DataBind();

    //                    divTransactions.Visible = bDDLTxnVisiable;
    //                    GV_Kiosk_Health.DataSource = null;
    //                    GV_Kiosk_Health.DataBind();
    //                    //Response.Write("<script type='text/javascript'>alert('" + objRes.strError + "')</script>");
    //                }
    //            }
    //        }
    //        else if (ddlKioskIDs.SelectedIndex == 0)
    //        {
    //            bindKioskTxnReport();
    //            divTransactions.Visible = false;
    //            ddlTransactionIDs.Items.Clear();
    //        }
    //        else
    //        {
    //            bDDLTxnVisiable = false;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void ddlServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlServices.SelectedIndex > 0)
            {

                //divServiceStatus.Visible = true;
                lblError.InnerText = "";
                GV_Kiosk_Health.DataSource = null;
                GV_Kiosk_Health.DataBind();
                divError.Visible = true;
                lblError.InnerText = "PLEASE SELECT SERVICE STATUS";
            }
            else
            {

                ddlServiceStatus.SelectedIndex = -1;
                //divServiceStatus.Visible = false;
                divError.Visible = true;

                lblError.InnerText = "PLEASE SELECT SERVICE TYPE";
                GV_Kiosk_Health.DataSource = null;
                GV_Kiosk_Health.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void ddlServiceStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlServiceStatus.SelectedIndex > 0)
            {
                string toSendParam = ddlServices.SelectedItem.Text.ToLower().Trim().ToString() + "#" + ddlServiceStatus.SelectedItem.Text.ToLower().Trim().ToString();
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "text/json";
                    string jsondata = JsonConvert.SerializeObject(toSendParam); //json input to send

                    string result = client.UploadString(URL + "/GetAllTxnByServiceStatus", "POST", jsondata);

                    EncResponse response = JsonConvert.DeserializeObject<EncResponse>(result);


                    response.ResponseData = AesGcm256.Decrypt(response.ResponseData);
                    Newtonsoft.Json.JsonSerializer json = new JsonSerializer();
                    json.NullValueHandling = NullValueHandling.Ignore;
                    StringReader sr = new StringReader(response.ResponseData);
                    Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                    Reply objRes = json.Deserialize<Reply>(reader);

                    if (objRes.res == true)
                    {

                        foreach (DataRow item in objRes.DS.Tables[0].Rows)
                        {

                            //  item[] = Encoding.UTF8.GetString(Encoding.Default.GetBytes(objRes.DS.Tables[0].Rows[i][4].ToString()));
                            string name = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(objRes.DS.Tables[0].Rows[0][4].ToString()));
                            item[4] = name;
                        }

                        btn1.Attributes.Remove("disabled");
                        btnExport.Attributes.Remove("disabled");
                        txtSearch.Attributes.Remove("disabled");
                        divError.Visible = false;
                        GV_Kiosk_Health.Visible = true;
                       // divServiceStatus.Visible = true;
                        objGlobal.DS = objRes.DS;
                        GV_Kiosk_Health.DataSource = objRes.DS;
                        GV_Kiosk_Health.DataBind();
                        lblError.InnerText = "";

                    }
                    else
                    {
                        btn1.Attributes.Add("disabled", "disabled");
                        txtSearch.Attributes.Add("disabled", "disabled");
                        btnExport.Attributes.Add("disabled", "disabled");
                        divError.Visible = true;
                        lblError.InnerText = "NO RECORD FOUND!";
                        GV_Kiosk_Health.Visible = false;
                        GV_Kiosk_Health.DataSource = null;
                        GV_Kiosk_Health.DataBind();

                        //Response.Write("<script type='text/javascript'>alert('" + objRes.strError + "')</script>");
                    }
                }
            }
            else
            {
                btn1.Attributes.Add("disabled", "disabled");
                txtSearch.Attributes.Add("disabled", "disabled");
                btnExport.Attributes.Add("disabled", "disabled");

                GV_Kiosk_Health.Visible = false;
                GV_Kiosk_Health.DataSource = null;
                GV_Kiosk_Health.DataBind();
                if (ddlServices.Enabled)
                {
                    ddlServices.SelectedIndex = -1;
                }
               // divServiceStatus.Visible = false;
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}