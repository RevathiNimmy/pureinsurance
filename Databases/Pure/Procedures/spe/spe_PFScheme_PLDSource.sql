SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_PLDSource'
GO
CREATE PROCEDURE spe_PFScheme_PLDSource
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
	@UserID INT,
	@UniqueID VARCHAR(50),
	@ScreenHierarchy VARCHAR(500)
AS
UPDATE PFSchemeSource SET UserId=@UserID, UniqueId=@UniqueID
WHERE
    CompanyNo = @CompanyNo
AND SchemeNo = @SchemeNo
AND SchemeVersion = @SchemeVersion

DELETE FROM
    PFSchemeSource
WHERE
    CompanyNo = @CompanyNo
AND SchemeNo = @SchemeNo
AND SchemeVersion = @SchemeVersion
GO

