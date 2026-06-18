SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_claim_party_link_add'
GO


CREATE PROCEDURE spu_claim_party_link_add
    @claim_id int,
    @party_cnt int,
    @risk_type_id int,
    @peril_type_id int    
AS

BEGIN

UPDATE Claim 
SET Last_modified_date = Getdate()
WHERE Claim_id = @claim_id

IF @peril_type_id > 0
BEGIN
	INSERT INTO claim_party_link (
		claim_id ,
		party_cnt,
		peril_type_id)
	VALUES (
		@claim_id,
		@party_cnt,
		@peril_type_id)
END
ELSE
BEGIN
	INSERT INTO claim_party_link (
		claim_id ,
		party_cnt,
		risk_type_id)
	VALUES (
		@claim_id,
		@party_cnt,
		@risk_type_id)

END

END

GO


