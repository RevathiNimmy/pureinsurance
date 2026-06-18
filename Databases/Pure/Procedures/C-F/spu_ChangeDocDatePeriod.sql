SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ChangeDocDatePeriod'
GO

CREATE PROCEDURE spu_ChangeDocDatePeriod
					@DocumentRef Varchar(25),
					@PeriodID Int,
					@DocDate DateTime
AS
-- @DocumentRef = document ref which need period_id change
-- @PeriodID = new period_id to change docucument to
-- @DocDate = new date to change document to

Begin Transaction

Declare @ReturnValue Int

SELECT @ReturnValue = 0

-- change period id for this document_ref
UPDATE Stats_Folder
SET posting_period_number = @PeriodID,
	posting_period_year = (SELECT Convert(Int,Right(period_name,4)) FROM Period WHERE period_id = @PeriodID),
	document_date = @DocDate,
	accounting_date = @DocDate,
	transaction_date = @DocDate
WHERE document_ref = @DocumentRef

IF @@ERROR <> 0
	GOTO Catch

UPDATE Transaction_Export_Folder SET posting_period_number = @PeriodID WHERE document_ref = @DocumentRef

IF @@ERROR <> 0
	GOTO Catch

UPDATE Transdetail SET period_id = @PeriodID,
	   Accounting_Date = @DocDate,
	   Ref_Date = @DocDate
FROM document d JOIN Transdetail td ON d.document_id = td.document_id
WHERE d.document_ref = @DocumentRef

IF @@ERROR <> 0
	GOTO Catch

UPDATE Document SET document_date = @DocDate,
					created_date = @DocDate,
					authorised_date = @DocDate
WHERE document_ref = @DocumentRef

IF @@ERROR <> 0
	GOTO Catch

Commit Transaction
GOTO Finally

Catch:
	RollBack Transaction

	SELECT @ReturnValue = -1

Finally:
	RETURN @ReturnValue