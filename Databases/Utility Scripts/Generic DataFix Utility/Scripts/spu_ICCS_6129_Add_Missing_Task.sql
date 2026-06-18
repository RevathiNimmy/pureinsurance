
EXECUTE DDLDropProcedure 'spu_ICCS_6129_Add_Missing_Task'
GO

CREATE PROCEDURE [dbo].[spu_ICCS_6129_Add_Missing_Task]
	@BordereauTransactionId INT
AS

DECLARE @CashListID AS INT 
DECLARE @CashListItemID AS INT
DECLARE @BordereauStatusID AS INT -- 4: Recommendation, 5: Authorisation
DECLARE @SourceID INT
DECLARE @CreatedByUserID INT

SELECT 
@CashListID = CashListKey, 
@CashListItemID = CashListItemKey,
@BordereauStatusID = BordereauStatusID,
@SourceID = Branch_source_id,
@CreatedByUserID = CreatedBy_user_id
FROM [etana].[BordereauTransaction] WHERE BordereauTransactionId = @BordereauTransactionId

DECLARE @CashListTypeID AS INT
DECLARE @AccountID AS INT
DECLARE @ShortCode AS CHAR(30)
DECLARE @OurRef AS VARCHAR(30)
DECLARE @Description VARCHAR(255)
DECLARE @Amount NUMERIC(19,4)
DECLARE @Date DATETIME


SELECT 
@AccountID = CLI.account_id,
@OurRef = CLI.our_ref,
@Amount = CLI.amount
FROM CashListItem CLI
WHERE CLI.cashlistitem_id = @CashListItemID

SELECT @CashListTypeID =  cashlisttype_id FROM CashList WHERE cashlist_id = @CashListID

SELECT @ShortCode = short_code FROM Account WHERE account_id = @AccountID

SELECT @Description = 'Payments - Cash / Cheque  - Reference:   - The Amount: ' + CONVERT(VARCHAR,@Amount)

SELECT @Date = CAST(CONVERT(char(8), GETDATE(), 112) + ' 23:59:59.99' as DATETIME)

DECLARE @p1 INT
exec spe_PMWrk_Task_Instance_add @pmwrk_task_instance_cnt=@p1 OUTPUT,
@pmwrk_task_group_id=1,
@pmwrk_task_id=206,
@customer=@ShortCode,
@task_due_date=@Date,
@pmuser_group_id=1,
@user_id=NULL,
@description=@Description,
@task_status=0,
@is_urgent=0,
@date_created=@Date,
@created_by_id=@CreatedByUserID,
@last_modified=NULL,
@modified_by_id=NULL,
@is_visible=1,
@workflow_information=NULL,
@source_id=@SourceID,
@Is_task_review=0


exec spu_PMWrk_Task_Inst_Key_add @pmwrk_task_instance_cnt=@p1,@key_name=N'cashlistitem_id',@key_value=@CashListItemID

exec spu_PMWrk_Task_Inst_Key_add @pmwrk_task_instance_cnt=@p1,@key_name=N'cashlist_id',@key_value=@CashListID

exec spu_PMWrk_Task_Inst_Key_add @pmwrk_task_instance_cnt=@p1,@key_name=N'cashlisttype_id',@key_value=N'1'

exec spu_PMWrk_Task_Inst_Key_add @pmwrk_task_instance_cnt=@p1,@key_name=N'actionkey',@key_value=N'approve'

exec spu_PMWrk_Task_Inst_Key_add @pmwrk_task_instance_cnt=@p1,@key_name=N'payment_id',@key_value=N'0'