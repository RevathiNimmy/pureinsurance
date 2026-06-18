ddldropprocedure 'spu_SAM_GetRiskFromRIArrangement'
go

CREATE PROCEDURE spu_SAM_GetRiskFromRIArrangement  
@ri_arrangement_id int  
AS  
Select ifrl.Insurance_file_cnt,ifrl.Risk_cnt from ri_arrangement ri join insurance_file_risk_link ifrl  
ON ifrl.risk_cnt=ri.risk_cnt  
WHERE ri_arrangement_id=@ri_arrangement_id  
GO