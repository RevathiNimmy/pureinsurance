SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_policy_del'
GO

CREATE PROCEDURE spe_prospect_policy_del
    @party_cnt int,
    @prospect_policy_id int
AS
DELETE FROM prospect_policy
WHERE party_cnt = @party_cnt AND prospect_policy_id = @prospect_policy_id

GO

