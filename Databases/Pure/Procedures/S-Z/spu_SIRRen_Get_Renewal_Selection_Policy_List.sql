
--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************
EXECUTE DDLDropProcedure 'spu_SIRRen_Get_Renewal_Selection_Policy_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[spu_SIRRen_Get_Renewal_Selection_Policy_List]
     @Batch_Renewal_Job_Code VARCHAR(20) = NULL,
     @batch_id     INT = NULL,
     @insurance_folder_cnt  INT = NULL,
     @userName VARCHAR(255) = NULL,
	 @nGetCount INT = NULL
AS
/*
	ModifiedBy	Date		Description
	---------------------------------------------------------------------------------------------------------------------------------------------------------------
	GHarris		01/08/18	Changed the origincal CTE and added a new one to better cater to return only the top record as the max statement was inefficient

*/
BEGIN

 DECLARE @compare_date DATETIME
 DECLARE @Start_Date DATETIME
 DECLARE @Batch_Renewal_Job_ID INT
 DECLARE @UserID INT
 DECLARE @AllAgents INT
 DECLARE @Include_Direct_Policies TINYINT

 DECLARE @SSQL VARCHAR(8000)

SET @compare_date = convert(varchar(40), getdate(),106) + ' 11:59PM'
SET @Start_Date = convert(varchar(40), getdate(),106) + ' 12:00AM'
If @userName Is NOT NULL
SELECT @UserId = [USER_ID] FROM PMUser WHERE username = RTRIM(@userName)

IF @Batch_Renewal_Job_Code  IS NOT NULL
SELECT @Batch_Renewal_Job_ID = BRJ.batch_renewal_job_id,@AllAgents = all_agents,
  @Include_Direct_Policies = Include_Direct_Policies
  FROM Batch_Renewal_Job BRJ
  INNER JOIN batch_renewal_job_type BRJT ON BRJT.batch_renewal_job_type_id = BRJ.batch_renewal_job_type_id
  WHERE (BRJ.code = @Batch_Renewal_Job_Code )
  AND BRJ.is_active = 1
  AND BRJT.code = 'SEL'
ELSE
SELECT @Batch_Renewal_Job_ID = BRJ.batch_renewal_job_id,@AllAgents = all_agents,
  @Include_Direct_Policies = Include_Direct_Policies
  FROM Batch_Renewal_Job BRJ
  INNER JOIN batch_renewal_job_type BRJT ON BRJT.batch_renewal_job_type_id = BRJ.batch_renewal_job_type_id
  INNER JOIN Batch_Renewal_Job_Run_Insurance_Folder
ON  Batch_Renewal_Job_Run_Insurance_Folder.batch_renewal_job_id = BRJ.batch_renewal_job_id
  WHERE (Batch_Renewal_Job_Run_Insurance_Folder.batch_id = @batch_id
            AND  Batch_Renewal_Job_Run_Insurance_Folder.insurance_folder_cnt  =@insurance_folder_cnt)
  AND BRJ.is_active = 1
  AND BRJT.code = 'SEL'

  SET @SSQL = '	;WITH Insurance_File_CTE(InsuranceFolderCnt,InsuranceFileCnt,RowNumber)
				AS
				(
				SELECT insurance_folder_cnt, insurance_file_cnt,
				ROW_NUMBER() OVER ( Partition By IFL_Sub.insurance_folder_cnt Order By IFL_Sub.inception_date_tpi DESC,IFL_Sub.insurance_file_cnt DESC) As RowNumber
				FROM Insurance_File IFL_Sub
				JOIN Batch_Renewal_Job_Products BRJP ON IFL_Sub.product_id = BRJP.product_id
				JOIN Batch_Renewal_Job_Branches BRJB ON IFL_Sub.source_id = BRJB.source_id
				JOIN Product p  ON  p.product_id = IFL_Sub.product_id
				INNER JOIN Renewal_Frequency RF ON  
                IFL_Sub.renewal_frequency_id = RF.renewal_frequency_id
				WHERE ISNULL(RF.number_of_months,0) <> 0 AND  IFL_Sub.insurance_file_type_id IN (2,5,9)'
				SET @SSQL = @SSQL + ' AND BRJP.batch_renewal_job_id = ' + CONVERT(VARCHAR,@Batch_Renewal_Job_ID)   + CHAR(13)
				SET @SSQL = @SSQL + ' AND BRJB.batch_renewal_job_id = ' + CONVERT(VARCHAR,@Batch_Renewal_Job_ID)   + CHAR(13)
				IF ISNULL(@insurance_folder_cnt,0)<>0
				SET @sSQL = @sSQL + ' AND IFL_Sub.insurance_folder_cnt = '+  CONVERT(VARCHAR,@insurance_folder_cnt) + CHAR(13)
				SET @SSQL = @SSQL +	')'

SET @SSQL = @SSQL +',Insurance_Folder_CTE(InsuranceFolderCnt)
				AS
				(
				SELECT insurance_folder_cnt 
				FROM renewal_status r 
				join insurance_file i on r.renewal_insurance_file_cnt=i.insurance_file_cnt 
				where anniversary_copy=0 
				UNION ALL
				SELECT insurance_folder_cnt 
				FROM renewal_status r 
				join insurance_file i on r.renewal_insurance_file_cnt=i.insurance_file_cnt 
				where anniversary_copy=1 
				AND Exists (Select NULL From Insurance_File iFile Where iFile.insurance_folder_cnt = i.insurance_folder_cnt AND iFile.renewal_date = i.cover_start_date and iFile.insurance_file_type_id = 2)
				)'

  IF ISNULL(@nGetCount,0) = 1 BEGIN
  SET @SSQL =  @SSQL + '	SELECT Count(ifi.insurance_file_cnt)  FROM Insurance_File ifi ' + CHAR(13)
  END
  ELSE BEGIN
  SET @SSQL =  @SSQL + '	SELECT ifi.insurance_file_cnt, ifo.insurance_folder_cnt  FROM Insurance_File ifi ' + CHAR(13)
  END

  SET @SSQL =  @SSQL + '	JOIN Insurance_Folder ifo  ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
							JOIN Product p  ON  p.product_id = ifi.product_id
							JOIN Insurance_File_CTE ifi_cte ON ifi_cte.InsuranceFileCnt = ifi.insurance_file_cnt' + CHAR(13)
							--JOIN (SELECT MAX(rownumber) RowNumber, InsuranceFolderCnt FROM Insurance_File_CTE Group BY InsuranceFolderCnt) ifi_cte_max ON ifi_cte_max.InsuranceFolderCnt = ifi_cte.InsuranceFolderCnt AND ifi_cte_max.RowNumber = ifi_cte.RowNumber ' + CHAR(13)

       IF ISNULL(@UserId,0) > 0
       BEGIN
							SET @sSQL = @sSQL + ' LEFT JOIN (SELECT source_id FROM pmuser_source WITH(NOLOCK) WHERE user_id = ' + CONVERT(VARCHAR,@UserId )+ ') userbranch ON userbranch.source_id=ifi.source_id ' + CHAR(13)
       END

  IF @AllAgents = 0 AND @Include_Direct_Policies = 0 --Selected agents and no direct business
  BEGIN
       SET @sSQL = @sSQL + 'INNER JOIN Batch_Renewal_Job_Agents BRJA ON ifi.lead_agent_cnt = BRJA.party_cnt  ' + CHAR(13)
  END

	   SET @sSQL = @sSQL + 'WHERE (ISNULL(ifi.insurance_file_status_id, 4) IN (4 ,309))' + CHAR(13)
	   SET @sSQL = @sSQL + 'AND ifi.renewal_date <= DATEADD(DAY, ISNULL(p.renewal_period, 0),''' + CONVERT(VARCHAR,@compare_date) + ''')  ' + CHAR(13)

  IF @AllAgents = 0 AND @Include_Direct_Policies = 0 --Selected agents and no direct business
  BEGIN
       SET @sSQL = @sSQL + ' AND BRJA.batch_renewal_job_id = ' + CONVERT(VARCHAR,@Batch_Renewal_Job_ID)   + CHAR(13)
  END

  IF ISNULL(@UserId,0) > 0
   SET @sSQL = @sSQL + ' AND userbranch.source_id IS NULL ' + CHAR(13)

  SET @sSQL = @sSQL + ' AND IFI.insurance_folder_cnt NOT IN (SELECT InsuranceFolderCnt FROM Insurance_Folder_CTE) ' + CHAR(13)

  IF @AllAgents = 1 AND @Include_Direct_Policies = 0 --ALL agents and no direct business
  BEGIN
    SET @sSQL = @sSQL + 'AND ISNULL(ifi.lead_agent_cnt,0) > 0' + CHAR(13)
  END

  IF @AllAgents = 0 AND @Include_Direct_Policies = 1 --Selected agents and all direct business
  BEGIN
       SET @sSQL = @sSQL + 'AND (ifi.lead_agent_cnt in (SELECT party_cnt from Batch_Renewal_Job_Agents WHERE ' + CHAR(13)
       SET @sSQL = @sSQL + 'Batch_Renewal_Job_ID = ' + CONVERT(VARCHAR,@Batch_Renewal_Job_ID) + ') OR (ISNULL(ifi.lead_agent_cnt,0) = 0))' + CHAR(13)
  END

  IF ISNULL(@insurance_folder_cnt,0)<>0
   SET @sSQL = @sSQL + ' AND ifi.insurance_folder_cnt = '+  CONVERT(VARCHAR,@insurance_folder_cnt)
   SET @sSQL = @sSQL + ' AND ifi_cte.RowNumber = 1'

   IF ISNULL(@nGetCount,0) <> 1 BEGIN
   SET @sSQL = @sSQL + ' Order by ifi.insurance_file_cnt' + CHAR(13)
   END

	--PRINT @sSQL
  EXEC(@sSQL)

 END  

GO
