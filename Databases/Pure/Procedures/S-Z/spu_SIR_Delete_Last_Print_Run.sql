SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Delete_Last_Print_Run'
GO

CREATE PROCEDURE spu_SIR_Delete_Last_Print_Run
	@renewal_insurance_file_cnt int
AS

BEGIN

	DELETE FROM last_print_run 
	WHERE renewal_status_cnt in (
		SELECT renewal_status_cnt
		FROM renewal_status
		WHERE renewal_insurance_file_cnt = @renewal_insurance_file_cnt )


END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
