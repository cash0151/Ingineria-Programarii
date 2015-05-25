using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForms_AcceptCourseRegistrationPage : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        int idDoritor = Int32.Parse(Request["idDoritor"]);
        int idCurs = Int32.Parse(Request["idCurs"]);

        SqlConnection conn = DbConnection.GetSqlConnection();
        conn.Open();

        SqlCommand c = new SqlCommand("UPDATE Participanti SET Status='ACTIVE', Vazut='SEEN' WHERE IdUser=@IdUser AND IdCurs=@idCurs", conn);
        c.Parameters.Add(new SqlParameter("@IdCurs", TypeCode.String));
        c.Parameters["@IdCurs"].Value = idCurs;
        c.Parameters.Add(new SqlParameter("@idUser", TypeCode.Int32));
        c.Parameters["@idUser"].Value = idDoritor;
        SqlDataReader r = c.ExecuteReader();

        conn.Close();

        Response.Redirect(Request.UrlReferrer.ToString());
    }
}