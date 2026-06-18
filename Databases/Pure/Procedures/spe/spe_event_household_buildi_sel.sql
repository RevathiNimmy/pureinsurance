SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_household_buildi_sel'
GO

CREATE PROCEDURE spe_event_household_buildi_sel
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
FROM event_household_buildings
WHERE insurance_file_cnt = @insurance_file_cnt

GO

