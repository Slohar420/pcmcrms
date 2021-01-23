using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Dashboard_CreatePartnerRole : System.Web.UI.Page
{
    public static string URL = System.Configuration.ConfigurationManager.AppSettings["ServiceURL1"].ToString();
    public static DataSet objds;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null)
        {
            Response.Redirect("../Default.aspx");
            return;
        }
        if (!IsPostBack)
        {
            gridErrorDiv.Visible = false;
            bindRoleGrid();
          
        }
    }

    
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
           
              
        }
        catch (Exception excp)
        {
            Response.Write("<script type='text/javascript'>alert( 'Error catch " + excp.Message + "')</script>");
        }
    }


    private void bindRoleGrid() {
        try
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            
            string res = client.UploadString(URL + "/GetPartnerRoles", "POST", "");

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(Reply));
            Reply objReply = (Reply)jsonObj.ReadObject(ms);

            if (objReply.res)
            {
                rolesGrid.DataSource = objReply.DS.Tables[0];
                rolesGrid.DataBind();
            }
            else {
                rolesGrid.DataSource = null;
                rolesGrid.DataBind();
                lblGridError.InnerText = "Opps! No records!";
                gridErrorDiv.Visible = true;
                rolesGrid.Visible = false;
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void btnAddRole_Click(object sender, EventArgs e)
    {
        try
        {
            string partnerRole = "";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";

            if (txtPartnerRole.Text.Trim() != "")
                partnerRole = txtPartnerRole.Text.ToLower().Trim();
            else
                return;

            string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(partnerRole+"#"+Session["username"].ToString());
            string res = client.UploadString(URL + "/AddPartnerRole", "POST", jsonstring);

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
            DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(Reply));
            Reply objReply = (Reply)jsonObj.ReadObject(ms);


            if (objReply.res)
            {               
                bindRoleGrid();
                lblerror.Text = objReply.strError;
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



    protected void rolesGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "delete_role")
            {
                string roleID = "";
                GridViewRow gvr = rolesGrid.Rows[Convert.ToInt32(e.CommandArgument)];
                roleID = gvr.Cells[1].Text;

                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "text/json";


                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(roleID);
                string res = client.UploadString(URL + "/DeletePartnerRoles", "POST", jsonstring);

                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(res));
                DataContractJsonSerializer jsonObj = new DataContractJsonSerializer(typeof(Reply));
                Reply objReply = (Reply)jsonObj.ReadObject(ms);


                if (objReply.res)
                {
                    bindRoleGrid();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('"+objReply.strError+"');", true);
                }
                else
                {
                    divError.Visible = true;
                    lblerror.Text = objReply.strError;
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
