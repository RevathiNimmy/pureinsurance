SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_ACT_SuspendedAccountsTransactions_Add'
Go

--Object:  Stored Procedure dbo.spu_ACT_SuspendedAccountsTransactions_Add   
-- Script Date: 10/21/2003 8:07:25 AM ******/

CREATE PROCEDURE spu_ACT_SuspendedAccountsTransactions_Add
        @SuspendedTransdetailId int,
        @LinkedTransdetailId int,
        @LinkedPercentage float,
        @PremiumFinanceCnt int,
        @PremiumFinanceVersion int,
        @InsuranceFileCnt int,
        @DestinationAccountId int,
        @DocumentTypeId int,
        @TransdetailTypeId int,
        @Spare varchar(50),
        @IsDeleted tinyint,
        @manually_released tinyint,  --'(RC) PLICO 9-10
	@released_on_full_settlement tinyint,  --'(RC) PLICO 9-10
	@released_for_whole_posting tinyint,  --'(RC) PLICO 9-10
	@released_on_policy_effective tinyint  --'(RC) PLICO 9-10
AS

IF @PremiumFinanceCnt = 0  
SELECT @PremiumFinanceCnt = NULL
IF @PremiumFinanceVersion = 0  
SELECT @PremiumFinanceVersion = NULL

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
manually_released,  --'(RC) PLICO 9-10
released_on_full_settlement,  --'(RC) PLICO 9-10
released_for_whole_posting,  --'(RC) PLICO 9-10
released_on_policy_effective  --'(RC) PLICO 9-10
)
VALUES
(
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
@IsDeleted,
@manually_released,  --'(RC) PLICO 9-10
@released_on_full_settlement,  --'(RC) PLICO 9-10
@released_for_whole_posting,  --'(RC) PLICO 9-10
@released_on_policy_effective  --'(RC) PLICO 9-10
)

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
