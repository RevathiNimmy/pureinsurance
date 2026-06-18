SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Tax_Group_Details'
GO

CREATE PROCEDURE spu_SIR_Get_Tax_Group_Details    
 @is_withholding_tax int = NULL,   
 @transaction_type_code varchar(20) = NULL  
AS    
    
BEGIN    
     
 DECLARE @TTCP int  -- claim payment  
 DECLARE @TTCR int  -- claim third party recovery  
 DECLARE @TTCS int  -- claim salvage recovery  
  
 SET @TTCS = NULL    
 SET @TTCR = NULL  
 SET @TTCP = NULL   

 IF NOT @transaction_type_code IS NULL
 	SET @transaction_type_code = LTRIM(RTRIM(@transaction_type_code))
  
 IF @transaction_type_code = 'C_CP'  
 	SET @TTCP = 1  
  
 IF @transaction_type_code = 'C_RV'  
	SET @TTCR = 1  
  
 IF @transaction_type_code = 'C_SA'  
	SET @TTCS = 1  
  
 SELECT DISTINCT    
  tg.tax_group_id,    
  tg.description,    
  tg.code,    
  tg.is_withholding_tax,    
  tg.advanced_tax_script,
  tg.is_tax_amount_editable    
 FROM tax_group tg    
    
 INNER JOIN tax_group_tax_band tgtb ON    
  tg.tax_group_id = tgtb.tax_group_id    
    
  INNER JOIN tax_band tb ON    
   tb.tax_band_id = tgtb.tax_band_id    
    
   INNER JOIN tax_band_rate tbr ON    
    tb.tax_band_id = tbr.tax_band_id    
    
 WHERE ((@TTCP IS NULL) OR (tbr.TTCP = @TTCP))  
 AND  ((@TTCR IS NULL) OR (tbr.TTCR = @TTCR))  
 AND  ((@TTCS IS NULL) OR (tbr.TTCS = @TTCS))  
 AND tg.is_deleted = 0    
 AND tg.effective_date <= GetDate()    
 AND ((@is_withholding_tax IS NULL) OR (tg.is_withholding_tax = @is_withholding_tax))    
    
END    
  
  
  



GO
