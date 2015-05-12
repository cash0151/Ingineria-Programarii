DROP TABLE [dbo] . [Participanti] ;
DROP TABLE [dbo] . [Preferinte] ;
DROP TABLE [dbo] . [Reviewuri] ;
DROP TABLE [dbo] . [Cursuri] ;
DROP TABLE [dbo] . [Categorii_Cursuri] ;
DROP TABLE [dbo] . [Useri] ;



CREATE TABLE [dbo].[Useri] (
    [Id]  INT          IDENTITY (1, 1) NOT NULL,
	[Nume] VARCHAR(50) NOT NULL,
	[Parola] VARCHAR(50) NOT NULL,
    [Tip] VARCHAR(50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
	);
CREATE TABLE [dbo].[Categorii_Cursuri] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [NumeCategorie] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Cursuri] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [NumeCurs]  VARCHAR (50) NOT NULL,
    [Profesor]  INT          NOT NULL,
    [Categorie] INT          NULL,
	[Continut] VARCHAR(5000) NOT NULL,
	[Locatie] VARCHAR(5000) NOT NULL,
	[Program] VARCHAR(5000) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Cursuri_ToTable] FOREIGN KEY ([Categorie]) REFERENCES [dbo].[Categorii_Cursuri] ([Id]),
    CONSTRAINT [FK_Cursuri_ToTable_1] FOREIGN KEY ([Profesor]) REFERENCES [dbo].[Useri] ([Id])
);
CREATE TABLE [dbo].[Reviewuri] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [CursId]     INT           NULL,
    [ProfesorId] INT           NULL,
    [Nota]       INT           NULL,
    [Text]       VARCHAR (500) NULL,
	[UserId]   	INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Reviewuri_ToTable] FOREIGN KEY ([ProfesorId]) REFERENCES [dbo].[Useri] ([Id]),
    CONSTRAINT [FK_Reviewuri_ToTable_1] FOREIGN KEY ([CursId]) REFERENCES [dbo].[Cursuri] ([Id])
);
CREATE TABLE [dbo].[Preferinte] (
    [IdUser]    INT NOT NULL,
    [Categorie] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([IdUser] ASC, [Categorie] ASC),
    CONSTRAINT [FK_Preferinte_ToTable] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[Useri] ([Id]),
    CONSTRAINT [FK_Preferinte_ToTable_1] FOREIGN KEY ([Categorie]) REFERENCES [dbo].[Categorii_Cursuri] ([Id])
);
CREATE TABLE [dbo].[Participanti] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [IdCurs] INT          NOT NULL,
    [IdUser] INT          NOT NULL,
    [Status] VARCHAR (50) NOT NULL,
    [Vazut] NVARCHAR(50) NOT NULL DEFAULT 'NOT_SEEN', 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Participanti_ToTable] FOREIGN KEY ([IdCurs]) REFERENCES [dbo].[Useri] ([Id]),
    CONSTRAINT [FK_Participanti_ToTable_1] FOREIGN KEY ([IdCurs]) REFERENCES [dbo].[Cursuri] ([Id])
);






INSERT INTO Useri(Nume,Parola,Tip) VALUES('a','a','normal');
INSERT INTO Useri(Nume,Parola,Tip) VALUES('b','b','normal');
INSERT INTO Useri(Nume,Parola,Tip)  VALUES('c','c','normal');
INSERT INTO Useri(Nume,Parola,Tip) VALUES('d','d','normal');
INSERT INTO Useri(Nume,Parola,Tip) VALUES('e','e','profesor');
INSERT INTO Useri(Nume,Parola,Tip) VALUES('f','f','profesor');
INSERT INTO Useri(Nume,Parola,Tip) VALUES('h','h','profesor');
INSERT INTO Useri(Nume,Parola,Tip) VALUES('i','i','profesor');
--INSERT INTO Categorii_Cursuri VALUES('');
INSERT INTO Categorii_Cursuri VALUES('Dezvoltare Web');
INSERT INTO Categorii_Cursuri VALUES('Algoritmi');
INSERT INTO Categorii_Cursuri VALUES('Limbaje procedurale');
INSERT INTO Categorii_Cursuri VALUES('Limbaje functionale');
INSERT INTO Categorii_Cursuri VALUES('Tehnici de dezvoltare');
INSERT INTO Categorii_Cursuri VALUES('Securitate');
INSERT INTO Categorii_Cursuri VALUES('Compilatoare');
INSERT INTO Categorii_Cursuri VALUES('Grafica');

INSERT INTO [dbo].[Cursuri] ( [NumeCurs], [Profesor], [Categorie], [Continut], [Locatie], [Program]) VALUES ( N'SISTEME DE GESTIUNE A BAZELOR DE DATE', 5, 3, N'&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspConcepte&nbspgenerale.&nbspStructura&nbspfizica&nbspsi&nbsplogica&nbspa&nbspunui&nbspSGBD.&nbspArhitectura&nbspde&nbspreferinta&nbspa&nbspunui&nbspSGBD.<br/>Evolutie.&nbspParticularizare&nbspla&nbsparhitectura&nbspOracle9i&nbsp(Oracle9i&nbspDatabase,&nbspApplication&nbspServer,&nbspDeveloper&nbspSuite).<br/>Arhitectura&nbspmultitier,&nbspstructura&nbspfizica&nbspsi&nbsplogica&nbspa&nbspbazei&nbspde&nbspdate,&nbsparhitectura&nbspinterna&nbsp(memorie,&nbspprocese),<br/>gestionarea&nbspsi&nbspprelucrarea&nbspbazei&nbspde&nbspdate.&nbspSecuritatea&nbspbazei&nbspde&nbspdate&nbsp(administrare&nbsputilizatori&nbspsi&nbspresurse,&nbspprofiluri,<br/>privilegii,&nbsprole-uri,&nbspauditare).<br/>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspImplementarea&nbspprocedurala&nbspa&nbspmodelelor&nbspproiectate&nbspîn&nbspcadrul&nbspcursului&nbspde&nbspbaze&nbspde&nbspdate.&nbspProcedural<br/>Language/SQL.&nbspStructuri&nbspcomplexe&nbspde&nbspdate,&nbsptipuri&nbspobiect&nbspsi&nbspcolectie,&nbspblocuri,&nbspcomenzi,&nbspcursoare.&nbspModularizare<br/>prin&nbsputilizarea&nbspsubprogramelor&nbspsi&nbspa&nbsppachetelor.&nbspImplementarea&nbspdeclansatorilor&nbsp(trigger).&nbspTratarea&nbspsi&nbspgestiunea<br/>erorilor.&nbspSQL&nbspdinamic.&nbspToate&nbspexemplificarile&nbspse&nbspvor&nbspreferi&nbspla&nbspOracle9i.', N'Bulevardul&nbspTimisoara&nbspnr&nbsp50&nbspbloc&nbspA23&nbspetaj&nbsp1&nbspap&nbsp32', N'Luni-Miercuri:&nbsp12:00-15:00<br/>Joi-Vineri:&nbsp&nbsp&nbsp&nbsp17:00-19:00')
INSERT INTO [dbo].[Cursuri] ([NumeCurs], [Profesor], [Categorie], [Continut], [Locatie], [Program]) VALUES (N'DEZVOLTAREA APLICATIILOR WEB', 5, 1, N'&nbspScopul&nbspcursului&nbspeste&nbspprezentarea&nbsptehnicilor&nbspsi&nbsptehnologiilor&nbspnecesare&nbsppentru&nbspdezvoltarea&nbspde&nbspaplicatii&nbspweb&nbsppe<br/>partea&nbspde&nbspserver:<br/>*&nbspPlatforma&nbsp.NET<br/>*&nbspLimbajul&nbspde&nbspprogramare&nbspC#<br/>*&nbspASP.NET&nbsp(platforma&nbspde&nbspdezvoltare&nbspa&nbspaplicatiilor&nbspWeb&nbspde&nbspla&nbspMicrosoft)<br/>*&nbspXML<br/>', N'Bulevardul&nbspTimisoara&nbspnr&nbsp50&nbspbloc&nbspA23&nbspetaj&nbsp1&nbspap&nbsp32.<br/>', N'Luni-Miercuri:&nbsp12:00-15:00<br/>Joi-Vineri:&nbsp&nbsp&nbsp&nbsp17:00-19:00')
INSERT INTO [dbo].[Cursuri] ( [NumeCurs], [Profesor], [Categorie], [Continut], [Locatie], [Program]) VALUES (N'TEHNICI DE SIMULARE', 5, 2, N'1.&nbspNotiuni&nbspintroductive&nbspde&nbspsimulare&nbspa&nbspsistemelor.<br/>2.&nbspGenerarea&nbspnumerelor&nbspaleatoare.<br/>3.&nbspMetode&nbspgenerale&nbspde&nbspsimulare&nbspa&nbspvariabilelor&nbspaleatoare.<br/>4.&nbspSimularea&nbspunor&nbspvariabile&nbspcontinue&nbspparticulare.<br/>5.&nbspSimularea&nbspunor&nbspvariabile&nbspdiscrete&nbspparticulare.<br/>6.&nbspModele&nbspde&nbspsimulare&nbsppentru&nbspsisteme&nbspde&nbspasteptare.<br/>7.&nbspSimularea&nbspsistemelor&nbspinformatice.', N'Bulevardul&nbspTimisoara&nbspnr&nbsp50&nbspbloc&nbspA23&nbspetaj&nbsp1&nbspap&nbsp32.<br/>', N'Luni-Miercuri:&nbsp12:00-15:00<br/>Joi-Vineri:&nbsp&nbsp&nbsp&nbsp17:00-19:00')
INSERT INTO [dbo].[Cursuri] ( [NumeCurs], [Profesor], [Categorie], [Continut], [Locatie], [Program]) VALUES (N'TEHNICI DE OPTIMIZARE', 5, 2, N'-&nbspModele&nbspde&nbspoptimizare&nbspliniara&nbspsi&nbspprograme&nbspsoftware.<br/>-&nbspAlgoritmul&nbspsimplex&nbspprimal&nbspsi&nbspalgoritmul&nbspsimplex&nbspdual.<br/>-&nbspInterpretarea&nbspeconomica&nbspa&nbspvalorilor&nbspsi&nbspsolutiilor.<br/>-&nbspMetode&nbspde&nbsppartitionare&nbspsi&nbsprelaxare.<br/>-&nbspMetode&nbsppentru&nbspprobleme&nbspde&nbspoptimizare&nbspneliniara.<br/>', N'Bulevardul&nbspTimisoara&nbspnr&nbsp50&nbspbloc&nbspA23&nbspetaj&nbsp1&nbspap&nbsp32.<br/>', N'Luni-Miercuri:&nbsp12:00-15:00<br/>Joi-Vineri:&nbsp&nbsp&nbsp&nbsp17:00-19:00')
INSERT INTO [dbo].[Cursuri] ([NumeCurs], [Profesor], [Categorie], [Continut], [Locatie], [Program]) VALUES ( N'INGINERIA PROGRAMARII', 6, 5, N'Sisteme&nbspOrientate&nbsppe&nbspobiecte:<br/>&nbsp&nbspParadigma&nbsporientarii&nbsppe&nbspobiecte;&nbspConcepte&nbspavansate&nbsp(incapsulare,&nbspgeneralizare,&nbspmostenire)<br/>&nbsp&nbspSintaxa&nbspsi&nbspsemantica&nbsplimbajului&nbspUML&nbsp(Unified&nbspModeling&nbspLanguage)<br/>&nbsp&nbspÎnsusirea&nbspprogramului&nbspVisio&nbsppentru&nbsprealizarea&nbspdiagramelor&nbspUML<br/>Modelare&nbspStructurala:<br/>&nbsp&nbspDiagrama&nbspcazurilor&nbspde&nbsputilizare<br/>&nbsp&nbspDiagrama&nbspde&nbspclase&nbspsi&nbspde&nbspobiecte<br/>&nbsp&nbspDiagrama&nbspde&nbspcomponente&nbspsi&nbspde&nbspdesfasurare<br/>Modelare&nbspDinamica:<br/>&nbsp&nbspDiagrama&nbspde&nbspcolaborare<br/>&nbsp&nbspDiagrama&nbspde&nbspsecventa<br/>&nbsp&nbspDiagrama&nbspde&nbspstare<br/>&nbsp&nbspDiagrama&nbspde&nbspactivitate<br/>Proiectarea&nbspPaginilor&nbspWeb:<br/>&nbsp&nbspRealizarea&nbspsite-urilor&nbspdinamice&nbsputilizând&nbspHTML,&nbspCSS,&nbspJavaScript,&nbspPHP', N'Bulevardul&nbspTimisoara&nbspnr&nbsp50&nbspbloc&nbspA23&nbspetaj&nbsp1&nbspap&nbsp32.', N'Luni-Miercuri:&nbsp12:00-15:00<br/>Joi-Vineri:&nbsp&nbsp&nbsp&nbsp17:00-19:00')



INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('Php',5,'PHP',1);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('Asp.net',5,'',1);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('HTML , CSS si Javascript',6,'HTML , CSS si Javascript',1);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('C',7,'c',3);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('Java',8,'Java',3);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('C++',8,'C++',3);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('C#',6,'C#',3);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('PL/SQL',5,'PL/SQL',3);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('Programare Dinamica',6,'Programare Dinamica',2);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('Divide et Impera',6,'Divide et Impera',2);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('Metoda Greedy',6,'Metoda Greedy',3);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('SQL',7,'SQL',4);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('UML',7,'UML',5);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('Dezvoltare agila',7,'Dezvoltare agila',5);
INSERT INTO Cursuri(numeCurs,Profesor,Continut,Categorie) VALUES('Dezvoltare in cascada',7,'Dezvoltare in cascada',5);


