SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_policy_saa'
GO

CREATE PROCEDURE spe_prospect_policy_saa
    @party_cnt int
AS
SELECT
    party_cnt,
    prospect_policy_id,
    risk_group_id,
    renewal_date,
    no_of_times_quoted,
    target_premium
 FROM prospect_policy
WHERE party_cnt = @party_cnt
ORDER BY prospect_policy_id ASC

GO

