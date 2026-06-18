--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************

EXECUTE DDLDropProcedure 'spu_SIRRen_Get_Renewal_Acceptance_Policy_Totals'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIRRen_Get_Renewal_Acceptance_Policy_Totals
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

	SET @Renewal_date = convert(varchar(40), getdate(),106) 
	 
       IF @userName Is NOT NULL
       SELECT @UserId = [USER_ID]
        FROM  PMUser 
        WHERE username = LTRIM(RTRIM(@userName)) 
  
        SELECT @Batch_Renewal_Job_ID = BRJ.batch_renewal_job_id,@AllAgents = all_agents,
	@Include_Direct_Policies = Include_Direct_Policies, @DayBeforeRenewalDate = BRJ.days_before_renewal_date
        FROM Batch_Renewal_Job BRJ
	LEFT JOIN batch_renewal_job_type BRJT ON BRJT.batch_renewal_job_type_id = BRJ.batch_renewal_job_type_id
        Where BRJ.code = @Batch_Renewal_Job_Code 
	AND BRJ.is_active = 1 --Check for status active        
	AND BRJT.code = 'ACC'  --Check for valid job type

	IF @AllAgents = 1 AND @Include_Direct_Policies = 1  -- (11) ALL AGENTS AND ALL DIRECT BUSINESS
        BEGIN 
		SELECT 
			COUNT(rs.renewal_insurance_file_cnt),rs.renewal_exception_reason_id

		FROM insurance_file i 
		INNER JOIN renewal_status rs                  ON rs.renewal_insurance_file_cnt = i.insurance_file_cnt 
		INNER JOIN Batch_Renewal_Job_Products BRJP    ON BRJP.product_id = rs.product_id 
		INNER JOIN Batch_Renewal_Job_Branches BRJB    ON BRJB.source_id = i.source_id 
		INNER JOIN renewal_status_type rst            ON rst.renewal_status_type_id = rs.renewal_status_type_id 
		WHERE i.cover_start_date <= DATEADD(DAY, ISNULL(@DayBeforeRenewalDate, 0), @Renewal_date)
		AND (rs.renewal_status_type_id IN(SELECT renewal_status_type_id FROM renewal_status_type
        		                          WHERE UPPER(code) IN ('UPDATE')))
		AND (i.Source_Id IN (SELECT s.source_id 
                                     FROM   source s 
                                     WHERE  source_id NOT IN (SELECT source_id 
                                                              FROM   pmuser_source 
                                                              WHERE  [user_id] =@UserId )AND is_deleted = 0) or @UserId is null) 
		AND BRJB.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
		AND BRJP.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
		GROUP BY rs.renewal_exception_reason_id
	END
	ELSE IF @AllAgents = 0 AND @Include_Direct_Policies = 0  --(00) SELECTED AGENTS AND NO DIRECT BUSINESS
        BEGIN 
		SELECT 
			COUNT(rs.renewal_insurance_file_cnt),rs.renewal_exception_reason_id

		FROM insurance_file i 
		INNER JOIN renewal_status rs                  ON rs.renewal_insurance_file_cnt = i.insurance_file_cnt 
		INNER JOIN Batch_Renewal_Job_Products BRJP    ON BRJP.product_id = rs.product_id 
		INNER JOIN Batch_Renewal_Job_Branches BRJB    ON BRJB.source_id = i.source_id 
		INNER JOIN Batch_Renewal_Job_Agents BRJA      ON BRJA.party_cnt = i.lead_agent_cnt
		INNER JOIN renewal_status_type rst            ON rst.renewal_status_type_id = rs.renewal_status_type_id 
		WHERE i.cover_start_date <= DATEADD(DAY, ISNULL(@DayBeforeRenewalDate, 0), @Renewal_date)
		AND (rs.renewal_status_type_id IN(SELECT renewal_status_type_id FROM renewal_status_type
        		                          WHERE UPPER(code) IN ('UPDATE')))
		 AND (i.Source_Id IN (SELECT s.source_id 
                                      FROM   source s 
                                      WHERE  source_id NOT IN (SELECT source_id 
                                                                FROM  pmuser_source 
                                                                WHERE [user_id] =@UserId )AND is_deleted = 0) or @UserId is null) 
		AND BRJB.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
		AND BRJP.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
		AND BRJA.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
		GROUP BY rs.renewal_exception_reason_id
	END
	ELSE IF @AllAgents = 0 AND @Include_Direct_Policies = 1   --SELECTED AGENT WITH ALL DIRECT BUSINESS 
	BEGIN
		SELECT 
			COUNT(rs.renewal_insurance_file_cnt),rs.renewal_exception_reason_id

		FROM insurance_file i 
		INNER JOIN renewal_status rs                  ON rs.renewal_insurance_file_cnt = i.insurance_file_cnt 
		INNER JOIN Batch_Renewal_Job_Products BRJP    ON BRJP.product_id = rs.product_id 
		INNER JOIN Batch_Renewal_Job_Branches BRJB    ON BRJB.source_id = i.source_id 
		INNER JOIN renewal_status_type rst            ON rst.renewal_status_type_id = rs.renewal_status_type_id 
		WHERE i.cover_start_date <= DATEADD(DAY, ISNULL(@DayBeforeRenewalDate, 0), @Renewal_date)
		AND (rs.renewal_status_type_id IN(SELECT renewal_status_type_id FROM renewal_status_type
        		                          WHERE UPPER(code) IN ('UPDATE')))
		 AND (i.Source_Id IN (SELECT s.source_id 
                                       FROM   source s 
                                       WHERE  source_id NOT IN (SELECT source_id 
                                                                  FROM pmuser_source 
                                                                 WHERE [user_id] =@UserId )AND is_deleted = 0) or @UserId is null) 
		AND BRJB.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
		AND BRJP.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
		AND (i.lead_agent_cnt in (SELECT party_cnt from Batch_Renewal_Job_Agents WHERE 
		                     Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID) OR (i.lead_agent_cnt IS NULL))
 
        GROUP BY rs.renewal_exception_reason_id 
	END
	ELSE --ALL AGENT WITH NO DIRECT BUSINESS
	BEGIN 
		SELECT 
			COUNT(rs.renewal_insurance_file_cnt),rs.renewal_exception_reason_id

		FROM insurance_file i 
		INNER JOIN renewal_status rs                  ON rs.renewal_insurance_file_cnt = i.insurance_file_cnt 
		INNER JOIN Batch_Renewal_Job_Products BRJP    ON BRJP.product_id = rs.product_id 
		INNER JOIN Batch_Renewal_Job_Branches BRJB    ON BRJB.source_id = i.source_id 
		INNER JOIN renewal_status_type rst            ON rst.renewal_status_type_id = rs.renewal_status_type_id 
		WHERE i.cover_start_date <= DATEADD(DAY, ISNULL(@DayBeforeRenewalDate, 0), @Renewal_date)
		AND (rs.renewal_status_type_id IN(SELECT renewal_status_type_id FROM renewal_status_type
        		                          WHERE UPPER(code) IN ('UPDATE')))
		  AND (i.Source_Id IN (SELECT s.source_id 
                                       FROM   source s 
                                       WHERE  source_id NOT IN (SELECT source_id 
                                                                 FROM  pmuser_source 
                                                                 WHERE [user_id] =@UserId )AND is_deleted = 0) or @UserId is null) 
		AND BRJB.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
		AND BRJP.Batch_Renewal_Job_ID = @Batch_Renewal_Job_ID
        AND i.lead_agent_cnt > 0 
	    GROUP BY rs.renewal_exception_reason_id
	END
END
