EXECUTE DDLDropProcedure 'spu_PFSchemeSource_Sel'
GO

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PFSchemeSource_Sel
	@CompanyNo INT,
	@SchemeNo INT,
	@SchemeVersion INT
	
AS
	SELECT source_id
	FROM PFSchemeSource AS ss
	WHERE ss.CompanyNo = @CompanyNo 
		AND ss.SchemeNo = @SchemeNo 
		AND ss.SchemeVersion = @SchemeVersion
	ORDER BY source_id 

GO

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
