SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_combinedmotor'
GO


CREATE PROCEDURE spu_wp_combinedmotor
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @is_all_risks TinyInt OUTPUT,
    @ar_sum_insured Numeric,
    @is_business_interruption TinyInt OUTPUT,
    @bi_sum_insured Numeric,
    @is_employers_liability TinyInt OUTPUT,
    @el_description VarChar(255),
    @el_indemnity_limit Numeric,
    @is_public_liability TinyInt OUTPUT,
    @pl_description VarChar(255),
    @pl_wages_estimate Numeric,
    @pl_indemnity_limit Numeric,
    @is_personal_accident TinyInt OUTPUT,
    @pa_insured_person VarChar(255),
    @pa_age Int OUTPUT,
    @pa_duties VarChar(255),
    @pa_compensation_amount Numeric,
    @pa_payable_by VarChar(255),
    @is_engineering TinyInt OUTPUT,
    @e_air_oil_indemnity_limit Numeric,
    @e_electrical_indemnity_limit Numeric,
    @e_crane_indemnity_limit Numeric,
    @is_road_risks TinyInt OUTPUT,
    @rr_indemnity_limit Numeric
AS


SELECT
    @is_all_risks = combined_motor.is_all_risks,
    @ar_sum_insured = combined_motor.ar_sum_insured,
    @is_business_interruption = combined_motor.is_business_interruption,
    @bi_sum_insured = combined_motor.bi_sum_insured,
    @is_employers_liability = combined_motor.is_employers_liability,
    @el_description = combined_motor.el_description,
    @el_indemnity_limit = combined_motor.el_indemnity_limit,
    @is_public_liability = combined_motor.is_public_liability,

    @pl_description = combined_motor.pl_description,
    @pl_wages_estimate = combined_motor.pl_wages_estimate,
    @pl_indemnity_limit = combined_motor.pl_indemnity_limit,
    @is_personal_accident = combined_motor.is_personal_accident,
    @pa_insured_person = combined_motor.pa_insured_person,
    @pa_age = combined_motor.pa_age,
    @pa_duties = combined_motor.pa_duties,
    @pa_compensation_amount = combined_motor.pa_compensation_amount,
    @pa_payable_by = combined_motor.pa_payable_by,
    @is_engineering = combined_motor.is_engineering,
    @e_air_oil_indemnity_limit = combined_motor.e_air_oil_indemnity_limit,
    @e_electrical_indemnity_limit = combined_motor.e_electrical_indemnity_limit,
    @e_crane_indemnity_limit = combined_motor.e_crane_indemnity_limit,
    @is_road_risks = combined_motor.is_road_risks,
    @rr_indemnity_limit = combined_motor.rr_indemnity_limit

FROM combined_motor
WHERE combined_motor.insurance_file_cnt = @insurancefilecnt
GO


