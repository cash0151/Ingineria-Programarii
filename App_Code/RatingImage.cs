using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RatingImage
/// </summary>
public class RatingImage : System.Web.UI.WebControls.ImageButton
{
    private int nota;

    public int Nota
    {
        get
        {
            return nota;
        }
        set
        {
            nota = value;
        }
    }

	public RatingImage(string URL)
	{
        this.ImageUrl = URL;
        this.Width = 20;
        this.Height = 20;
	}

    
}