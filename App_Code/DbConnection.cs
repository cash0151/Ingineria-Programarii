using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DbConnection
/// </summary>
public class DbConnection
{
    private static SqlConnection my_connection;
    private static DbConnection instance;

    private DbConnection() { }

    public static DbConnection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DbConnection();
            }
            return instance;
        }
    }

    public static SqlConnection GetSqlConnection()
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
         my_connection = new SqlConnection(ConnectionString);
         return my_connection;
    }
}