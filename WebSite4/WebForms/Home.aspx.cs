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
        //daca utilizatorul este un vizitator nu il las sa creeze grupuri
        String utilizator = ((AppData)Session["login"]).Utilizator;
        String parola = ((AppData)Session["login"]).Parola;
        if (utilizator.Equals(""))
        {
            TextBox2.Visible = false;
            Button2.Visible = false;
        }
        }

    protected void Button2_Click(object sender, EventArgs e)
    {

        String nume = TextBox2.Text;
        Label1.Text = "Numele este deja folosit";

        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();

        SqlCommand c = new SqlCommand("SELECT '1' FROM grupuri WHERE UPPER(name)=UPPER(@nume)", conn);
        c.Parameters.Add(new SqlParameter("@nume", TypeCode.String));
        c.Parameters["@nume"].Value =nume;
        Object o = (object)c.ExecuteScalar();
        String raspuns = (String)o;
        if (o!=null) return;
        else
        {
            c = new SqlCommand("INSERT INTO grupuri(name) VALUES(@nume)", conn);
            c.Parameters.Add(new SqlParameter("@nume", TypeCode.String));
            c.Parameters["@nume"].Value = nume;

  
            c.ExecuteNonQuery();
            c = new SqlCommand("INSERT INTO profiluri(grup_name,tip) VALUES(@User,@tip)", conn);
            c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
            c.Parameters["@User"].Value = nume;
            c.Parameters.Add(new SqlParameter("@tip", TypeCode.String));
            c.Parameters["@tip"].Value = "public";
            c.ExecuteNonQuery();
            Label1.Text = "Grupul a fost creat";
        }
    }
}