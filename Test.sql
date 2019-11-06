SELECT p.Id, AcctNumber, p.[Type], p.CustomerId, c.Id, c.FirstName, c.LastName
FROM PaymentType p 
LEFT JOIN Customer c ON p.CustomerId = c.Id;
select * from customer;

;
--delete from PaymentType where Id = 13;

INSERT INTO PaymentType (AcctNumber, Type, CustomerId)
                        OUTPUT INSERTED.Id
                        VALUES (99999, 'testing', 1)
Select * from customer where customer.id = Id;
select * from PaymentType;

select * from PaymentType;
select * from customer;

--delete from customer where id = 10;

UPDATE PaymentType
    SET AcctNumber = '567567567',
        [Type] = 'new type',
        CustomerId = 1
    WHERE Id = 1;