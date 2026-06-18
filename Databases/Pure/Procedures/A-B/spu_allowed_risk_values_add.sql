SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_allowed_risk_values_add'
GO


CREATE PROCEDURE spu_allowed_risk_values_add
    @defined_risk_data_id int,
    @record_number int,
    @allowed_value_1 varchar(255),
    @allowed_value_2 varchar(255)
AS


BEGIN
INSERT INTO allowed_risk_values (
    defined_risk_data_id,
    record_number,
    allowed_value_1,
    allowed_value_2)
VALUES (
    @defined_risk_data_id,
    @record_number,
    @allowed_value_1,
    @allowed_value_2)
END
GO


