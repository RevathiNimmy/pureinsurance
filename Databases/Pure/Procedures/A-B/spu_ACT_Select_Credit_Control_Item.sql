SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Credit_Control_Item'
GO

CREATE PROCEDURE spu_ACT_Select_Credit_Control_Item
    @credit_control_item_id INT
AS

BEGIN

    SELECT credit_control_item_id,
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
			--jmf 28/7/2003 - claim debt credit control
			NULL as next_step_id,		
			pmuser_group_id,
			pmuser_id,
			claim_id,
			claim_debt_id,
			claim_debt_version,
			partial_amount,
			is_deleted,
			pfinstalments_id		
      FROM Credit_Control_Item
     WHERE credit_control_item_id = @credit_control_item_id

END
GO


