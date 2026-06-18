SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFPremiumFinance_GetRefundAmount'
GO

CREATE PROCEDURE spu_PFPremiumFinance_GetRefundAmount
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT      
AS
BEGIN

	DECLARE @TotalInstalement AS INT
	DECLARE @RemainingInstalment AS INT
	DECLARE @TotalInterestAmount AS MONEY
	DECLARE @InterestAmountPerInstalment AS Decimal(18,17)
	DECLARE @TotalInterestRefund AS Numeric(18,4)
	DECLARE @TotalFeeTax AS Numeric(18,4)
	
	DECLARE @bIsSuppressDecimal As TINYINT

	IF Exists(Select Value from Hidden_options WHERE option_number=112)
	BEGIN
		Select @bIsSuppressDecimal=ISNULL(Value,0) from Hidden_options WHERE option_number=112
	END
	ELSE
		SET @bIsSuppressDecimal=0

	SELECT @TotalInstalement=PFPrem.NoOfInstallments,@TotalInterestAmount=PFPrem.InterestCost FROM PFPremiumFinance  PFPrem
	WHERE PFPrem.pfprem_finance_cnt=@pfprem_finance_cnt and PFPrem.pfprem_finance_version=@pfprem_finance_version
	AND PFPrem.statusind IN ('040','999','140')

	SELECT 
	count(PFInst.PFInstalments_id) As RemainingInstalment,@TotalInstalement as TotalInstalement,@TotalInterestAmount as TotalInterestAmount
	,SUM(PFInst.Amount) As TotalunCollectedAmount ,0 As Tax
	,@bIsSuppressDecimal AS bIsSuppressDecimal
	
	FROM PFPremiumFinance PFPrem 
	JOIN 
	PFInstalments PFInst ON PFPrem.pfprem_finance_cnt=PFInst.pfprem_finance_cnt AND PFPrem.pfprem_finance_version=PFInst.pfprem_finance_version
	WHERE 
		PFPrem.pfprem_finance_cnt=@pfprem_finance_cnt AND PFPrem.pfprem_finance_version=@pfprem_finance_version
		AND PFInst.Status <>3 AND PFInst.InstalmentNumber<>0 AND PFPrem.statusind IN ('040','999','140')

		
END


