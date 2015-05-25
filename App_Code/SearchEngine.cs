﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SearchEngine
/// </summary>
public class SearchEngine
{

    SearchEngineUtilitary courseFinder = SearchEngineUtilitary.Instance;

	public SearchEngine()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    

    public List<CourseValueObject> getCoursesFromName(String name, int numberOfWantedCourses)
    {
        List<CourseValueObject> courses = new List<CourseValueObject>();
        DbConnection connection = DbConnection.Instance;
        
        SqlConnection sqlConnection1 = connection.GetSqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader reader = new SqlDataReader;

        cmd.CommandText = "SELECT * FROM Cursuri";
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Connection = sqlConnection1;

        courses = courseFinder.fillListFromCoursesTable(reader, cmd, sqlConnection1);

        courses = courseFinder.searchCoursesByNameRetrieveFirstXCourses(name, courses, numberOfWantedCourses);
        return courses;
    }

    public List<CourseValueObject> getCoursesFromNameAndCathegoryName(String name, String cathegoryName, int numberOfWantedCourses)
    {
        List<CourseValueObject> courses = new List<CourseValueObject>();
        DbConnection connection = DbConnection.Instance;

        SqlConnection sqlConnection1 = connection.GetSqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader reader = new SqlDataReader();

        cmd.CommandText = "SELECT * FROM Cursuri c, Categorii_Cursuri cc where c.Categorie = '" + cathegoryName + "'";
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Connection = sqlConnection1;

        courses = courseFinder.fillListFromCoursesTable(reader, cmd, sqlConnection1);

        courses = courseFinder.searchCoursesByNameRetrieveFirstXCourses(name, courses, numberOfWantedCourses);
        return courses;
    }
}