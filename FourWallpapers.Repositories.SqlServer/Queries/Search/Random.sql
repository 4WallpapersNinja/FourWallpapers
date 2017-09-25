--DEBUG ONLY VARIABLES... THESE WILL BE FILLED IN VIA PARAMS IN CODE
--DECLARE @PerPage TINYINT = 100;
--END OF DEBUG ONLY VARIABLES
;WITH [results]
AS (
	SELECT DISTINCT IMG.*
		,[RandomKey] = ABS(CAST((BINARY_CHECKSUM(*) * RAND()) AS INT))
	FROM [Image] IMG
    WHERE
		IMG.[Reported] IN (0,3)
	)
	,[resultsCte]
AS (
	SELECT TOP (@PerPage) *
	FROM [results]
	ORDER BY RandomKey DESC
	)
SELECT IMG.*
	,K.*
FROM [resultsCte] IMG
LEFT JOIN ImageKeyword IK ON IK.[ImageIdKey] = IMG.[IdKey]
LEFT JOIN Keyword K ON K.[IdKey] = IK.KeywordIdKey