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
        Label3.Text = "";
        String utilizator = ((AppData)Session["login"]).Utilizator;
        String parola = ((AppData)Session["login"]).Parola;

        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        //Crearea unui query
        conn.Open();
        SqlCommand c = new SqlCommand("Select '1' FROM persoane WHERE UPPER(username)=UPPER(@User) AND UPPER(parola)=UPPER(@parola)", conn);
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
            Label3.Text = "Datele introduse nu sunt valide";
            SqlConnection conn;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //Crearea unui query
            conn.Open();
            SqlCommand c = new SqlCommand("Select '1' FROM persoane WHERE UPPER(username)=UPPER(@User) AND UPPER(parola)=UPPER(@parola)", conn);
            c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
            c.Parameters["@User"].Value = TextBox1.Text;
            c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
            c.Parameters["@parola"].Value = TextBox2.Text;
            SqlDataReader r = c.ExecuteReader();
            while (r.Read())
            {
                Label3.Text = "Logat";
                Session["login"] = new AppData(TextBox1.Text, TextBox2.Text);
                Response.Redirect("Home.aspx");
            }
            conn.Close();

        }
    }
    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {

    }
    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
}