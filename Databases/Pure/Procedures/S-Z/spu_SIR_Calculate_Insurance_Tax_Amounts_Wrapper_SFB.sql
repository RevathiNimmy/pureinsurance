SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_Calculate_Insurance_Tax_Amounts_Wrapper_SFB'
GO
CREATE PROCEDURE spu_SIR_Calculate_Insurance_Tax_Amounts_Wrapper_SFB  
 	@insurance_file_cnt int,
 	@return_taxes tinyint 
AS  
  
BEGIN  
  
 DECLARE @insurance_section_Id int   
  
 DECLARE CURSOR_Policy_Section  CURSOR FOR   

  Select insurance_section_Id from insurance_COB_Section 
  Where insurance_file_cnt =@insurance_file_cnt   
  
   
 OPEN CURSOR_Policy_Section 
   
 FETCH NEXT FROM CURSOR_Policy_Section INTO   
  @insurance_section_Id   
    
   
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
   
  -- populate tax fields for the specified policy_fee_u record  
  EXEC spu_SIR_Calculate_Section_Tax_Amounts_SFB 
   @insurance_section_Id,
   @return_taxes  
  
  FETCH NEXT FROM CURSOR_Policy_Section INTO   
   	@insurance_section_Id    
  END  
  
 CLOSE CURSOR_Policy_Section 
 DEALLOCATE CURSOR_Policy_Section
         
  
END  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO   

 