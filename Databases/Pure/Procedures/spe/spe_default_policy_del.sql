SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_default_policy_del'
GO

CREATE PROCEDURE spe_default_policy_del
    @default_policy_id int
AS
DELETE FROM default_policy
WHERE default_policy_id = @default_policy_id

GO

