SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_vehicles'
GO


CREATE PROCEDURE spu_wp_vehicles
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


DECLARE @make_of_vehicle VARCHAR(70),
    @model_of_vehicle VARCHAR(70),
    @cubic_capacity INT,
    @number_of_seats INT,
    @gross_vehicle_weight INT,
    @gross_thing_weight INT,
    @year_of_make INT,
    @registration_number VARCHAR(20),
    @value NUMERIC(19,4),
    @is_garaged TINYINT,
    @cover VARCHAR(70),
    @excess NUMERIC(19,4)

    SELECT  @make_of_vehicle = make_of_vehicle,
        @model_of_vehicle = model_of_vehicle,
        @cubic_capacity = cubic_capacity,
        @number_of_seats = number_of_seats,
        @gross_vehicle_weight = gross_vehicle_weight,
        @gross_thing_weight = gross_thing_weight,
        @year_of_make = year_of_make,
        @registration_number = registration_number,
        @value = value,
        @is_garaged = is_garaged,
        @cover = cover,
        @excess = excess
    FROM    vehicles
    WHERE   insurance_file_cnt = @InsuranceFileCnt
    AND vehicle_number = @Instance1

    SELECT  'make_of_vehicle' = @make_of_vehicle,
        'model_of_vehicle' = @model_of_vehicle,
        'cubic_capacity' = @cubic_capacity,

        'number_of_seats' = @number_of_seats,
        'gross_vehicle_weight' = @gross_vehicle_weight,
        'gross_thing_weight' = @gross_thing_weight,
        'year_of_make' = @year_of_make,
        'registration_number' = @registration_number,
        'value' = @value,
        'is_garaged' = @is_garaged,
        'cover' = @cover,
        'excess' = @excess
GO


