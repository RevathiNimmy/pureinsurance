SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_combined_liability_add'
GO

CREATE PROCEDURE spe_combined_liability_add
    @insurance_file_cnt int,
    @is_employers_liability tinyint,
    @e_indemnity_limit numeric(19,4),
    @e_rating_basis varchar(70),
    @is_p_a_p_liability tinyint,
    @p_a_p_indemnity_limit numeric(19,4),
    @p_a_p_rating_basis varchar(70),
    @excess numeric(19,4)
AS
BEGIN
INSERT INTO combined_liability (
    insurance_file_cnt ,
    is_employers_liability ,
    e_indemnity_limit ,
    e_rating_basis ,
    is_p_a_p_liability ,
    p_a_p_indemnity_limit ,
    p_a_p_rating_basis ,
    excess )
VALUES (
    @insurance_file_cnt,
    @is_employers_liability,
    @e_indemnity_limit,
    @e_rating_basis,
    @is_p_a_p_liability,
    @p_a_p_indemnity_limit,
    @p_a_p_rating_basis,
    @excess)
END

GO

