
insert into [Order] (CustomerId, PaymentTypeId, OrderDate, IsCompleted) Values (1, 1, '2019-11-06', 1)
insert into [Order] (CustomerId, PaymentTypeId, OrderDate, IsCompleted) Values (2, 2, '2019-11-06', 1)
insert into [Order] (CustomerId, PaymentTypeId, OrderDate, IsCompleted) Values (3, 3, '2019-11-06', 1)
insert into [Order] (CustomerId, PaymentTypeId, OrderDate, IsCompleted) Values (4, 4, '2019-11-06', 1)
insert into [Order] (CustomerId, PaymentTypeId, OrderDate, IsCompleted) Values (5, 5, '2019-11-06', 1)
insert into OrderProduct (OrderId, ProductId) Values (1, 5)
insert into OrderProduct (OrderId, ProductId) Values (2, 8)
insert into OrderProduct (OrderId, ProductId) Values (3, 1)
insert into OrderProduct (OrderId, ProductId) Values (4, 10)
insert into OrderProduct (OrderId, ProductId) Values (5, 9)

UPDATE [ORDER] Set IsCompleted = 0 WHERE Id = 5