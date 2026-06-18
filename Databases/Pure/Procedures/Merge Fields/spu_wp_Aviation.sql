SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_Aviation'
GO


CREATE PROCEDURE spu_wp_Aviation
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @use_of_aircraft VARCHAR(255),
    @hangar_location VARCHAR(255),
    @maintenance VARCHAR(255),
    @area_of_cover VARCHAR(255),
    @scope_of_cover VARCHAR(255),
    @AD_hull_damage_cover VARCHAR(255),
    @is_flight_risk TINYINT OUTPUT,
    @is_ground_risk TINYINT OUTPUT,
    @is_mooring_risk TINYINT OUTPUT,
    @cargo_limit NUMERIC,
    @excess_limit NUMERIC,
    @number_of_passengers INT OUTPUT,
    @per_passenger_limit NUMERIC,
    @third_party_limit NUMERIC,
    @mail_excess NUMERIC,
    @baggage_excess NUMERIC,
    @registration_number VARCHAR(255)
AS


SELECT
    @use_of_aircraft = Aviation.use_of_aircraft,
    @hangar_location = Aviation.hangar_location,
    @maintenance = Aviation.maintenance,
    @area_of_cover = Aviation.area_of_cover,
    @scope_of_cover = Aviation.scope_of_cover,
    @AD_hull_damage_cover = Aviation.AD_hull_damage_cover,
    @is_flight_risk = Aviation.is_flight_risk,
    @is_ground_risk = Aviation.is_ground_risk,
    @is_mooring_risk = Aviation.is_mooring_risk,
    @cargo_limit = Aviation.cargo_limit,
    @excess_limit = Aviation.excess_limit,
    @number_of_passengers = Aviation.number_of_passengers,
    @per_passenger_limit = Aviation.per_passenger_limit,
    @third_party_limit = Aviation.third_party_limit,
    @mail_excess = Aviation.mail_excess,
    @baggage_excess = Aviation.baggage_excess,
    @registration_number = Aviation.registration_number
FROM Aviation
WHERE Aviation.insurance_file_cnt = @insurancefilecnt
GO


