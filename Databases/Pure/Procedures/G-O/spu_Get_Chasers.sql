
/* AK - 100402
    Stored procedure to return all the chaser descriptions
*/

EXECUTE DDLDropProcedure 'spu_get_chasers'
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spu_get_chasers
 AS
     	SELECT t.description from PMWrk_Task_Instance_Temp t, PMWrk_Task w
     	WHERE t.pmwrk_task_id = w.pmwrk_task_id 
     	AND   w.code = 'REMINDER'

GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

