SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_personal_accident_sel'
GO

CREATE PROCEDURE spe_personal_accident_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    name_of_insured,
    age_of_insured,
    occupation_duties,
    compensation_amount,
    is_lump_sum
 FROM personal_accident
WHERE insurance_file_cnt = @insurance_file_cnt

GO

