using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net;
using Newtonsoft.Json;
using System.IO;

public partial class Dashboard_DSMaster : System.Web.UI.MasterPage
{
    public const string AntiXsrfTokenKey = "__AntiXsrfToken";
    public const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    public string _antiXsrfTokenValue;
    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    public static bool healthDetailsPanelVisiable = false;
    public static bool kioskDetailsPanelVisiable = false;
    public static bool txnDetailsPanelVisiable = false;
    public static bool adminPanelVisiable = false;
    public static bool usermgmtPanelVisiable = false;

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            //First, check for the existence of the Anti-XSS cookie
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;

            //If the CSRF cookie is found, parse the token from the cookie.
            //Then, set the global page variable and view state user
            //key. The global variable will be used to validate that it matches in the view state form field in the Page.PreLoad
            //method.
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                //Set the global token variable so the cookie value can be
                //validated against the value in the view state form field in
                //the Page.PreLoad method.
                _antiXsrfTokenValue = requestCookie.Value;

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            //If the CSRF cookie is not found, then this is a new session.
            else
            {
                //Generate a new Anti-XSRF token
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                //Create the non-persistent CSRF cookie
                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    //Set the HttpOnly property to prevent the cookie from
                    //being accessed by client side script
                    HttpOnly = true,

                    //Add the Anti-XSRF token to the cookie value
                    Value = _antiXsrfTokenValue
                };

                //If we are using SSL, the cookie should be set to secure to
                //prevent it from being sent over HTTP connections
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                    responseCookie.Secure = true;

                //Add the CSRF cookie to the response
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Error catch : '" + ex.Message + "')</script>");
            //throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Session Null');window.location ='../Default.aspx';", true);

            //  Response.Redirect("../Default.aspx");
            return;
        }

        hdnRole.Value = Session["PartnerRole"].ToString().ToLower();



        if (!IsPostBack)
        {
            try
            {


                //System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
                //healthDetailsPanelVisiable = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["HealthPanel"].ToString());
                //kioskDetailsPanelVisiable = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["KioskDetailsPanel"].ToString());
                //txnDetailsPanelVisiable = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["TransactionPanel"].ToString());
                //adminPanelVisiable = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AdminPanel"].ToString());
                //usermgmtPanelVisiable = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UserManagement"].ToString());


                //if (!healthDetailsPanelVisiable)
                //    dashboardsection.Visible = false;

                //if (!kioskDetailsPanelVisiable)
                //    Li7.Visible = false;
                //if (!usermgmtPanelVisiable)
                //    usermanagement.Visible = false;
                //if (!txnDetailsPanelVisiable)
                //    transactionreportsection.Visible = false;
                //if (!adminPanelVisiable)
                //    Li2.Visible = false;

            }
            catch (Exception ex)
            {

                throw;
            }


            lblUserName.Text = lblUSName.Text = lblUname.Text = Session["Username"].ToString();


        }





        Reply objRes = new Reply();

        using (WebClient objClient = new WebClient())
        {

            string JsonString = JsonConvert.SerializeObject(Session["Username"].ToString().ToLower());
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            objClient.Headers[HttpRequestHeader.ContentType] = "text/json";
            string result = objClient.UploadString(URL + "/GetUserType", "POST", dataEncrypted);

            EncResponse objEncResponse = JsonConvert.DeserializeObject<EncResponse>(result);
            objEncResponse.ResponseData = AesGcm256.Decrypt(objEncResponse.ResponseData);

            JsonSerializer json = new JsonSerializer();
            json.NullValueHandling = NullValueHandling.Ignore;
            StringReader sr = new StringReader(objEncResponse.ResponseData);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            objRes = json.Deserialize<Reply>(reader);

            if (objRes.res == true)
            {
                if (objRes.DS.Tables[0].Rows[0]["credential_type"].ToString().ToLower().Contains("location") && !objRes.DS.Tables[0].Rows[0]["credential_type"].ToString().ToLower().Contains("administrator"))
                {
                    quesnid.InnerText = objRes.DS.Tables[0].Rows[0]["question"].ToString();
                    Li3.Visible = false;
                    terminalVervicesView.Visible = false;
                    usermanagement.Visible = false;
                    servicesection.Visible = false;
                    transactionreportsection.Visible = true;
                    healthreportsection.Visible = false;
                    Li1.Visible = false;
                    terminalVervicesView.Visible = false;
                    dashboardsection.Visible = false;
                    kioskDataservicemanagement.Visible = false;
                    ServiceDataExchange.Visible = false;
                }


                if (objRes.DS.Tables[0].Rows[0]["role"].ToString().ToLower() != "rms")
                {
                    Li3.Visible = false;
                    terminalVervicesView.Visible = false;
                    usermanagement.Visible = false;
                    // servicesection.Visible = false;
                    transactionreportsection.Visible = true;
                    healthreportsection.Visible = false;
                    Li1.Visible = false;
                    terminalVervicesView.Visible = false;
                    dashboardsection.Visible = false;
                    kioskDataservicemanagement.Visible = false;
                    ServiceDataExchange.Visible = false;
                }



            }
            lblUserName.Text = lblUSName.Text = lblUname.Text = Session["Username"].ToString();
            if (lblUserName.Text.ToLower() != "administrator")
                quesnid.InnerText = objRes.DS.Tables[0].Rows[0]["question"].ToString();
        }
    }
    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        try
        {

            //During the initial page load, add the Anti-XSRF token and user
            //name to the ViewState
            if (!IsPostBack)
            {
                // LNKLogOut.Visible = true;
                //Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

                //If a user name is assigned, set the user name
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            //During all subsequent post backs to the page, the token value from
            //the cookie should be validated against the token in the view state
            //form field. Additionally user name should be compared to the
            //authenticated users name
            else
            {
                //Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] !=
                (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");

                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Error catch : '" + ex.Message + "')</script>");
        }
    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["Username"] = null;
        Response.Redirect("../Default.aspx");
        return;

    }


    protected void btnResetPWD_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void resetPWD_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtpwd.Text.Length > 0 && txtconfirmpwd.Text.Length > 0)
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "text/json";
                    UserDetailsInfo objUserReq = new UserDetailsInfo() { UserName = Session["username"].ToString() + "#" + Txtquestion.Text, Pwd = AesGcm256.MD5Hash(txtconfirmpwd.Text) };
                    string JsonString = JsonConvert.SerializeObject(objUserReq);
                    EncRequest objEncRequest = new EncRequest();
                    objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                    string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                    string result = client.UploadString(URL + "/UpdatePassword", "POST", dataEncrypted);

                    EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                    objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                    JsonSerializer json = new JsonSerializer();
                    json.NullValueHandling = NullValueHandling.Ignore;
                    StringReader sr = new StringReader(objResponse.ResponseData);
                    Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                    Reply objRes = json.Deserialize<Reply>(reader);

                    if (objRes.res == true)
                    {
                        txtpwd.Text = txtconfirmpwd.Text = string.Empty;
                        lblerror.Text = "Password has been reset!";
                        // Response.Redirect("~/Default.aspx");
                        // Response.Write("<script> alert('Password has been reset!')</script> ");
                    }
                    else
                    {
                        txtpwd.Text = txtconfirmpwd.Text = string.Empty;
                        lblerror.Text = objRes.strError;
                    }
                }
            }
            else
            {
                lblerror.Text = "Blank password not allowed!";
                return;
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    public class UserDetailsInfo
    {
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string Role { get; set; }
    }
}
