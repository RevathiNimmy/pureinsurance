SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_PLDProduct'
GO
CREATE PROCEDURE spe_PFScheme_PLDProduct
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
	@UserID INT,
	@UniqueID VARCHAR(50),
	@ScreenHierarchy VARCHAR(500)
AS

UPDATE PFSchemeProducts SET UserId=@UserID, UniqueId=@UniqueID
WHERE
    CompanyNo = @CompanyNo
AND SchemeNo = @SchemeNo
AND SchemeVersion = @SchemeVersion

DELETE FROM
    PFSchemeProducts
WHERE
    CompanyNo = @CompanyNo
AND SchemeNo = @SchemeNo
AND SchemeVersion = @SchemeVersion
GO

