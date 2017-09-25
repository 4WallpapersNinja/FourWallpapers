SELECT 
    COUNT(*) 
FROM ImageKeyword 
WHERE 
    [KeywordIdKey] = @KeywordIdKey
    AND [ImageIdKey] = @ImageIdKey