SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_Process_Partial_Payments_For_Instalments'
GO
Create Procedure spu_ACT_Process_Partial_Payments_For_Instalments

	@PFInstalment_ID		int,
	@PartialPayAmount		numeric (19,4),
	@NewPFInstalment_ID		int OUTPUT
AS

declare @pfprem_Finance_cnt		int
declare	@pfprem_finance_version		int
declare @InstalmentNumber		int
declare @DueDate			datetime
declare @Fee				numeric (19,4)
declare @Amount				numeric	(19,4)
declare @TransactionCode		int
declare	@Status				int
declare @BatchNumber			int
declare	@BatchExportDate		datetime
declare @PostedDate			datetime
declare @PFTransaction_ID		int
declare	@Commission			numeric (19,4)
declare @Tax				numeric (19,4)
declare @pfinstalmetns_result_id	int
declare @batch_id			int
declare @loyalty_scheme_flag		tinyint
declare @financefee			numeric (19,4)
declare @group_id			int
declare @NewAmount			numeric (19,4)
declare @pfmediatype_history_id  int

Select 	@pfprem_Finance_cnt = pfprem_finance_cnt,
	@pfprem_finance_version = pfprem_finance_version,
	@InstalmentNumber = InstalmentNumber,
	@DueDate = DueDate, @Fee = Fee, @Amount = Amount,
	@TransactionCode = TransactionCode, @Status = Status,
	@BatchNumber = BatchNumber, @BatchExportDate =  BatchExportDate, 
	@PostedDate = PostedDate, @PFTransaction_ID = PFTransaction_id,
	@Commission = commission, @Tax = tax, @pfinstalmetns_result_id = pfinstalments_result_id,
	@batch_id = batch_id, @loyalty_scheme_flag = loyalty_scheme_flag,
	@financefee= financefee, @group_id = group_id,
	@pfmediatype_history_id=pfmediatype_history_id
FROM	PFInstalments 
WHERE 	pfinstalments_id = @PFInstalment_ID

SELECT @NewAmount = @Amount - @PartialPayAmount

INSERT INTO 	PFInstalments
            	(pfprem_finance_cnt, pfprem_finance_version,InstalmentNumber, 
		DueDate, Fee, Amount, TransactionCode, Status, BatchNumber, BatchExportDate, 
             	PostedDate, PFTransaction_id, commission, tax, pfinstalments_result_id, batch_id, 
		loyalty_scheme_flag, financefee, group_id,pfmediatype_history_id)
VALUES     	(@pfprem_Finance_cnt,@pfprem_finance_version,@InstalmentNumber,
		@DueDate,@Fee,@NewAmount ,@TransactionCode,@Status,@BatchNumber,@BatchExportDate,
		@PostedDate,@PFTransaction_ID,@Commission,@Tax,@pfinstalmetns_result_id,@batch_id,
		@loyalty_scheme_flag,@financefee,@group_id,@pfmediatype_history_id)

SELECT @NewPFInstalment_ID = @@IDENTITY

UPDATE	PFInstalments
SET	Amount = @PartialPayAmount
WHERE 	pfinstalments_id = @PFInstalment_ID
