using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class WebForms_SetareCategorii : System.Web.UI.Page
{
    SqlConnection con;
    string username;
    int userId;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            con = DbConnection.GetSqlConnection();
            con.Open();
            if (Session["login"] != null)
            {
                username = ((AppData)Session["login"]).Utilizator;
                userId = GetUserId(username);
                fillTableCategoriiSelecate();
                fillTableAlteCategorii();
            }
        }
        catch { }
        finally
        {
            con.Close();
        }
    }

    private void fillTableAlteCategorii()
    {
        SqlCommand cmd = new SqlCommand("Select * From Categorii_Cursuri", con);
        SqlDataReader reader = cmd.ExecuteReader();

        TableRow th = TabelPreferintePosibile.Rows[0];
        TabelPreferintePosibile.Rows.Clear();
        TabelPreferintePosibile.Rows.Add(th);

        while (reader.Read())
        {
            if (hasNotSelected(reader.GetInt32(0)) == true)
            {

            TableRow row = new TableRow();

            TableCell numeCategorie = new TableCell();
            // Label1.Text = ((int)reader.GetInt32(0)).ToString();
            numeCategorie.Text = reader.GetValue(1).ToString();
            //Label1.Text = numeCategorie.Text;
            row.Cells.Add(numeCategorie);

            TableCell AdaugaPreferintaCell = new TableCell();
            IntInfoBut Adauga = new IntInfoBut();
            Adauga.Text = "Adauga Preferinta";
            Adauga.Info = (int)reader.GetInt32(0);
            Adauga.Click += new EventHandler(AdaugaCursul);
            AdaugaPreferintaCell.Controls.Add(Adauga);
            row.Cells.Add(AdaugaPreferintaCell);

            TabelPreferintePosibile.Rows.Add(row);
            }
        }
        reader.Close();

    }

    private void AdaugaCursul(object sender, EventArgs e)
    {

         IntInfoBut Adauga = (IntInfoBut)sender;
        try
        {
            con = DbConnection.GetSqlConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Preferinte VALUES( "+userId+" , "+ Adauga.Info+")" , con);
            cmd.ExecuteNonQuery();
        }
        catch { }
        finally
        {
            con.Close();
        }
        Response.Redirect(Request.RawUrl);  
    }

    private bool hasNotSelected(int idCategorie)
    {
        SqlConnection tempCon = DbConnection.GetSqlConnection();
        tempCon.Open();
        SqlCommand cmd = new SqlCommand("Select 1 From Preferinte Where IdUser = " + userId + " And Categorie = " + idCategorie, tempCon);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
     
            tempCon.Close();
            return false;
        }
        tempCon.Close();
        return true;
    }

    private void fillTableCategoriiSelecate()
    {
        SqlCommand cmd = new SqlCommand("Select Categorie From Preferinte Where IdUser = " + userId, con);
        SqlDataReader reader = cmd.ExecuteReader();

        TableRow th = TabelPreferinteSelectate.Rows[0];
        TabelPreferinteSelectate.Rows.Clear();
        TabelPreferinteSelectate.Rows.Add(th);

        while (reader.Read()){
            TableRow row = new TableRow();

            TableCell numeCategorie = new TableCell();
           // Label1.Text = ((int)reader.GetInt32(0)).ToString();
            numeCategorie.Text = getNumeCategorie((int)reader.GetInt32(0));
            //Label1.Text = numeCategorie.Text;
            row.Cells.Add(numeCategorie);

            TableCell delPreferintaCell = new TableCell();
            IntInfoBut Delete = new IntInfoBut();
            Delete.Text = "Sterge";
            Delete.Info = (int)reader.GetInt32(0);
            Delete.Click += new EventHandler(StergeCursul);
            delPreferintaCell.Controls.Add(Delete);
            row.Cells.Add(delPreferintaCell);

            TabelPreferinteSelectate.Rows.Add(row);
        }
        reader.Close();
    }

    private string getNumeCategorie(int idCategorie)
    {
        SqlConnection conn = DbConnection.GetSqlConnection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("Select NumeCategorie From Categorii_Cursuri Where Id = " + idCategorie, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        string nume = reader.GetValue(0).ToString();
        reader.Close();
        conn.Close();
        return nume;
    }

    public void StergeCursul(object sender, EventArgs e)
    {
        IntInfoBut Delete = (IntInfoBut)sender;
        try
        {
            con = DbConnection.GetSqlConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("Delete From Preferinte Where Categorie = " + Delete.Info + " AND IdUser = " + userId, con);
            cmd.ExecuteNonQuery();
        }
        catch { }
        finally
        {
            con.Close();
        }
        Response.Redirect(Request.RawUrl);  
    }

    public int GetUserId(string Name){
        SqlCommand cmd = new SqlCommand("select id from useri where Nume = '" + Name + "'", con);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        int id = (int)reader.GetInt32(0);
        reader.Close();
        return id;
    }

}