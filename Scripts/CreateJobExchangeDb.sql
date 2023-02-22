CREATE DATABASE JobExchangeDB
ON PRIMARY
(
      NAME = N'JobExchangeDB',
      FILENAME = N'D:\Projects\Core7\db\JobExchangeDB.mdf'
)
LOG ON
(
     NAME = N'JobExchangeDB_log',
     FILENAME = N'D:\Projects\Core7\db\JobExchangeDB_log.ldf'
)
GO

USE JobExchangeDB
GO

CREATE TABLE Companies
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(250) NOT NULL,
	Country VARCHAR(100),
	City VARCHAR(100),
	Address VARCHAR(500),
)

INSERT INTO Companies(Name, Country, City, Address) VALUES ('IBA', 'Belarus', 'Gomel', 'Universy 15 Street')
INSERT INTO Companies(Name, Country, City, Address) VALUES ('EPAM', 'Belarus', 'Gomel', 'Lizukova 19')

CREATE TABLE Departments
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(100),
	CompanyId int foreign key references Companies(Id),
)

INSERT INTO Departments(Name, CompanyId) VALUES ('Software Development Department', 1)

CREATE TABLE Specialties
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(400),
	Responsibilities VARCHAR(4000),
)

INSERT INTO Specialties(Name, Responsibilities) VALUES ('Web developer', 'The Development front-end part. Tech stack - HTML, CSS, JavaScript, TypeScript')
INSERT INTO Specialties(Name, Responsibilities) VALUES ('.Net full-stack developer', 'The Development front-end and back-end parts. Tech stack - C#, HTML, CSS, JavaScript')
INSERT INTO Specialties(Name, Responsibilities) VALUES ('Java full-stack developer', 'The Development front-end and back-end parts. Tech stack - Java, HTML, CSS, JavaScript, TypeScript')

CREATE TABLE Roles
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50),
	Responsibilities VARCHAR(4000),
)

INSERT INTO Roles(Name, Responsibilities) VALUES ('Junior', 'Development')
INSERT INTO Roles(Name, Responsibilities) VALUES ('Middle', 'Development')
INSERT INTO Roles(Name, Responsibilities) VALUES ('Senior', 'Development')
INSERT INTO Roles(Name, Responsibilities) VALUES ('Team leader', 'Development team management')

CREATE TABLE Avatars
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(100),
	Image Image
)


CREATE TABLE Users
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Username VARCHAR(100) NOT NULL,
	FirstName VARCHAR(100),
	LastName VARCHAR(100),
	Password VARCHAR(100) NOT NULL,
	Token VARCHAR(500),
	Email VARCHAR(100),
	Role VARCHAR(50),
	RefreshToken VARCHAR(500),
	RefreshTokenExpiryTime Datetime,
)

INSERT INTO Users(Username, Password, Role, FirstName, LastName, Email) 
	VALUES ('velogos', 'veles1988', 'admin', 'Vasil', 'Veliasnitski', 'velogos@rambler.ru')
INSERT INTO Users(Username, Password, Role, FirstName, LastName, Email) 
	VALUES ('ivan2000', 'ivan2000', 'user', 'Ivan', 'Petrov', 'petrovVanja2345@gmail.com')
INSERT INTO Users(Username, Password, Role, FirstName, LastName, Email) 
	VALUES ('semenivanov', 'ivanov213123', 'user', 'Semen', 'Ivanov', 'ivanovR234@gmail.com')

CREATE TABLE Staff
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	FirstName VARCHAR(200) NOT NULL,
	LastName VARCHAR(200) NOT NULL,
	DataOfBirth DateTime2(7) NOT NULL,
	Phone VARCHAR(200),
	Email VARCHAR(200),
	DepartmentId int foreign key references Departments(Id),
	SpecId int foreign key references Specialties(Id),
	RoleId int foreign key references Roles(Id),
	AvatarId int foreign key references Avatars(Id),
	UserId int foreign key references Users(Id),
)

INSERT INTO Staff(FirstName, LastName, DataOfBirth, Phone, Email, DepartmentId, SpecId, RoleId) 
	VALUES ('Vasil', 'Veliasnitski', '03/19/1988', '+375291407134', 'velogos@rambler.ru', 1, 2, 2)
INSERT INTO Staff(FirstName, LastName, DataOfBirth, Phone, Email, DepartmentId, SpecId, RoleId) 
	VALUES ('Ivan', 'Petrov', '03/02/2000', '+375291307435', 'petrovVanja2345@gmail.com', 1, 1, 1)
INSERT INTO Staff(FirstName, LastName, DataOfBirth, Phone, Email, DepartmentId, SpecId, RoleId) 
	VALUES ('Semen', 'Ivanov', '01/01/1999', '+375295677164', 'ivanovR234@gmail.com', 1, 2, 4)

CREATE TABLE TaskStatuses
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Description VARCHAR(5000),
)

INSERT INTO TaskStatuses(Description) VALUES ('Active')
INSERT INTO TaskStatuses(Description) VALUES ('Closed')
INSERT INTO TaskStatuses(Description) VALUES ('Completed')
INSERT INTO TaskStatuses(Description) VALUES ('In progress')

CREATE TABLE Tasks
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Title VARCHAR(100) NOT NULL,
	Description VARCHAR(5000),
	CreateDate DateTime2(7) NOT NULL,
	UpdateDate DateTime2(7) NOT NULL,
	StatusId int foreign key references TaskStatuses(Id),
	CreaterId int foreign key references Staff(Id),
	AppointerId int foreign key references Staff(Id),
	ImplementerId int foreign key references Staff(Id),
)

INSERT INTO Tasks(Title, Description, CreateDate, UpdateDate, StatusId, CreaterId, AppointerId, ImplementerId) 
	VALUES ('Create new project', 'Create asp.net core mvc', GETDATE(), GETDATE(), 1, 3, 3, 1)
INSERT INTO Tasks(Title, Description, CreateDate, UpdateDate, StatusId, CreaterId, AppointerId, ImplementerId) 
	VALUES ('Create new web project', 'Create angular project', GETDATE(), GETDATE(), 1, 3, 3, 2)
INSERT INTO Tasks(Title, Description, CreateDate, UpdateDate, StatusId, CreaterId, AppointerId, ImplementerId) 
	VALUES ('Create new db', 'Create MS SQL DB ', GETDATE(), GETDATE(), 1, 3, 3, 3)