SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_del'
GO
/* Cascade delete the Branch/Product/PFRF records with the PFScheme under a transaction */
CREATE PROCEDURE spe_PFScheme_del
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
	@UniqueId VARCHAR(50)='',
	@UserId INT=0
AS
BEGIN

UPDATE PFScheme SET UserId=@UserId,UniqueId=@UniqueId, ScreenHierarchy='Instalment Scheme('+ SchemeName + ')'    
	WHERE
        PFScheme.CompanyNo = @CompanyNo
		AND PFScheme.SchemeNo = @SchemeNo
		AND PFScheme.SchemeVersion = @SchemeVersion

    DELETE FROM
        PFScheme
    WHERE
        PFScheme.CompanyNo = @CompanyNo
    AND PFScheme.SchemeNo = @SchemeNo
    AND PFScheme.SchemeVersion = @SchemeVersion

END

GO

