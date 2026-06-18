SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Event_Policy_Coinsurers_sel'
GO


CREATE PROCEDURE spu_Event_Policy_Coinsurers_sel
    @insurance_file_cnt int
AS


SELECT
    insurance_file_cnt,
    party_cnt,
    coinsurer_percentage,
    coinsurer_value,
    coinsurer_commission_rate,
    coinsurer_commission_amount,
    coinsurer_ipt_amount,
    coinsurer_policy_number
FROM Event_Policy_Coinsurers
WHERE insurance_file_cnt = @insurance_file_cnt
GO


