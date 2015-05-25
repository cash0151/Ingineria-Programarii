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
    protected void Page_Init(object sender, EventArgs e)
    {

        Object o;

        if (Request.QueryString["Curs"] != null)
        {
            SqlConnection conn = DbConnection.GetSqlConnection();
            conn.Open();

            SqlCommand c = new SqlCommand("SELECT Continut,Locatie,Program FROM cursuri WHERE id=(Select id FROM cursuri WHERE NumeCurs=@NumeCurs )", conn);
            c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
            c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
            SqlDataReader r = null;
            r=c.ExecuteReader();
            bool existaCursul = false;
            while (r.Read())
            {
                existaCursul = true;
                divContent1.InnerHtml = (String)r["Continut"];
                divContent2.InnerHtml = (String)r["Locatie"];
                divContent3.InnerHtml = (String)r["Program"];
            }
            r.Close();
            if (existaCursul == false)
            {
                //in cazul in care cursul nu exista ma intorc pe pagina principala
                Response.Redirect("Home.aspx");
            }
            //Scriu ce titlu are cursul
            titluCurs.Text = Request.QueryString["Curs"];

            String utilizator;
            if (Session["login"] != null)
            {
                utilizator = ((AppData)Session["login"]).Utilizator;
            }
            else
            {
                utilizator = null;
            }

          
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
        else
        {
            //in cazul in care query stringul este gol ma intorc pe pagina principala
            Response.Redirect("Home.aspx");
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
}