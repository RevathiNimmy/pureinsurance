SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_accumulation_values_add'
GO

CREATE PROCEDURE spe_accumulation_values_add
    @accumulation_code_1 int,
    @accumulation_code_2 int,
    @accumulation_code_3 int,
    @accumulation_code_4 int,
    @accumulation_code_5 int,
    @accumulation_code_6 int,
    @accumulation_code_7 int,
    @accumulation_code_8 int,
    @accumulation_code_9 int,
    @insurance_file_cnt int,
    @risk_id int,
    @peril_type_id int,
    @sum_insured numeric(19,4)
AS
BEGIN
INSERT INTO accumulation_values (
    accumulation_code_1 ,
    accumulation_code_2 ,
    accumulation_code_3 ,
    accumulation_code_4 ,
    accumulation_code_5 ,
    accumulation_code_6 ,
    accumulation_code_7 ,
    accumulation_code_8 ,
    accumulation_code_9 ,
    risk_cnt ,
    peril_type_id ,
    sum_insured )
VALUES (
    @accumulation_code_1,
    @accumulation_code_2,
    @accumulation_code_3,
    @accumulation_code_4,
    @accumulation_code_5,
    @accumulation_code_6,
    @accumulation_code_7,
    @accumulation_code_8,
    @accumulation_code_9,
    @risk_id,
    @peril_type_id,
    @sum_insured)
END

GO

