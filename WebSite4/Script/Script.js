function RemoveComment(idComent) {
    PageMethods.RemoveComent(idComent, onSucess, onError);
    function onSucess(data) {
        /*codul de mai jos imi da reload la pagina*/
        window.location.reload(true)

    }
    function onError(data) {

    }
}
function AddComment(idComent) {
    var input = document.getElementById(idComent);
    var text = input.value;
    PageMethods.AddReply(idComent, text, onSucess, onError);
    function onSucess(data) {
    /*codul de mai jos imi da reload la pagina*/
        window.location.reload(true)


    }
    function onError(data) {

    }
}
function RemoveAlbum(idAlbum) {

    PageMethods.RemoveAlbum(idAlbum, onSucess, onError);
    function onSucess(data) {
    /*codul de mai jos imi da reload la pagina*/
        window.location.reload(true)


    }
    function onError(data) {

    }
}
function RemovePoza(idPoza) {

    PageMethods.RemovePoza(idPoza, onSucess, onError);
    function onSucess(data) {
        /*codul de mai jos imi da reload la pagina*/
        window.location.reload(true)


    }
    function onError(data) {

    }
}
function MarestePoza(idPoza) {

    id = "img" + idPoza;
    pozaOriginala = document.getElementById(id);
    var pozaCopie = document.createElement('img');
    pozaCopie.setAttribute("src", pozaOriginala.getAttribute("src"));
    pozaCopie.setAttribute("class", "pozaMare");
    var bigContainer = document.createElement('div');
    bigContainer.setAttribute("class", "pozaMareContainer");
    bigContainer.appendChild(pozaCopie);

    var xIcon = document.getElementById("xIcon");
    var xIconCopie = document.createElement('img');
    xIconCopie.setAttribute("src", xIcon.getAttribute("src"));
    xIconCopie.setAttribute("class", "vizibil");
    bigContainer.appendChild(xIconCopie);
    xIconCopie.addEventListener("click", function () {
        document.body.removeChild(bigContainer);

    }, false);



    var comentsContainer = document.createElement("div");
    comentsContainer.setAttribute("class", "comentsContainer");
    var input = document.createElement("input");
    input.setAttribute("id", "comentsContainerInput");
    input.setAttribute("type", "text");
    var buton = document.createElement("button");
    buton.setAttribute("type", "button");
    buton.setAttribute("id", "comentsContainerButton");
    buton.innerHTML = "Adauga Coment";
    comentsContainer.appendChild(input);
    comentsContainer.appendChild(buton);
    getComents(comentsContainer,idPoza);

  


   //baga comentul in baza de date
    buton.addEventListener("click", function () {
        var mesaj = input.value;
        PageMethods.AddComentPoza(idPoza, mesaj, onSucess, onError);
    }, false);

    function onSucess(data) {
        /*codul de mai jos imi da reload la pagina*/
        window.location.reload(true)
    }
    function onError(data) {
        alert(String(data));
    }

    bigContainer.appendChild(comentsContainer);
    document.body.appendChild(bigContainer);

    
}
function getComents(comentsContainer, idPoza) {
    PageMethods.GetComentsPoza(idPoza, onSucess, onError);
    function onSucess(data) {
        var rezultat = data.split(":123,p25ltDGGRTl-3lAB}}}!$#!###$#$#4;")
        var esteVizitator = rezultat[0];
        //aceasta functie nu se afla in interiorul celei ce o apeleaza asa ca nu stie cine sunt input si buton
        input = document.getElementById("comentsContainerInput");
        buton = document.getElementById("comentsContainerButton");
        if (esteVizitator == "da") {
            input.setAttribute("class", "invizibil");
            buton.setAttribute("class", "invizibil");
        }

        var arrayLength = rezultat.length;

        for (var i = 1; i < arrayLength - 1; i++) {

            var autorTextSiId = rezultat[i].split("geh35423l3,p2AB}}##asdw$,");
            var pozaComentContainer = document.createElement('div');
            pozaComentContainer.setAttribute("class", "pozaComentContainer");

            
            var paragraf = document.createElement('p');
            paragraf.innerHTML ="Autor: "+ autorTextSiId[0];
            pozaComentContainer.appendChild(paragraf);


            paragraf = document.createElement('p');
            paragraf.innerHTML ="Mesaj: "+ autorTextSiId[1];
            pozaComentContainer.appendChild(paragraf);

            var idComent = autorTextSiId[2];

            var dreptDeStergere = autorTextSiId[3];

           // alert(idComent);
            if (dreptDeStergere == "da") {
                var butonRemove = document.createElement("button");
                butonRemove.setAttribute("type", "button");
                butonRemove.setAttribute("id", idComent);
                butonRemove.innerHTML = "Sterge Coment";
                butonRemove.addEventListener("click", function () {
                    /*pun ca id idul comentului ce il sterge aceste buton mai sus si cand vreau sa sterg iau idul comentului
                    nu e problema sa avem duplicate de iduri pentru ca toate comenturile au iduri diferite si pozele au id cu img in fata*/
                    idul = this.getAttribute("id");
    
                    PageMethods.RemoveComent(idul, onSucess2, onError2);
                    function onSucess2(data) {
                        /*codul de mai jos imi da reload la pagina*/
                        window.location.reload(true)
                    }
                    function onError2(data) {
                        alert("esec");
                    }
                }, false);

                pozaComentContainer.appendChild(butonRemove);
            }


            comentsContainer.appendChild(pozaComentContainer);
           
        }
    }

    function onError(data) {

    }
}