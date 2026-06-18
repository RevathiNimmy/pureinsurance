SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_scheme_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_scheme_sel
    @risk_group_description varchar(255),
    @business_type_id int

AS


BEGIN
    -- [GII] prefix indicates GIS_Business_Type
    IF (LEFT(@risk_group_description, 5) = '[GII]')
    BEGIN
        SELECT GS.gis_scheme_id,
               GS.scheme_desc
        FROM GIS_Scheme GS
        WHERE GS.gis_business_type_id = @business_type_id
        AND ISNULL(GS.scheme_desc, '') <> ''
        ORDER BY GS.scheme_desc
    END
    ELSE
    BEGIN 
        -- Non [GII] prefix indicates Risk_Group
        SELECT DISTINCT GS.gis_scheme_id,
               GS.scheme_desc
        FROM       GIS_Scheme GS
        INNER JOIN GIS_QEM_Usage GQ
        ON         GQ.gis_scheme_id = GS.gis_scheme_id 
	INNER JOIN Risk_Group RG
	ON	   RG.risk_group_id = GQ.risk_group_id
        AND RG.description = @risk_group_description
        AND   ISNULL(GS.scheme_desc, '') <> ''
        ORDER BY GS.scheme_desc
	END
END

GO


