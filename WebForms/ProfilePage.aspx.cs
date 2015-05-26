using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Web.UI.HtmlControls;

public partial class WebForms_ProfilePage : System.Web.UI.Page
{   
    SqlConnection con ;
    string username;
    SqlCommand c;
    protected void Page_Init(object sender, EventArgs e)
    {
        LoadSesssion();
        try
        {
            con = DbConnection.GetSqlConnection();
            con.Open();

            Label1.Text = "";
      
            //daca nu este logat nu afisez butonul de adaugat traininguri
            if (Session["login"] == null)

            {
                divPreferinte.Visible = false;
                selectPreferinta.Visible = false;
                Button5.Visible = false;
                TabelCursuriLaCareSuntInscris.Visible = false;
                oameniInscrisiLaCursurileMele.Visible = false;
            }
            else
            {
                username = ((AppData)Session["login"]).Utilizator;
                //daca queryul nu este invalid spun asta
                if (Request.QueryString["Nume"] == null)
                {
                    Label1.Text = "Queryul nu este valid";
                    Button5.Visible = false;
                    con.Close();
                    Response.Redirect("Home.aspx");
                }
                //verific daca exista profilul in baza de date
                c = new SqlCommand("SELECT '1' FROM useri WHERE nume=@nume", con);
                c.Parameters.Add(new SqlParameter("@nume", TypeCode.String));
                c.Parameters["@nume"].Value = Request.QueryString["Nume"];
                Object o = c.ExecuteScalar();
                if (o == null)
                {
                    con.Close();
                    Response.Redirect("Home.aspx");
                }
           


                //pun numele
                h1.InnerHtml = Request.QueryString["Nume"];

                //doar daca userul curent este profilul lui si este profesor 
                //arat butonul de creare de trainguri.
                //prima data verific daca userul curent este pe profilul lui
                if (!Request.QueryString["Nume"].Equals(username))
                {
                    Button5.Visible = false;

                    divPreferinte.Visible = false;

                    TabelCursuriLaCareSuntInscris.Visible = false;
                    oameniInscrisiLaCursurileMele.Visible = false;

                }
                else
                {
                    //daca nu este profesor nu afisez butonul de adaugat traininguri
                    if (esteProfesor(username) == false)
                    {
                        Button5.Visible = false;
                        TabelCursuriLaCareSuntInscris.Visible = true;
                        oameniInscrisiLaCursurileMele.Visible = false;
                        LoadCursuriLaCareSuntInscris();
                    }
                    else
                    {
                        TabelCursuriLaCareSuntInscris.Visible = true;
                        oameniInscrisiLaCursurileMele.Visible = true;
                        LoadCursuriLaCareSuntInscris();
                        LoadPeopleEnrolledToMyCourses();
                    }
                }
                
            }

            //afisez revieweurile cursului
         
            SqlDataReader r;
            c = new SqlCommand("SELECT (SELECT u.nume FROM useri u WHERE u.id=r.UserId) \"nume\",r.text FROM reviewuri r WHERE ProfesorId=(SELECT id FROM useri WHERE Nume=@Nume) AND r.Text IS NOT NULL", con);
            c.Parameters.Add(new SqlParameter("Nume", TypeCode.String));
            c.Parameters["Nume"].Value = Request.QueryString["Nume"];
            r = c.ExecuteReader();
            while (r.Read())
            {
                HtmlGenericControl divcontrol = new HtmlGenericControl();
                divcontrol.Attributes["class"] = "reviewCurs";
                divcontrol.TagName = "div";
                divcontrol.InnerHtml ="Utilizator:"+ (String)r["nume"] +
                    "<br/>" + (String)r["text"];
                Panel1.Controls.Add(divcontrol);
            }


            LoadTraining();
        
        }
        catch{ }
        finally
        {
           con.Close();
        }               
    }



    public void LoadSesssion()
    {
        if (Session["login"] != null)
        {
            AppData appData = Session["login"] as AppData;
            username = appData.Utilizator;

            /***/
            fillTableProfiSelecati();
            fillTableCursuriNeparticipate();
            fillTablePerechi();
            /***/
        }
        else
        {
            username = "";
            TextBox1.Visible = false;
            Button4.Visible = false;
        }
    }



    public void LoadTraining()
    {
        string Profilname = Request.QueryString["Nume"];
        int UserId = GetUserId(Profilname);
        bool OtherProfil = IsOtherProfil();

        SqlDataReader reader = GetTraining(UserId);
        TableRow row1 = TabelCursuri.Rows[0];
        TabelCursuri.Rows.Clear();
        TabelCursuri.Rows.Add(row1);

        while (reader.Read())
        {
            TableRow row = new TableRow();

            TableCell TrainingCell = new TableCell();
            LinkButton Curs = new LinkButton();
            Curs.Text = (string)reader["NumeCurs"];
            Curs.Click += new EventHandler(ViewCurs);
            TrainingCell.Controls.Add(Curs);
            row.Cells.Add(TrainingCell);

            if (OtherProfil == false)
            {
                TableCell DeleteTrainingCell = new TableCell();
                ButtonTraining Delete = new ButtonTraining();
                Delete.Text = "Sterge";
                Delete.Training = (string)reader["NumeCurs"];
                Delete.Click += new EventHandler(StergeCursul);
                DeleteTrainingCell.Controls.Add(Delete);
                row.Cells.Add(DeleteTrainingCell);
            }
            
            TabelCursuri.Rows.Add(row);
        }
        reader.Close();
        if (ProfilProfesor())
        {
            if (!VerifyNota(GetUserId(username), UserId))
            {
                if (ApartineProfesorului())
                {
                    LoadRating();
                }
                else
                {
                    TextBox1.Visible = false;
                    Button4.Visible = false;
                }
            }
        }
    }

    public void LoadCursuriLaCareSuntInscris()
    {
        string Profilname = Request["Nume"];
        int UserId = GetUserId(Profilname);
        bool OtherProfil = IsOtherProfil();

        SqlCommand c;
        SqlDataReader r;
        string numeCurs = null;
        
        if (OtherProfil == false)
        {
            c = new SqlCommand("SELECT * FROM Participanti WHERE IdUser=@IdUser AND Status IN ('ACTIVE', 'PENDING')", con);
            c.Parameters.Add(new SqlParameter("@idUser", TypeCode.Int32));
            c.Parameters["@idUser"].Value = UserId;
            r = c.ExecuteReader();

            if (r.HasRows)
            {
                while (r.Read())
                {
                    numeCurs = getCourseNameById(r["idCurs"].ToString());

                    TableRow row = new TableRow();

                    TableCell TrainingCell = new TableCell();
                    LinkButton Curs = new LinkButton();
                    Curs.Text = numeCurs;
                    Curs.Click += new EventHandler(ViewCurs);
                    TrainingCell.Controls.Add(Curs);
                    row.Cells.Add(TrainingCell);

                    TableCell DeleteTrainingCell = new TableCell();
                    ButtonTraining Delete = new ButtonTraining();
                    if (r["Status"].ToString() == "ACTIVE")
                    {
                        Delete.Text = "Sterge-ma de la acest curs";
                    }
                    else
                    {
                        Delete.Text = "Cerere trimisa. Anuleaza cererea";
                    }
                    Delete.Training = r["IdCurs"] + "*" + UserId.ToString();
                    Delete.Click += new EventHandler(deleteMeFromThisCourse);
                    DeleteTrainingCell.Controls.Add(Delete);
                    row.Cells.Add(DeleteTrainingCell);

                    TabelCursuriLaCareSuntInscris.Rows.Add(row);
                }
            }
            else
            {
                TableRow row = new TableRow();

                TableCell notRegisteredToAnyCourse = new TableCell();
                notRegisteredToAnyCourse.Text = "Nu sunteti inregistrat la nici un curs!";
                row.Cells.Add(notRegisteredToAnyCourse);

                TabelCursuriLaCareSuntInscris.Rows.Add(row);
            }
        }
    }

    public void LoadPeopleEnrolledToMyCourses()
    {
        string Profilname = Request["Nume"];
        int UserId = GetUserId(Profilname);
        bool isTeacher = esteProfesor(Profilname);
        bool OtherProfil = IsOtherProfil();
        List<int> teacherCoursesList = new List<int>();

        SqlCommand c;
        SqlDataReader r, s;
        string numeCurs = null;

        if (OtherProfil == false && isTeacher == true)
        {
            c = new SqlCommand("SELECT * FROM Cursuri WHERE Profesor=@IdProfesor", con);
            c.Parameters.Add(new SqlParameter("@IdProfesor", TypeCode.Int32));
            c.Parameters["@IdProfesor"].Value = UserId;
            r = c.ExecuteReader();

            while (r.Read())
            {
                teacherCoursesList.Add(Int32.Parse(r["Id"].ToString()));
            }
            int[] teacherCoursesArray = teacherCoursesList.ToArray();

            c = new SqlCommand("SELECT * FROM Participanti WHERE IdCurs IN (" + string.Join(",", teacherCoursesArray) + ")", con);
            r = c.ExecuteReader();

            TableHeaderRow headerRow = new TableHeaderRow();
            headerRow.BackColor = System.Drawing.ColorTranslator.FromHtml("Aqua");

            TableCell CourseNameCell = new TableCell();
            CourseNameCell.Text = "Nume curs";
            headerRow.Cells.Add(CourseNameCell);

            TableCell UserNameCell = new TableCell();
            UserNameCell.Text = "Nume user";
            headerRow.Cells.Add(UserNameCell);

            TableCell ActionCell = new TableCell();
            ActionCell.Text = "Actiuni";
            headerRow.Cells.Add(ActionCell);

            oameniInscrisiLaCursurileMele.Rows.Add(headerRow);

            if (r.HasRows)
            {
                while (r.Read())
                {
                    TableRow row = new TableRow();

                    TableCell TrainingCell = new TableCell();
                    LinkButton Curs = new LinkButton();
                    Curs.Text = getCourseNameById(r["IdCurs"].ToString());
                    Curs.Click += new EventHandler(ViewCurs);
                    TrainingCell.Controls.Add(Curs);
                    row.Cells.Add(TrainingCell);

                    TableCell UserCell = new TableCell();
                    UserCell.Text = getUsernameById(r["IdUser"].ToString());
                    row.Cells.Add(UserCell);

                    TableCell DeleteCell = new TableCell();
                    ButtonTraining Delete = new ButtonTraining();
                    if (r["Status"].ToString() == "ACTIVE")
                    {
                        Delete.Text = "Sterge";
                    }
                    else if (r["Status"].ToString() == "PENDING")
                    {
                        Delete.Text = "Anuleaza cererea primita de la utilizator";
                    }
                    Delete.Training = "/WebForms/DeclineCourseRegistrationPage.aspx?idDoritor=" + r["IdUser"].ToString() + "&idCurs=" + r["idCurs"];
                    Delete.Click += new EventHandler(goToDeletePage);
                    DeleteCell.Controls.Add(Delete);
                    row.Cells.Add(DeleteCell);

                    oameniInscrisiLaCursurileMele.Rows.Add(row);
                }
            }
            else
            {
                TableRow row = new TableRow();

                TableCell notRegisteredToAnyCourse = new TableCell();
                notRegisteredToAnyCourse.Text = "Nu aveti nici o persoana inregistrata la vreun curs!";
                row.Cells.Add(notRegisteredToAnyCourse);

                oameniInscrisiLaCursurileMele.Rows.Add(row);
            }
        }
    }

    public void deleteMeFromThisCourse(object sender, EventArgs e)
    {
        ButtonTraining Delete = (ButtonTraining)sender;
        string[] courseAndUserIds = Delete.Training.Split('*');

        SqlConnection conn = DbConnection.GetSqlConnection();
        conn.Open();
        SqlCommand c = new SqlCommand("DELETE FROM Participanti WHERE IdCurs=@idCurs AND IdUser=@idUser", conn);
        c.Parameters.Add(new SqlParameter("@idCurs", TypeCode.Int32));
        c.Parameters["@idCurs"].Value = Int32.Parse(courseAndUserIds[0]);
        c.Parameters.Add(new SqlParameter("@idUser", TypeCode.Int32));
        c.Parameters["@idUser"].Value = Int32.Parse(courseAndUserIds[1]);
        c.ExecuteReader();

        conn.Close();

        Response.Redirect(Request.RawUrl);
    }

    public string getCourseNameById(string courseId)
    {
        SqlCommand c;
        SqlDataReader r;
        string numeCurs = null;

        c = new SqlCommand("SELECT NumeCurs FROM Cursuri WHERE Id=@IdCurs", con);
        c.Parameters.Add(new SqlParameter("@idCurs", TypeCode.Int32));
        c.Parameters["@idCurs"].Value = courseId;
        r = c.ExecuteReader();

        while (r.Read())
        {
            numeCurs = r["NumeCurs"].ToString();
        }

        return numeCurs;
    }

    public string getUsernameById(string userId)
    {
        SqlCommand c;
        SqlDataReader r;
        string numeUser = null;

        c = new SqlCommand("SELECT Nume FROM Useri WHERE Id=@IdUser", con);
        c.Parameters.Add(new SqlParameter("@IdUser", TypeCode.Int32));
        c.Parameters["@IdUser"].Value = userId;
        r = c.ExecuteReader();

        while (r.Read())
        {
            numeUser = r["Nume"].ToString();
        }

        return numeUser;
    }

    public void goToDeletePage(object sender, EventArgs e)
    {
        ButtonTraining Delete = (ButtonTraining)sender;
        Response.Redirect(Delete.Training);
    }

    public string GetUsername()
    {
        AppData app = (AppData)Session["login"];
        return app.Utilizator;
    }

    public bool ApartineProfesorului()
    {
        string Profilname = Request.QueryString["Nume"];
        int UserId = GetUserId(Profilname);
        int myId = GetUserId(GetUsername());
        string SqlText = "select * from participanti p join Cursuri c on p.IdCurs = c.Id and c.Profesor = " + UserId + " and p.Status='ACTIVE' and p.IdUser ="+myId;
        SqlCommand cmd = new SqlCommand(SqlText,con);
        SqlDataReader reader = cmd.ExecuteReader();
        if(reader.Read())
            return true;
        return false;         
    }


    public void ViewCurs(object sender, EventArgs e)
    {
        LinkButton Curs = (LinkButton)sender;
        Response.Redirect("Courses.aspx?Curs=" + Curs.Text);
    }

    public void StergeCursul(object sender, EventArgs e)
    {
        ButtonTraining Delete = (ButtonTraining)sender;
        try
        {
            con = DbConnection.GetSqlConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Cursuri where NumeCurs ='" + Delete.Training + "'", con);
            cmd.ExecuteNonQuery();
        }
        catch { }
        finally
        {
            con.Close();
        }
        Response.Redirect(Request.RawUrl);
    }

    


    public SqlDataReader GetTraining(int UserId)
    {   
        SqlCommand cmd = new SqlCommand("Select * from Cursuri where Profesor = " + UserId, con);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    


    /******/
    public int GetUserId(string Name)
    {
        SqlConnection coection = DbConnection.GetSqlConnection();
        SqlCommand cmd = new SqlCommand("select id from useri where Nume = '" + Name + "'", coection);
        coection.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int id = (int)reader.GetInt32(0);
        reader.Close();

        coection.Close();
        return id;
    }
    /******/

    protected void Button5_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCourse.aspx");
    }

    public bool esteProfesor(String user)
    {
        SqlCommand c = new SqlCommand("SELECT '1' FROM useri WHERE nume=@nume AND tip='profesor'", con);
        c.Parameters.Add(new SqlParameter("@nume", TypeCode.String));
        c.Parameters["@nume"].Value = user;
        Object o = (object)c.ExecuteScalar();
        if (o == null) return false;
        else return true;
    }

    public bool IsOtherProfil()
    {
        if (username.Equals(""))
            return true;
        else
        {
            string Profilname = Request.QueryString["Nume"];
            if (GetUserId(Profilname) != GetUserId(username))
                return true;
            return false;
        }
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
            ratingimg.Attributes.Add("onmouseover", "change"+index+"()");
            ratingimg.Click += new ImageClickEventHandler(GetNota);
            PanelRating.Controls.Add(ratingimg);
        }
    }




    public void GetNota(object sender, EventArgs e)
    {
       RatingImage NrStele = (RatingImage)sender;
       int Nota = NrStele.Nota;
       con.Open();
       string Profilname = Request.QueryString["Nume"];
       SqlCommand cmd = new SqlCommand("insert into Reviewuri (ProfesorId,Nota,UserId) values(" + GetUserId(Profilname) + "," + Nota + "," + GetUserId(username) + ")", con);
       cmd.ExecuteNonQuery();
       con.Close();
       Response.Redirect(Request.RawUrl);
    }



    protected void Button4_Click(object sender, EventArgs e)
    {       
        con.Open();
        string Profilname = Request.QueryString["Nume"];
        SqlCommand cmd = new SqlCommand("insert into Reviewuri (ProfesorId,Text,UserId) values(" + GetUserId(Profilname) + ",'" + TextBox1.Text + "'," + GetUserId(username) + ")", con);
        cmd.ExecuteNonQuery();
        con.Close();
        Response.Redirect(Request.RawUrl);
    }



    public bool ProfilProfesor()
    {   
        string Profilname = Request.QueryString["Nume"];
        SqlCommand cmd = new SqlCommand("Select Tip from Useri where Id = "+ GetUserId(Profilname) , con);        
        string tip = (string)cmd.ExecuteScalar();
        if (tip.Equals("profesor"))
            return true;
        return false;
    }



    public bool VerifyNota(int UserId,int ProfesorId)
    {
        SqlCommand cmd = new SqlCommand("Select * from Reviewuri where UserId = " + UserId + "and ProfesorId =" + ProfesorId + "and Nota is not NULL", con);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            
            return true;
        }
        return false;
    }

    //*******
    protected void selectPreferinta_Click(object sender, EventArgs e)
    {
        String username = ((AppData)Session["login"]).Utilizator;
        Response.Redirect("SetareCategorii.aspx?Nume=" + username);
    }


    private void fillTableProfiSelecati()
    {
        SqlConnection coection = DbConnection.GetSqlConnection();
        coection.Open();
        SqlCommand cmd = new SqlCommand("Select u.Nume,AVG(Nota) From Reviewuri r,Cursuri c,Preferinte p, Useri u Where r.ProfesorId = c.Profesor AND p.Categorie = c.Categorie AND c.Profesor = u.Id And p.IdUser = " + GetUserId(username) + " GROUP BY u.Nume Order BY AVG(Nota) Desc", coection);
        SqlDataReader reader = cmd.ExecuteReader();

        TableRow th = TabelProfCursuriSelectate.Rows[0];
        TabelProfCursuriSelectate.Rows.Clear();
        TabelProfCursuriSelectate.Rows.Add(th);

        while (reader.Read())
        {
            TableRow row = new TableRow();

            TableCell numeProf = new TableCell();
            numeProf.Text = reader.GetValue(0).ToString();
            row.Cells.Add(numeProf);

            TableCell notaCell = new TableCell();
            notaCell.Text = reader.GetValue(1).ToString();
            row.Cells.Add(notaCell);

            TabelProfCursuriSelectate.Rows.Add(row);
        }
        reader.Close();
        coection.Close();
    }

    private void fillTableCursuriNeparticipate()
    {
        SqlConnection coection = DbConnection.GetSqlConnection();
        coection.Open();
        SqlCommand cmd = new SqlCommand("Select  c.NumeCurs,AVG(Nota),c.Id From Reviewuri r,Cursuri c,Preferinte p Where r.CursId = c.Id AND p.Categorie =c.Categorie And p.IdUser = " + GetUserId(username) + " GROUP BY c.NumeCurs,c.Id Order BY AVG(Nota) Desc", coection);
        SqlDataReader reader = cmd.ExecuteReader();

        TableRow th = TableCursuri.Rows[0];
        TableCursuri.Rows.Clear();
        TableCursuri.Rows.Add(th);

        while (reader.Read())
            if (nuParicipa(reader.GetInt32(2)))
            {
            TableRow row = new TableRow();

            TableCell nume = new TableCell();
            nume.Text = reader.GetValue(0).ToString();
            row.Cells.Add(nume);

            TableCell notaCell = new TableCell();
            notaCell.Text = reader.GetValue(1).ToString();
            row.Cells.Add(notaCell);

            TableCursuri.Rows.Add(row);
        }
        reader.Close();
        coection.Close();
    }

    private bool nuParicipa(int idCurs)
    {
        SqlConnection coection = DbConnection.GetSqlConnection();
        coection.Open();
        SqlCommand cmd = new SqlCommand("Select  1 From Participanti p Where p.IdCurs = " + idCurs + " And p.IdUser = " + GetUserId(username), coection);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            reader.Close();
            coection.Close();
            return false;
        }
        reader.Close();
        coection.Close();
        return true;
    }

    private void fillTablePerechi()
    {
        SqlConnection coection = DbConnection.GetSqlConnection();
        coection.Open();
        SqlCommand cmd = new SqlCommand("Select c.NumeCurs,u.Nume ,c.Id,c.Profesor From Cursuri c, Preferinte p , Useri u Where c.Categorie = p.Categorie AND u.Id = c.Profesor And p.IdUser  = " + GetUserId(username) + " GROUP by c.Id,c.Profesor,c.NumeCurs,u.Nume Order by ((Select ISNULL(AVG(r.Nota),-1) From Reviewuri r Where r.CursId = c.Id)+(Select ISNULL(AVG(r.Nota),-1) From Reviewuri r Where r.CursId = c.Profesor))/2 DESC", coection);
        SqlDataReader reader = cmd.ExecuteReader();

        TableRow th = TablePerechi.Rows[0];
        TablePerechi.Rows.Clear();
        TablePerechi.Rows.Add(th);

        while (reader.Read())
            if (nuParicipa(reader.GetInt32(2)))
            {
            TableRow row = new TableRow();

            TableCell nume = new TableCell();
            nume.Text = reader.GetValue(0).ToString();
            row.Cells.Add(nume);

            TableCell notaCell = new TableCell();
            notaCell.Text = reader.GetValue(1).ToString();
            row.Cells.Add(notaCell);

            TablePerechi.Rows.Add(row);
        }
        reader.Close();
        coection.Close();
    }

    //******
}