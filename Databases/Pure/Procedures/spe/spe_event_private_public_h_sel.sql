SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_private_public_h_sel'
GO

CREATE PROCEDURE spe_event_private_public_h_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    driving_restriction,
    usage,
    cover,
    excess,
    no_claim_discount_years,
    is_ncd_protected,
    is_self_employed,
    taxi_firm,
    radio_value
FROM event_private_public_hire
WHERE insurance_file_cnt = @insurance_file_cnt

GO

