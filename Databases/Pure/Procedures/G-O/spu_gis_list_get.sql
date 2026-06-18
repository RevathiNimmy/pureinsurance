SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_gis_list_get  Script Date: 13/02/2002 15:55:47 ******/
EXECUTE DDLDropProcedure 'spu_gis_list_get'
GO

CREATE PROCEDURE spu_gis_list_get

AS

Select gis_QEM_ID,Description from gis_qem
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

