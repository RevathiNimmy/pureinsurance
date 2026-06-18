SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_SAM_clm_is_progress_status_closed'
GO

CREATE PROCEDURE spu_SAM_clm_is_progress_status_closed 
@is_closed_check_status int output, 
@ProgressStatusCode varchar(30) 
AS 
BEGIN 
SELECT @is_closed_check_status = is_closed_check_status 
FROM progress_status 
WHERE is_deleted=0 
and code = @ProgressStatusCode 
END  

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

