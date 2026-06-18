/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select details from Insurance_File  table
Test Code     : Exec spu_ACT_GetDetailsForAutoAllocate_ForSIP
 */

SET QUOTED_IDENTIFIER OFF 
GO
Execute DDLDropProcedure 'spu_ACT_GetDetailsForAutoAllocate_ForSIP'
GO

CREATE PROCEDURE spu_ACT_GetDetailsForAutoAllocate_ForSIP
	@nCredit_control_item_id INT  
AS  
	SELECT top 1 ac.account_id,pfmf.pfprem_finance_cnt 
	FROM Insurance_File I  
	INNER JOIN pfpremiumFinance pfmf ON I.insurance_file_cnt=pfmf.insurance_file_cnt  
	INNER JOIN Credit_Control_Item CCI  ON pfmf.pfprem_finance_cnt=CCI.pfprem_finance_cnt  
	INNER JOIN Account ac ON i.lead_agent_cnt=ac.account_key  
	WHERE CCI.credit_control_item_id=@nCredit_control_item_id  
GO
