using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CourseValueObject
/// </summary>
public class CourseValueObject : IEquatable<CourseValueObject>, IComparable<CourseValueObject>
{
    private String cathegoryName;
    private String courseName;
    private String courseProfessorName;
    private String courseDescription;
    public int lewensteinFactor;


    public CourseValueObject(String courseName, String catName, String profName, String description)
	{
        this.cathegoryName = catName;
        this.courseName = courseName;
        this.courseProfessorName = profName;
        this.courseDescription = description;
        this.lewensteinFactor = 0;
	}

    public String getCathegoryName()
    {
        return this.cathegoryName;
    }

    public void setCathegoryName(String catName)
    {
        this.cathegoryName = catName;
    }

    public String getCourseName()
    {
        return this.courseName;
    }

    public void setCourseName(String name)
    {
        this.courseName = name;
    }

    public String getProfessorName()
    {
        return this.courseProfessorName;
    }

    public void setProfessorName(String name)
    {
        this.courseProfessorName = name;
    }

    public String getCourseDescription()
    {
        return this.courseDescription;
    }

    public void setCourseDescription(String desc)
    {
        this.courseDescription = desc;
    }

    //------------------- ComparableObjectethods--------------------------//


    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        CourseValueObject objAsCourseValueObject = obj as CourseValueObject;
        if (objAsCourseValueObject == null) return false;
        else return Equals(objAsCourseValueObject);
    }
    public int SortByNameAscending(string name1, string name2)
    {

        return name1.CompareTo(name2);
    }

    // Default comparer for Part type. 
    public int CompareTo(CourseValueObject comparePart)
    {
        // A null value means that this object is greater. 
        if (comparePart == null)
            return 1;

        else
            return this.lewensteinFactor.CompareTo(comparePart.lewensteinFactor);
    }
    public override int GetHashCode()
    {
        return lewensteinFactor;
    }
    public bool Equals(CourseValueObject other)
    {
        if (other == null) return false;
        return (this.lewensteinFactor.Equals(other.lewensteinFactor));
    }
}