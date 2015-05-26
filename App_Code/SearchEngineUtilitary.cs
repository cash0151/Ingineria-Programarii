using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;
using System.Data.SqlClient;

public class SearchEngineUtilitary
{
    private static SearchEngineUtilitary instance;

	private SearchEngineUtilitary() {}

    public static SearchEngineUtilitary Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SearchEngineUtilitary();
            }
            return instance;
        }
    }

    public int computeLevenshteinDistance(String source, String target)
    {
        if (String.IsNullOrEmpty(source))
        {
            if (String.IsNullOrEmpty(target)) 
                return 0;
            return target.Length;
        }
        if (String.IsNullOrEmpty(target))
            return source.Length;

        if (source.Length > target.Length)
        {
            var temp = target;
            target = source;
            source = temp;
        }

        var targetLength = target.Length;
        var sourceLength = source.Length;
        var distance = new int[2, targetLength + 1];
        // Initialize the distance 'matrix'
        for (var j = 1; j <= targetLength; j++) 
            distance[0, j] = j;

        var currentRow = 0;
        for (var i = 1; i <= sourceLength; ++i)
        {
            currentRow = i & 1;
            distance[currentRow, 0] = i;
            var previousRow = currentRow ^ 1;
            for (var j = 1; j <= targetLength; j++)
            {
                var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                distance[currentRow, j] = Math.Min(Math.Min(
                            distance[previousRow, j] + 1,
                            distance[currentRow, j - 1] + 1),
                            distance[previousRow, j - 1] + cost);
            }
        }
        return distance[currentRow, targetLength];
    }

    public List<CourseValueObject> searchCoursesByNameRetrieveFirstXCourses(String name, List<CourseValueObject> courses, int numberOfWantedCourses)
    {
        foreach(CourseValueObject course in courses) {
            course.lewensteinFactor = computeLevenshteinDistance(name.ToLower(), course.getCourseName().ToLower());
        }

        List<CourseValueObject> sortedCourses = sortListWithXElements(courses, numberOfWantedCourses);
        return sortedCourses;
    }

    public List<CourseValueObject> sortListWithXElements(List<CourseValueObject> unsortedCourses, int numberOfWantedCourses) 
    {
        List<CourseValueObject> sortedCourses = unsortedCourses;
        sortedCourses.Sort();
        //sortedCourses.Reverse();
        List<CourseValueObject> courses = new List<CourseValueObject>();

        for (int index = 0; index < numberOfWantedCourses; index++)
        {
            courses.Add(sortedCourses[index]);
        }
        return courses;
    }

    public List<CourseValueObject> fillListFromCoursesTable(SqlCommand cmd, SqlConnection sqlConnection1)
    {
        List<CourseValueObject> courses = new List<CourseValueObject>();
        sqlConnection1.Open();

        SqlDataReader reader;

        reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                String courseName = Convert.ToString(reader["NumeCurs"]);
                String proffesorName = Convert.ToString(reader["Profesor"]);
                String cathegory = Convert.ToString(reader["Categorie"]);
                String description = Convert.ToString(reader["Continut"]);
                CourseValueObject course = new CourseValueObject(courseName, cathegory, proffesorName, description);
                courses.Add(course);
            }
        }
        else
        {
            throw new NullReferenceException();
        }
        sqlConnection1.Close();
        return courses;
    }

}