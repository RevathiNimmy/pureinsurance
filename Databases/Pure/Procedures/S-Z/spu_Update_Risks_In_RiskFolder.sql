SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Update_Risks_In_RiskFolder'
GO

CREATE PROCEDURE spu_Update_Risks_In_RiskFolder  
 @nBaseInsuranceFileCnt INT,  
 @nNewInsuranceFileCnt INT,  
 @nOriginalInsuranceFileCnt INT,  
 @nPreChangeInsuranceFileCnt INT,  
 @nOldRiskCnt INT,  
 @nNewRiskCnt INT  
AS  
BEGIN  
DECLARE @nRiskFolderCnt INT  
DECLARE @status VARCHAR(1)  
DECLARE @nNextIFile INT  
  
SELECT @nRiskFolderCnt = risk_folder_cnt  
   FROM risk Where risk_cnt = @nOldRiskCnt  
  
SELECT @nNextIFile = 0  
SELECT @nNextIFile = mifl_next.new_linked_insurance_file_cnt FROM mta_insurance_file_link mifl_next  
 Inner Join mta_insurance_file_link mifl_curr  
   ON mifl_curr.insurance_file_cnt = mifl_next.insurance_file_cnt  
    And mifl_curr.sequence_number = mifl_next.sequence_number - 1  
 WHERE mifl_curr.new_linked_insurance_file_cnt = @nNewInsuranceFileCnt  
  
IF @nNextIFile > 0 -- if future version exists  
BEGIN  
 -- If risk is deleted in current version remove from all future version  
 IF Exists(SELECT Null From insurance_file_risk_link ifrl WITH (NOLOCK)  
    Inner Join mta_insurance_file_link mifl WITH (NOLOCK) ON mifl.new_linked_insurance_file_cnt = ifrl.insurance_file_cnt  
    Inner Join risk r ON r.risk_cnt = ifrl.risk_cnt  
     WHERE ifrl.insurance_file_cnt = @nNewInsuranceFileCnt And r.risk_folder_cnt = @nRiskFolderCnt And status_flag = 'D')  
 BEGIN  
  DELETE insurance_file_risk_link  
   FROM insurance_file_risk_link ifrl  
   Inner Join risk r ON r.risk_cnt = ifrl.risk_cnt  
   Inner Join mta_insurance_file_link mifl ON mifl.new_linked_insurance_file_cnt = ifrl.insurance_file_cnt  
   WHERE ifrl.insurance_file_cnt >= @nNextIFile  
   AND     r.risk_folder_cnt = @nRiskFolderCnt  
  
  RETURN  
 END  
 ELSE -- If risk is not deleted in current version  
 BEGIN  
  -- Check if not exist in future, copy the link  
  If Not Exists(SELECT Null From insurance_file_risk_link ifrl WITH (NOLOCK)  
     Inner Join mta_insurance_file_link mifl WITH (NOLOCK) On mifl.new_linked_insurance_file_cnt = ifrl.insurance_file_cnt  
     Inner Join risk r ON r.risk_cnt = ifrl.risk_cnt  
      WHERE ifrl.insurance_file_cnt > @nNewInsuranceFileCnt And r.risk_folder_cnt = @nRiskFolderCnt)  
  Begin  
   -- For renewal version insert R flag  
   IF Exists (Select Null FROM insurance_file WHERE insurance_file_cnt = @nNextIFile And insurance_file_type_id = 3)  
    SELECT @status = 'R'  
   ELSE  
    SELECT @status = 'U'  
  
   IF  @status = 'U' OR EXISTS (  
    SELECT NULL FROM Risk r  
     Inner Join insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt  
     Inner Join mta_insurance_file_link mifl ON mifl.original_linked_insurance_file_cnt = ifrl.insurance_file_cnt  
     WHERE mifl.new_linked_insurance_file_cnt = @nNextIFile AND r.risk_folder_cnt = @nRiskFolderCnt  
    ) BEGIN  
     INSERT INTO insurance_file_risk_link (  
         insurance_file_cnt,  
         risk_cnt,  
         status_flag)  
     SELECT   @nNextIFile,  
         risk_cnt,  
         @status  
     FROM insurance_file_risk_link  
     WHERE insurance_file_cnt = @nNewInsuranceFileCnt  
     AND     risk_cnt = @nOldRiskCnt  
    END  
    ELSE BEGIN  
     DELETE insurance_file_risk_link  
      FROM insurance_file_risk_link ifrl  
      Inner Join risk r ON r.risk_cnt = ifrl.risk_cnt  
      WHERE ifrl.insurance_file_cnt = @nNextIFile  
      AND     r.risk_folder_cnt = @nRiskFolderCnt  
     RETURN  
    END  
  End  
 End  
End  
  
IF (@nOldRiskCnt = @nNewRiskCnt) BEGIN  
 SELECT @nOldRiskCnt = r.risk_cnt  
  FROM insurance_file_risk_link ifrl  
   Inner Join risk r ON r.risk_cnt = ifrl.risk_cnt  
  WHERE ifrl.insurance_file_cnt = @nOriginalInsuranceFileCnt  
    And r.risk_folder_cnt = @nRiskFolderCnt  
  
 -- point the risk back to original version and reset the status  
 UPDATE insurance_file_risk_link  
  SET status_flag = 'U',  
    risk_cnt = @nOldRiskCnt  
    WHERE insurance_file_cnt = @nNewInsuranceFileCnt  
      AND risk_cnt = @nNewRiskCnt  
	  
 -- Update all future unquoted risks as well
 Update insurance_file_risk_link SET risk_cnt = @nOldRiskCnt FROM insurance_file_risk_link ifrl  
  Inner Join mta_insurance_file_link mifl ON mifl.new_linked_insurance_file_cnt = ifrl.insurance_file_cnt  
  WHERE ifrl.insurance_file_cnt > @nNewInsuranceFileCnt And risk_cnt = @nNewRiskCnt  

END  
ELSE BEGIN  
 -- Update all future unquoted risks with new risk_cnt to derive from  
 Update insurance_file_risk_link SET risk_cnt = @nNewRiskCnt FROM insurance_file_risk_link ifrl  
  Inner Join mta_insurance_file_link mifl ON mifl.new_linked_insurance_file_cnt = ifrl.insurance_file_cnt  
  WHERE ifrl.insurance_file_cnt > @nNewInsuranceFileCnt And risk_cnt = @nOldRiskCnt  
  
 -- if risk was edited in base version, unquote it if not deleted  
 IF Exists(SELECT NULL FROM insurance_file_risk_link ifrl  
    Inner Join risk r ON r.risk_cnt = ifrl.risk_cnt  
     WHERE ifrl.is_risk_edited = 1 And r.risk_folder_cnt = @nRiskFolderCnt  
          And ifrl.insurance_file_cnt = @nBaseInsuranceFileCnt)  
 BEGIN  
  Update risk SET risk_status_id = 4 FROM Risk r  
    Inner Join insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt  
    WHERE r.risk_cnt = @nNewRiskCnt and ifrl.status_flag <> 'D'  
 END  
  
END  
END  