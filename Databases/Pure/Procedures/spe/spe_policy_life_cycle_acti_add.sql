SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_policy_life_cycle_acti_add'
GO

CREATE PROCEDURE spe_policy_life_cycle_acti_add
    @sequence_no smallint,
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int,
    @diary_action_code_id int
AS
BEGIN
INSERT INTO policy_life_cycle_actions (
    sequence_no,
    action_no,
    transaction_type,
    gis_scheme_id,
    diary_action_code_id)
VALUES (
    @sequence_no,
    @action_no,
    @transaction_type,
    @gis_scheme_id,
    @diary_action_code_id)
END

GO

