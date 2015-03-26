using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class AppData
{
    public String Utilizator;
    public String Parola;
    public AppData()
	{

	}
    public AppData(String Utilizator,String Parola)
    {
        this.Utilizator = Utilizator;
        this.Parola = Parola;
    }
}