SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_travel_sel'
GO

CREATE PROCEDURE spe_event_travel_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    scheme_type,
    trip_option,
    area,
    is_baggage_and_money,
    name_of_insured,
    age_of_insured,
    is_premium_person,
    is_golf_cover,
    date_of_departure,
    travel_period,
    travel_period_qualifier
FROM event_travel
WHERE insurance_file_cnt = @insurance_file_cnt

GO

