using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Text;

public partial class WebForms_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String utilizator = ((AppData)Session["login"]).Utilizator;
        String parola = ((AppData)Session["login"]).Parola;

        if (utilizator.Equals(""))
        {
            Button2.Visible = false;
            Button1.Visible = false;
            TextBox1.Visible = false;
        }
        
        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
       
       
        //VERIFIC DACA UTILIZATORUL ESTE ADMIN
        SqlCommand c = new SqlCommand("SELECT tip FROM persoane WHERE UPPER(username)=UPPER(@user) AND UPPER(parola)=UPPER(@parola)" , conn);
        c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
        c.Parameters["@User"].Value = utilizator;
        c.Parameters.Add(new SqlParameter("@parola", TypeCode.String));
        c.Parameters["@parola"].Value = parola;
        Object o = (object)c.ExecuteScalar();
        String tip_utilizator;
        //daca o este null inseamna ca utilizatorul este vizitator asa ca nu are date in baza de date deci nu are tip_utilizator
        if(o!=null)
        tip_utilizator = (String)o;
        else  tip_utilizator="";

        //verificare daca parametrul din get este ok si daca da creaza pagina corecta
        if (Request.QueryString["profil_persoana"] == null &&  Request.QueryString["profil_grup"] == null ) Response.Redirect("PageNotFound.aspx");
        //caz ca ambele sunt diferite de null  nu am ce sa fac asa ca trimit la pagenotfound
        if (Request.QueryString["profil_persoana"] != null && Request.QueryString["profil_grup"] != null) Response.Redirect("PageNotFound.aspx");
        String nume_profil="";
        bool esteGrup=false;
        if (Request.QueryString["profil_persoana"] != null)
        {
            nume_profil = Request.QueryString["profil_persoana"];
            esteGrup = false;
        }
        if (Request.QueryString["profil_grup"] != null){
            nume_profil = Request.QueryString["profil_grup"];
            esteGrup = true;
        }
       
        if(!esteGrup){
       c = new SqlCommand("SELECT id,tip FROM profiluri WHERE UPPER(persoana_username)=UPPER(@user)", conn);

        c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
        c.Parameters["@User"].Value = nume_profil;
        }
        else{
        c = new SqlCommand("SELECT id,tip FROM profiluri WHERE UPPER(grup_name)=UPPER(@user)", conn);

        c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
        c.Parameters["@User"].Value = nume_profil;
        }
        SqlDataReader r = c.ExecuteReader();
        bool profilExistent = false;
        int id_profil=-1;
        String tipProfil = "";
        while (r.Read())
        {
            profilExistent = true;
            id_profil = (int)r["id"];
            tipProfil = (String)r["tip"];
        }
        r.Close();
        //daca profilul din get nu exista sau este profil privat userul este redirectionat
        if (profilExistent == false) Response.Redirect("PageNotFound.aspx");
            //daca este grup ele au toate tipul public dar pot vedea si adauga mesajele doar membri
        if (String.Compare(tipProfil, "privat") == 0 && String.Compare(nume_profil,utilizator) != 0 && !tip_utilizator.Equals("admin")) Response.Redirect("ProfilPrivat.aspx");


        if (esteGrup)
        {

            Label1.Text = String.Format("Grupul: {0}", nume_profil);
            Button2.Visible = false;
            Label2.Visible = false;
            Public.Visible = false;
            Privat.Visible = false;
            Button3.Visible = false;
            Button5.Visible = false;
            //vizitatorii nu pot vedea gurpurile asa ca ii redirectionez la o pagina sa le afisez mesjul
            if (utilizator.Equals("")) Response.Redirect("ProfilGrupuriVizitatori.aspx");
        }
        else
        {
            Label1.Text = String.Format("Profilul lui : {0}", nume_profil);
            Button4.Visible = false;
        }
        //pentru grupuri si persoana ce nu sunt ownerul profilului ascund optiunea de schimbarea tipului profilului
        if (!nume_profil.Equals(utilizator) && !esteGrup)
        {
            Public.Visible = false;
            Privat.Visible = false;
            Button3.Visible = false;
        }
        //VERIFIC DACA userul APARTINE GRUPULUI
        if (esteGrup)
        {
            c = new SqlCommand("SELECT '1' FROM persoane_grupuri WHERE UPPER(persoana_username)=UPPER(@user) AND UPPER(grup_name)=UPPER(@name)", conn);

            c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
            c.Parameters["@User"].Value = utilizator;
            c.Parameters.Add(new SqlParameter("@name", TypeCode.String));
            c.Parameters["@name"].Value = nume_profil;
            o = (object)c.ExecuteScalar();
            if (o == null && !tip_utilizator.Equals("admin"))
            {
                //daca nu apartine grupului ii recomand sa i se alature ca sa poate vedea mesajele sau sa poata scrie mesaje
                Label2.Visible = true;
                Label2.Text = "Trebuie sa apartii grupului ca sa poti vedea mesajele";
                TextBox1.Visible = false;
                Button1.Visible = false;
                return;
            }
            else
            {
                //daca apartine grupului ascund butonul pentru a intra in grup
                Button4.Visible = false;
            }
            //daca utilizatorul este admin il las sa intre in grupuri
            if (o == null && tip_utilizator.Equals("admin"))
            {
                Button4.Visible = true;
            }
        }
        //daca ajung aici stiu ca exista grupul si ca persoana apartine grupului 

        //ascund butonul de adaugat la priten pentru ownerul profilului si transform in scoatere de la prieteni daca este deja
        //prieten cu acea persoana
        if (String.Compare(nume_profil, utilizator) == 0)
        {
            Button2.Visible = false;
        }

        //afisez ce tip de profil e/daca e grup acet label nu apare
        Label2.Text = String.Format("Profilul este :{0}", tipProfil);
            //daca sunt pe profil de grup nu are rost butonul
            if(!esteGrup){
        //transform in remove friend daca il are dea la prieteni
        c = new SqlCommand("Select'1'FROM lista_prieteni WHERE UPPER(username)=@utilizator AND UPPER(prieten)=@prieten", conn);
        c.Parameters.Add(new SqlParameter("@utilizator", TypeCode.String));
        c.Parameters["@utilizator"].Value = utilizator;
        c.Parameters.Add(new SqlParameter("@prieten", TypeCode.String));
        c.Parameters["@prieten"].Value = nume_profil;
       o = (object)c.ExecuteScalar();
      String  raspuns = (String)o;
        if (String.Compare(raspuns, "1") == 0)
        {
            Button2.Text = "Scoate de la prieteni";
        }
            }

        //populez cu comenturi
        c = new SqlCommand("Select id,textul,autor FROM comenturi WHERE profil_id = @id_profil ORDER BY timp DESC ", conn);
        c.Parameters.Add(new SqlParameter("@id_profil", TypeCode.Int32));
        c.Parameters["@id_profil"].Value = id_profil;
         r = c.ExecuteReader();

        
        while(r.Read()){
            HtmlGenericControl divcontrol = new HtmlGenericControl();
            divcontrol.Attributes["class"] = "comment";
            divcontrol.TagName = "div";
            int idComment = (Int32)r["id"];
   
           /*
            *pun pentru buttonRemove un buton care are in functie ca argument idul comentului ce va fii sters din baza 
            *de date daca se va dori asta
            */
            String buttonRemove="";
            //dacat cel carui ii apartine profilul si cel care a scris comentul si administratorul pot elimina comenturi
            if (String.Compare(((String)r["autor"]), utilizator) == 0 || String.Compare(nume_profil, utilizator) == 0 || tip_utilizator.Equals("admin"))
                buttonRemove = String.Format("<button type=\"button\"  onclick=\"RemoveComment({0})\">Elimina</button>", idComment);
            
            
            //REPLY
            SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //verificare daca userul curent este logat
            conn2.Open();
            SqlCommand c2 = new SqlCommand("Select id,textul,autor FROM comenturi WHERE coment_id = @id_coment ORDER BY timp DESC ", conn2);
            c2.Parameters.Add(new SqlParameter("@id_coment", TypeCode.Int32));
            c2.Parameters["@id_coment"].Value = idComment;
            SqlDataReader r2 = c2.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (r2.Read())
            {
            int idComment2 = (Int32)r2["id"];
            String buttonRemove2 = "";
            //dacat cel carui ii apartine profilul si cel care a scris comentul si administratorul pot elimina comenturi
            if (String.Compare(((String)r2["autor"]), utilizator) == 0 || String.Compare(nume_profil, utilizator) == 0 || tip_utilizator.Equals("admin"))
                buttonRemove2 = String.Format("<button type=\"button\"  onclick=\"RemoveComment({0})\">Elimina</button>", idComment2);
          
            sb.Append(String.Format("<div class=\"reply\"><p>Autor: {0}</p> <p>mesaj:{1}</p> {2} </div>", ((String)r2["autor"]), ((String)r2["textul"]), buttonRemove2));
            }
            r2.Close();
            conn2.Close();
            //concatenarea tuturor

            /*pentru addReply adaug ca id al elementului input idul comentului ce va primii raspuns si 
    si ca argument al functiei pentru buton ca stiu idul carui element il voi lua in functie 
    * si acel id il folosi sa iau inputul si textul din acela + voi stii idul comentului din
    * baza de date carui va trebuii sa ii dau un reply
    * */
            String addReply="";
            if (!utilizator.Equals(""))
                addReply = String.Format("<input id=\"{0}\" type=\"text\" /><button type=\"button\"  onclick=\"AddComment({0})\">Comenteaza</button> ", idComment);

            divcontrol.InnerHtml = String.Format("<p>Autor: {0}</p> <p>mesaj:{1}</p> {2}<br/>{3} <br/>{4}", ((String)r["autor"]), ((String)r["textul"]), buttonRemove, sb.ToString(), addReply);
            Panel1.Controls.Add(divcontrol);
        }
        r.Close();
        conn.Close();

    }
    //adauga coment
    protected void Button1_Click(object sender, EventArgs e)
    {
        //daca ajung aici stiu ca  exista cel putin un query string
        String nume_profil = "";
        bool esteGrup = false;
        if (Request.QueryString["profil_persoana"] != null)
        {
            nume_profil = Request.QueryString["profil_persoana"];
            esteGrup = false;
        }
        if (Request.QueryString["profil_grup"] != null)
        {
            nume_profil = Request.QueryString["profil_grup"];
            esteGrup = true;
        }
        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        SqlCommand c;
        if (!esteGrup)
        {
            c = new SqlCommand("SELECT id FROM profiluri WHERE UPPER(persoana_username)=UPPER(@user)", conn);
            c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
            c.Parameters["@User"].Value = nume_profil;
        }
        else
        {
            c = new SqlCommand("SELECT id FROM profiluri WHERE UPPER(grup_name)=UPPER(@user)", conn);
            c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
            c.Parameters["@User"].Value = nume_profil;
        }
        object o = (object)c.ExecuteScalar();
        int id_profil = (int)o;

        c = new SqlCommand("INSERT INTO comenturi(profil_id,textul,autor,timp) VALUES(@id_profil,@mesaj,@autor,@timp )", conn);
        c.Parameters.Add(new SqlParameter("@id_profil", TypeCode.Int32));
        c.Parameters["@id_profil"].Value = id_profil;
        c.Parameters.Add(new SqlParameter("@mesaj", TypeCode.String));
        c.Parameters["@mesaj"].Value = TextBox1.Text.ToString();
        c.Parameters.Add(new SqlParameter("@autor", TypeCode.String));
        c.Parameters["@autor"].Value = ((AppData)Session["login"]).Utilizator;
        c.Parameters.Add(new SqlParameter("@timp", TypeCode.DateTime));
        c.Parameters["@timp"].Value = DateTime.Now;

        c.ExecuteNonQuery();
        Response.Redirect(Request.RawUrl);
    }

    //scoate coment sau un reply
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
    //adauga reply la un coment
    [WebMethod]
    public static string AddReply(string id, string text)
    {
        SqlConnection conn;

        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        SqlCommand c = new SqlCommand("INSERT INTO comenturi(coment_id,textul,autor,timp) VALUES(@coment_id,@mesaj,@autor,@timp )", conn);
        c.Parameters.Add(new SqlParameter("@coment_id", TypeCode.Int32));
        c.Parameters["@coment_id"].Value = Int32.Parse(id);
        c.Parameters.Add(new SqlParameter("@mesaj", TypeCode.String));
        c.Parameters["@mesaj"].Value = text;
        c.Parameters.Add(new SqlParameter("@autor", TypeCode.String));
        //Am gasit pe internet ca sa accesez sessionul curent dintr-o metoda statica trebuie sa foloesc HttpContext.Current.Session
        c.Parameters["@autor"].Value = ((AppData)HttpContext.Current.Session["login"]).Utilizator;
        c.Parameters.Add(new SqlParameter("@timp", TypeCode.DateTime));
        c.Parameters["@timp"].Value = DateTime.Now;

        c.ExecuteNonQuery();

        return "succes";
    }
    //schimba statusul de prietenie
    protected void Button2_Click(object sender, EventArgs e)
    {
        SqlConnection conn;
        //daca ajung aici stiu ca  exista cel putin un query string
        String nume_profil = Request.QueryString["profil_persoana"];



        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();

       SqlCommand  c = new SqlCommand("SELECT '1' FROM lista_prieteni WHERE UPPER(username)=UPPER(@user) AND UPPER(prieten)= UPPER(@prieten)", conn);
       c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
       c.Parameters["@user"].Value = ((AppData)Session["login"]).Utilizator;
       c.Parameters.Add(new SqlParameter("@prieten", TypeCode.String));
       c.Parameters["@prieten"].Value = nume_profil;
       object o = (object)c.ExecuteScalar();
       String raspuns = (String)o;
       if (raspuns != "1")
       {
           //PE ACEST CAZ NU SUNT PRIETENI ASA CA II ADAUG CA PRIETENI
           c = new SqlCommand("INSERT INTO lista_prieteni VALUES(@user,@prieten,'da')", conn);
           c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
           c.Parameters["@user"].Value = ((AppData)Session["login"]).Utilizator;
           c.Parameters.Add(new SqlParameter("@prieten", TypeCode.String));
           c.Parameters["@prieten"].Value = nume_profil;
           c.ExecuteNonQuery();
           c = new SqlCommand("INSERT INTO lista_prieteni VALUES(@user,@prieten,'da')", conn);
           c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
           c.Parameters["@user"].Value = nume_profil;
           c.Parameters.Add(new SqlParameter("@prieten", TypeCode.String));
           c.Parameters["@prieten"].Value = ((AppData)Session["login"]).Utilizator;
           c.ExecuteNonQuery();
       }
       else
       {
           //pe acest caz sunt prieteni deci ii scot de la prieteni
           c = new SqlCommand("DELETE FROM lista_prieteni WHERE UPPER(username)=UPPER(@user) AND UPPER(prieten)= UPPER(@prieten)", conn);
           c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
           c.Parameters["@user"].Value = ((AppData)Session["login"]).Utilizator;
           c.Parameters.Add(new SqlParameter("@prieten", TypeCode.String));
           c.Parameters["@prieten"].Value = nume_profil;
           c.ExecuteNonQuery();
           c = new SqlCommand("DELETE FROM lista_prieteni WHERE UPPER(username)=UPPER(@user) AND UPPER(prieten)= UPPER(@prieten)", conn);
           c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
           c.Parameters["@user"].Value = nume_profil;
           c.Parameters.Add(new SqlParameter("@prieten", TypeCode.String));
           c.Parameters["@prieten"].Value = ((AppData)Session["login"]).Utilizator;
           c.ExecuteNonQuery();
       }
       Response.Redirect(Request.RawUrl);
    }
    //cand schimb din profil public in privat sau invers
    protected void Button3_Click(object sender, EventArgs e)
    {
        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        String tip="neselectat";
        if (Public.Checked) tip="public";
        if (Privat.Checked) tip = "privat";
        //daca nu e selctata nici o optiune nu fac nimic
        if (String.Compare(tip, "neselectat") == 0) return;
        SqlCommand c = new SqlCommand("UPDATE profiluri SET tip=@tip WHERE id=(SELECT id FROM profiluri WHERE UPPER(persoana_username)=UPPER(@user))", conn);
        c.Parameters.Add(new SqlParameter("@tip", TypeCode.String));
        c.Parameters["@tip"].Value = tip;
        c.Parameters.Add(new SqlParameter("@user", TypeCode.String));
        c.Parameters["@user"].Value = ((AppData)Session["login"]).Utilizator;
        c.ExecuteNonQuery();
        conn.Close();
        Response.Redirect(Request.RawUrl);
    }
    //este folosit pentru a se alatura grupului daca suntem pe pagina de profil a unui grup
    protected void Button4_Click(object sender, EventArgs e)
    {
        //daca ajung aici stiu ca exista query stringul si ca acesta este pentru grupuri
        String nume_profil= Request.QueryString["profil_grup"];

        String utilizator = ((AppData)Session["login"]).Utilizator;
        SqlConnection conn;
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        conn.Open();
        SqlCommand c = new SqlCommand("INSERT INTO persoane_grupuri(grup_name,persoana_username) VALUES(@grup_name,@persoana_username)", conn);
        c.Parameters.Add(new SqlParameter("@grup_name", TypeCode.String));
        c.Parameters["@grup_name"].Value = nume_profil;
        c.Parameters.Add(new SqlParameter("@persoana_username", TypeCode.String));
        c.Parameters["@persoana_username"].Value = utilizator;
        c.ExecuteNonQuery();
        conn.Close();
        Response.Redirect(Request.RawUrl);
    }
    //trimitere catre albume 
    protected void Button5_Click(object sender, EventArgs e)
    {
       String nume_profil = Request.QueryString["profil_persoana"];
       Response.Redirect("Poze.aspx?profil_persoana=" + nume_profil);
    }
}