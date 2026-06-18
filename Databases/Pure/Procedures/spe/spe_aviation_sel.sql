SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_aviation_sel'
GO

CREATE PROCEDURE spe_aviation_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    use_of_aircraft,
    hangar_location,
    maintenance,
    area_of_cover,
    scope_of_cover,
    AD_hull_damage_cover,
    is_flight_risk,
    is_ground_risk,
    is_mooring_risk,
    cargo_limit,
    excess_limit,
    number_of_passengers,
    per_passenger_limit,
    third_party_limit,
    mail_excess,
    baggage_excess,
    registration_number
 FROM aviation
WHERE insurance_file_cnt = @insurance_file_cnt

GO

