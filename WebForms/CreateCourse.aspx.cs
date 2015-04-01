using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForms_CreateCourse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "";
        if (DropDownList1.SelectedIndex < 0)
        {
            Label1.Text = "Nu ai selectat categorie";
            return;
        }
        if (Session["login"] != null)
        {

            String nume_utilizator=((AppData) Session["login"]).Utilizator;
            SqlConnection conn= DbConnection.GetSqlConnection();
           
            conn.Open();
            SqlCommand c = new SqlCommand("SELECT id FROM useri WHERE nume=@nume AND tip='profesor'", conn);
            c.Parameters.Add(new SqlParameter("@nume", TypeCode.String));
            c.Parameters["@nume"].Value = nume_utilizator;
            object o = (object)c.ExecuteScalar();
            int idProfesor = (int)o;
            if (idProfesor == null)
            {
                Label1.Text = "Nu am putut crea cursul";
                return;
            } 


            c = new SqlCommand("INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES(@NumeCurs,@idProfesor,@Continut,@idCategorie);", conn);
            c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
            c.Parameters["@NumeCurs"].Value = TextBox1.Text;
            c.Parameters.Add(new SqlParameter("@idProfesor", TypeCode.Int32));
            c.Parameters["@idProfesor"].Value = idProfesor;
            c.Parameters.Add(new SqlParameter("@Continut", TypeCode.String));
            TextBox2.Text = TextBox2.Text.Replace(Environment.NewLine, "<br/>");
            c.Parameters["@Continut"].Value = TextBox2.Text;
            c.Parameters.Add(new SqlParameter("@idCategorie", TypeCode.Int32));
            c.Parameters["@idCategorie"].Value = DropDownList1.SelectedItem.Value;
            c.ExecuteNonQuery();
            conn.Close();
        }
        else
        {
            Label1.Text = "Nu esti logat";
        }

    }

}