SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_personal_acciden_sel'
GO

CREATE PROCEDURE spe_event_personal_acciden_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    name_of_insured,
    age_of_insured,
    occupation_duties,
    compensation_amount,
    is_lump_sum
FROM event_personal_accident
WHERE insurance_file_cnt = @insurance_file_cnt

GO

