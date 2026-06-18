SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_claim_sel'
GO

CREATE PROCEDURE spe_risk_claim_sel
    @insurance_file_cnt int,
    @is_buildings tinyint,
    @record_no int
AS
SELECT
    insurance_file_cnt,
    is_buildings,
    record_no,
    claim_date,
    claim_value
 FROM risk_claim
WHERE insurance_file_cnt = @insurance_file_cnt AND is_buildings = @is_buildings AND record_no = @record_no

GO

