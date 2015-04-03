using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Threading;

public partial class WebForms_ProfilePage : System.Web.UI.Page
{   
    SqlConnection con ;
    string MyUserName;    

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadSesssion();
        try
        {
            con = DbConnection.GetSqlConnection();
            con.Open();
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
            MyUserName = appData.Utilizator;
        }
        else
        {
            MyUserName = "";
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
            if (!VerifyNota(GetUserId(MyUserName), UserId))
            {
                LoadRating();
            }
        }
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

    


    public int GetUserId(string Name)
    {
        SqlCommand cmd = new SqlCommand("select id from useri where Nume = '" + Name + "'", con);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int id = (int)reader.GetInt32(0);
        reader.Close();
        return id;
    }





    public bool IsOtherProfil()
    {
        if (MyUserName.Equals(""))
            return true;
        else
        {
            string Profilname = Request.QueryString["Nume"];
            if (GetUserId(Profilname) != GetUserId(MyUserName))
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
       SqlCommand cmd = new SqlCommand("insert into Reviewuri (ProfesorId,Nota,UserId) values(" + GetUserId(Profilname) + "," + Nota + "," + GetUserId(MyUserName) + ")", con);
       cmd.ExecuteNonQuery();
       con.Close();
       Response.Redirect(Request.RawUrl);
    }



    protected void Button4_Click(object sender, EventArgs e)
    {       
        con.Open();
        string Profilname = Request.QueryString["Nume"];
        SqlCommand cmd = new SqlCommand("insert into Reviewuri (ProfesorId,Text,UserId) values(" + GetUserId(Profilname) + ",'" + TextBox1.Text + "'," + GetUserId(MyUserName) + ")", con);
        System.Diagnostics.Debug.WriteLine(cmd.CommandText);
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
        SqlCommand cmd = new SqlCommand("Select * from Reviewuri where UserId = " + UserId + "and ProfesorId =" + ProfesorId, con);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            
            return true;
        }
        return false;
    }

}