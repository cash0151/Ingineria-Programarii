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
