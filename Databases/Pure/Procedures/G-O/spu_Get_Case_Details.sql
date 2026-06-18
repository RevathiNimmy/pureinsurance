SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Get_Case_Details'
GO


CREATE PROCEDURE spu_Get_Case_Details
	@case_id INT
AS

  
SELECT   
    C.case_number,
    C.case_opened_date,
    C.case_version,
    (SELECT description FROM case_progress WHERE case_progress_id=C.case_progress_id) 'case_progress',
    C.analyst_handler_id 'case_analyst',
    C.admin_handler_id 'case_administrator',
    
    ISNULL((SELECT SUM((R.initial_reserve + R.revised_reserve) - paid_to_date ) FROM claim_peril CP  
       JOIN reserve R  
       	   ON CP.claim_peril_id = R.claim_peril_id  
       JOIN reserve_type RT  
   	   ON R.reserve_type_id = RT.reserve_type_id  
   	   AND RT.is_indemnity=1   
       WHERE CP.claim_id=CL.claim_id),0) 'total_indemnity',  
     
    ISNULL((SELECT   
    SUM((R.initial_reserve + R.revised_reserve) - paid_to_date )   
    FROM claim_peril CP  
    JOIN reserve R  
       ON CP.claim_peril_id = R.claim_peril_id  
    JOIN reserve_type RT  
       ON R.reserve_type_id = RT.reserve_type_id  
       AND RT.is_expense=1   
    WHERE CP.claim_id=CL.claim_id),0) 'total_expense',  
     
    ISNULL((SELECT   
    SUM((R.initial_reserve + R.revised_reserve) - paid_to_date )   
    FROM claim_peril CP  
    JOIN reserve R  
       ON CP.claim_peril_id = R.claim_peril_id  
    JOIN reserve_type RT  
       ON R.reserve_type_id = RT.reserve_type_id  
       AND RT.is_excess=1   
    WHERE CP.claim_id=CL.claim_id),0) 'total_excess'
  
FROM [case] C  
JOIN Claim CL
    ON CL.Base_Case_Id = C.Base_Case_id
WHERE C.case_id = @case_id
 
GO


