SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Update_Case_Progress_Status'
GO

CREATE PROCEDURE spu_Update_Case_Progress_Status  
    @case_id INT
    
As
  
UPDATE [case]  
SET case_progress_id = (SELECT case_progress_id FROM case_progress WHERE code='CLOSED' )
WHERE  
case_id = @case_id 

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

