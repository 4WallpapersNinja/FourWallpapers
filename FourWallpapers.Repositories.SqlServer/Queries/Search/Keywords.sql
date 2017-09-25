SELECT DISTINCT TOP 50 [Keyword] = K.Value
	,[Count] = COUNT(*)
FROM ImageKeyword IK
INNER JOIN IMAGE I ON I.IdKey = IK.ImageIdKey
	AND I.Reported IN (
		0
		,3
		)
INNER JOIN Keyword K ON K.IdKey = IK.KeywordIdKey
GROUP BY K.Value
ORDER BY COUNT(*) DESC