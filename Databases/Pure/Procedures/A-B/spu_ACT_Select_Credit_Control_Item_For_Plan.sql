SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Credit_Control_Item_For_Plan'
GO

CREATE PROCEDURE spu_ACT_Select_Credit_Control_Item_For_Plan
    @pfinstalments_id INT
AS

BEGIN

    SELECT  cci.credit_control_item_id,
            cci.credit_control_reason,
            cci.account_id,
            cci.document_id,
            cci.document_date,
            cci.insurance_file_cnt,
            cci.pfprem_finance_cnt,
            cci.pfprem_finance_version,
            cci.amount,
            cci.can_auto_cancel,
            cci.will_auto_cancel,
            cci.credit_control_step_id,
            cci.created_date,
            cci.due_date,
            cci.letter_sent,
            cci.recurrence_count,
            ccs.credit_control_step_id,
            --jmf 28/7/2003 - claim debt credit control
            cci.pmuser_group_id,
            cci.pmuser_id,
            cci.claim_id,
            cci.claim_debt_id,
            cci.claim_debt_version,
            cci.partial_amount,
            cci.is_deleted,
            cci.pfinstalments_id
      FROM  Credit_Control_Item cci
INNER JOIN  Credit_Control_Step ccs
        ON  cci.credit_control_step_id = ccs.credit_control_step_id
     WHERE  cci.pfinstalments_id = @pfinstalments_id

END
GO


