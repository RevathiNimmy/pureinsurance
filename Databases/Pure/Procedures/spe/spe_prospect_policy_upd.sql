SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_policy_upd'
GO

CREATE PROCEDURE spe_prospect_policy_upd
    @party_cnt int,
    @prospect_policy_id int,
    @risk_group_id int,
    @renewal_date datetime,
    @no_of_times_quoted int,
    @target_premium numeric(19,4)
AS
BEGIN
UPDATE prospect_policy
    SET

    risk_group_id=@risk_group_id,
    renewal_date=@renewal_date,
    no_of_times_quoted=@no_of_times_quoted,
    target_premium=@target_premium
WHERE party_cnt = @party_cnt AND prospect_policy_id = @prospect_policy_id
END

GO

