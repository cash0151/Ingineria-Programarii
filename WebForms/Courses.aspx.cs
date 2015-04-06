using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebForms_Courses : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SqlConnection conn=DbConnection.GetSqlConnection();
        conn.Open();
        SqlCommand c = new SqlCommand("SELECT Continut FROM cursuri WHERE id=23", conn);
        object o = (object)c.ExecuteScalar();
        String continut = (String)o;

        divcontent1.InnerHtml = continut;
      
        conn.Close();
    }
}