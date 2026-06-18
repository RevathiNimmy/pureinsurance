SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_claim_upd'
GO

CREATE PROCEDURE spe_risk_claim_upd
    @insurance_file_cnt int,
    @is_buildings tinyint,
    @record_no int,
    @claim_date datetime,
    @claim_value numeric(19,4)
AS
BEGIN
UPDATE risk_claim
    SET
    claim_date=@claim_date,
    claim_value=@claim_value
WHERE insurance_file_cnt = @insurance_file_cnt AND is_buildings = @is_buildings AND record_no = @record_no
END

GO

