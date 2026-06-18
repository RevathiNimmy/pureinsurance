SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_policy_life_cycle_acti_del'
GO

CREATE PROCEDURE spe_policy_life_cycle_acti_del
    @sequence_no smallint,
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int
AS
DELETE FROM policy_life_cycle_actions
WHERE sequence_no = @sequence_no AND action_no = @action_no AND transaction_type = @transaction_type AND gis_scheme_id = @gis_scheme_id

GO

