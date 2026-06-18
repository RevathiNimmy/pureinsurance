SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_combined_motor_add'
GO

CREATE PROCEDURE spe_combined_motor_add
    @insurance_file_cnt int,
    @is_all_risks tinyint,
    @ar_sum_insured numeric(19,4),
    @is_business_interruption tinyint,
    @bi_sum_insured numeric(19,4),
    @is_employers_liability tinyint,
    @el_description varchar(60),
    @el_indemnity_limit numeric(19,4),
    @is_public_liability tinyint,
    @pl_description varchar(60),
    @pl_wages_estimate numeric(19,4),
    @pl_indemnity_limit numeric(19,4),
    @is_personal_accident tinyint,
    @pa_insured_person varchar(60),
    @pa_age int,
    @pa_duties varchar(70),
    @pa_compensation_amount numeric(19,4),
    @pa_payable_by varchar(20),
    @is_engineering tinyint,
    @e_air_oil_indemnity_limit numeric(19,4),
    @e_electrical_indemnity_limit numeric(19,4),
    @e_crane_indemnity_limit numeric(19,4),
    @is_road_risks tinyint,
    @rr_indemnity_limit numeric(19,4)
AS
BEGIN
INSERT INTO combined_motor (
    insurance_file_cnt ,
    is_all_risks ,
    ar_sum_insured ,
    is_business_interruption ,
    bi_sum_insured ,
    is_employers_liability ,
    el_description ,
    el_indemnity_limit ,
    is_public_liability ,
    pl_description ,
    pl_wages_estimate ,
    pl_indemnity_limit ,
    is_personal_accident ,
    pa_insured_person ,
    pa_age ,
    pa_duties ,
    pa_compensation_amount ,
    pa_payable_by ,
    is_engineering ,
    e_air_oil_indemnity_limit ,
    e_electrical_indemnity_limit ,
    e_crane_indemnity_limit ,
    is_road_risks ,
    rr_indemnity_limit )
VALUES (
    @insurance_file_cnt,
    @is_all_risks,
    @ar_sum_insured,
    @is_business_interruption,
    @bi_sum_insured,
    @is_employers_liability,
    @el_description,
    @el_indemnity_limit,
    @is_public_liability,
    @pl_description,
    @pl_wages_estimate,
    @pl_indemnity_limit,
    @is_personal_accident,
    @pa_insured_person,
    @pa_age,
    @pa_duties,
    @pa_compensation_amount,
    @pa_payable_by,
    @is_engineering,
    @e_air_oil_indemnity_limit,
    @e_electrical_indemnity_limit,
    @e_crane_indemnity_limit,
    @is_road_risks,
    @rr_indemnity_limit)
END

GO

