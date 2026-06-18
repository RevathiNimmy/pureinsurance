SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_ReCalculate_Policy_Tax_Amounts_SFB'
GO
CREATE PROCEDURE spu_SIR_ReCalculate_Policy_Tax_Amounts_SFB  
 	@insurance_file_cnt int 
AS  
  
BEGIN  

/*Get Taxes For All sections */  
DECLARE @insurance_section_Id int   
DECLARE @insurer_cnt int
  
DECLARE CURSOR_Policy_Section  CURSOR FOR   
	SELECT insurance_section_Id 
	FROM insurance_COB_Section 
  	WHERE insurance_file_cnt =@insurance_file_cnt   
  
   
 	OPEN CURSOR_Policy_Section 
   
 	FETCH NEXT FROM CURSOR_Policy_Section INTO   
  	@insurance_section_Id   
    
   
 	WHILE @@FETCH_STATUS = 0  
    	BEGIN  
   
  -- populate tax fields for the specified section record  
  	EXEC spu_SIR_Calculate_Section_Tax_Amounts_SFB 
  	 @insurance_section_Id,0
  -- populate taxes for commission 
	--if coinsurers exist will need to call calculation for each party
	IF not exists (SELECT * from policy_coinsurers WHERE insurance_section_id = @insurance_section_Id)
	BEGIN
  		EXEC spu_SIR_Calculate_Commission_Tax_Amounts_SFB 
  	 	@insurance_section_Id,0  
  	END
	BEGIN
		DECLARE CURSOR_Policy_Section_Coinsurers  CURSOR FOR   
		SELECT party_cnt
		FROM policy_coinsurers 
  		WHERE insurance_section_id = @insurance_section_Id
  
  		OPEN CURSOR_Policy_Section_Coinsurers 
    		FETCH NEXT FROM CURSOR_Policy_Section_Coinsurers INTO   
  		@insurer_cnt  
    		WHILE @@FETCH_STATUS = 0  
    		BEGIN 
		 	EXEC spu_SIR_Calculate_Commission_Tax_Amounts_SFB 
  	 		@insurance_section_Id,0,@insurer_cnt  
			FETCH NEXT FROM CURSOR_Policy_Section_Coinsurers INTO   
  			@insurer_cnt
		END
  		CLOSE CURSOR_Policy_Section_Coinsurers
		DEALLOCATE CURSOR_Policy_Section_Coinsurers
	END
	FETCH NEXT FROM CURSOR_Policy_Section INTO   
   		@insurance_section_Id    
  	END  
  
 CLOSE CURSOR_Policy_Section 
 DEALLOCATE CURSOR_Policy_Section
         

/*Get Taxes For All Agents */  
DECLARE @policy_agents_Id int   
  
DECLARE CURSOR_Policy_agents  CURSOR FOR   
	SELECT policy_agents_Id 
	FROM policy_agents 
  	WHERE insurance_file_cnt =@insurance_file_cnt   
  
  
 	OPEN CURSOR_Policy_Agents
   
 	FETCH NEXT FROM CURSOR_Policy_Agents INTO   
  	@Policy_agents_Id   
    
   
 	WHILE @@FETCH_STATUS = 0  
    	BEGIN  
   
  -- populate tax fields for the specified section record  
  	EXEC spu_SIR_Calculate_Agent_Tax_Amounts_SFB 
  	 @policy_agents_Id,0
  
  
  	FETCH NEXT FROM CURSOR_Policy_Agents INTO   
   		@policy_agents_Id    
  	END  
  
 CLOSE CURSOR_Policy_Agents
 DEALLOCATE CURSOR_Policy_Agents

 
/*Get Taxes For All Fees */  
DECLARE @policy_fee_Id int   
  
DECLARE CURSOR_Policy_fee  CURSOR FOR   
	SELECT policy_fee_Id 
	FROM policy_fee 
  	WHERE insurance_file_cnt =@insurance_file_cnt   
  
  
 	OPEN CURSOR_Policy_fee
   
 	FETCH NEXT FROM CURSOR_Policy_fee INTO   
  	@Policy_fee_Id   
    
   
 	WHILE @@FETCH_STATUS = 0  
    	BEGIN  
   
  -- populate tax fields for the specified Fee record  
  	EXEC spu_SIR_Calculate_Fee_Tax_Amounts_SFB 
  	 @policy_fee_Id,0
  -- populate commissiontax fields for the specified Fee record  
  	EXEC spu_SIR_Calculate_Fee_Commission_Tax_Amounts_SFB 
  	 @policy_fee_Id,0
    
  
  	FETCH NEXT FROM CURSOR_Policy_fee INTO   
   		@policy_fee_Id    
  	END  
  
 CLOSE CURSOR_Policy_fee
 DEALLOCATE CURSOR_Policy_fee



  
END  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO   

 