SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_clm_is_progress_status_closed'
GO

CREATE PROCEDURE
spu_clm_is_progress_status_closed
(
@StatusId int
)
AS

SELECT
is_closed_check_status
FROM
progress_status
WHERE
progress_status_id = @StatusId

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

