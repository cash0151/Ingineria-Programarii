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
    String utilizator;

    protected void Page_Init(object sender, EventArgs e)
    {
 
        Object o;
        SqlDataReader r = null;

        if (Request.QueryString["Curs"] != null)
        {
         
            SqlConnection conn = DbConnection.GetSqlConnection();
            conn.Open();

            SqlCommand c = new SqlCommand("SELECT Continut,Locatie,Program,(SELECT nume FROM useri WHERE id=Profesor) \"prof\"  FROM cursuri  WHERE id=(Select id FROM cursuri WHERE NumeCurs=@NumeCurs )", conn);
            c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
            c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
            r=c.ExecuteReader();
            bool existaCursul = false;
            while (r.Read())
            {
                existaCursul = true;
                divContent1.InnerHtml = (String)r["Continut"];
                divContent2.InnerHtml = (String)r["Locatie"];
                divContent3.InnerHtml = (String)r["Program"];
                //zic numele profesorului
                HyperLink1.Text = (String)r["prof"];
                HyperLink1.NavigateUrl= "ProfilePage.aspx?Nume=" + (String)r["prof"];
            }
            r.Close();
            if (existaCursul == false)
            {
                //in cazul in care cursul nu exista ma intorc pe pagina principala
                Response.Redirect("Home.aspx");
            }
            //Scriu ce titlu are cursul
            titluCurs.Text = Request.QueryString["Curs"];

            //afisez revieweurile cursului
            c = new SqlCommand("SELECT (SELECT u.nume FROM useri u WHERE u.id=r.UserId) \"nume\",r.text FROM reviewuri r WHERE CursId=(SELECT id FROM cursuri WHERE numeCurs=@NumeCurs) AND r.Text IS NOT NULL", conn);
            c.Parameters.Add(new SqlParameter("@NumeCurs", TypeCode.String));
            c.Parameters["@NumeCurs"].Value = Request.QueryString["Curs"];
            r = c.ExecuteReader();
            while (r.Read())
            {
                HtmlGenericControl divcontrol = new HtmlGenericControl();
                divcontrol.Attributes["class"] = "reviewCurs";
                divcontrol.TagName = "div";
                divcontrol.InnerHtml = "Utilizator:" + (String)r["nume"] + 
                    "<br/>" + (String)r["text"];
                Panel1.Controls.Add(divcontrol);
            }
            
            if (Session["login"] != null)
            {
                utilizator = ((AppData)Session["login"]).Utilizator;
                if (EsteInscris(conn))
                {
                    TextBox1.Visible = true;
                    Button4.Visible = true;
                    if ( !VerifyNota(GetUserId(GetUsername()),GetIdCurs(conn).ToString()))
                    {
                        LoadRating();
                    }

                }
                else
                {
                    
                }
            }
            else
            {
                utilizator = null;
                //ascund recomandarile daca nu e logat
                div4.InnerHtml = "";
                div5.InnerHtml = "";
            }

            if (utilizator != null)
            {
                c = new SqlCommand("SELECT id from Useri WHERE Nume=@NumeUser", conn);
                c.Parameters.Add(new SqlParameter("@NumeUser", TypeCode.String));
                c.Parameters["@NumeUser"].Value = utilizator;
                o = (object)c.ExecuteScalar();
                int idUser = Convert.ToInt32(o);

                //recomandari dinamice in functie de ce cursuri este inscris utilizatorul curent.Ia categoria cu cele mai multe cursuri in care
                //este inscris userul curent si din acea categorie ofera primele primele 2 in functie de review si in care nu este inscris userul
                //Daca nu gaseste nici un curs incearca in categoria de pe locul 2. si tot asa pana gaseste 2 cursuri.
                creazaRecomandariDinIstoric(idUser);

                //recomandari statice in functie de cursul pe care il vizioneaza utilizatorul.
                creazaRecomandariStatice(Request.QueryString["Curs"],idUser);

                //recomandari dinamice in functie de cursul in care te afli
                //aici se incheie recomandarile



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
                string status = null;
                while (r.Read())
                {
                    status = r["Status"].ToString();
                }
                if (status == "ACTIVE")
                {
                    registerToThisCourse.Visible = false;
                    deleteFromThisCourse.Visible = true;
                }
                else
                {
                    registerToThisCourse.Visible = false;
                    deleteFromThisCourse.Visible = true;
                    deleteFromThisCourse.Text = "Anuleaza cererea de inscriere trimisa";
                }
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
                //registeredUsers.Visible = true;
                registeredUsers.Visible = false;

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

        conn.Close();

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

        conn.Close();

        Response.Redirect(Request.RawUrl);

    }

    public bool EsteInscris(SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand("Select * from participanti where IdUser = " +GetUserId(GetUsername()) + " and IdCurs = " + GetIdCurs(con) +" and Status = 'ACTIVE'", con);
        System.Diagnostics.Debug.WriteLine(cmd.CommandText);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
            return true;
        return false;            
    }

    public string GetUsername()
    {
        AppData app = (AppData)Session["login"];
        return app.Utilizator;
    }

    int GetIdCurs(SqlConnection con)
    {
        string NumeCurs = Request.QueryString["Curs"];
        SqlCommand cmd = new SqlCommand("Select Id from cursuri where NumeCurs = '" + NumeCurs + "'", con);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int id = (int)reader.GetInt32(0);
        reader.Close();       
        return id;
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
        SqlCommand cmd = new SqlCommand("Select * from Reviewuri where UserId = " + UserId + " and CursId =" + CursId + " and Nota is not NULL", con);
        System.Diagnostics.Debug.WriteLine(cmd.CommandText);
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
        int CursId = GetIdCurs(con);
        SqlCommand cmd = new SqlCommand("insert into Reviewuri (CursId,Nota,UserId) values(" + CursId+ "," + Nota + "," + GetUserId(GetUsername()) + ")", con);
        System.Diagnostics.Debug.WriteLine(cmd.CommandText);
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Redirect(Request.RawUrl);
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();
        int CursId = GetIdCurs(con);
        SqlCommand cmd = new SqlCommand("insert into Reviewuri (CursId,Text,UserId) values(" + CursId  + ",'" + TextBox1.Text + "'," + GetUserId(utilizator) + ")", con);
        System.Diagnostics.Debug.WriteLine(cmd.CommandText);
        cmd.ExecuteNonQuery();
        con.Close();

        Response.Redirect(Request.RawUrl);
    }
    public void creazaRecomandariDinIstoric(int idUser)
    {
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();
        SqlCommand c = new SqlCommand("SELECT ca.id \"idul\" FROM Categorii_Cursuri ca,Cursuri cu,Participanti p WHERE ca.id=cu.Categorie AND cu.id=p.IdCurs AND p.IdUser=@idUserCurent GROUP BY ca.id ORDER BY COUNT(cu.id) DESC", con);
        c.Parameters.Add(new SqlParameter("@idUserCurent", TypeCode.Int32));
        c.Parameters["@idUserCurent"].Value = idUser;
          SqlDataReader r= c.ExecuteReader();
        List<Int32> categoriiPreferate=new List<Int32>();  
        while (r.Read())
          {
              categoriiPreferate.Add((Int32)r["idul"]);
          }
        int nrRezultate = 0;
       // div5.InnerHtml = "";
        for (int i = 0; i < categoriiPreferate.Count; i++)
        {
            c = new SqlCommand("SELECT cu.numeCurs,AVG(nota) \"nota\" FROM cursuri cu LEFT OUTER JOIN Reviewuri r ON r.CursId=cu.id  WHERE  cu.Categorie=@idCategorie   AND NOT EXISTS(SELECT '1' FROM Participanti WHERE IdCurs=cu.id AND IdUser=@idUserCurent) GROUP BY cu.numeCurs ORDER BY AVG(Nota) DESC", con);
          c.Parameters.Add(new SqlParameter("@idUserCurent", TypeCode.Int32));
          c.Parameters["@idUserCurent"].Value = idUser;
          c.Parameters.Add(new SqlParameter("@idCategorie ", TypeCode.Int32));
          c.Parameters["@idCategorie "].Value = categoriiPreferate[i];
          r = c.ExecuteReader();
          
          while (r.Read() && nrRezultate<4)
          {
              nrRezultate++;
              div5.InnerHtml += "<a class=\"ElementeCategorie\" href=\"Courses.aspx?Curs=" + (String)r["numeCurs"] + "\">" + (String)r["numeCurs"] + "</a>Nota:" + r["nota"] + "<br/>";
           
          }
          if (nrRezultate >= 4) break;
        }
        if (nrRezultate == 0) div5.InnerHtml = "";
            con.Close();
        
    }
    public void creazaRecomandariStatice(String curs,int idUser)
    {
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();
        SqlCommand c;

        c = new SqlCommand("SELECT cu.numeCurs,AVG(nota) \"nota\" FROM cursuri cu LEFT OUTER JOIN Reviewuri r ON r.CursId=cu.id  WHERE cu.numeCurs<>@numeCurs AND cu.Categorie=(SELECT Categorie FROM cursuri WHERE numeCurs=@numeCurs)   AND NOT EXISTS(SELECT '1' FROM Participanti WHERE IdCurs=cu.id AND IdUser=@idUserCurent) GROUP BY cu.numeCurs ORDER BY AVG(Nota) DESC", con);
        c.Parameters.Add(new SqlParameter("@idUserCurent", TypeCode.Int32));
        c.Parameters["@idUserCurent"].Value = idUser;
        c.Parameters.Add(new SqlParameter("@numeCurs", TypeCode.String));
        c.Parameters["@numeCurs"].Value = curs;
        SqlDataReader r = c.ExecuteReader();
        int nrRezultate = 0;
       // div4.InnerHtml = "";
        while (r.Read() && nrRezultate < 4)
        {
            nrRezultate++;
          

             div4.InnerHtml += "<a class=\"ElementeCategorie\" href=\"Courses.aspx?Curs=" + (String)r["numeCurs"] + "\">" + (String)r["numeCurs"] + "</a> Nota:" + r["nota"] + "<br/>";
        }
        if (nrRezultate == 0) div4.InnerHtml = "";
        con.Close();
    }
}