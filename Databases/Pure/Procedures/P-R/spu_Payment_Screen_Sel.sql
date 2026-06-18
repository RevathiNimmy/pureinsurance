SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Payment_Screen_Sel'
GO

CREATE PROCEDURE spu_Payment_Screen_Sel
    @insurance_file_cnt int
AS
BEGIN

DECLARE @agent_count as int


SELECT @agent_count = 1

SELECT 
    pt.description, 
    p.shortname, 
    pf.fee_percentage, 
    pf.fee_amount, 
    pf.party_cnt, 
    pf.commission_percentage, 
    pf.commission_amount, 
    pf.isIPTable 
FROM 
    policy_fee pf, 
    party p, 
    party_type pt 
WHERE 
    pf.insurance_file_cnt = @insurance_file_cnt
AND p.party_cnt = pf.party_cnt 
AND pt.party_type_id = p.party_type_id

END
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO

