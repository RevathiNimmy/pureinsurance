SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Calculate_Fee_Tax_Amounts_Wrapper'
GO

CREATE PROCEDURE spu_SIR_Calculate_Fee_Tax_Amounts_Wrapper  
 @insurance_file_cnt int,  
 @risk_cnt int = NULL   
AS  
  
BEGIN  
  
 DECLARE @policy_fee_u_Id int   

IF ISNULL(@risk_cnt,0) = 0 
BEGIN
	DECLARE CURSOR_Policy_Fee_U  CURSOR FAST_FORWARD FOR   
	
	Select policy_fee_u_id from policy_fee_u   
	Where insurance_file_cnt =@insurance_file_cnt     
END
ELSE
BEGIN
	DECLARE CURSOR_Policy_Fee_U  CURSOR FAST_FORWARD FOR   
	Select policy_fee_u_id from policy_fee_u   
	Where insurance_file_cnt =@insurance_file_cnt   
	AND risk_cnt =@risk_cnt  
END

 OPEN CURSOR_Policy_Fee_U  
   
 FETCH NEXT FROM CURSOR_Policy_Fee_U INTO   
  @policy_fee_u_Id   
    
   
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
   
  -- populate tax fields for the specified policy_fee_u record  
  EXEC spu_SIR_Calculate_Fee_Tax_Amounts  
   @policy_fee_u_id  
  
  FETCH NEXT FROM CURSOR_Policy_Fee_U INTO   
   @policy_fee_u_Id   
    END  
  
 CLOSE CURSOR_Policy_Fee_U  
 DEALLOCATE CURSOR_Policy_Fee_U        
  
END  
   
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
