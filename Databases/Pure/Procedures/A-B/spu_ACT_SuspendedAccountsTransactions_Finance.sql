SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_ACT_SuspendedAccountsTransactions_Finance'
GO

-- Object:  Stored Procedure spu_ACT_SuspendedAccountsTransactions_Finance
-- Script Date: 10/02/2005  

CREATE PROCEDURE spu_ACT_SuspendedAccountsTransactions_Finance
	@OldTriggerTransdetailId int,
	@PlanTransdetailId int = NULL,
	@PFPremFinanceCnt int = NULL,
	@PFPremFinanceVersion int = NULL,
	@DepositTransdetailId int = NULL,
	@DepositPercentage numeric(19,2) = NULL
AS

DECLARE @SuspendedTransdetailId int,
        @LinkedTransdetailId int,
        @LinkedPercentage numeric(19,2),
        @PremiumFinanceCnt int,
        @PremiumFinanceVersion int,
        @InsuranceFileCnt int,
        @DestinationAccountId int,
        @DocumentTypeId int,
        @TransdetailTypeId int,
        @Spare varchar(50),
        @IsDeleted tinyint,
        @manually_released tinyint

DECLARE	@commission_option varchar(255),
        @branch_id int,
	@Paidpercentage numeric(19,2) 
	 
 

select @branch_id = d.company_id from document d
				join transdetail t on t.document_id = d.document_id
				where t.transdetail_Id = @OldTriggerTransdetailID
 

select @commission_option = rtrim((select value from system_options
			                  where option_number = 16
					  and branch_id = @branch_id))
 
--Only have to update triggers when commission taken when client pays
If @commission_option <> '1'
	Return
 
IF @DepositTransdetailId is not NULL
	IF @DepositTransdetailId = 0 
 		SELECT @DepositTransdetailId = NULL 

IF @DepositPercentage is NULL
 		SELECT @DepositPercentage = 0

 
IF @PFPremFinanceCnt is not NULL
BEGIN

	IF @PFPremFinanceCnt = 0 
 		SELECT @PFPremFinanceCnt = NULL 
	UPDATE suspended_accounts_transactions 
	SET pfprem_finance_cnt = @PFPremFinanceCnt
	WHERE linked_transdetail_id = @OldTriggerTransdetailId
	AND is_deleted = 0 AND manually_released = 0  --'(RC) PLICO 9-10  
END

IF @PFPremFinanceVersion is not NULL
BEGIN
	IF @PFPremFinanceVersion = 0 
 		SELECT @PFPremFinanceVersion = NULL  
	UPDATE suspended_accounts_transactions 
	SET pfprem_finance_version = @PFPremFinanceVersion
	WHERE linked_transdetail_id = @OldTriggerTransdetailId
	AND is_deleted = 0 AND manually_released = 0  --'(RC) PLICO 9-10  
END
 
DECLARE c_suspended CURSOR FAST_FORWARD FOR
	SELECT  suspended_transdetail_id,
		linked_transdetail_id,
		linked_percentage,
		pfprem_finance_cnt,
		pfprem_finance_version,
		insurance_file_cnt,
		destination_account_id,
		documenttype_id,
		transdetail_type_id,
		spare,
		is_deleted
	FROM 	suspended_accounts_transactions  
 	WHERE	linked_transdetail_id = @OldTriggerTransdetailId
	AND 	is_deleted = 0 AND manually_released = 0  --'(RC) PLICO 9-10 

OPEN c_suspended
FETCH NEXT FROM c_suspended INTO
	@SuspendedTransdetailId,
        @LinkedTransdetailId,
        @LinkedPercentage,
        @PremiumFinanceCnt,
        @PremiumFinanceVersion,
        @InsuranceFileCnt,
        @DestinationAccountId,
        @DocumentTypeId,
        @TransdetailTypeId,
        @Spare,
        @IsDeleted
 
WHILE (@@FETCH_STATUS = 0)
BEGIN
	-- Get Part Payment Percentage
	 
	 
	SELECT @Paidpercentage = isnull(sum(tm.currency_match_amount),0) / isnull(max(t.currency_amount),1)  
			FROM transmatch tm 
			JOIN transdetail t on t.transdetail_id = tm.transdetail_id 
			WHERE tm.transdetail_id = @SuspendedTransdetailId
			 
	-- Calculate O/S Percentage
	-- Write Deposit Suspended record
	 
	IF @DepositTransdetailId IS NOT NULL AND @DepositPercentage <> 0
		INSERT INTO Suspended_Accounts_Transactions
	    	(
	     	suspended_transdetail_id,
		linked_transdetail_id,
		linked_percentage,
		pfprem_finance_cnt, 
		pfprem_finance_version, 
		insurance_file_cnt,
		destination_account_id, 
		documenttype_id, 
		transdetail_type_id,
		spare,
		is_deleted,
		manually_released  --'(RC) PLICO 9-10 
		)
		VALUES        
		( 
		@SuspendedTransdetailId,
		@DepositTransdetailId,
		(@LinkedPercentage - @Paidpercentage) * @DepositPercentage,
		NULL,
		NULL,
		@InsuranceFileCnt,
		@DestinationAccountId,
		@DocumentTypeId,
		@TransdetailTypeId,
		@Spare,
		@IsDeleted,
		@manually_released  --'(RC) PLICO 9-10  
		)
	 

	-- Write Plan Suspended record
	INSERT INTO Suspended_Accounts_Transactions
    	(
	suspended_transdetail_id,
	linked_transdetail_id,
	linked_percentage,
	pfprem_finance_cnt, 
	pfprem_finance_version, 
	insurance_file_cnt,
	destination_account_id, 
	documenttype_id, 
	transdetail_type_id,
	spare,
	is_deleted,
	manually_released  --'(RC) PLICO 9-10  
	)
	VALUES        
	( 
	@SuspendedTransdetailId,
	@PlanTransdetailId,
	(@LinkedPercentage - @Paidpercentage) * (1 -@DepositPercentage),
	@PremiumFinanceCnt,
	@PremiumFinanceVersion,
	@InsuranceFileCnt,
	@DestinationAccountId,
	@DocumentTypeId,
	@TransdetailTypeId,
	@Spare,
	@IsDeleted,
	@manually_released  --'(RC) PLICO 9-10  
	)

	--Update Original Suspended record
	UPDATE suspended_accounts_transactions
		SET is_deleted = 1
	WHERE suspended_transdetail_id = @SuspendedTransdetailId
	AND linked_transdetail_id = @LinkedTransdetailId

        --Fetch Next suspended
	FETCH NEXT FROM c_suspended INTO
	@SuspendedTransdetailId,
        @LinkedTransdetailId,
        @LinkedPercentage,
        @PremiumFinanceCnt,
        @PremiumFinanceVersion,
        @InsuranceFileCnt,
        @DestinationAccountId,
        @DocumentTypeId,
        @TransdetailTypeId,
        @Spare,
        @IsDeleted
END

    --decommission suspended
CLOSE c_suspended
DEALLOCATE c_suspended

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

