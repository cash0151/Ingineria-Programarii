using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForms_LogIn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "";
        if (Session["login"] != null)
        {
            String utilizator = ((AppData)Session["login"]).Utilizator;
            String parola = ((AppData)Session["login"]).Parola;

            SqlConnection conn;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //Crearea unui query
            conn.Open();
            SqlCommand c = new SqlCommand("Select '1' FROM Useri WHERE UPPER(Nume)=UPPER(@User) AND UPPER(Parola)=UPPER(@parola)", conn);
            c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
            c.Parameters["@User"].Value = utilizator;
            c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
            c.Parameters["@parola"].Value = parola;
            object o = (object)c.ExecuteScalar();
            String raspuns = (String)o;
            if (raspuns == "1") Response.Redirect("Home.aspx");

            conn.Close();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        //Crearea unui query
        conn.Open();
        SqlCommand c = new SqlCommand("Select '1' FROM Useri WHERE UPPER(Nume)=UPPER(@User) AND UPPER(Parola)=UPPER(@parola)", conn);
        c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
        c.Parameters["@User"].Value = TextBox1.Text;
        c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
        c.Parameters["@parola"].Value = TextBox2.Text;
        SqlDataReader r = c.ExecuteReader();
        bool logatCuSucces = false;
        while (r.Read())
        {
            logatCuSucces = true;
            Session["login"] = new AppData(TextBox1.Text, TextBox2.Text);
            Response.Redirect("Home.aspx");
        }
        if (logatCuSucces == false) Label1.Text="Numele de utilizator sau parola sunt incorecte";
        conn.Close();
    }
}