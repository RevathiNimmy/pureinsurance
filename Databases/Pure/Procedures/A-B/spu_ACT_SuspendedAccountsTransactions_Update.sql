SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_ACT_SuspendedAccountsTransactions_Update'
GO

-- Object:  Stored Procedure spu_ACT_SuspendedAccountsTransactions_Update   
-- Script Date: 3/12/2004  

CREATE PROCEDURE spu_ACT_SuspendedAccountsTransactions_Update
        @SuspendedTransdetailId int,
	@LinkedTransdetailId int,
	@LinkedPercentage float,
	@PFPremFinanceCnt int,
	@PFPremFinanceVersion int,
	@InsuranceFileCnt int,
        @DestinationAccountId int,
        @DocumentTypeId int,
        @TransdetailTypeId int,
	@Spare varchar(50),
        @IsDeleted tinyint
AS

UPDATE   Suspended_Accounts_Transactions
SET
 
	linked_percentage=@LinkedPercentage,
	pfprem_finance_cnt=@PFPremFinanceCnt,
	pfprem_finance_version=@PFPremFinanceVersion,
	insurance_file_cnt=@InsuranceFileCnt,
        destination_account_id=@DestinationAccountId,
        documenttype_id=@DocumentTypeId,
        transdetail_type_id=@TransdetailTypeId,
	spare=@Spare,
        is_deleted=@IsDeleted
 
WHERE   suspended_transdetail_id = @SuspendedTransdetailID
AND	linked_transdetail_id = @LinkedTransdetailID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

