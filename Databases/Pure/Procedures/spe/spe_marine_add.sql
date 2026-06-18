SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_marine_add'
GO

CREATE PROCEDURE spe_marine_add
    @insurance_file_cnt int,
    @vessel_name varchar(60),
    @model varchar(70),
    @category varchar(70),
    @engine_type varchar(70),
    @fuel_type varchar(70),
    @engine_bhp int,
    @length int,
    @speed int,
    @hull_material varchar(70),
    @year_built int,
    @when_purchased datetime,
    @cost numeric(19,4),
    @current_value numeric(19,4),
    @usage varchar(70),
    @cruising_range int,
    @is_moored tinyint,
    @where_moored varchar(70),
    @months_in_commission int,
    @laid_up_from datetime,
    @laid_up_to datetime,
    @is_ashore tinyint
AS
BEGIN
INSERT INTO marine (
    insurance_file_cnt ,
    vessel_name ,
    model ,
    category ,
    engine_type ,
    fuel_type ,
    engine_bhp ,
    length ,
    speed ,
    hull_material ,
    year_built ,
    when_purchased ,
    cost ,
    current_value ,
    usage ,
    cruising_range ,
    is_moored ,
    where_moored ,
    months_in_commission ,
    laid_up_from ,
    laid_up_to ,
    is_ashore )
VALUES (
    @insurance_file_cnt,
    @vessel_name,
    @model,
    @category,
    @engine_type,
    @fuel_type,
    @engine_bhp,
    @length,
    @speed,
    @hull_material,
    @year_built,
    @when_purchased,
    @cost,
    @current_value,
    @usage,
    @cruising_range,
    @is_moored,
    @where_moored,
    @months_in_commission,
    @laid_up_from,
    @laid_up_to,
    @is_ashore)
END

GO

