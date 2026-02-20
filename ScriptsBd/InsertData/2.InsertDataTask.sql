USE TaskManagementDB;

INSERT INTO [TaskManagementDB].[dbo].[Tasks]
    ([Title], [Description], [Status], [UserId], [TaskPriority], [CreatedAt])
VALUES
(
    'Implement Login API',
    'Create login endpoint for users using JWT.',
    'Pending',
    1, 
    N'{"priority": "high"}',
    GETDATE()
),
(
    'Design Dashboard',
    'Create dashboard UI for admin panel.',
    'InProgress',
    2,
    N'{"priority": "medium"}',
    GETDATE()
),
(
    'Write Unit Tests',
    'Write unit tests for TaskController endpoints.',
    'Pending',
    1,
    N'{"priority": "low"}',
    GETDATE()
);
