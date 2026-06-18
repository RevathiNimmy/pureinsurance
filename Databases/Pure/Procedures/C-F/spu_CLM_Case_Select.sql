SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Case_Select'
GO

CREATE PROCEDURE spu_CLM_Case_Select   
     @Case_ID Integer  
AS  
    SELECT   
        case_id,       
        case_number,   
        case_opened_date,   
        case_version,   
        case_progress_id,   
        analyst_handler_id,   
        admin_handler_id,   
        base_case_id,   
        user_id   
    FROM [Case]   
    WHERE case_id=@Case_id  
    


  
