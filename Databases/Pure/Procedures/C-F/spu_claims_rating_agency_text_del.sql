SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_claims_rating_agency_text_del'
GO

CREATE PROCEDURE spu_claims_rating_agency_text_del
 @claims_rating_agency_id int
AS
DELETE
FROM claims_rating_agency_text
WHERE claims_rating_agency_id = @claims_rating_agency_id

GO

