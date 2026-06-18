SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Batch_Renewal_Audit_Details'
GO

CREATE PROCEDURE spu_Report_Batch_Renewal_Audit_Details         
	@start_date				DATETIME,  
	@end_date		   		DATETIME,  
	@branch_id				INT,  
	@AgentShortName		    VARCHAR(40),
	@include_direct_client	VARCHAR(5),
	@policy_number			VARCHAR(100),
	@run_status				VARCHAR(10), --ALL, FAILED, SUCCEEDED  
	@job_type				VARCHAR(10)  --SELECTION,INVITATION,ACCEPTANCE : 1,2,3
AS  	
	
    DECLARE @sSQL1 VARCHAR(4000)
    DECLARE @sSQL2 VARCHAR(255)
    DECLARE @sSQL3 VARCHAR(255)
    DECLARE @sSQL4 VARCHAR(255)
    DECLARE @sSQL5 VARCHAR(255)
	DECLARE @sSQL6 VARCHAR(255)
	DECLARE @sSQL7 VARCHAR(255)

	IF @branch_id = 0 BEGIN SET @branch_id = NULL END
	IF @AgentShortName IS NULL BEGIN SET @AgentShortName = '' END
	IF @AgentShortName = 'ALL' BEGIN SET @AgentShortName ='' END
	IF @policy_number = 'ALL' BEGIN SET @policy_number ='' END
	
	IF  @AgentShortName <> '' BEGIN    	
		SET @sSQL2 = ' AND P.Shortname = ''' + @AgentShortName + '''' + CHAR(13)
    END 

    IF RTRIM(UPPER(@include_direct_client)) = 'NO' BEGIN
    	SET @sSQL3 = ' AND IFI.lead_agent_cnt IS NOT NULL '
    END

    IF RTRIM(UPPER(@run_status)) = 'FAILED' BEGIN
	 	SET @sSQL4 = ' AND BRJR.is_failed = 1 ' + CHAR(13)  END	
	ELSE IF RTRIM(UPPER(@run_status)) = 'SUCCEEDED' BEGIN	
	   	SET @sSQL4 = ' AND BRJR.is_failed = 0 ' + CHAR(13)  	
	END    
	
 	IF RTRIM(UPPER(@job_type)) = 'SELECTION' BEGIN	    
		SET @sSQL5 = ' AND BRJT.batch_renewal_job_type_id = 1 ' + CHAR(13) END
	ELSE IF RTRIM(UPPER(@job_type)) = 'INVITATION' BEGIN	
		SET @sSQL5 = ' AND BRJT.batch_renewal_job_type_id = 2 ' + CHAR(13) END
	ELSE IF RTRIM(UPPER(@job_type)) = 'ACCEPTANCE' BEGIN	
		SET @sSQL5 = ' AND BRJT.batch_renewal_job_type_id = 3 ' + CHAR(13)
    END

 	IF @branch_id > 0 BEGIN	    
		SET @sSQL6 = ' AND S.source_id = ' + CONVERT(VARCHAR,@branch_id ) + CHAR(13)
    END
	
	IF @policy_number <> '' BEGIN
	    SET @sSQL7 = ' AND IFI.insurance_ref = ''' + @policy_number + '''' + CHAR(13)
	END

    SET @sSQL1 = 'SELECT BRJT.Description, BRJ.Code, BRJR.run_date, IFI.insurance_ref, P.shortname AS Agent , ' + CHAR(13) +  
          		 ' CASE BRJR.is_failed ' + 
                 ' WHEN 0 THEN ' + '''Succeeded''' +                              
                 ' ELSE ' + '''Failed''' + ' END AS Run_Status,' +
		         ' BRJR.failure_reason ' +
                 ' FROM Batch_Renewal_Job_Runs BRJR ' + CHAR(13) +  
              	 ' LEFT JOIN Insurance_file IFI ON BRJR.insurance_file_cnt = IFI.insurance_file_cnt ' + CHAR(13) +				 
              	 ' LEFT JOIN source S ON IFI.source_id = s.source_id ' + CHAR(13) +  
               	 ' LEFT JOIN Party P ON p.party_cnt = ISNULL(IFI.lead_agent_cnt, IFI.insured_cnt) ' + CHAR(13) +                   
               	 ' LEFT JOIN Batch_Renewal_Job BRJ ON BRJ.batch_renewal_job_id = BRJR.batch_renewal_job_id ' + CHAR(13) +  
               	 ' LEFT JOIN Batch_Renewal_Job_Type BRJT ON BRJT.batch_renewal_job_type_id = BRJ.batch_renewal_job_type_id ' + CHAR(13) +				 
               	 ' WHERE CONVERT(VARCHAR,BRJR.run_date,112) >= ''' + CONVERT(VARCHAR,@start_date,112) + ''' AND CONVERT(VARCHAR,BRJR.run_date,112) <= ''' + CONVERT(VARCHAR,@end_date,112) + '''' + CHAR(13)                  

	EXEC(@sSQL1 + @sSQL2 + @sSQL3 + @sSQL4 + @sSQL5 + @sSQL6 + @sSQL7)

GO