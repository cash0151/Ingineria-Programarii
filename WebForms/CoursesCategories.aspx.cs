using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class WebForms_CoursesCategories : System.Web.UI.Page
{
    SqlConnection con;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Categorie"] != null)
        {
            con = DbConnection.GetSqlConnection();
            con.Open();


           SqlCommand c = new SqlCommand("SELECT numeCategorie FROM Categorii_Cursuri WHERE id=@idul", con);
            c.Parameters.Add(new SqlParameter("@idul", TypeCode.Int32));
            c.Parameters["@idul"].Value = Int32.Parse(Request.QueryString["Categorie"]);

            Object o = c.ExecuteScalar();
            String nume = (String)o;
            if (nume == null)
            {
                Response.Redirect("Home.aspx");
                return;
            }
            h1.InnerText = nume;

            SqlDataReader Courses = GetCourses();
            LoadCourses(Courses);
            con.Close();
        }
        else Response.Redirect("Home.aspx");
    }

    public void LoadCourses(SqlDataReader reader)
    {

        while (reader.Read())
        {
            LinkButton Curs = new LinkButton();
            Curs.Text = (string)reader["NumeCurs"];
            Curs.CssClass = "ElementeCategorie";
            Curs.Click += new EventHandler(CourseClick);
            Panel1.Controls.Add(Curs);

            Literal br = new Literal();
            br.Text = " <br/> ";
            Panel1.Controls.Add(br); 
        }
    }

    public SqlDataReader GetCourses()
    {
        string Categorie = Request.QueryString["Categorie"];       
        SqlCommand cmd = new SqlCommand("select * from Cursuri where Categorie = " + Categorie, con);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public void CourseClick(object sender, EventArgs e)
    {
        LinkButton Curs = (LinkButton)sender;
        Response.Redirect("Courses.aspx?Curs=" + Curs.Text);
    }
}