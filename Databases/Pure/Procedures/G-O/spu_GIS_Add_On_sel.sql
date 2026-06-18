SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Add_On_sel'
GO


CREATE PROCEDURE spu_GIS_Add_On_sel
    @gis_data_model_code char(10),
    @gis_business_type_code char(10),
    @gis_add_on_code char(10),
    @gis_add_on_cover_level_code char(10),
    @effective_date smalldatetime
AS


DECLARE @gis_data_model_id INTEGER
    DECLARE @gis_business_type_id INTEGER

    -- Get the data model id from code
    SELECT 
        @gis_data_model_id = (SELECT gis_data_model_id
    FROM 
        gis_data_model
    WHERE 
        code = @gis_data_model_code)
    
    -- Get the business type id from code
    SELECT 
        @gis_business_type_id = (SELECT gis_business_type_id
    FROM 
        gis_business_type
    WHERE 
        code = @gis_business_type_code)

    -- Get the rate information
    SELECT 
        gaor.new_business_fee,
        gaor.new_business_rate,
        gaor.new_business_ipt_rate,
        gaor.new_business_vat_rate,
        gaor.commission_amt,
        gaor.commission_pct,
        gao.party_cnt
    FROM 
        gis_add_on_rate gaor
    INNER JOIN
        gis_add_on_cover_level gaocl
        ON  gaor.gis_add_on_cover_level_id = gaocl.gis_add_on_cover_level_id
        AND gaor.gis_add_on_id = gaocl.gis_add_on_id
    INNER JOIN
        gis_add_on gao
        ON  gaocl.gis_add_on_id = gao.gis_add_on_id
    WHERE 
        gao.code = @gis_add_on_code
    AND gaocl.code = @gis_add_on_cover_level_code
    AND gaor.gis_data_model_id = @gis_data_model_id
    AND gaor.gis_business_type_id = @gis_business_type_id
    AND @effective_date BETWEEN gaor.effective_date And IsNull(gaor.expiry_date, '3000.01.01')
    AND gao.is_deleted = 0
GO


