using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForms_CreateCourse : System.Web.UI.Page
{
    SqlConnection conn;
    SqlCommand c;
    object o;
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "";
        Label1.ForeColor = Color.Red;
        //verific daca este un profesor cel care intra pe aceasta pagina
        if (Session["login"] == null)
        {
            Response.Redirect("home.aspx");
        }
        else
        {
            conn = DbConnection.GetSqlConnection();
            String nume_utilizator = ((AppData)Session["login"]).Utilizator;
            conn.Open();
            int idProfesor = getIdProfesor(nume_utilizator);
            conn.Close();
            if (idProfesor == -1)
            {
                Response.Redirect("home.aspx");
            }
        }

    }
    public bool numeExistent(String nume)
    {
        c = new SqlCommand("SELECT '1' FROM cursuri WHERE numeCurs=@NumeCurs", conn);
        c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
        c.Parameters["@NumeCurs"].Value = nume;
        o = c.ExecuteScalar();
        if (o== null) return false;
        else return true;
        
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        if (TextBox1.Text.Equals(""))
        {
            Label1.Text = "Cursul trebuie sa aibe un nume";
            return;
        }
        if (TextBox2.Text.Equals(""))
        {
            Label1.Text = "Cursul trebuie sa aibe o descriere";
            return;
        }
        if (TextBox3.Text.Equals(""))
        {
            Label1.Text = "Cursul trebuie sa aibe un loc de desfasurare";
            return;
        }
        if (TextBox4.Text.Equals(""))
        {
            Label1.Text = "Cursul trebuie sa aibe un program";
            return;
        }

            if (DropDownList1.SelectedIndex < 0)
            {
                Label1.Text = "Nu ai selectat categorie";
                return;
            }
            if (Session["login"] != null)
            {

                String nume_utilizator = ((AppData)Session["login"]).Utilizator;
                conn = DbConnection.GetSqlConnection();

                conn.Open();
                int idProfesor = getIdProfesor(nume_utilizator);
                //daca este -1 inseamna ca userul nu este profesor sau nu exista
                if (idProfesor ==-1)
                {
                    Label1.Text = "Nu am putut crea cursul pentru ca nu aveti drepturi";
                    conn.Close();
                    return;
                }

                //verific daca numele cursuri exista deja, daca da nu il las sa faca
                bool existaNumeleCursului = numeExistent(TextBox1.Text);
                if (existaNumeleCursului == true)
                {
                    Label1.Text = "Acest nume pentru training este deja in folositna";
                    conn.Close();
                    return;
                }

                c = new SqlCommand("INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie,Locatie,Program) VALUES(@NumeCurs,@idProfesor,@Continut,@idCategorie,@Locatie,@Program);", conn);
                c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
                c.Parameters["@NumeCurs"].Value = TextBox1.Text;
                c.Parameters.Add(new SqlParameter("@idProfesor", TypeCode.Int32));
                c.Parameters["@idProfesor"].Value = idProfesor;
                c.Parameters.Add(new SqlParameter("@Continut", TypeCode.String));
                TextBox2.Text = TextBox2.Text.Replace(Environment.NewLine, "<br/>");
                TextBox2.Text = TextBox2.Text.Replace(" ", "&nbsp");
                c.Parameters["@Continut"].Value = TextBox2.Text;

                c.Parameters.Add(new SqlParameter("@idCategorie", TypeCode.Int32));
                c.Parameters["@idCategorie"].Value = DropDownList1.SelectedItem.Value;
                c.Parameters.Add(new SqlParameter("@Locatie", TypeCode.String));
                TextBox3.Text = TextBox3.Text.Replace(Environment.NewLine, "<br/>");
                TextBox3.Text = TextBox3.Text.Replace(" ", "&nbsp");
                c.Parameters["@Locatie"].Value = TextBox3.Text;
                c.Parameters.Add(new SqlParameter("@Program", TypeCode.String));
                TextBox4.Text = TextBox4.Text.Replace(Environment.NewLine, "<br/>");
                TextBox4.Text = TextBox4.Text.Replace(" ", "&nbsp");
                c.Parameters["@Program"].Value = TextBox4.Text;
                c.ExecuteNonQuery();
                conn.Close();
                TextBox1.Text = "";
                TextBox2.Text = "";
                TextBox3.Text = "";
                TextBox4.Text = "";
                Label1.ForeColor = Color.Green;
                Label1.Text = "Curs creat cu succes";
                
            }
            else
            {
                Label1.Text = "Nu esti logat";
            }
          
        
    }
    public int getIdProfesor(String user)
    {
        c = new SqlCommand("SELECT id FROM useri WHERE nume=@nume AND tip='profesor'", conn);
        c.Parameters.Add(new SqlParameter("@nume", TypeCode.String));
        c.Parameters["@nume"].Value = user;
        o = (object)c.ExecuteScalar();
        if (o == null) return -1;
        else
        {
            return (int)o;
        }
    }
}