SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CreditControlItemAddForTrans'
GO

CREATE PROCEDURE spu_CreditControlItemAddForTrans
    @account_id Int,
    @document_id Int,
    @document_date datetime,
    @amount numeric(9,4),
    @insurance_file_cnt Int = NULL,
    @pfprem_finance_cnt Int = NULL
AS
DECLARE @credit_control_step_id Int
DECLARE @trans_type VARCHAR(25)
DECLARE @processingdays smallint
SET @trans_type='TRANS'
SET @credit_control_step_id=(SELECT TOP 1 credit_control_step_id
FROM credit_control_step
WHERE credit_control_rule_id in (SELECT TOP 1 credit_control_rule_id 
FROM credit_control_rule WHERE business_type=@trans_type AND is_active=1))
SET @processingdays=(SELECT ISNULL(processing_days,0) FROM credit_control_rule WHERE business_type=@trans_type AND is_active=1)

IF @credit_control_step_id IS NOT NULL OR @credit_control_step_id<>0
BEGIN
INSERT INTO Credit_Control_Item (
    credit_control_reason,
    account_id,
    document_id,
    document_date,
    insurance_file_cnt,
    pfprem_finance_cnt,
    pfprem_finance_version,
    amount,
    can_auto_cancel,
    will_auto_cancel,
    credit_control_step_id,
    created_date,
    due_date,
    letter_sent,
    recurrence_count,
    PFInstalments_Id, 
    is_deleted)                                
VALUES  (@trans_type,
    @account_id,
    @document_id,
    @document_date,
    @insurance_file_cnt,  
    @pfprem_finance_cnt, 
    NULL,
    @amount,
    0,
    0,
    @credit_control_step_id,
    getdate(),
    dateadd(d,@processingdays,getdate()),
    0,
    0,
    NULL,
    0)
END
GO
