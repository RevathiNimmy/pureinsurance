SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_personalaccident'
GO


CREATE PROCEDURE spu_wp_personalaccident
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @insurance_file_cnt INT OUTPUT,
    @name_of_insured Varchar(255),
    @age_of_insured INT OUTPUT,
    @occupation_duties Varchar(255),
    @compensation_amount NUMERIC,
    @is_lump_sum TINYINT OUTPUT
AS


SELECT
    @name_of_insured = personal_accident.name_of_insured,
    @age_of_insured = personal_accident.age_of_insured,
    @occupation_duties = personal_accident.occupation_duties,
    @compensation_amount = personal_accident.compensation_amount,
    @is_lump_sum = personal_accident.is_lump_sum

FROM personal_accident
WHERE personal_accident.insurance_file_cnt = @insurancefilecnt
GO


