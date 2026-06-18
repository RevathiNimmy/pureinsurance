SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_household_buildings_sel'
GO

CREATE PROCEDURE spe_household_buildings_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    address_cnt,
    sum_insured,
    rebuild_cost,
    cover_type,
    number_of_bedrooms,
    type_of_property,
    is_subsidence,
    excess
 FROM household_buildings
WHERE insurance_file_cnt = @insurance_file_cnt

GO

