SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_CLM_Get_GIS_Policy_Link_Details'
GO

CREATE  PROCEDURE spu_CLM_Get_GIS_Policy_Link_Details
    @claim_id int
AS
SELECT
    gpl.gis_policy_link_id,
    gpl.risk_id,
    gpl.quote_ref_password

FROM
    GIS_Policy_Link AS gpl WITH (NOLOCK)
WHERE
    gpl.claim_id = @claim_id
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

