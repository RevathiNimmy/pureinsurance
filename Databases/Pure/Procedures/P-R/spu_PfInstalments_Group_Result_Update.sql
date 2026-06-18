SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PfInstalments_Group_Result_Update'
GO


CREATE PROCEDURE spu_PfInstalments_Group_Result_Update


@group_id int, 
@pfInstalments_result_id int

AS

BEGIN

	UPDATE pfinstalments 
	SET pfinstalments_result_id = @pfinstalments_result_id 
	WHERE group_id = @group_id

END




GO
