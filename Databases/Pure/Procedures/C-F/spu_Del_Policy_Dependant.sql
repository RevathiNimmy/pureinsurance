SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

EXECUTE DDLDropProcedure 'spu_Del_Policy_Dependant'
GO


CREATE PROCEDURE spu_Del_Policy_Dependant
					@insurance_file_cnt int

AS

/*********************************************************************************************************
* Name : spu_Del_Policy_Dependant
*
* Desc : delete stuff that hangs of policy eg risk, perils...etc
*
* Hist : 15/02/2001 Created - Tinny
*      : 24 july 2001 - delete insurance_file_tax and policy_standard_wording - Tinny
*	   : 28/07/2003 Tracy Richards - Now deletes from Document_Spooler
* Ver  : 1.00.0002
*
*	1.00.0003	Remove associated agent_commission records.	23/08/01	RWH
*
* Note : for now we only delete insurance_file_risk_link, ins_file_ri_arrangement and ri_arrangement_line
*		 delete policy_standard_wording
*        DO NOT FORGET TO DELETE THE REST OF THEM AS WELL
**********************************************************************************************************/

BEGIN TRANSACTION

DECLARE @nPFprem_finance_cnt INT  
DECLARE @nPFprem_finance_version INT
DECLARE @nRenewal_status_cnt INT

-- delete insurance file tax
DELETE 	FROM tax_calculation
WHERE	insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0
	GOTO Error_Label

--RWH(23/08/01) Remove associated agent_commission records.
DELETE 	agent_commission
WHERE	 insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0
	GOTO Error_Label

-- delete insurance_file_agent
DELETE 	FROM insurance_file_agent
WHERE	insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0
	GOTO Error_Label

-- delete policy_standard_wording
DELETE 	FROM policy_standard_wording
WHERE	insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0
	GOTO Error_Label


-- delete insurance_file_risk_link
DELETE	FROM insurance_file_risk_link
WHERE	insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0
	GOTO Error_Label

-- delete Document_Spooler records
DELETE	FROM Document_Spooler
WHERE	insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0
	GOTO Error_Label

-- delete event_log
--UPDATE event_log
DELETE FROM event_log    
WHERE insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0    
 GOTO Error_Label    

-- delete renewal_report    
DELETE FROM renewal_report    
WHERE insurance_file_cnt = @insurance_file_cnt    


IF @@ERROR <> 0
	GOTO Error_Label


SELECT @nPFprem_finance_cnt=pfprem_finance_cnt,@nPFprem_finance_version=pfprem_finance_version FROM pfpremiumfinance  
WHERE insurance_file_cnt = @insurance_file_cnt  
  
IF @@ERROR <> 0  
 GOTO Error_Label 

-- delete credit_control_item
DELETE FROM credit_control_item 
WHERE insurance_file_cnt = @insurance_file_cnt  

IF @@ERROR <> 0    
 GOTO Error_Label

DELETE FROM pfinstalments    
WHERE pfprem_finance_cnt = @nPFprem_finance_cnt AND pfprem_finance_version=@nPFprem_finance_version  
  
IF @@ERROR <> 0    
 GOTO Error_Label    
  
-- delete pfpremiumfinance    
DELETE FROM pfpremiumfinance    
WHERE insurance_file_cnt = @insurance_file_cnt  
  IF @@ERROR <> 0    
 GOTO Error_Label    

SELECT @nRenewal_status_cnt=renewal_status_cnt FROM renewal_status  
WHERE renewal_insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0  
 GOTO Error_Label 
-- delete Last_Print_Run  
DELETE FROM Last_Print_Run  
WHERE renewal_status_cnt = @nRenewal_status_cnt  
  
IF @@ERROR <> 0  
 GOTO Error_Label  

-- delete renewal_status  
DELETE FROM renewal_status  
WHERE renewal_insurance_file_cnt = @insurance_file_cnt 
  
IF @@ERROR <> 0  
 GOTO Error_Label 

-- delete batch renewal job
DELETE FROM Batch_Renewal_Job_Runs 
WHERE insurance_file_cnt = @insurance_file_cnt  
  
IF @@ERROR <> 0  
 GOTO Error_Label

 DELETE FROM Insurance_File_System     
WHERE insurance_file_cnt = @insurance_file_cnt    
IF @@ERROR <> 0    
 GOTO Error_Label     
   
DELETE FROM mid_vehicle 
  WHERE mid_policy_id in (SELECT mid_policy_id FROM   mid_policy
WHERE insurance_file_cnt = @insurance_file_cnt)  
   
  DELETE FROM mid_policy      
WHERE insurance_file_cnt = @insurance_file_cnt    
    
IF @@ERROR <> 0    

 GOTO Error_Label

--RWH(23/08/01) Remove associated insurance file payment records.

DELETE 	Insurance_File_Payment_Details

WHERE	 insurance_file_cnt = @insurance_file_cnt

IF @@ERROR <> 0

 GOTO Error_Label

--RWH(23/08/01) Remove associated insurance file Deferred RI records.

DELETE 	Insurance_File_Deferred_RI_Usage

WHERE	 insurance_file_cnt = @insurance_file_cnt
    
IF @@ERROR <> 0    
 GOTO Error_Label   

-- Remove associated insurance file Associates  records.
DELETE FROM Insurance_file_associates  
WHERE insurance_file_cnt = @insurance_file_cnt 

IF @@ERROR <> 0  
 GOTO Error_Label 
 
GOTO End_Label
Error_Label:    
    
ROLLBACK TRANSACTION    
 RETURN -1    
    
End_Label:    
-- save changes to database    
COMMIT TRANSACTION    
    
 RETURN 0   


GO
