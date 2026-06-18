SET QUOTED_IDENTIFIER ON 
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_UpdateStatus'
GO
CREATE PROCEDURE spu_PFPremiumFinance_UpdateStatus

@PremiumFinanceCnt int,

@PremiumFinanceVersion int,
@status CHAR(3),
@nCancelReasonId INT = NULL


AS 
BEGIN

    UPDATE PFPremiumFinance 
    SET StatusInd=@status,
    pfpremiumfinance_cancel_reason_id  = ISNULL(@nCancelReasonId,pfpremiumfinance_cancel_reason_id)

    WHERE PFPrem_Finance_cnt=@PremiumFinanceCnt

    AND PFPrem_Finance_version = @PremiumFinanceVersion

	--IF STATUS OF THE PLAN IS CANCELLED THEN DELETE CORRESPONDING CREDIT CONTROL ITEMS FOR REMAINING INSTALMENTS IF ANY
	IF @status IN ('900','999')
	BEGIN
		UPDATE Credit_Control_Item 
		SET is_deleted=1 	
		WHERE pfprem_finance_cnt =@PremiumFinanceCnt
		AND pfprem_finance_version=@PremiumFinanceVersion
	END
END

SET QUOTED_IDENTIFIER OFF 
GO

