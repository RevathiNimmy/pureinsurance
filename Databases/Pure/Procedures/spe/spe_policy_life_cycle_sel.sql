SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_policy_life_cycle_sel'
GO

CREATE PROCEDURE spe_policy_life_cycle_sel
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int
AS
SELECT
    action_no,
    transaction_type,
    gis_scheme_id,
    edi_message,
    policy_status
 FROM policy_life_cycle
WHERE action_no = @action_no AND transaction_type = @transaction_type AND gis_scheme_id = @gis_scheme_id

GO

