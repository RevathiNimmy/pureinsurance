SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_aviation_upd'
GO

CREATE PROCEDURE spe_aviation_upd
    @insurance_file_cnt int,
    @use_of_aircraft varchar(70),
    @hangar_location varchar(70),
    @maintenance varchar(70),
    @area_of_cover varchar(70),
    @scope_of_cover varchar(70),
    @AD_hull_damage_cover varchar(70),
    @is_flight_risk tinyint,
    @is_ground_risk tinyint,
    @is_mooring_risk tinyint,
    @cargo_limit numeric(19,4),
    @excess_limit numeric(19,4),
    @number_of_passengers int,
    @per_passenger_limit numeric(19,4),
    @third_party_limit numeric(19,4),
    @mail_excess numeric(19,4),
    @baggage_excess numeric(19,4),
    @registration_number varchar(20)
AS
BEGIN
UPDATE aviation
    SET
    use_of_aircraft=@use_of_aircraft,
    hangar_location=@hangar_location,
    maintenance=@maintenance,
    area_of_cover=@area_of_cover,
    scope_of_cover=@scope_of_cover,
    AD_hull_damage_cover=@AD_hull_damage_cover,
    is_flight_risk=@is_flight_risk,
    is_ground_risk=@is_ground_risk,
    is_mooring_risk=@is_mooring_risk,
    cargo_limit=@cargo_limit,
    excess_limit=@excess_limit,
    number_of_passengers=@number_of_passengers,
    per_passenger_limit=@per_passenger_limit,
    third_party_limit=@third_party_limit,
    mail_excess=@mail_excess,
    baggage_excess=@baggage_excess,
    registration_number=@registration_number
WHERE insurance_file_cnt = @insurance_file_cnt
END

GO

