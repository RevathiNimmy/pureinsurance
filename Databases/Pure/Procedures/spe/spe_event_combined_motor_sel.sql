SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_combined_motor_sel'
GO

CREATE PROCEDURE spe_event_combined_motor_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    is_all_risks,
    ar_sum_insured,
    is_business_interruption,
    bi_sum_insured,
    is_employers_liability,
    el_description,
    el_indemnity_limit,
    is_public_liability,
    pl_description,
    pl_wages_estimate,
    pl_indemnity_limit,
    is_personal_accident,
    pa_insured_person,
    pa_age,
    pa_duties,
    pa_compensation_amount,
    pa_payable_by,
    is_engineering,
    e_air_oil_indemnity_limit,
    e_electrical_indemnity_limit,
    e_crane_indemnity_limit,
    is_road_risks,
    rr_indemnity_limit
FROM event_combined_motor
WHERE insurance_file_cnt = @insurance_file_cnt

GO

