SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_CountryIDPostCodeVisibility'
GO

CREATE PROCEDURE spu_SAM_Get_CountryIDPostCodeVisibility
    	@country_id   INT  
    
AS
 	SELECT iso_code, postcode_visibility_id
	FROM country
        WHERE country_id = @country_id
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

