IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EmergencyContacts')
BEGIN
    CREATE TABLE EmergencyContacts (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT,
        ContactName NVARCHAR(100),
        ContactPhone NVARCHAR(20)
    );
END