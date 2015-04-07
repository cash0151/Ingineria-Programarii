using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebForms_Courses : System.Web.UI.Page
{
    string username;
    protected void Page_Load(object sender, EventArgs e)
    {
        SqlConnection conn=DbConnection.GetSqlConnection();
        conn.Open();
        string CursId = Request.QueryString["Curs"];
        SqlCommand c = new SqlCommand("SELECT Continut FROM cursuri WHERE id="+CursId, conn);
        object o = (object)c.ExecuteScalar();
        String continut = (String)o;

        divContent1.InnerHtml = continut;
        
        conn.Close();

        try
        {
            username = ((AppData)Session["login"]).Utilizator;

            if (!VerifyNota(GetUserId(username), CursId))
            {
                LoadRating();
            }
        }
        catch { }
        
    }

    public int GetUserId(string Name)
    {
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("select id from useri where Nume = '" + Name + "'", con);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int id = (int)reader.GetInt32(0);
        reader.Close();
        con.Close();
        return id;
    }

    public void LoadRating()
    {
        Literal text = new Literal();
        text.Text = "Nota<br />";
        PanelRating.Controls.Add(text);
        for (int index = 1; index <= 5; index++)
        {
            RatingImage ratingimg = new RatingImage("~/Images/steaalba.png");
            ratingimg.Nota = index;
            ratingimg.ID = index.ToString();
            ratingimg.Attributes.Add("onmouseout", "ClearRating()");
            ratingimg.Attributes.Add("onmouseover", "change" + index + "()");
            ratingimg.Click += new ImageClickEventHandler(GetNota);
            PanelRating.Controls.Add(ratingimg);
        }
    }

    public bool ProfilProfesor()
    {
        string Profilname = Request.QueryString["Nume"];
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("Select Tip from Useri where Id = " + GetUserId(Profilname), con);
        string tip = (string)cmd.ExecuteScalar();
        if (tip.Equals("profesor"))
        {
            con.Close();
            return true;
        }
        con.Close();
        return false;
    }

    public bool VerifyNota(int UserId, string CursId)
    {
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("Select * from Reviewuri where UserId = " + UserId + "and CursId =" + CursId + "and Nota is not NULL", con);

        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            con.Close();
            return true;
        }
        con.Close();
        return false;
    }

    public void GetNota(object sender, EventArgs e)
    {
        RatingImage NrStele = (RatingImage)sender;
        int Nota = NrStele.Nota;
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();
        string CursId = Request.QueryString["Curs"];
        SqlCommand cmd = new SqlCommand("insert into Reviewuri (CursId,Nota,UserId) values(" + CursId+ "," + Nota + "," + GetUserId(username) + ")", con);
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Redirect(Request.RawUrl);
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();       
        string CursId = Request.QueryString["Curs"];
        SqlCommand cmd = new SqlCommand("insert into Reviewuri (CursId,Text,UserId) values(" + CursId  + ",'" + TextBox1.Text + "'," + GetUserId(username) + ")", con);
        System.Diagnostics.Debug.WriteLine(cmd.CommandText);
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Redirect(Request.RawUrl);
    }
}