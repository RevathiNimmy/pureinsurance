

/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select details from party_agent table
Test Code     : Exec spu_Is_SingleInstallment_For_Credit_Control
 */

SET QUOTED_IDENTIFIER OFF
GO
Execute DDLDropProcedure 'spu_Is_SingleInstallment_For_Credit_Control'
GO

CREATE PROCEDURE spu_Is_SingleInstallment_For_Credit_Control
	@nCredit_control_item_id INT 
AS
	SELECT top 1 pta.is_single_instalment_plan From party_agent pta
	LEFT Join pfpremiumFinance pfmf On pta.party_cnt=pfmf.agent_cnt  
	Left Join Credit_Control_Item CCI  ON pfmf.pfprem_finance_cnt=CCI.pfprem_finance_cnt
	WHERE CCI.credit_control_item_id=@nCredit_control_item_id 
GO
