using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Text;

public partial class WebForms_Poze : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        String utilizator = ((AppData)Session["login"]).Utilizator;
        String parola = ((AppData)Session["login"]).Parola;
        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        SqlCommand c;
        SqlDataReader r;
        if (Request.QueryString["profil_persoana"] == null) Response.Redirect("PageNotFound.aspx");
        //voi lista albumele
        if ((Request.QueryString["profil_persoana"] != null && Request.QueryString["album"] == null) || Request.QueryString["profil_persoana"] != null && Request.QueryString["album"] != null)
        {

            String nume_profil = Request.QueryString["profil_persoana"];
            //verific daca utilizatorul curent este ownerul profilului. Daca nu ascund butoanele de adaugat albume/poze
            if (!nume_profil.Equals(utilizator))
            {
                FileUpload1.Visible = false;
                Button2.Visible = false;
                TextBox2.Visible = false;
                Button3.Visible = false;
            }


            //VERIFIC DACA UTILIZATORUL ESTE ADMIN
             c= new SqlCommand("SELECT tip FROM persoane WHERE UPPER(username)=UPPER(@user) AND UPPER(parola)=UPPER(@parola)", conn);
            c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
            c.Parameters["@User"].Value = utilizator;
            c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
            c.Parameters["@parola"].Value = parola;
            Object o = (object)c.ExecuteScalar();
            String tip_utilizator;
            //daca o este null inseamna ca utilizatorul este vizitator asa ca nu are date in baza de date deci nu are tip_utilizator
            if (o != null)
                tip_utilizator = (String)o;
            else tip_utilizator = "";
            c = new SqlCommand("SELECT id,tip FROM profiluri WHERE UPPER(persoana_username)=UPPER(@user)", conn);

            c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
            c.Parameters["@User"].Value = nume_profil;
            r = c.ExecuteReader();
            bool profilExistent = false;
            int id_profil = -1;
            String tipProfil = "";
            while (r.Read())
            {
                profilExistent = true;
                id_profil = (int)r["id"];
                tipProfil = (String)r["tip"];
            }
            r.Close();
            //daca profilul din get nu exista sau este profil privat si userul nu are drepturi de acces , userul este redirectionat
            if (profilExistent == false) Response.Redirect("PageNotFound.aspx");
           
            if (String.Compare(tipProfil, "privat") == 0 && String.Compare(nume_profil, utilizator) != 0 && !tip_utilizator.Equals("admin")) Response.Redirect("ProfilPrivat.aspx");
           //pe acest caz profilul exista si vreau sa afisez toate albumele de pe profil care poate fii selectat sa afiseze pozele din acel album
            if (Request.QueryString["profil_persoana"] != null && Request.QueryString["album"] == null)
            {
                FileUpload1.Visible = false;
                Button2.Visible = false;
                c = new SqlCommand("SELECT id,nume_album FROM albume_poze WHERE profil_id=@profil_id", conn);
                c.Parameters.Add(new SqlParameter("@profil_id", TypeCode.Int32));
                c.Parameters["@profil_id"].Value = id_profil;
                r = c.ExecuteReader();

                while (r.Read())
                {//albume
                    int id_album=(int)r["id"];
                    HtmlGenericControl container = new HtmlGenericControl();
                    container.TagName = "div";
                    container.Attributes["class"] = "albumContainer";

                    String album = String.Format("<a href=Poze.aspx?profil_persoana={0}&album={1} class={2}>{3}</a>", nume_profil, id_album, "albumLink", (String)r["nume_album"]);
                    String buttonRemove=""; 
                    if (String.Compare(nume_profil, utilizator) == 0 || tip_utilizator.Equals("admin"))
                   buttonRemove = String.Format("<button type=\"button\"  onclick=\"RemoveAlbum({0})\">Elimina</button>", id_album);
                   container.InnerHtml=String.Format("<br/>{0}<br/><br/>{1}", album, buttonRemove);
                    Panel2.Controls.Add(container);
                }
                r.Close();
            }
                // pe acest caz arat pozele din albumul selectat
            else
            {
                //poze
                TextBox2.Visible = false;
                Button3.Visible = false;
                int album_id = Int32.Parse(Request.QueryString["album"]);
                c = new SqlCommand("SELECT id,cale FROM poze WHERE album_id=@album_id", conn);
                c.Parameters.Add(new SqlParameter("@album_id", TypeCode.Int32));
                c.Parameters["@album_id"].Value = album_id;
                r = c.ExecuteReader();
                bool albumExistent = false;
                while (r.Read())
                {
                    albumExistent = true;
                    String cale = (String)r["cale"];
                    HtmlGenericControl container = new HtmlGenericControl();
                    container.TagName = "div";
                    container.Attributes["class"] = "albumContainer";
                    int id_poza=(int) r["id"];
                    String img = String.Format("<img src={0} class={1}  onclick=\"MarestePoza({2})\" id=\"{3}\"/>", "../Images/" + cale, "\"imagineMica\"",id_poza,"img"+id_poza);
                    String buttonRemove="";
                    if (String.Compare(nume_profil, utilizator) == 0 || tip_utilizator.Equals("admin"))
                    buttonRemove = String.Format("<button type=\"button\"  onclick=\"RemovePoza({0})\">Elimina</button>",id_poza);
                    container.InnerHtml = String.Format("<br/>{0}<br/><br/>{1}", img, buttonRemove);
                    Panel2.Controls.Add(container);
                }
                //verific daca exista albumul pe cazul in care getul albumului nu este null
                if (albumExistent == false) Response.Redirect("Album Neexistent.aspx");
            }  

        }

        conn.Close();
    }
    //buton pentru adaugat poze
    protected void Button2_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            int album_id = Int32.Parse(Request.QueryString["album"]);
            SqlConnection conn;
            SqlCommand c;
            Object o;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
        


            //imi intoarce idul
             c = new SqlCommand("INSERT INTO poze(album_id,cale) OUTPUT INSERTED.ID VALUES(@album_id,@cale)", conn);
            c.Parameters.Add(new SqlParameter("@cale", TypeCode.String));
            c.Parameters["@cale"].Value = "temp";
            c.Parameters.Add(new SqlParameter("@album_id", TypeCode.Int32));
            c.Parameters["@album_id"].Value = album_id;
            o = c.ExecuteScalar();
            int id = (int)o;
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            EnsureDirectoriesExist("Images");
            String extension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();

            String[] extensiiPermise = { ".jpg", ".png", "jpeg", ".gif" };
            bool extensiePermisa = false;
            for (int i = 0; i < extensiiPermise.Length; i++)
            {
                if (extension.Equals(extensiiPermise[i])) extensiePermisa = true;
            }
            if (extensiePermisa == false)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Tipul de fisier nu este permis";
                return;
            }
            String numeFisier = String.Format("{0}{1}", id, extension);
            FileUpload1.PostedFile.SaveAs(Server.MapPath("../Images/") + numeFisier);
            c = new SqlCommand("UPDATE poze SET cale=@cale WHERE id=@id", conn);
            c.Parameters.Add(new SqlParameter("@cale", TypeCode.String));
            c.Parameters["@cale"].Value = numeFisier;
            c.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
            c.Parameters["@id"].Value = id;
            c.ExecuteNonQuery();
            conn.Close();
            Response.Redirect(Request.RawUrl);
        }
        else lblMessage.Text = "Nu a fost selectat nici un fisier";
    }
    //functie care daca nu exista folderul Images il creaza
    public void EnsureDirectoriesExist(String nume)
    {

        //Daca folderul nu exista il creaza
        if (!System.IO.Directory.Exists(Server.MapPath(String.Format(@"../{0}/",nume))))
        {
            System.IO.Directory.CreateDirectory(Server.MapPath(String.Format(@"../{0}/", nume)));
        }

    }
    //adauga album
    protected void Button3_Click(object sender, EventArgs e)
    {

        String utilizator = ((AppData)Session["login"]).Utilizator;
        String parola = ((AppData)Session["login"]).Parola;
        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        SqlCommand c;
        SqlDataReader r;
        c = new SqlCommand("SELECT id FROM profiluri WHERE persoana_username=@user", conn);
        c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
        c.Parameters["@User"].Value = utilizator;
        Object o = (object)c.ExecuteScalar();
        int profil_id = (int)o;
        c = new SqlCommand("INSERT INTO albume_poze(profil_id,nume_album) VALUES(@id,@nume)", conn);
        c.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
        c.Parameters["@id"].Value = profil_id;
        c.Parameters.Add(new SqlParameter("@nume", TypeCode.String));
        c.Parameters["@nume"].Value = TextBox2.Text;
        c.ExecuteNonQuery();
        Response.Redirect(Request.RawUrl);
    }

    //sterge albume
         [WebMethod]
    public static string RemoveAlbum(string id)
    {
        SqlConnection conn;

        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        SqlCommand c;
        //PRIMA DATA STERG COMENTURILE POZELOR
      c = new SqlCommand("DELETE FROM comenturi WHERE poza_id=(SELECT id FROM poze WHERE album_id=@idul)", conn);
        c.Parameters.Add(new SqlParameter("@idul", TypeCode.Int32));
        c.Parameters["@idul"].Value = Int32.Parse(id);
        c.ExecuteNonQuery();
             //dupa pozele
       c = new SqlCommand("DELETE FROM poze WHERE album_id=@idul", conn);
        c.Parameters.Add(new SqlParameter("@idul", TypeCode.Int32));
        c.Parameters["@idul"].Value = Int32.Parse(id);
        c.ExecuteNonQuery();
//dupa albumul
        c = new SqlCommand("DELETE FROM albume_poze WHERE id=@idul", conn);
        c.Parameters.Add(new SqlParameter("@idul", TypeCode.Int32));
        c.Parameters["@idul"].Value = Int32.Parse(id);
        c.ExecuteNonQuery();
        return "succes";
         }
    //sterge poze
         [WebMethod]
         public static string RemovePoza(string id)
         {
             SqlConnection conn;
             SqlCommand c;
             conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
             conn.Open();
             //PRIMA DATA STERG COMENTURILE POZEI
              c = new SqlCommand("DELETE FROM comenturi WHERE poza_id=@idul", conn);
             c.Parameters.Add(new SqlParameter("@idul", TypeCode.Int32));
             c.Parameters["@idul"].Value = Int32.Parse(id);
             c.ExecuteNonQuery();

            //dupa sterg poza
             c = new SqlCommand("DELETE FROM poze WHERE id=@idul", conn);
             c.Parameters.Add(new SqlParameter("@idul", TypeCode.Int32));
             c.Parameters["@idul"].Value = Int32.Parse(id);
             c.ExecuteNonQuery();
             return "succes";
         }
    //adauga comenturi la poze
             [WebMethod]
    public static string  AddComentPoza(String idPoza,String mesaj){

        SqlConnection conn;

        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        SqlCommand c = new SqlCommand("INSERT INTO comenturi(poza_id,autor,textul,timp) VALUES(@poza_id,@autor,@textul,@timp)", conn);
        c.Parameters.Add(new SqlParameter("@poza_id", TypeCode.Int32));
        c.Parameters["@poza_id"].Value = Int32.Parse(idPoza);
        c.Parameters.Add(new SqlParameter("@autor", TypeCode.String));
        c.Parameters["@autor"].Value = ((AppData)HttpContext.Current.Session["login"]).Utilizator;
        c.Parameters.Add(new SqlParameter("@textul", TypeCode.String));
        c.Parameters["@textul"].Value = mesaj;
        c.Parameters.Add(new SqlParameter("@timp", TypeCode.DateTime));
        c.Parameters["@timp"].Value = DateTime.Now;
        c.ExecuteNonQuery();
        return "succes";
             }
    //imi intoarce toate comenturile ale unei poze
     [WebMethod]
             public static string GetComentsPoza(String idPoza)

             {
                 SqlConnection conn;
                 SqlCommand c;
                 conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                 conn.Open();
                 SqlDataReader r;
                 String utilizator = ((AppData)HttpContext.Current.Session["login"]).Utilizator;
                 String parola = ((AppData)HttpContext.Current.Session["login"]).Parola;


                 //VERIFIC DACA UTILIZATORUL ESTE ADMIN
                 c = new SqlCommand("SELECT tip FROM persoane WHERE UPPER(username)=UPPER(@user) AND UPPER(parola)=UPPER(@parola)", conn);
                 c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
                 c.Parameters["@User"].Value = utilizator;
                 c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
                 c.Parameters["@parola"].Value = parola;
                 Object o = (object)c.ExecuteScalar();
                 String tip_utilizator;
                 //daca o este null inseamna ca utilizatorul este vizitator asa ca nu are date in baza de date deci nu are tip_utilizator
                 if (o != null)
                     tip_utilizator = (String)o;
                 else tip_utilizator = "";
            
         //stiu doar idul pozei si trebuie sa fac rost de profil sa vad daca are drept de stergere deci o sa
         //ii aflu albumul ce ii aprtine si dupa numele profilului ca sa compar cu utilizatorul
                c = new SqlCommand("SELECT persoana_username FROM profiluri WHERE id=(SELECT profil_id FROM albume_poze WHERE id="
                + "(SELECT album_id FROM poze WHERE id=@id))", conn);
                c.Parameters.Add(new SqlParameter("@id", TypeCode.Int32));
                c.Parameters["@id"].Value = Int32.Parse(idPoza);
                o = (object)c.ExecuteScalar();
                String nume_profil = (String)o;


               


                  //despartitor1 este folosit sa disting comenturi ce contin id,autor,text si drept de delete
                 String despartitor1 = ":123,p25ltDGGRTl-3lAB}}}!$#!###$#$#4;";
                  //despartitor2 este folosit ca in fiecare coment sa stiu cine e idul cine e autorul si asa mai departe
                 String despartitor2 = "geh35423l3,p2AB}}##asdw$,";
                 StringBuilder sb = new StringBuilder();

                 String esteVizitator = "nu";
                     if(utilizator.Equals("")) esteVizitator="da";
                sb.Append(esteVizitator);
                sb.Append(despartitor1);
                  c = new SqlCommand("SELECT id,autor,textul FROM comenturi WHERE poza_id=@poza_id ORDER BY timp DESC", conn);
                 c.Parameters.Add(new SqlParameter("@poza_id", TypeCode.Int32));
                 c.Parameters["@poza_id"].Value = Int32.Parse(idPoza);
                r=c.ExecuteReader();
                while (r.Read())
                {
                    sb.Append((String)r["autor"]);
                    sb.Append(despartitor2);
                    sb.Append((String)r["textul"]);
                    sb.Append(despartitor2);
                    sb.Append((int)r["id"]);
                    sb.Append(despartitor2);

                    String dreptDeStergere = "nu";
                    if (nume_profil.Equals(utilizator) || utilizator.Equals((String)r["autor"]) || tip_utilizator.Equals("admin")) dreptDeStergere = "da";
                   // if ( utilizator.Equals((String)r["autor"]) || tip_utilizator.Equals("admin")) dreptDeStergere = "da";
                    sb.Append(dreptDeStergere);
                    sb.Append(despartitor1);
                }
                 return sb.ToString();
             }
    //elimina un coment dupa idul lui .Aceasi metoda ca in pagina de profil 
    [WebMethod]
     public static string RemoveComent(string id)
     {
         SqlConnection conn;

         conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
         conn.Open();
         //sterg reply-urile ale acestul coment prima data dupa comentul in sine altfel o sa am probleme cu key externe 
         SqlCommand c = new SqlCommand("DELETE FROM comenturi WHERE coment_id=@idul", conn);
         c.Parameters.Add(new SqlParameter("@idul", TypeCode.Int32));
         c.Parameters["@idul"].Value = Int32.Parse(id);
         c.ExecuteNonQuery();

         c = new SqlCommand("DELETE FROM comenturi WHERE id=@idul", conn);
         c.Parameters.Add(new SqlParameter("@idul", TypeCode.Int32));
         c.Parameters["@idul"].Value = Int32.Parse(id);
         c.ExecuteNonQuery();
         return "succes";
     }
}