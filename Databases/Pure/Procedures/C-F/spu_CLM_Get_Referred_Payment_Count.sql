SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Referred_Payment_Count'
GO
CREATE PROCEDURE spu_CLM_Get_Referred_Payment_Count  
    @claim_id  int  
AS  
  
SELECT Count(*)TotaPayments,
ISNULL(sum(cp2.Amount),0) + ISNULL(sum(cp2.tax_Amount),0) + ISNULL(sum(cp2.tax_amount_WHT),0)Amount
FROM Claim_Payment CP1 
    JOIN Claim_Payment CP2 
    ON CP1.Claim_payment_id = CP2.Base_Claim_payment_id 
WHERE CP1.is_referred =1 AND CP2.Claim_ID=@claim_id AND CP2.Amount<>0
    GROUP BY cp2.claim_id


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
