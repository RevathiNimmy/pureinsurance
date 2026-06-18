
/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select data from table
Test Code     : Exec spu_Renewal_PlanExitsForSingleInstalmentPolicy    
 */

EXECUTE DDLDropProcedure 'spu_Renewal_PlanExitsForSingleInstalmentPolicy'
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE spu_Renewal_PlanExitsForSingleInstalmentPolicy    
    @nInsurance_file_cnt INT    
AS    
	DECLARE @nproduct_id INT    
	DECLARE @nlead_agent_cnt INT    
	DECLARE @npfprem_finance_cnt INT    
	DECLARE @nRenewalPlanExists INT   
	DECLARE @nRenewal_insurance_file_cnt INT   
	SELECT @nproduct_id=product_id,@nlead_agent_cnt=lead_agent_cnt FROM insurance_file WHERE insurance_file_cnt=@nInsurance_file_cnt    
	    
	SELECT TOP 1  @npfprem_finance_cnt= pfpm.pfprem_finance_cnt FROM pfpremiumfinance pfpm    
	INNER JOIN insurance_file ifl ON pfpm.insurance_file_cnt=ifl.insurance_file_cnt    
	WHERE ifl.product_id=@nproduct_id AND pfpm.agent_cnt=@nlead_agent_cnt AND TransType='REN'    
	ORDER BY pfpm.pfprem_finance_cnt DESC    

	IF EXISTS (SELECT 1 FROM pfpremiumfinance WHERE  pfpremiumfinance.pfprem_finance_cnt=@npfprem_finance_cnt AND pfpremiumfinance.statusind='999') 
	BEGIN
		SET @npfprem_finance_cnt=0
	END
    
	IF ISNULL(@npfprem_finance_cnt,0)=0    
	BEGIN    
		SET @nRenewalPlanExists=0    
		SELECT @nRenewalPlanExists,0    
	END    
	ELSE    
	BEGIN    
		SELECT TOP 1 @nRenewal_insurance_file_cnt=insurance_file_cnt FROM pfpremiumfinance WHERE pfprem_finance_cnt=@npfprem_finance_cnt   
		ORDER BY pfprem_finance_cnt,pfprem_finance_version DESC  
  
		SET @nRenewalPlanExists=1		
		SELECT @nRenewalPlanExists,@nRenewal_insurance_file_cnt   
END 