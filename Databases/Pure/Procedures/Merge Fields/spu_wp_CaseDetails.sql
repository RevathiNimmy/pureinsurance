SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_CaseDetails'
GO


CREATE PROCEDURE spu_wp_CaseDetails  
@PartyCnt INT,  
@InsuranceFileCnt INT,  
@RiskId INT,  
@ClaimCnt INT,  
@DocumentRef VARCHAR(25),  
@Instance1 INT = NULL,  
@Instance2 INT = NULL,  
@Instance3 INT = NULL  

AS  
  
SELECT 
    case_number = C.case_number,
    case_opened_date = C.case_opened_date,
    case_version = C.case_version,
    case_progress = (SELECT description FROM case_progress WHERE case_progress_id=C.case_progress_id),
    case_analyst = (SELECT description FROM handler WHERE handler_id = C.analyst_handler_id),
    case_administrator = (SELECT description FROM handler WHERE handler_id = C.admin_handler_id),
    total_indemnity=    ISNULL((SELECT SUM((R.initial_reserve + R.revised_reserve) - paid_to_date ) FROM claim_peril CP  
           		JOIN reserve R  
           	   	ON CP.claim_peril_id = R.claim_peril_id  
			JOIN reserve_type RT  
			    ON R.reserve_type_id = RT.reserve_type_id  
			AND RT.is_indemnity=1   
		   	WHERE CP.claim_id=CL.claim_id),0),  
         
    total_expense =  	ISNULL((SELECT   
			SUM((R.initial_reserve + R.revised_reserve) - paid_to_date )   
			FROM claim_peril CP  
			JOIN reserve R  
			   ON CP.claim_peril_id = R.claim_peril_id  
			JOIN reserve_type RT  
			   ON R.reserve_type_id = RT.reserve_type_id  
			   AND RT.is_expense=1   
			WHERE CP.claim_id=CL.claim_id),0),  
         
    total_excess=   	ISNULL((SELECT   
			SUM((R.initial_reserve + R.revised_reserve) - paid_to_date )   
			FROM claim_peril CP  
			JOIN reserve R  
			   ON CP.claim_peril_id = R.claim_peril_id  
			JOIN reserve_type RT  
			   ON R.reserve_type_id = RT.reserve_type_id  
			   AND RT.is_excess=1   
		    	WHERE CP.claim_id=CL.claim_id),0) 
    
FROM [case] C  
INNER JOIN 
    (SELECT MAX(Case_Version) as version_id,MAX(Case_Id) as case_id, base_case_id 
     FROM [case] GROUP BY base_case_id ) case_version  
ON c.case_id = case_version.case_id  
JOIN Claim CL
ON case_version.base_case_id=CL.base_case_id
And claim_id = @ClaimCnt

GO
