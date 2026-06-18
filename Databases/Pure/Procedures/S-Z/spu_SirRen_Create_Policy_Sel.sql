SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Create_Policy_Sel'
GO


CREATE PROCEDURE spu_SirRen_Create_Policy_Sel
    @effective_date datetime,
    @insurance_folder_cnt int
AS


BEGIN

        /* Insurance folder passed in */
        SELECT rc.insurance_folder_cnt,
                  rc.gis_scheme_id,
                  rc.party_cnt,
         gb.gis_business_type_id,
         g.code,
         gbt.code as business_type_code,
         rc.renewal_insurance_file_cnt
        FROM renewal_control rc
        INNER JOIN gis_data_model g
            ON g.gis_data_model_id = rc.gis_data_model_id
        INNER JOIN gis_data_model_business gb
            ON g.gis_data_model_id = gb.gis_data_model_id
        INNER JOIN gis_business_type gbt
    ON gbt.gis_business_type_id = gb.gis_business_type_id
        WHERE rc.insurance_folder_cnt = @insurance_folder_cnt
        AND ISNULL(rc.suspension_level, 0) = 0

END
GO


