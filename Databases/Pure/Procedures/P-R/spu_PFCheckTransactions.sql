EXECUTE DDLDropProcedure 'spu_PFCheckTransactions'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_PFCheckTransactions
    @FinancePlanCnt int,
    @FinancePlanVersion int,
    @TransactionsExist tinyint OUTPUT


AS BEGIN


        SELECT @TransactionsExist = ISNULL(PlanTransaction_id,0)
        FROM pfPremiumFinance
        WHERE
            pfprem_finance_cnt = @FinancePlanCnt
            AND pfprem_finance_version = @FinancePlanVersion


END
GO


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO



