SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Update_Risk_Return_Premium_During_Cancellation'
GO

CREATE PROCEDURE spu_Update_Risk_Return_Premium_During_Cancellation
@risk_cnt INT
AS
BEGIN
	DECLARE @billed_total_premium NUMERIC(19, 4)
	DECLARE @return_premium NUMERIC(19, 4)
	DECLARE @diff NUMERIC(19, 4)
	
	CREATE TABLE #tmpRiskBilledPremiumsDetails
        (
           total_premium_amount		  NUMERIC(19, 4),
           total_temp_premium_amount  NUMERIC(19, 4)
        )

	INSERT INTO #tmpRiskBilledPremiumsDetails
	EXEC spu_sir_get_risks_billed_premium @risk_cnt

	SELECT @billed_total_premium = total_premium_amount + total_temp_premium_amount FROM #tmpRiskBilledPremiumsDetails

	SELECT @return_premium = ROUND(SUM(ISNULL(this_premium,0)), 4)
                        FROM   Rating_Section
                        WHERE  risk_cnt = @risk_cnt

	SET @diff = ABS(@return_premium) - ABS(@billed_total_premium)
	UPDATE Peril SET this_premium=this_premium + @diff
		WHERE risk_cnt=@risk_cnt 
			AND this_premium<>0
			AND sum_insured<>0
			AND sequence_number = (SELECT TOP 1 sequence_number
									FROM Rating_Section
									WHERE risk_cnt=@risk_cnt
									AND this_premium<>0
									AND sum_insured<>0
									ORDER BY ABS(this_premium) DESC)
			AND rating_section_id = (SELECT TOP 1 rating_section_id
									FROM Rating_Section
									WHERE risk_cnt=@risk_cnt
									AND this_premium<>0
									AND sum_insured<>0
									ORDER BY ABS(this_premium) DESC)

	UPDATE Rating_Section SET this_premium = this_premium + @diff
		WHERE risk_cnt=@risk_cnt
			AND this_premium<>0
			AND sum_insured<>0
			AND rating_section_id = (SELECT TOP 1 Rating_Section_id
									FROM PERIL
									WHERE risk_cnt = @risk_cnt
									AND ISNULL(is_levy_tax, 0) <> 1
									AND this_premium<>0
									AND sum_insured<>0
									ORDER BY ABS(this_premium) DESC)
			AND sequence_number = (SELECT TOP 1 sequence_number
									FROM   Peril
									WHERE  risk_cnt=@risk_cnt
									AND ISNULL(is_levy_tax, 0) <> 1
									AND this_premium<>0
									AND sum_insured<>0
									ORDER BY ABS(this_premium) DESC)
	DROP TABLE #tmpRiskBilledPremiumsDetails
END