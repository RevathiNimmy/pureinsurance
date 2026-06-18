SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  

Execute DDLDROPPROCEDURE spu_RI_Arrangement_sel_versions
GO

CREATE PROCEDURE spu_RI_Arrangement_sel_versions  
    @nRisk_cnt int  
AS  
DECLARE @effective_date datetime
  BEGIN  
    If EXISTS(Select null from RI_Arrangement where risk_cnt =@nRisk_cnt and Effective_Date is null )  
     BEGIN  
     
     SELECT @effective_date = CASE WHEN r.inception_date > ifi.inception_date_tpi THEN r.inception_date  
   ELSE ifi.inception_date_tpi END  
    FROM insurance_file ifi  
    INNER JOIN insurance_file_risk_link ifrl ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt  
    INNER JOIN risk r ON r.risk_cnt = ifrl.risk_cnt  
    WHERE r.risk_cnt = @nRisk_cnt
    
   UPDATE RI_Arrangement SET Effective_Date=@effective_date 
  WHERE Effective_Date Is Null and RI_Arrangement.risk_cnt =@nRisk_cnt  
  
     END  
  
    SELECT Distinct RA.version_id ,CONVERT(VARCHAR(100),RIVT.Description)+'-FY'+CONVERT(VARCHAR(100),YEAR(RA.Effective_Date)) 'Description', RA.Effective_Date  
  FROM RI_Arrangement RA JOIN RI_Version_Type RIVT ON RIVT.RI_Version_Type_id = ra.RI_Version_Type_id  
   WHERE risk_cnt =@nRisk_cnt 
  END  
GO
