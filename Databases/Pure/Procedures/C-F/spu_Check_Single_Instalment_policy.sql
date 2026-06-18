

/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select details from Insurance_File  table
Test Code     : Exec spu_Check_Single_Instalment_policy  
 */


SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_Check_Single_Instalment_policy'
GO

CREATE PROCEDURE spu_Check_Single_Instalment_policy  
   @nInsurance_file_cnt INT  
AS  
	SELECT ISNULL(pya.is_single_instalment_plan,0) FROM insurance_file ifl        
	INNER JOIN pfpremiumfinance pfp ON ifl.insurance_file_cnt = pfp.insurance_file_cnt  
	INNER JOIN party_agent pya ON  pfp.agent_cnt=pya.party_cnt  
	WHERE ifl.insurance_file_cnt=@nInsurance_file_cnt  

