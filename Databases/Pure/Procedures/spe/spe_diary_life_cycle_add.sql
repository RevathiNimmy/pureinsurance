SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_diary_life_cycle_add'
GO

CREATE PROCEDURE spe_diary_life_cycle_add
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int,
    @diary_action_code_id int,
    @max_days smallint,
    @edi_message int,
    @policy_status int,
    @next_diary_action_code_id int
AS
BEGIN
INSERT INTO diary_life_cycle (
    action_no,
    transaction_type,
    gis_scheme_id,
    diary_action_code_id,
    max_days,
    edi_message,
    policy_status,
    next_diary_action_code_id)
VALUES (
    @action_no,
    @transaction_type,
    @gis_scheme_id,
    @diary_action_code_id,
    @max_days,
    @edi_message,
    @policy_status,
    @next_diary_action_code_id)
END

GO

