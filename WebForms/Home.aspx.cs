using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class WebForms_Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
            categoryItem.NavigateUrl = "Coursescategories.aspx?Categorie="+(int)parentItem["Id"];  

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
}