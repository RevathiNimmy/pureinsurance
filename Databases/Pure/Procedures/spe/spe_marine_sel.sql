SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_marine_sel'
GO

CREATE PROCEDURE spe_marine_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    vessel_name,
    model,
    category,
    engine_type,
    fuel_type,
    engine_bhp,
    length,
    speed,
    hull_material,
    year_built,
    when_purchased,
    cost,
    current_value,
    usage,
    cruising_range,
    is_moored,
    where_moored,
    months_in_commission,
    laid_up_from,
    laid_up_to,
    is_ashore
 FROM marine
WHERE insurance_file_cnt = @insurance_file_cnt

GO

