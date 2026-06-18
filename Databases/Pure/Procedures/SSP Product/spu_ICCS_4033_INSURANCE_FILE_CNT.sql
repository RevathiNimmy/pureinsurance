DDLDROPPROCEDURE 'spu_ICCS_4033_INSURANCE_FILE_CNT'
GO            
CREATE PROCEDURE spu_ICCS_4033_INSURANCE_FILE_CNT            
        @lPartyCnt INT,            
        @lPolicyLinkId INT,            
        @lPolicyBinderId INT,            
        @lInsuranceFileCnt INT,            
        @lInsuranceFolderCnt INT            
AS     
         
SELECT @lInsuranceFileCnt 'insurance_file_cnt',    
I_F_T.CODE FROM INSURANCE_FILE I_F    
INNER JOIN INSURANCE_FILE_TYPE I_F_T    
ON I_F.INSURANCE_FILE_TYPE_ID=I_F_T.INSURANCE_FILE_TYPE_ID    
WHERE  --I_F_T.CODE='WRITTEN'    
--AND     
INSURANCE_FILE_CNT=@lInsuranceFileCnt
GO