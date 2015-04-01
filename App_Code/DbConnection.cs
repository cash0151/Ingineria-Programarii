using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DbConnection
/// </summary>
public static class DbConnection
{
    private static SqlConnection my_connection;

    public static SqlConnection GetSqlConnection()
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
         my_connection = new SqlConnection(ConnectionString);
         return my_connection;
    }
}