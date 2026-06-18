SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Update_Insurance_File_Details'
GO

CREATE PROCEDURE spu_Update_Insurance_File_Details  
 @nInsuranceFileCnt INT,  
 @nNewInsuranceFileCnt INT  
AS  
BEGIN  
DECLARE 
  @nLeadAgent  INT,
  @nBusinessType INT
SELECT  @nBusinessType= business_type_id,
  @nLeadAgent= lead_agent_cnt
  FROM insurance_file WHERE insurance_file_cnt =@nInsuranceFileCnt 
UPDATE insurance_file  
SET business_type_id=@nBusinessType,
 lead_agent_cnt=@nLeadAgent
WHERE insurance_file_cnt = @nNewInsuranceFileCnt
END 