using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //daca nu sunt logat sa nu afisez log out si nici butonul de profil
        if (Session["login"] == null)
        {

            Button1.Visible = false;
            Button4.Visible = false;
        }
        else
        {
            //daca sunt deja logat sa nu afisez login si sign up
            Button2.Visible = false;
            Button3.Visible = false;

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["login"] = null;
        Button1.Visible = false;
        Button2.Visible = true;
        Button3.Visible = true;
        
    }
    protected void Button1_Click2(object sender, EventArgs e)
    {
        Response.Redirect("login.aspx");
    }
    protected void Button1_Click3(object sender, EventArgs e)
    {
        Response.Redirect("Sign Up.aspx");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        String username=((AppData) Session["login"]).Utilizator;
        Response.Redirect("ProfilePage.aspx?Nume="+username);
    }
}
