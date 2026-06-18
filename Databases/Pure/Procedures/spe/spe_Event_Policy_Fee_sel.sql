SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Event_Policy_Fee_sel'
GO

-- AMB 03/10/2003: Accident Management RFC development

CREATE PROCEDURE spe_Event_Policy_Fee_sel
    @insurance_file_cnt int
AS

SELECT
    insurance_file_cnt,
    party_cnt,
    fee_percentage,
    fee_amount,
    extra_scheme_id,
    insurer_fee_type
FROM 
    Event_Policy_Fee
WHERE 
    insurance_file_cnt = @insurance_file_cnt

GO

