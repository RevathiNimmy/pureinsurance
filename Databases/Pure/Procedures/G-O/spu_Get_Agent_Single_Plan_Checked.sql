/* 
Description   : This is used to select is_single_instalment_plan details from party_agent table
Test Code     : Exec spu_Get_Agent_Single_Plan_Checked
 */
SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_Agent_Single_Plan_Checked'
GO

CREATE PROCEDURE spu_Get_Agent_Single_Plan_Checked
    @nInsurance_file_cnt INT   
AS    

	SELECT ISNULL(is_single_instalment_plan,0) 
	FROM party_agent PG
	INNER JOIN insurance_file INF ON PG.party_cnt=INF.lead_agent_cnt 
	WHERE INF.insurance_file_cnt = @nInsurance_file_cnt
  
GO

