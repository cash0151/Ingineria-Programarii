using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForms_Sign_Up : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "";
    
            if (Session["login"] != null)
            {
                String utilizator = ((AppData)Session["login"]).Utilizator;
                String parola = ((AppData)Session["login"]).Parola;

                SqlConnection conn= DbConnection.GetSqlConnection();
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

        if (Page.IsValid)
        {
            SqlConnection conn = DbConnection.GetSqlConnection();
            conn.Open();
            SqlCommand c = new SqlCommand("Select '1' FROM Useri WHERE UPPER(Nume)=UPPER(@User)", conn);
            c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
            c.Parameters["@User"].Value = TextBox1.Text;
            object o = (object)c.ExecuteScalar();
            String raspuns = (String)o;
            if (raspuns == "1")
            {
                Label1.Text = "Numele este deja in folosinta";
                return;
            }


            //aici intru daca numele nu este folosit
            c = new SqlCommand("INSERT INTO Useri(Nume,Parola,Tip) VALUES(@User,@parola,@tip)", conn);
            c.Parameters.Add(new SqlParameter("@User", TypeCode.String));
            c.Parameters["@User"].Value = TextBox1.Text;
            c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
            c.Parameters["@parola"].Value = TextBox2.Text;
            c.Parameters.Add(new SqlParameter("@tip", TypeCode.String));
            //daca bifeaza ca vrea sa fie profesor il bagam ca profesor daca nu bifeaza il bagam ca user normal
            if (CheckBox1.Checked) c.Parameters["@tip"].Value ="profesor";
            else c.Parameters["@tip"].Value = "normal";
          
            c.ExecuteNonQuery();

            Response.Redirect("Home.aspx");
            conn.Close();
        }
    }

}