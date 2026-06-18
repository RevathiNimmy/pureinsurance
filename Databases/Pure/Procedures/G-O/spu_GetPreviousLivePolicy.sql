SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetPreviousLivePolicy'
GO

CREATE PROCEDURE spu_GetPreviousLivePolicy  
  @nInsuranceFileCnt INT ,  
  @nRiskCnt INT  
  
AS  
BEGIN
  
DECLARE @nMaxVersion INT  
DECLARE @nInsuranceFolderCnt Int  
DECLARE @dtCoverStartDate DATETIME  
DECLARE @nRiskFolderCnt INT  
DECLARE @nInsuranceFileType INT  
  
SELECT @dtCoverStartDate = cover_start_date , @nInsuranceFolderCnt = insurance_folder_cnt , @nInsuranceFileType = insurance_file_type_id  
 FROM insurance_file  
  WHERE insurance_file_cnt = @nInsuranceFileCnt  
SELECT @nRiskFolderCnt = risk_folder_cnt FROM risk WHERE risk_cnt = @nRiskCnt  
  
IF @nInsuranceFileType = 3  
SELECT  
    @nMaxVersion = MAX(policy_version)  
FROM insurance_file  
WHERE insurance_folder_cnt=@nInsuranceFolderCnt  
        AND insurance_file_type_id in (2, 5, 8, 9)  
                  AND cover_start_date < @dtCoverStartDate  
ELSE  
SELECT  
    @nMaxVersion = MAX(policy_version)  
FROM insurance_file  
WHERE insurance_folder_cnt=@nInsuranceFolderCnt  
        AND insurance_file_type_id in (2, 5, 8, 9)  
                  AND cover_start_date <= @dtCoverStartDate  
                        AND insurance_file_cnt < @nInsuranceFileCnt  
  
SELECT  
    i.insurance_file_cnt ,  
    ifrl.risk_cnt  
FROM insurance_file i  
JOIN insurance_file_risk_link ifrl  
ON i.insurance_file_cnt = ifrl.insurance_file_cnt  
JOIN risk  r ON r.risk_cnt = ifrl .risk_cnt  
WHERE i.policy_version = @nMaxVersion  
AND i.insurance_folder_cnt = @nInsuranceFolderCnt  
AND r.risk_folder_cnt = @nRiskFolderCnt  

End
Go