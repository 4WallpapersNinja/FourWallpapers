--DEBUG ONLY VARIABLES... THESE WILL BE FILLED IN VIA PARAMS IN CODE
--DECLARE @Class AS INT = - 1
--DECLARE @Source AS INT = 9999
--DECLARE @Criteria AS NVARCHAR(1024) = 'asian, boobs'
--DECLARE @PerPage TINYINT = 100;
--DECLARE @Page INT = 1;
--DECLARE @ResolutionSearch INT = - 1
--DECLARE @Ratio NVARCHAR(16) = '16:9'
--DECLARE @X INT = '1600'
--DECLARE @Y INT = '900'
--DECLARE @Size DECIMAL(18, 0) = 0
--END OF DEBUG ONLY VARIABLES
IF (@Criteria <> '')
BEGIN
	DECLARE @KeywordMatches TABLE (
		[ImageIdKey] DECIMAL(18, 0) NOT NULL
		,[KeywordMatches] DECIMAL(3, 0) NOT NULL
		)
	DECLARE @FullMatch DECIMAL = 0

	SELECT @FullMatch = COUNT(*)
	FROM STRING_SPLIT(@Criteria, ',') Match

	--get all keyword matches and how many matches occurred
	INSERT INTO @KeywordMatches
	SELECT IK.[ImageIdKey]
		,(COUNT(*) / @FullMatch) * 100
	FROM STRING_SPLIT(@Criteria, ',') Match
	INNER JOIN [Keyword] K
		ON K.[Value] LIKE LTRIM(RTRIM(Match.[value]))
	LEFT JOIN [ImageKeyword] IK
		ON IK.[KeywordIdKey] = K.[IdKey]
	GROUP BY IK.[ImageIdKey];

	WITH [resultsCte]
	AS (
		SELECT DISTINCT IMG.*
			,KM.KeywordMatches
		FROM @KeywordMatches KM
		INNER JOIN [Image] IMG
			ON IMG.[IdKey] = KM.[ImageIdKey]
		LEFT JOIN [ImageKeyword] IK
			ON IK.[ImageIdKey] = IMG.[IdKey]
		LEFT JOIN [Keyword] K
			ON K.[IdKey] = IK.KeywordIdKey
		WHERE IMG.[Reported] IN (0, 3)
			AND (
				(
					@Source = 9999
					OR Img.[IndexSource] = @Source
					)
				AND (
					@Class = - 1
					OR Img.[Class] = @Class
					)
				AND (
					@ResolutionSearch = - 1
					OR (
						@ResolutionSearch = 0 --ExactMatch
						AND (
							Img.[ResolutionX] = @X
							AND Img.[ResolutionY] = @Y
							)
						)
					OR (
						@ResolutionSearch = 1 --EqualOrGreater
						AND (
							Img.[ResolutionX] >= @X
							AND Img.[ResolutionY] >= @Y
							)
						)
					OR (
						@ResolutionSearch = 2 --MaintainAspectRatio
						AND IMG.[Ratio] = @Ratio
						)
					)
				AND (
					@Size = 0
					OR IMG.[Size] >= @Size
					)
				)
		ORDER BY KM.[KeywordMatches] DESC OFFSET(@PerPage * (@Page - 1)) ROWS FETCH NEXT @PerPage ROWS ONLY
		)
	SELECT IMG.*
		,K.*
	FROM [resultsCte] IMG
	LEFT JOIN ImageKeyword IK
		ON IK.[ImageIdKey] = IMG.[IdKey]
	LEFT JOIN Keyword K
		ON K.[IdKey] = IK.KeywordIdKey
END
ELSE
BEGIN
	--no keywords passed in go based off normal criteria
		;

	WITH [resultsCte]
	AS (
		SELECT DISTINCT IMG.*
		FROM [Image] IMG
		WHERE IMG.[Reported] IN (0, 3)
			AND (
				(
					@Source = 9999
					OR Img.[IndexSource] = @Source
					)
				AND (
					@Class = - 1
					OR Img.[Class] = @Class
					)
				AND (
					@ResolutionSearch = - 1
					OR (
						@ResolutionSearch = 0 --ExactMatch
						AND (
							Img.[ResolutionX] = @X
							AND Img.[ResolutionY] = @Y
							)
						)
					OR (
						@ResolutionSearch = 1 --EqualOrGreater
						AND (
							Img.[ResolutionX] >= @X
							AND Img.[ResolutionY] >= @Y
							)
						)
					OR (
						@ResolutionSearch = 2 --MaintainAspectRatio
						AND IMG.[Ratio] = @Ratio
						)
					)
				AND (
					@Size = 0
					OR IMG.[Size] >= @Size
					)
				)
		ORDER BY IMG.[IdKey] DESC OFFSET(@PerPage * (@Page - 1)) ROWS FETCH NEXT @PerPage ROWS ONLY
		)
	SELECT IMG.*
		,K.*
	FROM [resultsCte] IMG
	LEFT JOIN ImageKeyword IK
		ON IK.[ImageIdKey] = IMG.[IdKey]
	LEFT JOIN Keyword K
		ON K.[IdKey] = IK.KeywordIdKey
END