SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'Spu_SIR_Upd_BaseInsuranceFileCnt'
GO
CREATE PROCEDURE Spu_SIR_Upd_BaseInsuranceFileCnt  
@BaseIfileCnt int,  
@NewIFileCnt int  
AS  
BEGIN  
  
UPDATE insurance_file SET base_insurance_file_cnt = @BaseIfileCnt   
Where insurance_file_cnt = @NewIFileCnt  
  
END  
