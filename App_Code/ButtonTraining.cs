using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ButtonTraining
/// </summary>
public class ButtonTraining : System.Web.UI.WebControls.Button 
{
     private string training;

     public string Training
     {
         get
         {
             return training;
         }

         set
         {
             training = value;
         }
     }

	public ButtonTraining()
	{
		
	}
}