SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_PFAccountsTransactions_Add'
Go

--Object:  Stored Procedure dbo.spu_PFAccountsTransactions_Add   
-- Script Date: 10/21/2003 8:07:25 AM ******/

CREATE PROCEDURE spu_PFAccountsTransactions_Add
        @PremiumFinanceCnt int,
        @PremiumFinanceVersion int,
        @AccountID int,
        @Spare varchar(50),
        @TransDetailID int,
        @TransactionType int

AS

INSERT INTO PF_Accounts_Transactions
                    (PFPrem_Finance_Cnt, PFPrem_Finance_Version, TransDetail_ID, 
			Account_ID, Transaction_Type_ID, Spare)
VALUES        (@PremiumFinanceCnt, @PremiumFinanceVersion, @TransDetailID,
			@AccountID, @TransactionType, @Spare)


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

