SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_policy_sel'
GO

CREATE PROCEDURE spe_prospect_policy_sel
    @party_cnt int,
    @prospect_policy_id int
AS
SELECT
    party_cnt,
    prospect_policy_id,
    risk_group_id,
    renewal_date,
    no_of_times_quoted,
    target_premium
 FROM prospect_policy
WHERE party_cnt = @party_cnt AND prospect_policy_id = @prospect_policy_id

GO

