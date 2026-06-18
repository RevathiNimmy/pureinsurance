SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_risk_claim_sel'
GO
CREATE PROCEDURE spe_event_risk_claim_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    is_buildings,
    record_no,
    claim_date,
    claim_value
FROM event_risk_claim
WHERE insurance_file_cnt = @insurance_file_cnt

GO

