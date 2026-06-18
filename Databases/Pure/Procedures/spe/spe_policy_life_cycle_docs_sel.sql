SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_policy_life_cycle_docs_sel'
GO

CREATE PROCEDURE spe_policy_life_cycle_docs_sel
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int,
    @policy_life_cycle_docs_id int
AS
SELECT
    action_no,
    transaction_type,
    gis_scheme_id,
    policy_life_cycle_docs_id,
    documents_id
 FROM policy_life_cycle_docs
WHERE action_no = @action_no AND transaction_type = @transaction_type AND gis_scheme_id = @gis_scheme_id AND policy_life_cycle_docs_id = @policy_life_cycle_docs_id

GO

