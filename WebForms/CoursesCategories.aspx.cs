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
        con = DbConnection.GetSqlConnection();
        con.Open();
        SqlDataReader Courses = GetCourses();
        LoadCourses(Courses);
        con.Close();
    }

    public void LoadCourses(SqlDataReader reader)
    {
        ContentPlaceHolder cph = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder2");        
        
        while (reader.Read())
        {
            LinkButton Curs = new LinkButton();
            Curs.Text = (string)reader["NumeCurs"];
            Curs.Click += new EventHandler(CourseClick);
            cph.Controls.Add(Curs);

            Literal br = new Literal();
            br.Text = " <br/> ";
            cph.Controls.Add(br); 
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
        con = DbConnection.GetSqlConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("Select Id from Cursuri where NumeCurs = '" + Curs.Text + "'", con);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int CursId = reader.GetInt32(0);
        Response.Redirect("Courses.aspx?Curs=" + CursId);
    }
}