SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_gis_qem_usage_del    Script Date: 13/02/2002 15:49:22 ******/
EXECUTE DDLDropProcedure 'spu_gis_qem_usage_del'
GO

CREATE PROCEDURE spu_gis_qem_usage_del 

@schemeId integer

 AS

Delete gis_qem_usage where gis_scheme_id= @schemeid

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

