using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


/// <summary>
/// Summary description for RecommendationSystem
/// </summary>
public class RecommendationSystem
{
    SearchEngineUtilitary courseFinder = SearchEngineUtilitary.Instance;

	public RecommendationSystem()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public List<CourseValueObject> recommendCoursesFromCathegory(int userId, int numberOfWantedCourses)
    {
        List<CourseValueObject> courses = new List<CourseValueObject>();
        DbConnection connection = DbConnection.Instance;

        SqlConnection sqlConnection1 = DbConnection.GetSqlConnection();
        SqlCommand cmd = new SqlCommand();
        //SqlDataReader reader = new SqlDataReader();

        cmd.CommandText = "SELECT * FROM Preferinte p, Categorii_Cursuri cc, Useri u where u.Id = p.IdUser and cc.Id = p.Categorie and u.Id=@id";
        cmd.Parameters.Add(new SqlParameter("@Id", TypeCode.Int32));
        cmd.Parameters["@Id"].Value = userId;
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Connection = sqlConnection1;

        courses = courseFinder.fillListFromCoursesTable(cmd, sqlConnection1);

        if(numberOfWantedCourses > courses.Count)
            numberOfWantedCourses = courses.Count;

        if(courses.Count == 0) {
            cmd.CommandText = "SELECT *  FROM Cursuri";
        }

        courses = courseFinder.fillListFromCoursesTable(cmd, sqlConnection1);

        List<CourseValueObject> recommendedCourses = new List<CourseValueObject>();

        for (int index = 0; index < numberOfWantedCourses; index++)
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(0,courses.Count);
            while(recommendedCourses[randomNumber] != null) {
                randomNumber = rnd.Next(0,courses.Count);
            }
            recommendedCourses.Add(courses[randomNumber]);
        }

        return recommendedCourses;
    }

}