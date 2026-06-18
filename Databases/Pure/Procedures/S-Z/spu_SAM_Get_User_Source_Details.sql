SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_User_Source_Details'
GO


CREATE PROCEDURE spu_SAM_Get_User_Source_Details

AS


SELECT 
	code, 
	source_id, 
	base_currency_id
FROM source



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
