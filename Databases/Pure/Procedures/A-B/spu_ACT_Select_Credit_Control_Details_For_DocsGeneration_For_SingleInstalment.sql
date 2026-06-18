

/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select details from Insurance_File  table
Test Code     : Exec spu_ACT_Select_Credit_Control_Details_For_DocsGeneration_For_SingleInstalment  
 */


SET QUOTED_IDENTIFIER OFF
GO

Execute DDLDropProcedure 'spu_ACT_Select_Credit_Control_Details_For_DocsGeneration_For_SingleInstalment'
GO

CREATE PROCEDURE spu_ACT_Select_Credit_Control_Details_For_DocsGeneration_For_SingleInstalment  
	@nCredit_control_item_id INT  
AS  
DECLARE @enumLivePolicy                   INT = 2
            ,@enumPolicyUnderRenewal      INT = 3
            ,@enumMTAPermanent            INT = 5
            ,@enumMTAReinstated           INT = 9
	        
SELECT I.lead_agent_cnt,I.insurance_file_cnt,p.shortname   
	FROM Insurance_File I  
	INNER JOIN pfpremiumFinance pfmf ON I.insurance_file_cnt=pfmf.insurance_file_cnt  
	INNER JOIN Credit_Control_Item CCI  ON pfmf.pfprem_finance_cnt=CCI.pfprem_finance_cnt
	INNER JOIN Party p ON i.lead_agent_cnt=p.party_cnt  
	WHERE CCI.credit_control_item_id=@nCredit_control_item_id  
		AND I.policy_version=(SELECT MAX(ifl.policy_version) 
	FROM insurance_file ifl WHERE I.insurance_folder_cnt =ifl.insurance_folder_cnt AND 
		ifl.insurance_file_type_id in (@enumLivePolicy,@enumMTAPermanent,@enumMTAReinstated))  

GO





