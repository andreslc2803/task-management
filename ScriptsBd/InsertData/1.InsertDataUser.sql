USE TaskManagementDB;

INSERT INTO [TaskManagementDB].[dbo].[User] ([Name], [Email], [CreatedAt])
VALUES 
('Andres Londoño', 'andres@example.com', GETDATE()),
('Maria Perez', 'maria@example.com', GETDATE()),
('John Doe', 'john@example.com', GETDATE());