SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_conviction_saa'
GO

CREATE PROCEDURE spe_party_conviction_saa
    @party_cnt int
AS
SELECT
    party_cnt,
    party_conviction_id,
    code,
    conviction_date,
    description,
    fine_amt,
    sentence_code,
    sentence_description,
    sentence_duration,
    sentence_duration_qualifier,
    sentence_effective_date,
    status_code,
    alcohol_level,
    alcohol_measurement_method,
    driving_licence_penalty_pts
 FROM party_conviction
WHERE party_cnt = @party_cnt
ORDER BY party_conviction_id ASC

GO

