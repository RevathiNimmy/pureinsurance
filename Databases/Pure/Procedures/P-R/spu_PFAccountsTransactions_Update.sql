SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spu_PFAccountsTransactions_Update'
GO

-- Object:  Stored Procedure spu_PFAccountsTransactions_Update   
-- Script Date: 10/9/2003 12:06:42 PM

CREATE PROCEDURE spu_PFAccountsTransactions_Update
        @PremiumFinanceCnt int,
        @PremiumFinanceVersion int

AS

UPDATE   Suspended_Accounts_Transactions
SET      pfprem_finance_version = @PremiumFinanceVersion
WHERE    pfprem_finance_cnt = @PremiumFinanceCnt

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

