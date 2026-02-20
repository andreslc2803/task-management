USE TaskManagementDB;

CREATE TABLE Tasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Status NVARCHAR(20) NOT NULL,
    UserId INT NOT NULL,
    TaskPriority NVARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserId)
        REFERENCES Users(Id),

    CONSTRAINT CK_Tasks_Status CHECK (Status IN ('Pending','InProgress','Done')),

    CONSTRAINT CK_Tasks_JSON CHECK (TaskPriority IS NULL OR ISJSON(TaskPriority) = 1)
);
