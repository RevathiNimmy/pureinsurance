SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_gis_scheme_get_risk_group_id    Script Date: 13/02/2002 15:49:22 ******/
EXECUTE DDLDropProcedure 'spu_gis_scheme_get_risk_group_id'
GO

CREATE PROCEDURE spu_gis_scheme_get_risk_group_id 

@RiskGroup varchar(50)

 AS

Select Risk_group_id from risk_group where description=@RiskGroup
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

