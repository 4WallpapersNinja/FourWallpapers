SELECT [Keyword] = 'Total Images'
	,[Count] = COUNT(*)
FROM IMAGE
WHERE Reported IN (0, 3)

UNION ALL


SELECT [Keyword] = 'Total Images from 4chan'
	,[Count] = COUNT(*)
FROM IMAGE
WHERE Reported IN (0, 3)
AND [IndexSource] BETWEEN 0 AND 99
UNION ALL

SELECT [Keyword] = 'Total Images from 7chan'
	,[Count] = COUNT(*)
FROM IMAGE
WHERE Reported IN (0, 3)
AND [IndexSource] BETWEEN 100 and 199
UNION ALL

SELECT [Keyword] = 'Total Images from Reddit'
	,[Count] = COUNT(*)
FROM IMAGE
WHERE Reported IN (0, 3)
AND (
	[IndexSource] BETWEEN 200 and 299
	OR [IndexSource] = 9998
)

UNION ALL

SELECT [Keyword] = 'Total Images from 8chan'
	,[Count] = COUNT(*)
FROM IMAGE
WHERE Reported IN (0, 3)
AND [IndexSource] BETWEEN 300 and 399
UNION ALL

SELECT [Keyword] = CASE [Class]
		WHEN 0
			THEN 'Total Images that are Unrated'
		WHEN 1
			THEN 'Total Images that are Safe For Work'
		WHEN 2
			THEN 'Total Images that are Borderline'
		WHEN 3
			THEN 'Total Images that are Not Safe for Work'
		ELSE 'Unknown Class'
		END
	,[Count] = COUNT(*)
FROM IMAGE
WHERE Reported IN (0, 3)
GROUP BY CASE [Class]
		WHEN 0
			THEN 'Total Images that are Unrated'
		WHEN 1
			THEN 'Total Images that are Safe For Work'
		WHEN 2
			THEN 'Total Images that are Borderline'
		WHEN 3
			THEN 'Total Images that are Not Safe for Work'
		ELSE 'Unknown Class'
		END
UNION ALL
SELECT [Keyword] = CASE [Reported]
		WHEN 1
			THEN 'Total Images that are Reported'
		WHEN 2
			THEN 'Total Images that are Reported'
		WHEN 4
			THEN 'Total Images that are Lost'
		ELSE 'Unknown Reported'
		END
	,[Count] = COUNT(*)
FROM IMAGE
WHERE Reported IN (1, 2, 4)
GROUP BY Reported

UNION ALL

SELECT DISTINCT [Keyword] = 'Total Unique Keywords'
	,[Count] = COUNT(*)
FROM Keyword