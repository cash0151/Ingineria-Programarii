using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

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
}