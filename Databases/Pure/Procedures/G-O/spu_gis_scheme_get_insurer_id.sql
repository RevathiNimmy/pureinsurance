SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_gis_scheme_get_insurer_id    Script Date: 13/02/2002 15:48:14 ******/
EXECUTE DDLDropProcedure 'spu_gis_scheme_get_insurer_id'
GO

CREATE PROCEDURE spu_gis_scheme_get_insurer_id 

@insurer varchar(50)

AS

select gis_insurer_id from gis_insurer where description=@insurer

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

