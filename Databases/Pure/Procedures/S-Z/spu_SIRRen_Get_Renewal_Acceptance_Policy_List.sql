--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************
EXECUTE DDLDropProcedure 'spu_SIRRen_Get_Renewal_Acceptance_Policy_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIRRen_Get_Renewal_Acceptance_Policy_List
     @Batch_Renewal_Job_Code VARCHAR(20),
     @userName VARCHAR(255) = NULL

AS  
BEGIN  
        DECLARE @Renewal_date DATETIME
	DECLARE @Start_Date DATETIME
	DECLARE @Batch_Renewal_Job_ID INT
        DECLARE @UserID INT
	DECLARE @AllAgents INT
	DECLARE @Include_Direct_Policies TINYINT
	DECLARE @DayBeforeRenewalDate SMALLINT
	DECLARE @sSQL VARCHAR(8000)
	DECLARE @StateCnt INT 

  SELECT @UserId = [USER_ID] 
  FROM   PMUser 
  WHERE  username = LTRIM(RTRIM(ISNULL(@userName,''))) 
	
	SET @UserId = ISNULL(@UserId,0)
	SET @Renewal_date = convert(varchar(40), getdate(),106) 
	
        	SELECT  @Batch_Renewal_Job_ID = BRJ.batch_renewal_job_id,@AllAgents = all_agents,
		@Include_Direct_Policies = Include_Direct_Policies, @DayBeforeRenewalDate = BRJ.days_before_renewal_date
		FROM Batch_Renewal_Job BRJ
		LEFT JOIN batch_renewal_job_type BRJT ON BRJT.batch_renewal_job_type_id = BRJ.batch_renewal_job_type_id
		Where BRJ.code = @Batch_Renewal_Job_Code
		AND BRJ.is_active = 1 --Check for status active
		AND BRJT.code = 'ACC'  --Check for valid job type
	  
		 
		SET @sSQL =  'SELECT ' + CHAR(13)  
		SET @sSQL = @sSQL + 'rs.renewal_insurance_file_cnt, i.insurance_folder_cnt' + CHAR(13) 
	 

		SET @sSQL = @sSQL + 'FROM insurance_file i  ' + CHAR(13) 
		SET @sSQL = @sSQL + 'INNER JOIN renewal_status rs                  ON rs.renewal_insurance_file_cnt = i.insurance_file_cnt ' + CHAR(13)  
		SET @sSQL = @sSQL + 'INNER JOIN Batch_Renewal_Job_Products BRJP    ON BRJP.product_id = rs.product_id  ' + CHAR(13) 
		SET @sSQL = @sSQL + 'INNER JOIN Batch_Renewal_Job_Branches BRJB    ON BRJB.source_id = i.source_id  ' + CHAR(13) 
		SET @sSQL = @sSQL + 'INNER JOIN renewal_status_type rst            ON rst.renewal_status_type_id = rs.renewal_status_type_id  ' + CHAR(13) 
		
 
		IF @AllAgents = 0 AND @Include_Direct_Policies = 0 --Selected agents and no direct business
		BEGIN
			SET @sSQL = @sSQL + 'INNER JOIN Batch_Renewal_Job_Agents BRJA ON i.lead_agent_cnt = BRJA.party_cnt  ' + CHAR(13)
		END 

		IF @AllAgents = 1 AND @Include_Direct_Policies = 0 --ALL agents and no direct business
		BEGIN	
			SET @sSQL = @sSQL + 'AND ISNULL(i.lead_agent_cnt,0) > 0' + CHAR(13)                  
		END

		IF @AllAgents = 0 AND @Include_Direct_Policies = 1 --Selected agents and all direct business
		BEGIN	
			SET @sSQL = @sSQL + 'AND (i.lead_agent_cnt in (SELECT party_cnt from Batch_Renewal_Job_Agents WHERE ' + CHAR(13)
			SET @sSQL = @sSQL + 'Batch_Renewal_Job_ID = ' + CONVERT(VARCHAR,@Batch_Renewal_Job_ID) + ') OR (ISNULL(i.lead_agent_cnt,0) = 0))' + CHAR(13)
		END

		SET @sSQL = @sSQL + 'WHERE i.cover_start_date <=  DATEADD(DAY, ISNULL(' + CONVERT(VARCHAR,@DayBeforeRenewalDate)+', 0' + '),''' + CONVERT(VARCHAR,@Renewal_date) + ''')' + CHAR(13)

 

		SET @sSQL = @sSQL + 'AND (rs.renewal_status_type_id IN(SELECT renewal_status_type_id FROM renewal_status_type ' + CHAR(13) 
		SET @sSQL = @sSQL + 'WHERE UPPER(code) IN ('+'''UPDATE'''+')))' + CHAR(13)
		SET @sSQL = @sSQL + 'AND BRJB.Batch_Renewal_Job_ID =' + CONVERT(VARCHAR,@Batch_Renewal_Job_ID) + CHAR(13)
		SET @sSQL = @sSQL + 'AND BRJP.Batch_Renewal_Job_ID =' + CONVERT(VARCHAR,@Batch_Renewal_Job_ID) + CHAR(13)

		IF @AllAgents = 0 AND @Include_Direct_Policies = 0 --Selected agents and no direct business
		BEGIN
			SET @sSQL = @sSQL + 'AND BRJA.Batch_Renewal_Job_ID =' + CONVERT(VARCHAR,@Batch_Renewal_Job_ID) + CHAR(13)
		END 


		 IF ISNULL(@UserId,0) > 0 
		BEGIN
			SET @sSQL = @sSQL + ' AND i.Source_Id IN (SELECT s.source_id FROM source s WHERE  source_id NOT IN (SELECT source_id FROM pmuser_source WHERE [user_id] = ' + CONVERT(VARCHAR, @UserID) + ' )AND is_deleted = 0) '
		END
  
		SET @sSQL = @sSQL + ' AND ISNULL(i.out_of_sequence_replaced,0) <> 1 ' + CHAR(13)
		

		SET @sSQL = @sSQL + ' Order by i.insurance_folder_cnt,i.cover_start_date '
	EXEC(@sSQL)
END

