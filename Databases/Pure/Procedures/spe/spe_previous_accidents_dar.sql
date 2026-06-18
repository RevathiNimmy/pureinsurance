SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_previous_accidents_dar'
GO

CREATE PROCEDURE spe_previous_accidents_dar
 @party_cnt int,
 @UserId int,
 @UniqueId VARCHAR(50) = NULL
AS

IF @UniqueId IS NOT NULL
BEGIN
	UPDATE pa
	SET 
		UserId = @UserId,
		UniqueId = @UniqueId,
		ScreenHierarchy = 'Other Party(' + LTRIM(RTRIM(p.shortname)) + ')\' + 'Accident(' + pa.Description + ')'
	FROM previous_accidents pa
	INNER JOIN party p ON p.party_cnt = pa.party_cnt WHERE pa.party_cnt = @party_cnt
END

DELETE
FROM previous_accidents
WHERE party_cnt = @party_cnt

GO

