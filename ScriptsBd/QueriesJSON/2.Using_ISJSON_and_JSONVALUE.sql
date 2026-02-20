USE TaskManagementDB;

-- Consulta que obtiene las tareas junto con la información del usuario asociado, validando que la columna TaskPriority contenga un 
-- JSON válido, extrayendo el valor del campo "priority" desde el JSON y filtrando únicamente aquellas tareas cuya prioridad sea 'high'.
SELECT 
    t.Id,
    t.Title,
    t.Description,
    t.Status,
    JSON_VALUE(t.TaskPriority, '$.priority') AS Priority,
    t.CreatedAt,
    u.Id AS UserId,
    u.Name,
    u.Email
FROM dbo.[Tasks] t
INNER JOIN dbo.[User] u ON t.UserId = u.Id
WHERE 
    ISJSON(t.TaskPriority) = 1
    AND JSON_VALUE(t.TaskPriority, '$.priority') = 'high';