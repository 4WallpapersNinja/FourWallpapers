SELECT IMG.*
	,K.*
FROM Image IMG
LEFT JOIN ImageKeyword IK
	ON IK.[ImageIdKey] = IMG.[IdKey]
LEFT JOIN Keyword K
	ON K.[IdKey] = IK.KeywordIdKey