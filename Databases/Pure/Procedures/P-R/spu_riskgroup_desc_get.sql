SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_riskgroup_desc_get  Script Date: 13/02/2002 15:55:47 ******/
EXECUTE DDLDropProcedure 'spu_riskgroup_desc_get'
GO

CREATE PROCEDURE spu_riskgroup_desc_get
@AllGroups as integer

AS

IF @AllGroups = 0
BEGIN
    SELECT DISTINCT(r.description), r.risk_group_id 
    FROM risk_group r
    JOIN gis_qem_usage gq ON r.risk_group_id = gq.risk_group_id
    WHERE gis_business_type_id = 4 AND r.is_deleted = 0
    ORDER BY description
END ELSE
BEGIN
    SELECT description, risk_group_id
    FROM risk_group 
    WHERE is_deleted = 0 
    ORDER BY description
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

