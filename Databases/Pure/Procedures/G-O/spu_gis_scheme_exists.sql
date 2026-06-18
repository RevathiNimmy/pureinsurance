SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_gis_scheme_exists    Script Date: 13/02/2002 15:45:50 ******/
EXECUTE DDLDropProcedure 'spu_gis_scheme_exists'
GO

CREATE PROCEDURE spu_gis_scheme_exists

@schemename varchar(50),
@SchemeNo int,
@Version int

AS

Select * from gis_scheme where 
scheme_no=@schemeno
and scheme_ver=@version
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

