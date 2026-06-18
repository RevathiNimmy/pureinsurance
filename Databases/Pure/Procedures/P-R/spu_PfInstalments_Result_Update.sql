SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PfInstalments_Result_Update'
GO

CREATE PROCEDURE spu_PfInstalments_Result_Update

@pfinstalments_id int, 
@pfinstalments_result_id int

AS

BEGIN

	UPDATE pfinstalments
	SET pfinstalments_result_id = @pfinstalments_result_id,
	    batch_id = null,
     	    group_id=null,
     	    write_off_Reason_id=null,
     	    write_off_transdetail_id=null
	WHERE pfinstalments_id = @pfinstalments_id

END


GO
