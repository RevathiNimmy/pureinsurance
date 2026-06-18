SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_get_country_postcode_visibility'
GO

CREATE PROCEDURE spu_SAM_get_country_postcode_visibility
AS

    SELECT c.country_id, 
           pv.code
      FROM Country c
 LEFT JOIN Postcode_visibility pv ON c.postcode_visibility_id = pv.Postcode_visibility_id

GO

