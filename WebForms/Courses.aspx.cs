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
        if (Request.QueryString["Curs"] != null)
        {
            SqlConnection conn = DbConnection.GetSqlConnection();
            conn.Open();
            SqlCommand c = new SqlCommand("SELECT Continut FROM cursuri WHERE id=(Select id FROM cursuri WHERE NumeCurs=@NumeCurs )", conn);
            c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
            c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
            object o = (object)c.ExecuteScalar();
            String continut = (String)o;
         
            divContent1.InnerHtml = continut;

            conn.Close();
        }
    }
}