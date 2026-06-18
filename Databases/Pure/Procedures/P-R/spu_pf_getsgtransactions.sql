DDLDROPPROCEDURE 'spu_pf_getsgtransactions'
GO
CREATE PROCEDURE [dbo].[spu_pf_getsgtransactions]  
 @pf_prem_finance_cnt [int],      
 @pf_prem_finance_version [int]      
AS      
select pft.pftransaction_id, ins.insurance_ref, td.amount, ins.insurance_file_cnt      
from insurance_file as ins, pftransaction_id as pft, transdetail as td    
where pft.pfprem_finance_cnt = @pf_prem_finance_cnt    
and pft.pfprem_finance_version = @pf_prem_finance_version      
and pft.insurance_file_cnt = ins.insurance_file_cnt    
and pft.pftransaction_id = td.transdetail_id    

GO

