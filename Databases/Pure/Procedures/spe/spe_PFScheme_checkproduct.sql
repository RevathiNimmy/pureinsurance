SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_checkproduct'
GO
/* Checks to see if a scheme is valid for a Product. Returns 1 if valid, 0 if not */
CREATE PROCEDURE spe_PFScheme_checkproduct
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
    @id INT
AS

DECLARE @Records INT

SELECT
    @Records = Count(*)
FROM
    PFSchemeProducts
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
        PFSchemeProducts
    WHERE
        CompanyNo = @CompanyNo
    AND SchemeNo = @SchemeNo
    AND SchemeVersion = @SchemeVersion
    AND product_id = @id

    IF @Records = 0
        SELECT 0
    ELSE
        SELECT 1
END
GO

