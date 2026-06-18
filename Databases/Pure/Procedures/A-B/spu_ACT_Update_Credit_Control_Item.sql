SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_Credit_Control_Item'
GO

CREATE PROCEDURE spu_ACT_Update_Credit_Control_Item
    @credit_control_item_id INT,
    @credit_control_reason VARCHAR(50),
    @account_id INT,
    @document_id INT,
    @document_date DATETIME,
    @insurance_file_cnt INT,
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT,
    @amount NUMERIC(19,4),
    @can_auto_cancel TINYINT,
    @will_auto_cancel TINYINT,
    @credit_control_step_id INT,
    @created_date DATETIME,
    @due_date DATETIME,
    @letter_sent TINYINT,
    @recurrence_count INT,
	--jmf 28/7/2003
	@pmuser_group_id INT,
	@pmuser_id INT,
	@claim_id INT,
	@claim_debt_id INT,
	@claim_debt_version INT,
	@partial_amount NUMERIC(19,4),
	@is_deleted tinyint,
	@pfinstalments_id INT
	--
AS

BEGIN

    UPDATE Credit_Control_Item WITH (ROWLOCK)
       SET credit_control_reason = @credit_control_reason,
           account_id = @account_id,
           document_id = @document_id,
           document_date = @document_date,
           insurance_file_cnt = @insurance_file_cnt,
           pfprem_finance_cnt = @pfprem_finance_cnt,
           pfprem_finance_version = @pfprem_finance_version,
           amount = @amount,
           can_auto_cancel = @can_auto_cancel,
           will_auto_cancel = @will_auto_cancel,
           credit_control_step_id = @credit_control_step_id,
           created_date = @created_date,
           due_date = @due_date,
           letter_sent = @letter_sent,
           recurrence_count = @recurrence_count,
			--jmf 28/7/2003
			pmuser_group_id=@pmuser_group_id ,
			pmuser_id=@pmuser_id ,
			claim_id=@claim_id ,
			claim_debt_id=@claim_debt_id ,
			claim_debt_version=@claim_debt_version ,
			partial_amount=@partial_amount ,
			is_deleted=@is_deleted ,
			pfinstalments_id=@pfinstalments_id
			--
     WHERE credit_control_item_id = @credit_control_item_id

END

GO


