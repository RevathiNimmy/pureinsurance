

/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select insurance file details from Insurance_File  table
Test Code     : Exec spu_ACT_Select_Credit_Control_AutoCancel_For_SingleInstalment  
 */

SET QUOTED_IDENTIFIER OFF
GO

Execute DDLDropProcedure 'spu_ACT_Select_Credit_Control_AutoCancel_For_SingleInstalment'
GO

CREATE PROCEDURE spu_ACT_Select_Credit_Control_AutoCancel_For_SingleInstalment  
	@nCredit_control_item_id INT 
AS
	Select I.insured_cnt,I.insurance_folder_cnt   
	FROM Insurance_File I
	LEFT JOIN pfpremiumFinance pfmf ON I.insurance_file_cnt=pfmf.insurance_file_cnt  
	LEFT JOIN Credit_Control_Item CCI  ON pfmf.pfprem_finance_cnt=CCI.pfprem_finance_cnt
	WHERE CCI.credit_control_item_id = @nCredit_control_item_id  
GO


