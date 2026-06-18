EXECUTE DDLDropProcedure 'spu_ACT_ReleasedAccountsTransactions_Recall'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC spu_ACT_ReleasedAccountsTransactions_Recall
    @TransDetailID int,
    @allocationId int = null

AS BEGIN

    UPDATE  Released_Accounts_Transactions
    SET recall_date = getdate() 
    WHERE suspended_transdetail_id = @TransDetailID
    AND (allocation_id = @allocationId
	OR @allocationId IS NULL )

 
	UPDATE susp
	SET susp.is_deleted = 0
	FROM suspended_accounts_transactions susp
	LEFT OUTER JOIN transdetail trans on Trans.transdetail_id = susp.linked_transdetail_id
	WHERE susp.suspended_transdetail_id = @TransDetailID
	AND ((select sum(currency_match_amount) from transmatch
					       where transdetail_id = @transdetailId)
     		<> ISNUll(trans.currency_amount,0))

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

 

