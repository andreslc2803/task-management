USE TaskManagementDB;

-- Consulta que convierte el JSON de TaskPriority en formato tabular usando OPENJSON,
-- une la tarea con su usuario y filtra las tareas cuya prioridad sea 'high'.
SELECT 
    t.Id,
    t.Title,
    t.Description,
    t.Status,
    j.priority,
    u.Name,
    u.Email
FROM Tasks t
INNER JOIN dbo.[User] u 
    ON t.UserId = u.Id
CROSS APPLY OPENJSON(t.TaskPriority)
WITH (
    priority NVARCHAR(50) '$.priority'
) j
WHERE 
    ISJSON(t.TaskPriority) = 1
    AND j.priority = 'high';