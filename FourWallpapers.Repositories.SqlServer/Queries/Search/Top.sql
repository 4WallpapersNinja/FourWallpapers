--DEBUG ONLY VARIABLES... THESE WILL BE FILLED IN VIA PARAMS IN CODE
--DECLARE @Class AS INT = 0
--DECLARE @Source AS INT = 0
--DECLARE @By AS NVARCHAR(16) = 'total'
--DECLARE @PerPage TINYINT = 100;
--END OF DEBUG ONLY VARIABLES
;WITH [resultsCte]
AS (
	SELECT DISTINCT TOP (@PerPage) IMG.*
	FROM [Image] IMG
	WHERE IMG.[Reported] IN (0, 3)
		AND (
			(
				LOWER(@By) = 'source'
				AND Img.[IndexSource] = @Source
				)
			OR (
				LOWER(@By) = 'classification'
				AND Img.[Class] = @Class
				)
			OR (
				LOWER(@By) = 'total'
				AND Img.[Class] IN (0, 1)
				)
			)
	ORDER BY Img.[Downloads] DESC
	)
SELECT IMG.*
	,K.*
FROM [resultsCte] IMG
LEFT JOIN ImageKeyword IK
	ON IK.[ImageIdKey] = IMG.[IdKey]
LEFT JOIN Keyword K
	ON K.[IdKey] = IK.KeywordIdKey
ORDER BY Img.[Downloads] DESC