SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_PLSProduct'
GO
CREATE PROCEDURE spe_PFScheme_PLSProduct
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
	@Key INT,
	@UserID INT,
	@UniqueID VARCHAR(50),
	@ScreenHierarchy VARCHAR(500)
AS

INSERT INTO
    PFSchemeProducts (CompanyNo, SchemeNo, SchemeVersion, product_id,UserId,UniqueId,ScreenHierarchy)
VALUES
    (@CompanyNo, @SchemeNo, @SchemeVersion, @Key,CAST(@UserId AS INT),@UniqueId,@ScreenHierarchy)
GO

