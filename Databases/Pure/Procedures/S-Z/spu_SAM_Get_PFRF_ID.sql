
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_PFRF_ID'
GO

CREATE PROCEDURE spu_SAM_Get_PFRF_ID

@CompanyNo int,
@SchemeNo int,
@SchemeVersion int

AS

BEGIN

SELECT TOP 1 pfrf_id 
FROM pfrf 
WHERE pfrf.CompanyNo = @CompanyNo
	AND pfrf.SchemeNo = @SchemeNo
	AND pfrf.SchemeVersion = @SchemeVersion
ORDER BY pfrf.pfrf_id

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

