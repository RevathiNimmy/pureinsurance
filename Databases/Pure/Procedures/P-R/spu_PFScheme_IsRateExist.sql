SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PFScheme_IsRateExist'
GO
CREATE PROCEDURE spu_PFScheme_IsRateExist
    @CompanyNo INT,
	@SchemeNo INT,
	@SchemeVersion INT

AS
BEGIN
	DECLARE @PFSchemeRateExist smallint
 
	IF EXISTS(	SELECT 1 FROM PFRF
			    WHERE Companyno=@CompanyNo AND 
				Schemeno=@SchemeNo AND 
				SchemeVersion=@SchemeVersion)
		BEGIN
			SET @PFSchemeRateExist=1
		END

	ELSE
		BEGIN
			SET @PFSchemeRateExist=0
		END

	SELECT @PFSchemeRateExist

END
GO


