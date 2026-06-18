SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_claim_del'
GO

CREATE PROCEDURE spe_risk_claim_del
    @insurance_file_cnt int,
    @is_buildings tinyint,
    @record_no int
AS
DELETE FROM risk_claim
WHERE insurance_file_cnt = @insurance_file_cnt AND is_buildings = @is_buildings AND record_no = @record_no

GO

