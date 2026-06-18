EXECUTE DDLDropProcedure spu_MTA_Get_Risk_Details
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_MTA_Get_Risk_Details

--get details for launching risk screen

@RiskCodeID int

AS

SELECT g.risk_group_id,g.gis_screen_id FROM risk_code c
INNER JOIN risk_group g
ON c.risk_group_id=g.risk_group_id
WHERE c.risk_code_id=@riskcodeID

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
