SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_personal_accident_add'
GO

CREATE PROCEDURE spe_personal_accident_add
    @insurance_file_cnt int,
    @name_of_insured varchar(60),
    @age_of_insured int,
    @occupation_duties varchar(70),
    @compensation_amount numeric(19,4),
    @is_lump_sum tinyint
AS
BEGIN
INSERT INTO personal_accident (
    insurance_file_cnt ,
    name_of_insured ,
    age_of_insured ,
    occupation_duties ,
    compensation_amount ,
    is_lump_sum )
VALUES (
    @insurance_file_cnt,
    @name_of_insured,
    @age_of_insured,
    @occupation_duties,
    @compensation_amount,
    @is_lump_sum)
END

GO

