EXECUTE DDLDropProcedure 'spu_PFSchemeProducts_Sel'
GO

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PFSchemeProducts_Sel
	@CompanyNo INT,
	@SchemeNo INT,
	@SchemeVersion INT
	
AS
	SELECT product_id
	FROM PFSchemeProducts AS ss
	WHERE ss.CompanyNo = @CompanyNo 
		AND ss.SchemeNo = @SchemeNo 
		AND ss.SchemeVersion = @SchemeVersion
	ORDER BY product_id 
GO

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
