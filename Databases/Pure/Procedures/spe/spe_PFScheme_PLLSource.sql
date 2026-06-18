SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_PLLSource'
GO
CREATE PROCEDURE spe_PFScheme_PLLSource
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
    @ExtraSQL VARCHAR(1000) = '',
	@UserID INT,
	@UniqueID VARCHAR(50),
	@ScreenHierarchy VARCHAR(500)
AS

IF @ExtraSQL <> ''
BEGIN
	DECLARE @SQL VARCHAR(1000)
	
	SELECT @SQL = 'SELECT B.Source_id, B.description,'
	SELECT @SQL = @SQL + 'CASE WHEN P.Source_id IS Null THEN 0 ELSE 1 END As Chosen '
	SELECT @SQL = @SQL + 'FROM Source B LEFT JOIN PFSchemeSource P ON '
	SELECT @SQL = @SQL + 'B.Source_id = P.Source_id '
	SELECT @SQL = @SQL + 'AND P.CompanyNo = ' + CONVERT(Varchar(1000),@CompanyNo) + ' '
	SELECT @SQL = @SQL + 'AND P.SchemeNo = ' + CONVERT(Varchar(1000),@SchemeNo) + ' '
	SELECT @SQL = @SQL + 'AND P.SchemeVersion = ' + CONVERT(Varchar(1000),@SchemeVersion) + ' '
	SELECT @SQL = @SQL + 'WHERE B.is_deleted <> 1 '
	SELECT @SQL = @SQL + @ExtraSQL

	EXECUTE (@SQL)
END
ELSE
BEGIN
	SELECT
		B.Source_id,
		B.description,
		CASE WHEN P.Source_id IS Null 
		  THEN 0
		  ELSE 1
		END As Chosen
	FROM
		Source B
	LEFT JOIN
		PFSchemeSource P
		ON
			B.Source_id = P.Source_id
		AND P.CompanyNo = @CompanyNo
		AND P.SchemeNo = @SchemeNo
		AND P.SchemeVersion = @SchemeVersion

	WHERE B.is_deleted <> 1     -- only display branches that have not been deleted (PN 16916)
END

GO
