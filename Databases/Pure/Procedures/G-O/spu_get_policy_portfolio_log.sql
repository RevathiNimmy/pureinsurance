EXECUTE DDLDropProcedure 'spu_get_policy_portfolio_log'
GO
CREATE  PROCEDURE spu_get_policy_portfolio_log  
    @nInsurance_file_cnt INTEGER
   
AS  
  
BEGIN  
  SELECT effective_date
  FROM insurance_file_pt_log pt 
  WHERE insurance_file_cnt=@nInsurance_file_cnt 
  AND EXISTS (SELECT risk_cnt FROM ri_arrangement WHERE risk_cnt=pt.risk_cnt and version_id=2)
END   
