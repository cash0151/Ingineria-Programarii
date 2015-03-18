using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class WebForms_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String utilizator = ((AppData)Session["login"]).Utilizator;
        if (!utilizator.Equals(""))
        {
            HtmlGenericControl divcontrol = new HtmlGenericControl();
            divcontrol.Attributes["href"] = String.Format("PaginaProfil.aspx?profil_persoana={0}", utilizator);

            divcontrol.Attributes["class"] = "linkNavigare";
            divcontrol.TagName = "a";

            divcontrol.InnerHtml = utilizator;
            Panel1.Controls.Add(divcontrol);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect(String.Format("SearchResults.aspx?query={0}", TextBox1.Text));
    }
}
