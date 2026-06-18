EXECUTE DDLDropProcedure 'spu_PFGetCashListItemFromInstalment'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC spu_PFGetCashListItemFromInstalment
    @InstalmentID int

AS BEGIN

SELECT i.cashlistitem_id, cli.transdetail_id,ba.account_id FROM CashListItem cli 
		INNER JOIN CashListItem_Instalments i ON i.CashListItem_ID = cli.CashListItem_ID 
		INNER JOIN CashList CL ON cl.cashlist_id=cli.cashlist_id 
		INNER JOIN BankAccount ba ON ba.bankaccount_id=cl.bankaccount_id
		WHERE i.pfinstalments_id = @InstalmentID 
        ORDER BY i.cashlistitem_id DESC
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
