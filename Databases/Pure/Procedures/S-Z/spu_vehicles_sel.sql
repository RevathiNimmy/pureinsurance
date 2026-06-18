SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_vehicles_sel'
GO


CREATE PROCEDURE spu_vehicles_sel
    @insurance_file_cnt int
AS


SELECT
    insurance_file_cnt,
    vehicle_number,
    make_of_vehicle,
    model_of_vehicle,
    cubic_capacity,
    number_of_seats,
    gross_vehicle_weight,
    gross_thing_weight,
    year_of_make,
    registration_number,
    value,
    is_garaged,
    cover,
    excess
 FROM vehicles
WHERE insurance_file_cnt = @insurance_file_cnt
GO


