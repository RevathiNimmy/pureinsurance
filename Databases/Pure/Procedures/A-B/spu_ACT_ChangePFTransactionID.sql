EXECUTE DDLDropProcedure 'spu_ACT_ChangePFTransactionID'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC spu_ACT_ChangePFTransactionID
    @PremiumFinanceCnt int,
    @PremiumFinanceVersion int,
    @DocumentRef VARCHAR(20)

AS BEGIN

    DECLARE @TransDetailID int

    SELECT @TransDetailID = TransDetail_ID
    FROM TransDetail t, document d
    WHERE t.document_sequence = 1
    AND d.document_id = t.document_id
    AND d.document_ref = @DocumentRef
 
    UPDATE pfPremiumFinance
    SET PlanTransAction_id = @TransDetailID
    WHERE
        pfprem_finance_cnt = @PremiumFinanceCnt
    AND pfprem_finance_version = @PremiumFinanceVersion

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO