SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_GetAllUserBranches'
GO

CREATE PROCEDURE spu_SIR_GetAllUserBranches
	@user_id int

AS

BEGIN
	SELECT source_id, code, description
	FROM source 
	WHERE source_id not in 
		(SELECT source_id 
		FROM pmuser_source 
		WHERE user_id = @user_id)
	ORDER BY description

END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
