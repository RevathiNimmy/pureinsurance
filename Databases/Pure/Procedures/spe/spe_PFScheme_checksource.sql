SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_checksource'
GO
CREATE PROCEDURE spe_PFScheme_checksource
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
    @id INT
AS

DECLARE @Records INT

SELECT
    @Records = Count(*)
FROM
    PFSchemeSource
WHERE
    CompanyNo = @CompanyNo
AND SchemeNo = @SchemeNo
AND SchemeVersion = @SchemeVersion

IF @Records = 0
    SELECT 1
ELSE
BEGIN
    SELECT
        @Records = Count(*)
    FROM
        PFSchemeSource
    WHERE
        CompanyNo = @CompanyNo
    AND SchemeNo = @SchemeNo
    AND SchemeVersion = @SchemeVersion
    AND source_id = @id

    IF @Records = 0
        SELECT 0
    ELSE
        SELECT 1
END
GO


