SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Check_IsRecommendClaimPaymentEnabledatProduct'
GO


CREATE PROCEDURE spu_Check_IsRecommendClaimPaymentEnabledatProduct 
AS  
Select Count(*) from Product  
WHERE is_Recommend_Claim_Payments = 1 