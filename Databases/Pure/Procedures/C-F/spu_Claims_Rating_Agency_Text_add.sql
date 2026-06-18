SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_claims_rating_agency_Text_add'
GO

CREATE PROCEDURE spu_claims_rating_agency_Text_add
    @claims_rating_agency_id int, 
    @claims_rating_agency_text varchar(4000)
AS
BEGIN
DECLARE @claims_rating_agency_text_id integer
SELECT  @claims_rating_agency_text_id  = isnull((select max(claims_rating_agency_text_id) 
					  from claims_rating_agency_text 
					  where claims_rating_agency_id = @claims_rating_agency_id),0) + 1
INSERT INTO claims_rating_agency_Text (
    claims_rating_agency_id ,
    claims_rating_agency_text_id ,
    claims_rating_agency_text)
VALUES (
    @claims_rating_agency_id,
    @claims_rating_agency_text_id,
    @claims_rating_agency_text)
END

GO

