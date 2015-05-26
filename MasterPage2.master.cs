using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class MasterPage2 : System.Web.UI.MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        LinkButton1.Click += new EventHandler(Button1_Click);
        LinkButton2.Click += new EventHandler(Button1_Click2);
        LinkButton3.Click += new EventHandler(Button1_Click3);
        //daca nu sunt logat sa nu afisez log out si nici butonul de profil
        if (Session["login"] == null)
        {
            LinkButton1.Visible = false;
            Button4.Visible = false;
            notificationsBlock.Visible = false;
        }
        else
        {
            //daca sunt deja logat sa nu afisez login si sign up
            LinkButton2.Visible = false;
            LinkButton3.Visible = false;
            notificationsBlock.Visible = true;

            checkForNotifications();
        }
    }

    protected void checkForNotifications()
    {
        if (Session["login"] != null)
        {
            String utilizator = ((AppData)Session["login"]).Utilizator;

            SqlConnection conn = DbConnection.GetSqlConnection();
            conn.Open();

            //selectam id-ul profesorului
            SqlCommand c = new SqlCommand("SELECT id from Useri WHERE Nume=@NumeUser", conn);
            c.Parameters.Add(new SqlParameter("@NumeUser", TypeCode.String));
            c.Parameters["@NumeUser"].Value = utilizator;
            object o = (object)c.ExecuteScalar();
            int idUser = Convert.ToInt32(o);

            //selectam id-urile cursurilor predate de profesorul respectiv
            c = new SqlCommand("SELECT * from Cursuri WHERE Profesor=@idProfesor", conn);
            c.Parameters.Add(new SqlParameter("@idProfesor", TypeCode.Int32));
            c.Parameters["@idProfesor"].Value = idUser;
            SqlDataReader r = c.ExecuteReader();

            //pentru fiecare curs predat de profesorul respectiv, verificam daca exista oameni ce doresc sa participe la el
            if (r.HasRows)
            {
                String notificare = "";
                while (r.Read())
                {
                    SqlCommand d = new SqlCommand("SELECT * from Participanti WHERE Vazut='NOT_SEEN' AND Status='PENDING' AND IdCurs=" + r["Id"], conn);
                    SqlDataReader s = d.ExecuteReader();

                    //daca exista doritori, afisam notificare profesorului
                    while (s.Read())
                    {
                        SqlCommand pendingUsers = new SqlCommand("SELECT Nume from Useri WHERE Id=" + s["IdUser"], conn);
                        SqlDataReader t = pendingUsers.ExecuteReader();
                        while (t.Read())
                        {
                            notificare += "User-ul '" + t["Nume"] + "' solicita inscrierea la cursul <a href='/WebForms/Courses.aspx?Curs=" + r["NumeCurs"] + "'>" + r["NumeCurs"] + "</a><br/>";
                        }
                    }
                }
                Label1.InnerHtml = notificare;
            }
            else
            {
                Label1.InnerHtml = "Nu aveti notificari!";
            }
            conn.Close();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["login"] = null;
        Response.Redirect(Request.RawUrl);

        /*

        Button1.Visible = false;
        Button2.Visible = true;
        Button3.Visible = true;
         */
    }
    protected void Button1_Click2(object sender, EventArgs e)
    {
        Response.Redirect("/WebForms/login.aspx");
    }
    protected void Button1_Click3(object sender, EventArgs e)
    {
        Response.Redirect("/WebForms/Sign Up.aspx");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        String username = ((AppData)Session["login"]).Utilizator;
        Response.Redirect("/WebForms/ProfilePage.aspx?Nume=" + username);
    }
}
