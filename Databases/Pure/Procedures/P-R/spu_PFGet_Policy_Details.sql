SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_PFGet_Policy_Details'
GO

CREATE PROCEDURE spu_PFGet_Policy_Details

                @insurance_file_cnt int

AS

BEGIN
    SELECT  G.gis_policy_link_id, 
            D.code data_model_code, 
            S.gis_business_type_id, 
            T.code gis_business_type_code, 
            G.risk_id,
            S.gis_scheme_id
    FROM Gis_Policy_Link G
    INNER JOIN Insurance_File I
    ON G.insurance_file_cnt = I.insurance_file_cnt
    AND I.insurance_file_cnt = @insurance_file_cnt
    INNER JOIN Gis_Data_Model D
    ON D.gis_data_model_id = G.gis_data_model_id
    LEFT OUTER JOIN Gis_Scheme S
    ON S.gis_scheme_id = G.gis_scheme_id
    LEFT OUTER JOIN Gis_Business_Type T
    ON T.gis_business_type_id = S.gis_business_type_id
END
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO

