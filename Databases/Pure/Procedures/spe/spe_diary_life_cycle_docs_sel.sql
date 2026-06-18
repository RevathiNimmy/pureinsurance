SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_diary_life_cycle_docs_sel'
GO

CREATE PROCEDURE spe_diary_life_cycle_docs_sel
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int,
    @diary_action_code_id int,
    @diary_life_cycle_docs_id int
AS
SELECT
    action_no,
    transaction_type,
    gis_scheme_id,
    diary_action_code_id,
    diary_life_cycle_docs_id,
    documents_id
 FROM diary_life_cycle_docs
WHERE action_no = @action_no AND transaction_type = @transaction_type AND gis_scheme_id = @gis_scheme_id AND diary_action_code_id = @diary_action_code_id AND diary_life_cycle_docs_id = @diary_life_cycle_docs_id

GO

