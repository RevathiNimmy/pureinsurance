EXECUTE DDLDropProcedure 'spu_GetCashListBankAccount'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_GetCashListBankAccount 
    @CashListItemID int
AS BEGIN
          
				SELECT   ac.account_id,
				ac.short_code AS Account, 
				ld.ledger_short_name LedgerCode,
				at.code AccountType
				FROM cashlistitem AS cl INNER JOIN cashlist as ct
				ON cl.cashlist_id = ct.cashlist_id INNER JOIN  bankaccount AS ba 
				ON ct.bankaccount_id = ba.bankaccount_id INNER JOIN  account AS ac
				ON ba.account_id = ac.account_id INNER JOIN Ledger AS ld  
				ON ac.ledger_id = ld.ledger_id INNER JOIN AccountType AS at
				ON ac.accounttype_id = at.accounttype_id
				WHERE cl.cashlistitem_id = @CashListItemID
        
END
GO
    
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
   