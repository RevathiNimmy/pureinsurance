SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_claim_add'
GO

CREATE PROCEDURE spe_risk_claim_add
    @insurance_file_cnt int,
    @is_buildings tinyint,
    @record_no int,
    @claim_date datetime,
    @claim_value numeric(19,4)
AS
BEGIN
INSERT INTO risk_claim (
    insurance_file_cnt ,
    is_buildings ,
    record_no ,
    claim_date ,
    claim_value )
VALUES (
    @insurance_file_cnt,
    @is_buildings,
    @record_no,
    @claim_date,
    @claim_value)
END

GO

