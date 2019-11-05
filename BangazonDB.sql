INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Andy', 'Collins', 2019-11-05, 2019-11-05, 1);
INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Adam', 'Schaffer', 2019-11-05, 2019-11-05, 1);
INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Steve', 'Brownlee', 2019-11-05, 2019-11-05, 1);
INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Jenna', 'Solis', 2019-11-05, 2019-11-05, 1);
INSERT INTO CUSTOMER (FirstName, LastName, CreationDate, LastActiveDate, IsActive) VALUES ('Bryan', 'Nilsen', 2019-11-05, 2019-11-05, 1);

INSERT INTO EMPLOYEE (FirstName, LastName, DepartmentId, IsSupervisor, StartDate) VALUES ('Carl', 'Barringer', 5, 1, 2019-11-05);
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

INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Tablet', 'Electronics', 400, 3, 'A tablet you can surf the web on', 5);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Roomba', 'Electronics', 300, 4, 'A robot vacuum cleaner', 1);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Toaster', 'Home', 40, 1, 'A brave little toaster.', 2);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Pillow', 'Home', 25, 7, 'An incredibly hard pillow that hurts your neck.', 3);
INSERT INTO Product (ProductName, ProductTypeId, Price, Quantity, [Description], CustomerId) VALUES ('Alternator', 'Auto', 130, 64, 'Alternates the car.', 5);






