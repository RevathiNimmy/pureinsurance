
/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to update details inPFTransaction_id table
Test Code     : Exec spu_PFTransaction_id_upd
 */

SET QUOTED_IDENTIFIER OFF
GO
Execute DDLDropProcedure 'spu_PFTransaction_id_upd'
GO

CREATE PROCEDURE spu_PFTransaction_id_upd
    @nPFprem_finance_cnt INT,  
    @nPFprem_finance_version INT,  
    @nInsurance_file_cnt INT  
AS  
BEGIN  
    UPDATE PFTransaction_id SET insurance_file_cnt=@nInsurance_file_cnt WHERE pfprem_finance_cnt=@nPFprem_finance_cnt
    AND pfprem_finance_version=@nPFprem_finance_version
END  

GO
