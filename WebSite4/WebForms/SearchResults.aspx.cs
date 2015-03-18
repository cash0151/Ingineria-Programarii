using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class WebForms_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["query"] == null) Response.Redirect("PageNotFound.aspx");

        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        SqlCommand c;
        SqlDataReader r;
        String query = Request.QueryString["query"];
        c = new SqlCommand("SELECT persoana_username FROM profiluri WHERE persoana_username LIKE(@user) ", conn);
        c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
        c.Parameters["@user"].Value = String.Format("%{0}%", query);
            //
         r = c.ExecuteReader();
         HtmlGenericControl  titlu = new HtmlGenericControl();
         titlu.TagName = "h2";
         titlu.InnerText = "PERSOANE";
         Panel2.Controls.Add(titlu);
         while (r.Read())
         {
             String user = (String)r["persoana_username"];
             HtmlGenericControl results = new HtmlGenericControl();
             results.TagName = "a";
             results.Attributes["href"] = String.Format("PaginaProfil.aspx?profil_persoana={0}", user);
             results.Attributes["class"] = "searchResults";
             results.InnerText = user;
             Panel2.Controls.Add(results);
         }
         r.Close();
         HtmlGenericControl titlu2 = new HtmlGenericControl();
         titlu2.TagName = "h2";
         titlu2.InnerText = "GRUPURI";
         Panel2.Controls.Add(titlu2);
         c = new SqlCommand("SELECT grup_name FROM profiluri WHERE grup_name LIKE (@user);", conn);
         c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
         c.Parameters["@user"].Value = String.Format("%{0}%", query);
         r = c.ExecuteReader();
         while (r.Read())
         {

             String user = (String)r["grup_name"];
             HtmlGenericControl results = new HtmlGenericControl();
             results.TagName = "a";
             results.Attributes["href"] = String.Format("PaginaProfil.aspx?profil_grup={0}", user);
             results.Attributes["class"] = "searchResults";
             results.InnerText = user;
             Panel2.Controls.Add(results);

         }
         r.Close();
    }
}