SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_combined_liability_sel'
GO

CREATE PROCEDURE spe_combined_liability_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    is_employers_liability,
    e_indemnity_limit,
    e_rating_basis,
    is_p_a_p_liability,
    p_a_p_indemnity_limit,
    p_a_p_rating_basis,
    excess
 FROM combined_liability
WHERE insurance_file_cnt = @insurance_file_cnt

GO

