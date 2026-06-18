SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_business_type_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_business_type_sel
AS


BEGIN

    SELECT RG.risk_group_id,
    RG.description
    FROM Risk_Group RG

UNION

    SELECT GBT.gis_business_type_id,
    '[GII] ' + GBT.description
    FROM GIS_Business_Type GBT

    ORDER BY description

END

GO


