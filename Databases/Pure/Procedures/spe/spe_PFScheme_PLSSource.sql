SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_PLSSource'
GO
CREATE PROCEDURE spe_PFScheme_PLSSource
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
    @Key INT,
	@UserID INT,
	@UniqueID VARCHAR(50),
	@ScreenHierarchy VARCHAR(500)
AS

INSERT INTO
    PFSchemeSource (CompanyNo, SchemeNo, SchemeVersion, Source_id,UserId,UniqueId,ScreenHierarchy)
VALUES
    (@CompanyNo, @SchemeNo, @SchemeVersion, CAST(@Key AS SMALLINT),@UserId,@UniqueId,@ScreenHierarchy)
GO

