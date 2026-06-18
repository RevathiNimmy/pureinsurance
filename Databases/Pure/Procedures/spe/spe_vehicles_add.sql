SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_vehicles_add'
GO

CREATE PROCEDURE spe_vehicles_add
    @insurance_file_cnt int,
    @vehicle_number int,
    @make_of_vehicle varchar(70),
    @model_of_vehicle varchar(70),
    @cubic_capacity int,
    @number_of_seats int,
    @gross_vehicle_weight int,
    @gross_thing_weight int,
    @year_of_make int,
    @registration_number varchar(20),
    @value numeric(19,4),
    @is_garaged tinyint,
    @cover varchar(70),
    @excess numeric(19,4)
AS
BEGIN
INSERT INTO vehicles (
    insurance_file_cnt ,
    vehicle_number ,
    make_of_vehicle ,
    model_of_vehicle ,
    cubic_capacity ,
    number_of_seats ,
    gross_vehicle_weight ,
    gross_thing_weight ,
    year_of_make ,
    registration_number ,
    value ,
    is_garaged ,
    cover ,
    excess )
VALUES (
    @insurance_file_cnt,
    @vehicle_number,
    @make_of_vehicle,
    @model_of_vehicle,
    @cubic_capacity,
    @number_of_seats,
    @gross_vehicle_weight,
    @gross_thing_weight,
    @year_of_make,
    @registration_number,
    @value,
    @is_garaged,
    @cover,
    @excess)
END

GO

