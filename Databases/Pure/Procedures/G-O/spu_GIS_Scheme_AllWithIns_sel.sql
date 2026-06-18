SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_GIS_Scheme_AllWithIns_sel
GO

CREATE PROCEDURE spu_GIS_Scheme_AllWithIns_sel
AS 
BEGIN

    SELECT  s.gis_scheme_id,
            s.gis_business_type_id,
            s.gis_insurer_id,
            s.scheme_no,
            s.scheme_ver,
            s.scheme_desc,
            s.agency_code,
            s.activation_level,
            i.description,
            'No of Questions' = COUNT(sp.property_name),
            s.country_id --JRD 08/12/2003 PN8722 Retrieve CountryID to allow filtering on UK/NI schemes for GII
    FROM    GIS_Scheme s
        INNER JOIN GIS_Insurer i
            ON s.gis_insurer_id = i.gis_insurer_id 
        INNER JOIN gis_business_type b 
            ON b.gis_business_type_id = s.gis_business_type_id 
        LEFT OUTER JOIN GIS_Scheme_Property sp
            ON s.gis_scheme_id = sp.gis_scheme_id  
            AND required_pre = 2
    --JRD 16/08/2004 PN13063/PN13365 Amended WHERE condition to match sp_ version  
    WHERE   i.is_deleted = 0  
        AND s.scheme_status > 0
    GROUP BY i.description,
            s.scheme_desc,  
            s.gis_scheme_id,  
            s.gis_business_type_id,  
            s.gis_insurer_id,  
            s.scheme_no,  
            s.scheme_ver,  
            s.agency_code,  
            s.activation_level,  
            s.country_id

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO