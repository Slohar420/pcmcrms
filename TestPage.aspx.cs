using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class TestPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string  connectionString = "SERVER=172.16.4.107;DATABASE=pcmc;UID=root;PASSWORD=lipimysql;Character Set=utf8;";
        //string connectionString = "SERVER=172.16.4.107;DATABASE=pcmc;UID=root;PASSWORD=lipimysql;";
        //string connectionString = "server=172.16.4.95;database=projectmanagement;user id=root;password=Ashok@2024";

        //MySqlConnection objSqlConnection = new MySqlConnection(connectionString);
        //objSqlConnection.Open();
        //MySqlDataAdapter da = new MySqlDataAdapter("select customername from txn",objSqlConnection);

        //DataSet ds = new DataSet();
        //da.Fill(ds);

        //if (ds.Tables[0].Rows.Count > 0) {
        //    GridHealth.DataSource = ds;
        //    GridHealth.DataBind();
        //}

    }

    protected void GridHealth_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //string sms = "&#2325;&#2375;&#2342;&#2366;&#2352;";
            //Response.Write(Server.HtmlDecode(sms));
            //var data = Encoding.Default.GetBytes(e.Row.Cells[10].Text);
            //e.Row.Cells[10].Text = Server.HtmlDecode(Encoding.UTF8.GetString());

        }
    }
}