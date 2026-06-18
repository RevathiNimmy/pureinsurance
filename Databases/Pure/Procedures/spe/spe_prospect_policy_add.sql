SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_policy_add'
GO

CREATE PROCEDURE spe_prospect_policy_add
    @party_cnt int ,
    @prospect_policy_id int OUTPUT ,
    @risk_group_id int ,
    @renewal_date datetime ,
    @no_of_times_quoted int ,
    @target_premium numeric(19,4)
AS
BEGIN
IF @prospect_policy_id = 0
                SELECT @prospect_policy_id = NULL
IF @prospect_policy_id = NULL
                SELECT @prospect_policy_id = MAX(prospect_policy_id) + 1
    FROM prospect_policy
                WHERE party_cnt = @party_cnt
IF @prospect_policy_id = NULL
    SELECT @prospect_policy_id = 1
INSERT INTO prospect_policy (
    party_cnt ,
    prospect_policy_id ,
    risk_group_id ,
    renewal_date ,
    no_of_times_quoted ,
    target_premium )
VALUES (
    @party_cnt,
    @prospect_policy_id,
    @risk_group_id,
    @renewal_date,
    @no_of_times_quoted,
    @target_premium)
END

GO

