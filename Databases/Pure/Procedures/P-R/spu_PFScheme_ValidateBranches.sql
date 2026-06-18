SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXEC DDLDropProcedure 'spu_PFScheme_ValidateBranches'
GO

CREATE Procedure spu_PFScheme_ValidateBranches
    @CompanyNo int,
    @SchemeNo int,
    @SchemeVersion int
AS

SELECT
	S.Description
FROM
    PFScheme PS
INNER JOIN
	PFSchemeSource PSS ON
    PSS.CompanyNo = PS.CompanyNo
    AND PSS.SchemeNo = PS.SchemeNo
    AND PSS.SchemeVersion = PS.SchemeVersion
INNER JOIN Source S ON
	S.source_id=PSS.source_id
WHERE
    PS.CompanyNo = @CompanyNo
    AND PS.SchemeNo = @SchemeNo
    AND PS.SchemeVersion = @SchemeVersion
	AND PS.currency_id <> S.base_currency_id

DELETE
	PFSchemeSource
FROM
    PFScheme PS
INNER JOIN
	PFSchemeSource PSS ON
    PSS.CompanyNo = PS.CompanyNo
    AND PSS.SchemeNo = PS.SchemeNo
    AND PSS.SchemeVersion = PS.SchemeVersion
INNER JOIN Source S ON
	S.source_id=PSS.source_id
WHERE
    PS.CompanyNo = @CompanyNo
    AND PS.SchemeNo = @SchemeNo
    AND PS.SchemeVersion = @SchemeVersion
	AND PS.currency_id <> S.base_currency_id

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO