SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXEC DDLDropProcedure 'spu_PFScheme_ValidateSchemeNumber'
GO

CREATE Procedure spu_PFScheme_ValidateSchemeNumber
    @SchemeNo int,
    @SchemeVersion int,
    @SchemeExists int OUTPUT
AS

SELECT @SchemeExists = 0

IF EXISTS(
	SELECT
		*
	FROM
	    PFScheme PS
	WHERE
	    PS.SchemeNo = @SchemeNo
	    AND PS.SchemeVersion = @SchemeVersion) 
	SELECT @SchemeExists = -1

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO