using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForms_ProfilePage : System.Web.UI.Page
{
    SqlConnection conn;
    SqlCommand c;
    object o;
    String username;
    protected void Page_Load(object sender, EventArgs e)
    {
        conn = DbConnection.GetSqlConnection();
        conn.Open();
        Label1.Text = "";
      
        //daca nu este logat nu afisez butonul de adaugat traininguri
        if (Session["login"] == null)
        {
            Button5.Visible = false;
        }
        else
        {
            username = ((AppData)Session["login"]).Utilizator;
            //daca queryul nu este invalid spun asta
            if (Request.QueryString["Nume"] == null)
            {
                Label1.Text = "Queryul nu este valid";
                Button5.Visible = false;
                return;
            }

             //doar daca userul curent este profilul lui si este profesor 
            //arat butonul de creare de trainguri.
            //prima data verific daca userul curent este pe profilul lui
            if (!Request.QueryString["Nume"].Equals(username))
            {
                Button5.Visible = false;
            }
            else
            {
                //daca nu este profesor nu afisez butonul de adaugat traininguri

                if (esteProfesor(username) == false) Button5.Visible = false;
            }
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCourse.aspx");
    }
    public bool esteProfesor(String user)
    {
        c = new SqlCommand("SELECT '1' FROM useri WHERE nume=@nume AND tip='profesor'", conn);
        c.Parameters.Add(new SqlParameter("@nume", TypeCode.String));
        c.Parameters["@nume"].Value = user;
        o = (object)c.ExecuteScalar();
        if (o == null) return false;
        else return true;
        
          
        
    }
}