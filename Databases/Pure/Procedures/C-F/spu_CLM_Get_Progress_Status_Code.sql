SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Progress_Status_Code'
GO

CREATE PROCEDURE spu_CLM_Get_Progress_Status_Code 

@Progress_status_id int

AS

BEGIN

	SELECT CODE 
	FROM Progress_status
	WHERE Progress_status_id =@Progress_status_id

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
