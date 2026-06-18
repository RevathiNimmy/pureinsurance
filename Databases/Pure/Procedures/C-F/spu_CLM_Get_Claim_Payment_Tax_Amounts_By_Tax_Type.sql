SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Payment_Tax_Amounts_By_Tax_Type'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Payment_Tax_Amounts_By_Tax_Type  
  
@claim_payment_id int  
  
AS  
  
BEGIN  
 SELECT MIN(tt.description), MIN(tt.code), SUM(value) FROM Tax_Calculation wtc  
  
 INNER JOIN Tax_Band tb ON  
  wtc.tax_band_id = tb.tax_band_id  
  
 INNER JOIN Tax_Type tt ON  
  tb.tax_type_id = tt.tax_type_id  
  
 WHERE claim_payment_id =@claim_payment_id  
  
 GROUP BY tb.tax_type_id  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
