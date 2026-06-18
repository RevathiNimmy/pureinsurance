SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_Process_Partial_Payments_For_CCI'
GO
Create Procedure spu_ACT_Process_Partial_Payments_For_CCI

	@PFInstalment_ID		int,
	@PartialPayAmount		numeric (19,4),
	@NewPFInstalment_ID		int
AS

declare @credit_control_reason		varchar(50)
declare @account_id			int
declare @document_id			int
declare @document_date			datetime
declare	@insurance_File_Cnt		int
declare	@pfprem_finance_cnt		int
declare @pfprem_finance_version		int
declare @amount 			numeric (19,4)
declare @can_auto_cancel		tinyint
declare @will_auto_cancel		tinyint
declare	@credit_control_step_id		int
declare @created_date			datetime
declare @due_date			datetime
declare @letter_sent			tinyint
declare @recurrence_count		int
declare	@PFInstalments_ID		int
declare @NewAmount 			numeric (19,4)

SELECT	@credit_control_reason = credit_control_reason , @account_id = account_id, 
		@document_id = document_id, @document_date = document_date, 
		@insurance_File_Cnt = insurance_file_cnt, @pfprem_finance_cnt = pfprem_finance_cnt, 	
		@pfprem_finance_version = pfprem_finance_version, @amount = amount, 
        @can_auto_cancel = can_auto_cancel, @will_auto_cancel = will_auto_cancel, 
		@credit_control_step_id = credit_control_step_id, @created_date = created_date, 
		@due_date = due_date, @letter_sent = letter_sent, @recurrence_count = recurrence_count
FROM    Credit_Control_Item
WHERE   (PFInstalments_Id = @PFInstalment_ID)

SELECT @NewAmount = @Amount - @PartialPayAmount

INSERT INTO 	Credit_Control_Item
            	(credit_control_reason, account_id, document_id, document_date, 
				insurance_file_cnt, pfprem_finance_cnt, pfprem_finance_version, amount, 
				can_auto_cancel, will_auto_cancel, credit_control_step_id, created_date, 
				due_date, letter_sent, recurrence_count, PFInstalments_Id, Is_Deleted, Is_Balance_Amount)
VALUES     	    (@credit_control_reason, @account_id, @document_id, @document_date,
		        @insurance_File_Cnt, @pfprem_finance_cnt, @pfprem_finance_version,
		        @NewAmount, @can_auto_cancel, @will_auto_cancel, @credit_control_step_id,
		        @created_date, @due_date, @letter_sent, @recurrence_count,  @NewPFInstalment_ID, 0, 1)

UPDATE	Credit_Control_Item
SET		Amount = @PartialPayAmount
WHERE	pfinstalments_id = @PFInstalment_ID
