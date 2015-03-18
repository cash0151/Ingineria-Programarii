using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class WebForms_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String utilizator = ((AppData)Session["login"]).Utilizator;
        String parola = ((AppData)Session["login"]).Parola;

        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        //Crearea unui query
        conn.Open();
        SqlCommand c = new SqlCommand("Select '1' FROM PERSOANE WHERE UPPER(username)=UPPER(@User) AND UPPER(parola)=UPPER(@parola)", conn);
        c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
        c.Parameters["@User"].Value = utilizator;
        c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
        c.Parameters["@parola"].Value = parola;
        object o = (object)c.ExecuteScalar();
        String raspuns = (String)o;
        if (raspuns == "1") Response.Redirect("Home.aspx");
        conn.Close();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (TextBox1.Text.Equals(""))
            {
                Label3.Text = "Nu se poate crea cont nu nume vid";
                return;
            }
                Label3.Text = "Datele introduse nu sunt valide";
            SqlConnection conn;
            bool unique = true;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //Crearea unui query
            conn.Open();
            SqlCommand c = new SqlCommand("Select '1' FROM PERSOANE WHERE UPPER(username)=UPPER(@User)", conn);
            c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
            c.Parameters["@User"].Value = TextBox1.Text;
            SqlDataReader r = c.ExecuteReader();

            while (r.Read())
            {
                unique = false;

            }
            r.Close();
            if (unique == false)
            {
                Label3.Text = "Numele este folosit";

            }
            else
            {
                c = new SqlCommand("INSERT INTO PERSOANE(username,parola,tip) VALUES(@User,@parola,@tip)", conn);
                c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
                c.Parameters["@User"].Value = TextBox1.Text;
                c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
                c.Parameters["@parola"].Value = TextBox2.Text;
                c.Parameters.Add(new SqlParameter("@tip", TypeCode.String));
                c.Parameters["@tip"].Value = "normal";
                c.ExecuteNonQuery();
                c = new SqlCommand("INSERT INTO profiluri(persoana_username,tip) OUTPUT INSERTED.ID VALUES(@User,@tip)", conn);
                c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
                c.Parameters["@User"].Value = TextBox1.Text;
                c.Parameters.Add(new SqlParameter("@tip", TypeCode.String));
                c.Parameters["@tip"].Value = "public";
                Object o=c.ExecuteScalar();
                int id = (int)o;
                c = new SqlCommand("INSERT INTO albume_poze(profil_id,nume_album) VALUES(@profil_id,@nume_album)", conn);
                c.Parameters.Add(new SqlParameter("@profil_id", TypeCode.Int32));
                c.Parameters["@profil_id"].Value = id;
                c.Parameters.Add(new SqlParameter("@nume_album", TypeCode.String));
                c.Parameters["@nume_album"].Value = "fara_album";
                c.ExecuteNonQuery();
                Response.Redirect("Login.aspx");
            }
            conn.Close();

        }
    }
}