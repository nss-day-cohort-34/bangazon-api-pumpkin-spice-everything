--INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Andy', 'Collins', 2019-11-05, 2019-11-05, 1);
--INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Adam', 'Schaffer', 2019-11-05, 2019-11-05, 1);
--INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Steve', 'Brownlee', 2019-11-05, 2019-11-05, 1);
--INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Jenna', 'Solis', 2019-11-05, 2019-11-05, 1);
--INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Bryan', 'Nilsen', 2019-11-05, 2019-11-05, 1);

--INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Carl', 'Barringer', 5, 1, 2019-11-05);
--INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Bennett', 'Foster', 1, 1, 2019-11-05);
--INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Brian', 'Wilson', 4, 1, 2019-11-05);
--INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Kevin', 'Sadler', 2, 1, 2019-11-05);
--INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Allison', 'Patton', 3, 1, 2019-11-05);

--INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'G3', 'Dell');
--INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'MacbookPro', 'Apple');
--INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'Stealth', 'MSI');
--INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'Aurora', 'Alienware');
--INSERT INTO COMPUTER (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES (2019-11-05, null, 'Blade15', 'Razor');

--INSERT INTO ProductType (TypeName) VALUES ('Electronics');
--INSERT INTO ProductType (TypeName) VALUES ('Home');
--INSERT INTO ProductType (TypeName) VALUES ('Auto');
--INSERT INTO ProductType (TypeName) VALUES ('Sports');
--INSERT INTO ProductType (TypeName) VALUES ('Clothing');

--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Tablet', 1, 400, 3, 'A tablet you can surf the web on', 5);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Roomba', 1, 300, 4, 'A robot vacuum cleaner', 1);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Toaster', 2, 40, 1, 'A brave little toaster.', 2);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Pillow', 2, 25, 7, 'An incredibly hard pillow that hurts your neck.', 3);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Alternator', 3, 130, 64, 'Alternates the car.', 5);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Floormat', 3, 30, 20, 'Already dirty', 5);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Futball', 4, 14.99, 30, 'European Ball, Inflate Yourself', 2);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Tennis Racket', 4, 29.98, 10, 'Used by Venus, no strings', 2);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Shirt', 5, 10.99, 12, 'Mesh', 2);
--INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Pants', 5, 19.99, 12, 'Leather', 2);

--INSERT INTO Department ([Name], Budget) VALUES ('Human Resources', 120000);
--INSERT INTO Department ([Name], Budget) VALUES ('Information Technology', 250000);
--INSERT INTO Department ([Name], Budget) VALUES ('Accounting', 110000);
--INSERT INTO Department ([Name], Budget) VALUES ('Marketing', 170000);
--INSERT INTO Department ([Name], Budget) VALUES ('Warehouse', 90000);

--INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('Forklift Driving', 2019-11-05, 2019-12-31, 10);
--INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('Business Ethics', 2019-11-05, 2019-12-31, 10);
--INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('Maths', 2019-11-05, 2019-12-31, 10);
--INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('Design', 2019-11-05, 2019-12-31, 10);
--INSERT INTO TrainingProgram (ProgramName, StartDate, EndDate, MaxAttendees) VALUES ('C#', 2019-11-05, 2019-12-31, 10);

--INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (1, '1234567','Visa');
--INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (2, '98765','Apple Pay');
--INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (3, '345676','American Express');
--INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (4, '12345789','Gift Card');
--INSERT INTO PaymentType (CustomerId, AcctNumber, [Type]) VALUES (5, '565656565','Money Order');

SELECT Id, FirstName, LastName, CreationDate, LastActiveDate, IsActive
                                          FROM Customer

UPDATE Customer SET LastActiveDate = '2019-11-05';

SELECT Id, FirstName, LastName, CreationDate, LastActiveDate, IsActive
                                          FROM Customer
                                        WHERE Id = 3

						INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate, IsActive)
    OUTPUT INSERTED.Id
    VALUES ('Brenda', 'Long', '2019-11-05', '2019-11-05', 1)

	 UPDATE Customer
                            SET FirstName = 'Linda', LastName = 'Williams', CreationDate = '2019-11-05', LastActiveDate = '2019-11-05', IsActive = 1
                            WHERE Id = 6

SELECT c.Id as CustomerId, c.FirstName, c.LastName, c.CreationDate, c.LastActiveDate, c.IsActive,
                                                   p.Id as ProductId, p.ProductName, p.ProductTypeId as ProductTypeId, p.Price, p.Quantity, p.Description
                                          FROM Customer c LEFT JOIN Product p ON c.Id = p.CustomerId



