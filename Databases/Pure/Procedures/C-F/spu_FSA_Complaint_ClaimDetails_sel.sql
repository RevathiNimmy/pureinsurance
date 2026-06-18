SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_ClaimDetails_sel'
GO
CREATE PROCEDURE spu_FSA_Complaint_ClaimDetails_sel
	@claim_id int
AS
BEGIN
SELECT 
	c.claim_number,
	p.party_cnt
FROM claim c,
     party p
WHERE c.claim_id = @claim_id
AND   c.client_short_name = p.shortname
END
GO

