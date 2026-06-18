SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

EXECUTE DDLDropProcedure 'spu_DeletePolicy'
GO

--*********************************************************************************************************
-- Name : spu_DeletePolicy
--
-- Desc : delete this version of the policy and its dependanies
--
-- Note : we will need to delete more, but this will do for now
--**********************************************************************************************************

CREATE PROCEDURE spu_DeletePolicy  
    @nInsuranceFileCnt int  
  
AS  
  
BEGIN  
  
 DECLARE @ReturnValue int,  
   @DocumentRef varchar(25),  
   @ClaimID int,  
   @RiskCnt int,  
   @RiskFolderCnt int  
  
 SELECT @ReturnValue = 0  
  
 BEGIN TRANSACTION  
  
  -- This should be the first statement to be executed  
  -- Assuming success , Failure record is inserted from BatchQuoteDeletion  
  EXEC SPU_INSERT_INSURANCE_FILE_DELETE_LOG @nInsuranceFileCnt,1  
  IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete credit control items  
 DELETE  FROM Credit_Control_Item  WITH (ROWLOCK)
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- **********************DELETE REINSURANCE FROM RISK ROUTE**************************  
 -- delete ri_arrangement_line  
 DELETE RI_Arrangement_Line  
 FROM RI_Arrangement_Line ral JOIN RI_Arrangement rra ON ral.ri_arrangement_id = rra.ri_arrangement_id  
   JOIN Insurance_File_Risk_Link ifrl ON ifrl.risk_cnt = rra.risk_cnt  
 WHERE ifrl.insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete ri_arrangement  
 DELETE RI_Arrangement  
 FROM RI_Arrangement rra JOIN Insurance_File_Risk_Link ifrl ON rra.risk_cnt = ifrl.risk_cnt  
 WHERE ifrl.insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete insurance file tax  
 DELETE  FROM Tax_Calculation  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete policy_fee_u  
 DELETE FROM Policy_Fee_U  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete policy_fee  
 DELETE FROM Policy_Fee  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 --*************************DELETE CLAIM****************************************  
 DECLARE CursorClaim CURSOR FAST_FORWARD FOR  
  SELECT claim_id FROM Claim WHERE policy_id = @nInsuranceFileCnt  
  
 OPEN CursorClaim  
 FETCH NEXT FROM CursorClaim INTO @ClaimID  
 WHILE @@FETCH_STATUS = 0  
 BEGIN  
  -- delete all claim's dependant  
  EXEC @ReturnValue = spu_delete_claim_details @ClaimID  
  IF @ReturnValue <> 0  
   GOTO Error_Label  
  
  FETCH NEXT FROM CursorClaim INTO @ClaimID  
 END  
  
 -- delete all claims for this policy version  
 DELETE FROM Claim WHERE policy_id = @nInsuranceFileCnt  
  
 --*************************DELETE ACCOUNT STUFF********************************  
 DECLARE CursorDocument CURSOR FAST_FORWARD FOR  
  SELECT document_ref FROM Stats_Folder WHERE insurance_file_cnt = @nInsuranceFileCnt  
  
 OPEN CursorDocument  
 FETCH NEXT FROM CursorDocument INTO @DocumentRef  
 WHILE @@FETCH_STATUS = 0  
 BEGIN  
  
  EXEC @ReturnValue = spu_DeleteDocument @DocumentRef  
  IF @ReturnValue <> 0  
   GOTO Error_Label  
  
  FETCH NEXT FROM CursorDocument INTO @DocumentRef  
 END 
 
 -- ************************DELETE RISK STUFF****************************  
 -- delete all risks for this policy version (only if it links to one version of the policy)  
 DECLARE CursorRisk CURSOR FOR  
  SELECT risk_cnt FROM Insurance_File_Risk_Link WHERE insurance_file_cnt = @nInsuranceFileCnt  
  
  OPEN CursorRisk  
  FETCH NEXT FROM CursorRisk INTO @RiskCnt  
  WHILE @@FETCH_STATUS = 0  
  BEGIN  
  
   -- only delete this risk if it links to only one version of the policy  
   IF (SELECT COUNT(*) FROM Insurance_File_Risk_Link WHERE risk_cnt = @RiskCnt GROUP BY risk_cnt) = 1  
   BEGIN  
  
	-- ************************DELETE DOCUMENTS START****************************    
	DECLARE @DATA_MODEL_CODE VARCHAR(255)
	DECLARE @STANDARD_WORDING VARCHAR(255) = '_standard_wording'
	DECLARE @POLICY_BINDER VARCHAR(255) = '_Policy_Binder'
	DECLARE @STANDARD_WORDING_TABLE VARCHAR(255) 
	DECLARE @POLICY_BINDER_TABLE VARCHAR(255) 
	DECLARE @sSQL varchar(1000)
	
	SELECT @DATA_MODEL_CODE = GDM.code
	FROM Insurance_File IFL
	JOIN insurance_file_risk_link IFRL ON IFRL.insurance_file_cnt = IFL.insurance_file_cnt
	JOIN GIS_Policy_Link GPL ON GPL.risk_id = IFRL.risk_cnt
	JOIN GIS_Data_Model GDM ON GDM.gis_data_model_id = GPL.gis_data_model_id
	WHERE IFL.insurance_file_cnt = @nInsuranceFileCnt
	SET @DATA_MODEL_CODE = LTRIM(RTRIM(@DATA_MODEL_CODE))
	
	IF @DATA_MODEL_CODE <> ''
	BEGIN  
	  SET @STANDARD_WORDING_TABLE = @DATA_MODEL_CODE + @STANDARD_WORDING  
	  SET @POLICY_BINDER_TABLE =  @DATA_MODEL_CODE + @POLICY_BINDER  
	  SET @sSQL = 'DELETE FROM ' + CONVERT(varchar,@STANDARD_WORDING_TABLE) + ' WHERE ' + @DATA_MODEL_CODE + '_Policy_binder_id IN ('  
	  SET @sSQL = + @sSQL + 'SELECT DISTINCT DMSW.' + @DATA_MODEL_CODE + '_Policy_binder_id  
	  FROM Insurance_File IFL  
	  JOIN insurance_file_risk_link IFRL ON IFRL.insurance_file_cnt = IFL.insurance_file_cnt  
	  JOIN GIS_Policy_Link GPL ON GPL.risk_id = IFRL.risk_cnt  
	  JOIN GIS_Data_Model GDM ON GDM.gis_data_model_id = GPL.gis_data_model_id '  
	  SET @sSQL = @sSQL + 'JOIN '  
	  SET @sSQL = @sSQL + CONVERT(varchar,@POLICY_BINDER_TABLE) + ' DMPB ON DMPB.gis_policy_link_id = GPL.gis_policy_link_id '  
	  SET @sSQL = @sSQL + 'JOIN '  
	  SET @sSQL = @sSQL + CONVERT(varchar,@STANDARD_WORDING_TABLE) + ' DMSW ON DMSW.' + @DATA_MODEL_CODE + '_Policy_binder_id = DMPB.gis_policy_link_id '  
	  SET @sSQL = @sSQL + 'WHERE IFL.insurance_file_cnt = ' + CONVERT(varchar,@nInsuranceFileCnt) + ' '  
	  SET @sSQL = @sSQL + ' ) '  
	  EXECUTE (@sSQL)  
	END  
	ELSE
	BEGIN
		-- Raise Error, 
		GOTO Error_Label
	END
	
	IF @@ERROR <> 0  
		GOTO Error_Label  
	 
	
	-- ************************DELETE DOCUMENTS END****************************  
	
	
    -- delete insurance_file_risk_link  
    DELETE FROM insurance_file_risk_link  
    WHERE insurance_file_cnt = @nInsuranceFileCnt  
    AND  risk_cnt = @RiskCnt  
    IF @@ERROR <> 0  
     GOTO Error_Label  
  
    -- delete tax calculation  
    DELETE Tax_Calculation WHERE risk_cnt = @RiskCnt  
    IF @@ERROR <> 0  
     GOTO Error_Label  
  
    -- delete accumulation values  
    DELETE Accumulation_Values WHERE risk_cnt = @RiskCnt  
    IF @@ERROR <> 0  
     GOTO Error_Label  
  
    -- delete perils for this risk  
    DELETE Peril WHERE risk_cnt = @RiskCnt  
    IF @@ERROR <> 0  
     GOTO Error_Label  
  
    -- delete rating_section for this risk  
    DELETE Rating_Section WHERE risk_cnt = @RiskCnt  
    IF @@ERROR <> 0  
     GOTO Error_Label  
  
    -- delete risk_folder if we've only got one risk in it  
    IF (SELECT Count(r.risk_cnt) FROM Risk r JOIN Risk r2 ON r.risk_folder_cnt = r2.risk_folder_cnt WHERE r.risk_cnt = @RiskCnt) = 1  
    BEGIN  
     --Fetch risk_folder_cnt before deleting it  
     Select @RiskFolderCnt = risk_folder_cnt From Risk Where risk_cnt = @RiskCnt  
  
     -- delete risk  
     DELETE Risk WHERE risk_cnt = @RiskCnt  
     IF @@ERROR <> 0  
      GOTO Error_Label  
  
     DELETE  Risk_Folder  
     WHERE  risk_folder_cnt = @RiskFolderCnt  
     IF @@ERROR <> 0  
      GOTO Error_Label  
    END  
    ELSE  
    BEGIN  
     -- delete risk  
     DELETE Risk WHERE risk_cnt = @RiskCnt  
     IF @@ERROR <> 0  
      GOTO Error_Label  
    END  
  
   END  
   ELSE  
   BEGIN  
    -- delete insurance_file_risk_link (Note: we want to delete this link regardless)  
    DELETE FROM insurance_file_risk_link  
    WHERE insurance_file_cnt = @nInsuranceFileCnt  
    AND  risk_cnt = @RiskCnt  
    IF @@ERROR <> 0  
     GOTO Error_Label  
   END  
  
   FETCH NEXT FROM CursorRisk INTO @RiskCnt  
  END  
  
 -- ************************DELETE POLICY LEVEL STUFF****************************  
  
 -- delete agent_commission  
 DELETE  agent_commission  
 WHERE  insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete insurance_file_agent  
 DELETE  FROM insurance_file_agent  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete policy_standard_wording  
 DELETE  FROM policy_standard_wording  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete Document_Spooler records  
 DELETE FROM Document_Spooler  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete event_log  
 DELETE FROM event_log  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete transaction_export_detail  
 DELETE Transaction_Export_Detail  
 FROM Transaction_Export_Detail ted JOIN Transaction_Export_Folder tef ON ted.transaction_export_folder_cnt = tef.transaction_export_folder_cnt  
 WHERE tef.insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete transaction_export_folder  
 DELETE FROM Transaction_Export_Folder  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete stats_detail  
 DELETE Stats_Detail  
 FROM Stats_Detail sd JOIN Stats_Folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt  
 WHERE sf.insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete stats_folder  
 DELETE FROM Stats_Folder  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete Insurance_File_Deferred_RI_Usage  
 DELETE FROM Insurance_File_Deferred_RI_Usage  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
 -- delete Insurance_File_System  
 DELETE FROM Insurance_File_System  
 WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  
  
 -- DELETE Insurance_Folder if we are deleting the last or only version of the policy  
  IF (SELECT count(ifi2.insurance_file_cnt) FROM Insurance_File ifi JOIN Insurance_File ifi2 ON ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt WHERE ifi.insurance_file_cnt = @nInsuranceFileCnt) = 1  
 BEGIN  
  
  -- delete Insurance_File  
   DELETE FROM Insurance_File  
   WHERE insurance_file_cnt = @nInsuranceFileCnt  
   IF @@ERROR <> 0  
   GOTO Error_Label  
  
  DELETE  Insurance_Folder  
  FROM  Insurance_Folder ifo JOIN Insurance_File ifi ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
   WHERE  ifi.insurance_file_cnt = @nInsuranceFileCnt  
  IF @@ERROR <> 0  
   GOTO Error_Label  
 END  
  ELSE  
  BEGIN  
 -- delete Insurance_File  
 DELETE FROM Insurance_File  
   WHERE insurance_file_cnt = @nInsuranceFileCnt  
 IF @@ERROR <> 0  
  GOTO Error_Label  
  END  
  
 COMMIT TRANSACTION  
  
 GOTO End_Label  
  
 Error_Label:  
  
  ROLLBACK TRANSACTION  
  SELECT @ReturnValue = -1  
  
 End_Label:  
  
  Close CursorRisk  
  Deallocate CursorRisk  
  
  Close CursorClaim  
  Deallocate CursorClaim  
  
  Close CursorDocument  
  Deallocate CursorDocument  
  
  RETURN @ReturnValue  
  
END  
GO
