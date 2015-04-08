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

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.QueryString["Curs"] != null)
        {
            SqlConnection conn = DbConnection.GetSqlConnection();
            conn.Open();
            SqlCommand c = new SqlCommand("SELECT Continut FROM cursuri WHERE id=(Select id FROM cursuri WHERE NumeCurs=@NumeCurs )", conn);
            c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
            c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
            object o = (object)c.ExecuteScalar();
            String continut = (String)o;
         
            divContent1.InnerHtml = continut;

            String utilizator;
            if (Session["login"] != null)
            {
                utilizator = ((AppData)Session["login"]).Utilizator;
            }
            else
            {
                utilizator = null;
            }

            SqlDataReader r = null;
            if (utilizator != null)
            {
                c = new SqlCommand("SELECT id from Useri WHERE Nume=@NumeUser", conn);
                c.Parameters.Add(new SqlParameter("@NumeUser", TypeCode.String));
                c.Parameters["@NumeUser"].Value = utilizator;
                o = (object)c.ExecuteScalar();
                int idUser = Convert.ToInt32(o);

                c = new SqlCommand("SELECT * FROM Participanti WHERE IdUser=@IdUser AND status IN ('ACTIVE', 'PENDING') AND IdCurs = (SELECT id from Cursuri WHERE NumeCurs = @NumeCurs)", conn);
                c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
                c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
                c.Parameters.Add(new SqlParameter("@idUser", TypeCode.Int32));
                c.Parameters["@idUser"].Value = idUser;
                r = c.ExecuteReader();
            }

            if (utilizator != null && r.HasRows == false)
            {
                registerToThisCourse.Visible = true;
                deleteFromThisCourse.Visible = false;
            }
            else if (utilizator != null && r.HasRows == true)
            {
                registerToThisCourse.Visible = false;
                deleteFromThisCourse.Visible = true;
            }
            else
            {
                registerToThisCourse.Visible = false;
                deleteFromThisCourse.Visible = false;
            }

            c = new SqlCommand("SELECT * FROM Participanti WHERE status='ACTIVE' AND IdCurs = (SELECT id from Cursuri WHERE NumeCurs = @NumeCurs)", conn);
            c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
            c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
            r = c.ExecuteReader();
            if (r.HasRows)
            {
                registeredUsers.Visible = true;

                while (r.Read())
                {
                    SqlCommand d = new SqlCommand("SELECT Nume FROM Useri WHERE Id = @idUser", conn);
                    d.Parameters.Add(new SqlParameter("@idUser", TypeCode.String));
                    d.Parameters["@idUser"].Value = r["IdUser"];
                    String numeUser = d.ExecuteScalar().ToString();

                    registeredUsers.InnerHtml = numeUser + "<br/>";
                }
            }
            else
            {
                registeredUsers.Visible = false;
            }
            
            conn.Close();
        }
    }

    protected void registerToThisCourseAction(object sender, EventArgs e)
    {
        SqlConnection conn = DbConnection.GetSqlConnection();
        conn.Open();
        SqlCommand c = new SqlCommand("SELECT id FROM Cursuri WHERE NumeCurs=@NumeCurs", conn);
        c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
        c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
        object o = (object)c.ExecuteScalar();
        int idCurs = Convert.ToInt32(o);

        String utilizator = ((AppData)Session["login"]).Utilizator;
        c = new SqlCommand("SELECT id from Useri WHERE Nume=@NumeUser", conn);
        c.Parameters.Add(new SqlParameter("@NumeUser", TypeCode.String));
        c.Parameters["@NumeUser"].Value = utilizator;
        o = (object)c.ExecuteScalar();
        int idUser = Convert.ToInt32(o);

        c = new SqlCommand("INSERT INTO Participanti(IdCurs, IdUser, Status) VALUES (@idCurs, @idUser, 'PENDING')", conn);
        c.Parameters.Add(new SqlParameter("@idCurs", TypeCode.Int32));
        c.Parameters["@idCurs"].Value = idCurs;
        c.Parameters.Add(new SqlParameter("@idUser", TypeCode.Int32));
        c.Parameters["@idUser"].Value = idUser;
        c.ExecuteReader();

        Response.Redirect(Request.RawUrl);
    }

    protected void deleteFromThisCourseAction(object sender, EventArgs e)
    {
        SqlConnection conn = DbConnection.GetSqlConnection();
        conn.Open();
        SqlCommand c = new SqlCommand("SELECT id FROM Cursuri WHERE NumeCurs=@NumeCurs", conn);
        c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
        c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
        object o = (object)c.ExecuteScalar();
        int idCurs = Convert.ToInt32(o);

        String utilizator = ((AppData)Session["login"]).Utilizator;
        c = new SqlCommand("SELECT id from Useri WHERE Nume=@NumeUser", conn);
        c.Parameters.Add(new SqlParameter("@NumeUser", TypeCode.String));
        c.Parameters["@NumeUser"].Value = utilizator;
        o = (object)c.ExecuteScalar();
        int idUser = Convert.ToInt32(o);

        c = new SqlCommand("DELETE FROM Participanti WHERE IdCurs=@idCurs AND IdUser=@idUser", conn);
        c.Parameters.Add(new SqlParameter("@idCurs", TypeCode.Int32));
        c.Parameters["@idCurs"].Value = idCurs;
        c.Parameters.Add(new SqlParameter("@idUser", TypeCode.Int32));
        c.Parameters["@idUser"].Value = idUser;
        c.ExecuteReader();

        Response.Redirect(Request.RawUrl);
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