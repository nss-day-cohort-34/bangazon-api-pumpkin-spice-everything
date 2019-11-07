----USE MASTER
----GO

----IF NOT EXISTS (
----    SELECT [name]
----    FROM sys.databases
----    WHERE [name] = N'BangazonAPITest'
----)
----CREATE DATABASE BangazonAPITest
----GO

----USE BangazonAPITest
----GO

----DROP TABLE IF EXISTS OrderProduct;
----DROP TABLE IF EXISTS [Order];
----DROP TABLE IF EXISTS PaymentType;
----DROP TABLE IF EXISTS Product;
----DROP TABLE IF EXISTS Customer;
----DROP TABLE IF EXISTS ProductType;
----DROP TABLE IF EXISTS EmployeeTraining;
----DROP TABLE IF EXISTS TrainingProgram;
----DROP TABLE IF EXISTS ComputerEmployee;
----DROP TABLE IF EXISTS Computer;
----DROP TABLE IF EXISTS Employee;
----DROP TABLE IF EXISTS Department;

----CREATE TABLE Department (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	[Name] VARCHAR(55) NOT NULL,
----	Budget 	INTEGER NOT NULL
----);

----CREATE TABLE Employee (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	FirstName VARCHAR(55) NOT NULL,
----	LastName VARCHAR(55) NOT NULL,
----	DepartmentId INTEGER NOT NULL,
----	IsSupervisor BIT NOT NULL,
----	StartDate DATETIME NOT NULL,
----    CONSTRAINT FK_EmployeeDepartment FOREIGN KEY(DepartmentId) REFERENCES Department(Id)
----);

----CREATE TABLE Computer (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	PurchaseDate DATETIME NOT NULL,
----	DecomissionDate DATETIME,
----	Make VARCHAR(55) NOT NULL,
----	Manufacturer VARCHAR(55) NOT NULL
----);

----CREATE TABLE ComputerEmployee (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	EmployeeId INTEGER NOT NULL,
----	ComputerId INTEGER NOT NULL,
----	AssignDate DATETIME NOT NULL,
----	UnassignDate DATETIME,
----    CONSTRAINT FK_ComputerEmployee_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
----    CONSTRAINT FK_ComputerEmployee_Computer FOREIGN KEY(ComputerId) REFERENCES Computer(Id)
----);


----CREATE TABLE TrainingProgram (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	ProgramName VARCHAR(255) NOT NULL,
----	StartDate DATETIME NOT NULL,
----	EndDate DATETIME NOT NULL,
----	MaxAttendees INTEGER NOT NULL
----);

----CREATE TABLE EmployeeTraining (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	EmployeeId INTEGER NOT NULL,
----	TrainingProgramId INTEGER NOT NULL,
----    CONSTRAINT FK_EmployeeTraining_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
----    CONSTRAINT FK_EmployeeTraining_Training FOREIGN KEY(TrainingProgramId) REFERENCES TrainingProgram(Id)
----);

----CREATE TABLE ProductType (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	TypeName VARCHAR(55) NOT NULL
----);

----CREATE TABLE Customer (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	FirstName VARCHAR(55) NOT NULL,
----	LastName VARCHAR(55) NOT NULL,
----	CreationDate DATETIME NOT NULL,
----	LastActiveDate DATETIME NOT NULL,
----	IsActive BIT NOT NULL
----);

----CREATE TABLE Product (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	ProductTypeId INTEGER NOT NULL,
----	CustomerId INTEGER NOT NULL,
----	Price MONEY NOT NULL,
----	ProductName VARCHAR(255) NOT NULL,
----	[Description] VARCHAR(255) NOT NULL,
----	Quantity INTEGER NOT NULL,
----    CONSTRAINT FK_Product_ProductType FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id),
----    CONSTRAINT FK_Product_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
----);


----CREATE TABLE PaymentType (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	AcctNumber VARCHAR(55) NOT NULL,
----	[Type] VARCHAR(55) NOT NULL,
----	CustomerId INTEGER NOT NULL,
----    CONSTRAINT FK_PaymentType_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
----);

----CREATE TABLE [Order] (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	CustomerId INTEGER NOT NULL,
----	PaymentTypeId INTEGER,
----	OrderDate DATETIME,
----    CONSTRAINT FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id),
----    CONSTRAINT FK_Order_Payment FOREIGN KEY(PaymentTypeId) REFERENCES PaymentType(Id)
----);

----CREATE TABLE OrderProduct (
----	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
----	OrderId INTEGER NOT NULL,
----	ProductId INTEGER NOT NULL,
----    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY(ProductId) REFERENCES Product(Id),
----    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY(OrderId) REFERENCES [Order](Id)
----);

INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Andy', 'Collins', 2019-11-05, 2019-11-05, 1);
INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Adam', 'Schaffer', 2019-11-05, 2019-11-05, 1);
INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Steve', 'Brownlee', 2019-11-05, 2019-11-05, 1);
INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Jenna', 'Solis', 2019-11-05, 2019-11-05, 1);
INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Bryan', 'Nilsen', 2019-11-05, 2019-11-05, 1);

INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Bennett', 'Foster', 1, 1, 2019-11-05);
INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Brian', 'Wilson', 4, 1, 2019-11-05);
INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Kevin', 'Sadler', 2, 1, 2019-11-05);
INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Allison', 'Patton', 3, 1, 2019-11-05);

INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'G3', 'Dell');
INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'MacbookPro', 'Apple');
INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'Stealth', 'MSI');
INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'Aurora', 'Alienware');
INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'Blade15', 'Razor');

INSERT INTO ProductType (TypeName) VALUES ('Electronics');
INSERT INTO ProductType (TypeName) VALUES ('Home');
INSERT INTO ProductType (TypeName) VALUES ('Auto');
INSERT INTO ProductType (TypeName) VALUES ('Sports');
INSERT INTO ProductType (TypeName) VALUES ('Clothing');

INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Tablet', 1, 400, 3, 'A tablet you can surf the web on', 5);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Toaster', 2, 40, 1, 'A brave little toaster.', 2);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Pillow', 2, 25, 7, 'An incredibly hard pillow that hurts your neck.', 3);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Alternator', 3, 130, 64, 'Alternates the car.', 5);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Floormat', 3, 30, 20, 'Already dirty', 5);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Futball', 4, 14.99, 30, 'European Ball, Inflate Yourself', 2);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Tennis Racket', 4, 29.98, 10, 'Used by Venus, no strings', 2);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Shirt', 5, 10.99, 12, 'Mesh', 2);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Pants', 5, 19.99, 12, 'Leather', 2);

INSERT INTO Department ([Name], Budget) VALUES ('Human Resources', 120000);
INSERT INTO Department ([Name], Budget) VALUES ('Information Technology', 250000);
INSERT INTO Department ([Name], Budget) VALUES ('Accounting', 110000);
INSERT INTO Department ([Name], Budget) VALUES ('Marketing', 170000);
INSERT INTO Department ([Name], Budget) VALUES ('Warehouse', 90000);

INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('Forklift Driving', 2019-11-05, 2019-12-31, 10);
INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('Business Ethics', 2019-11-05, 2019-12-31, 10);
INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('Maths', 2019-11-05, 2019-12-31, 10);
INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('Design', 2019-11-05, 2019-12-31, 10);
INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('C#', 2019-11-05, 2019-12-31, 10);

INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (1, '1234567','Visa');
INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (2, '98765','Apple Pay');
INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (3, '345676','American Express');
INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (4, '12345789','Gift Card');
INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (5, '565656565','Money Order');

UPDATE Customer
                            SET FirstName = 'John', LastName = 'Adams', CreationDate = '2019-05-05', LastActiveDate = '2019-05-05', IsActive = 1
                            WHERE Id = 1