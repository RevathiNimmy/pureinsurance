SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_copy_agent_commission'
GO


CREATE PROCEDURE spu_copy_agent_commission
    @OldInsuranceFileCnt int,
    @NewInsuranceFileCnt int
AS

/*
    1.0 Created to copy agemt commission for renewal and MTA.       Tomo    16/08/01

*/
BEGIN TRAN

INSERT INTO agent_commission (
        insurance_file_cnt,
        is_lead_agent,
        party_cnt,
        risk_type_id,
        commission_band_id,
        premium,
        commission_percentage,
        commission_value,
        is_amended, Maximum_rate, tax_group_id, tax_amount)
    SELECT  @NewInsuranceFileCnt,
        is_lead_agent,
        party_cnt,
        risk_type_id,
        commission_band_id,
        premium,
        commission_percentage,
        commission_value,
        is_amended, Maximum_rate, tax_group_id, tax_amount
    FROM    agent_commission
    WHERE   insurance_file_cnt = @OldInsuranceFileCnt

IF @@error > 0
    ROLLBACK TRAN

INSERT INTO lead_commission (
        insurance_file_cnt,
        commission_band,
        premium,
        [percent],
        value,
        risk_type_id)
    SELECT  @NewInsuranceFileCnt,
        commission_band,
        premium,
        [percent],
        value,
        risk_type_id
    FROM    lead_commission
    WHERE   insurance_file_cnt = @OldInsuranceFileCnt

IF @@error > 0
    ROLLBACK TRAN
ELSE
    COMMIT TRAN
GO


