using System;
using System.Collections.Generic;
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
            divContent1.InnerHtml = "";
            List<CourseValueObject> listaRezultate = se.getCoursesFromName(Request.QueryString["query"], 10);
            for (int i = 0; i < listaRezultate.Count; i++)
                divContent1.InnerHtml += listaRezultate[i].getCourseName()+"  ";
        }
        else
        {
            divContent1.InnerHtml = "QUERY INCORECT";
        }
    }
}
