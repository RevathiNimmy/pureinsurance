SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_claims_rating_agency_text_saa'
GO

CREATE PROCEDURE spu_claims_rating_agency_text_saa
    @claims_rating_agency_id int
AS
SELECT   claims_rating_agency_text
FROM claims_rating_agency_text
WHERE claims_rating_agency_id = @claims_rating_agency_id 
ORDER BY claims_rating_agency_text_id ASC

GO

 