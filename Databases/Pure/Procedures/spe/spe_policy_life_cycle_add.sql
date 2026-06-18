SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_policy_life_cycle_add'
GO

CREATE PROCEDURE spe_policy_life_cycle_add
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int,
    @edi_message int,
    @policy_status int
AS
BEGIN
INSERT INTO policy_life_cycle (
    action_no,
    transaction_type,
    gis_scheme_id,
    edi_message,
    policy_status)
VALUES (
    @action_no,
    @transaction_type,
    @gis_scheme_id,
    @edi_message,
    @policy_status)
END

GO

