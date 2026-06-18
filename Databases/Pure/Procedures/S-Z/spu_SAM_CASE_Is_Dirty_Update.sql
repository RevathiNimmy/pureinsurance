SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_CASE_Is_Dirty_Update'
GO

CREATE PROCEDURE spu_SAM_CASE_Is_Dirty_Update  
  
@case_id int,  
@is_dirty int  
  
AS  
  
BEGIN  
 UPDATE [Case]
 SET is_dirty_case = @is_dirty  
 WHERE case_id = @case_id  
  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

