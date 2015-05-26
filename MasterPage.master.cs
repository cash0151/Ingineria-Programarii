using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        
        LinkButton1.Click += new EventHandler(Button1_Click);
        LinkButton2.Click += new EventHandler(Button1_Click2);
        LinkButton3.Click += new EventHandler(Button1_Click3);
        LinkButton4.Click += new EventHandler(Button4_Click);
        //daca nu sunt logat sa nu afisez log out si nici butonul de profil
        if (Session["login"] == null)
        {
            LinkButton1.Visible = false;
            LinkButton4.Visible = false;
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
        if (!IsPostBack)
        {
            this.LoadMenu();
        }
    }

    private void LoadMenu()
    {
        DataSet ds = GetDataSetForMenu();
        MenuItem ButtonMeniu = new MenuItem("Cursuri");
        menu.Items.Add(ButtonMeniu);
        
        foreach (DataRow parentItem in ds.Tables["Categorii_Cursuri"].Rows)
        {
            MenuItem categoryItem = new MenuItem((string)parentItem["NumeCategorie"]);
            ButtonMeniu.ChildItems.Add(categoryItem);
            categoryItem.NavigateUrl = "WebForms/CoursesCategories.aspx?Categorie=" + (int)parentItem["Id"];
            
        }
        menu.DataBind();
    }

    private DataSet GetDataSetForMenu()
    {
        SqlConnection myConnection = DbConnection.GetSqlConnection();
        myConnection.Open();
        SqlDataAdapter adCat = new SqlDataAdapter("SELECT * FROM Categorii_Cursuri", myConnection);
        DataSet ds = new DataSet();
        adCat.Fill(ds, "Categorii_Cursuri");
        myConnection.Close();
        return ds;
    }
    protected void HomeButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("/WebForms/Home.aspx");
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
                    if (s.HasRows)
                    {
                        while (s.Read())
                        {
                            SqlCommand pendingUsers = new SqlCommand("SELECT Nume from Useri WHERE Id=" + s["IdUser"], conn);
                            SqlDataReader t = pendingUsers.ExecuteReader();

                            while (t.Read())
                            {
                                notificare += "User-ul '" + t["Nume"] + "' solicita inscrierea la cursul <a href='/WebForms/Courses.aspx?Curs=" + r["NumeCurs"] + "'>" + r["NumeCurs"] + "</a>&nbsp;<a href='/WebForms/AcceptCourseRegistrationPage.aspx?idDoritor=" + s["IdUser"].ToString() + "&idCurs=" + r["id"] + "'>Accept</a>&nbsp;<a href='/WebForms/DeclineCourseRegistrationPage.aspx?idDoritor=" + s["IdUser"].ToString() + "&idCurs=" + r["id"] + "'>Decline</a><br/>";
                            }
                        }
                    }
                    else
                    {
                        Label1.InnerHtml = "Nu aveti notificari!";
                    }
                }
                if (notificare.Equals("") == false)
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
        String username=((AppData) Session["login"]).Utilizator;
        Response.Redirect("/WebForms/ProfilePage.aspx?Nume="+username);
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        String query = TextBox1.Text;
        if(!query.Equals(""))
            Response.Redirect("/WebForms/SearchResult.aspx?query=" + query + "");
    }
}
