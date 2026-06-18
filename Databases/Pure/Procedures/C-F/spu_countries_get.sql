SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_countries_get  Script Date: 13/02/2002 15:55:47 ******/
EXECUTE DDLDropProcedure 'spu_countries_get'
GO

CREATE PROCEDURE spu_countries_get

AS

Select country_id, Description from Country order by description
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

