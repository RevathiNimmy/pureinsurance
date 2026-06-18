SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_get_duplicate_claim_override_users'
GO


CREATE PROCEDURE spu_clm_get_duplicate_claim_override_users

AS

BEGIN

	SELECT
		ua.User_Id,
		pmu.secure_password,
		pmu.Username

	FROM 
		user_authorities ua
	
		INNER JOIN pmuser pmu ON
			ua.user_id = pmu.user_id

	WHERE
		ua.can_override_duplicate_claims =1

END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
