SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_policy_dar'
GO

CREATE PROCEDURE spe_prospect_policy_dar
 @party_cnt int
AS
DELETE
FROM prospect_policy
WHERE party_cnt = @party_cnt

GO

