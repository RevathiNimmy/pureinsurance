SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_diary_life_cycle_del'
GO

CREATE PROCEDURE spe_diary_life_cycle_del
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int,
    @diary_action_code_id int
AS
DELETE FROM diary_life_cycle
WHERE action_no = @action_no AND transaction_type = @transaction_type AND gis_scheme_id = @gis_scheme_id AND diary_action_code_id = @diary_action_code_id

GO

