using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForms_SearchResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["query"] != null)
        {

            SearchEngine se = new SearchEngine();

            SqlConnection con = DbConnection.GetSqlConnection();
            con.Open();

            List<CourseValueObject> listaRezultate = se.getCoursesFromName(Request.QueryString["query"], 10);
            for (int i = 0; i < listaRezultate.Count; i++)
                divContent1.InnerHtml += "<a class=\"ElementeCategorie\" href=\"Courses.aspx?Curs=" + listaRezultate[i].getCourseName() + "\">" + listaRezultate[i].getCourseName() + "</a></br>";
            
            //SqlCommand c;
            //c = new SqlCommand("SELECT NumeCurs FROM cursuri WHERE numeCurs LIKE '%'+@query+'%' ", con);
            //c.Parameters.Add(new SqlParameter("@query", TypeCode.String));
            //c.Parameters["@query"].Value = Request.QueryString["query"];
            //SqlDataReader r = c.ExecuteReader();
            //while (r.Read())
            //{
                //divContent1.InnerHtml += (String)r["NumeCurs"];
              //  divContent1.InnerHtml += "<a class=\"ElementeCategorie\" href=\"Courses.aspx?Curs=" + (String)r["NumeCurs"] + "\">" + (String)r["NumeCurs"] + "</a></br>";
            //}
            con.Close();
			
        }

        else
        {
            divContent1.InnerHtml = "QUERY INCORECT";
        }
    }
}
