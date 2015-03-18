using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class AppData
{
	public AppData(string utilizator,string parola)
	{
        Utilizator = utilizator;
        Parola = parola;
	}
  public string Utilizator{
get;
private set;
}
public string Parola{
get;
private set;
}
}