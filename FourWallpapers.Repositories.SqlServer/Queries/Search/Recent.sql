--DEBUG ONLY VARIABLES... THESE WILL BE FILLED IN VIA PARAMS IN CODE
--DECLARE @PerPage TINYINT = 100;
--END OF DEBUG ONLY VARIABLES

;WITH [resultsCte]
AS (
	SELECT DISTINCT TOP (@PerPage) IMG.*
	FROM [Image] IMG
    WHERE
		IMG.[Reported] IN (0,3)
    ORDER BY IMG.[DateDownloaded] DESC
	)

SELECT IMG.*
	,K.*
FROM [resultsCte] IMG
LEFT JOIN ImageKeyword IK ON IK.[ImageIdKey] = IMG.[IdKey]
LEFT JOIN Keyword K ON K.[IdKey] = IK.KeywordIdKey
ORDER BY IMG.[DateDownloaded] DESC