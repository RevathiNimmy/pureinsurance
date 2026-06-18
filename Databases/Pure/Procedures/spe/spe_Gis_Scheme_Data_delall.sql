SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Gis_Scheme_Data_delall'
GO

CREATE PROCEDURE spe_Gis_Scheme_Data_delall
    @code char(10)
AS

DELETE FROM gis_scheme_data
WHERE gis_scheme_id in
    (SELECT s.gis_scheme_id
    FROM gis_scheme s, gis_business_type b
    WHERE b.gis_business_type_id = s.gis_business_type_id
    AND b.code = @code)

GO

