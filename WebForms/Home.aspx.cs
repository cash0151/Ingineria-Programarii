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
        SqlConnection con = DbConnection.GetSqlConnection();
        con.Open();
        SqlCommand c;
        c = new SqlCommand("SELECT NumeCurs,Count(p.id) \"nrParticipanti\" FROM cursuri c,Participanti p WHERE p.idCurs=c.id GROUP BY c.NumeCurs ORDER BY Count(p.id) DESC", con);
        SqlDataReader r = c.ExecuteReader();
        TableRow row1 = Clasament.Rows[0];
        Clasament.Rows.Clear();
        Clasament.Rows.Add(row1);
        int nrRezultate = 0;
        while (r.Read() &&nrRezultate<10)
        {
            nrRezultate++;
            TableRow row = new TableRow();

            TableCell cell1 = new TableCell();
            cell1.Text = nrRezultate + "";
            row.Cells.Add(cell1);

            TableCell cell2 = new TableCell();
            LinkButton Curs = new LinkButton();
            Curs.Text = (string)r["NumeCurs"];
            Curs.Click += new EventHandler(ViewCurs);
            cell2.Controls.Add(Curs);
            row.Cells.Add(cell2);

            TableCell cell3 = new TableCell();
            cell3.Text = (Int32)r["nrParticipanti"]+"";
            row.Cells.Add(cell3);

            Clasament.Rows.Add(row);
            
   
           
        }
        con.Close();
    }
    public void ViewCurs(object sender, EventArgs e)
    {
        LinkButton Curs = (LinkButton)sender;
        Response.Redirect("/WebForms/Courses.aspx?Curs=" + Curs.Text);
    }
}