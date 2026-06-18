SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_policy_life_cycle_docs_add'
GO

CREATE PROCEDURE spe_policy_life_cycle_docs_add
    @action_no smallint,
    @transaction_type int,
    @gis_scheme_id int,
    @policy_life_cycle_docs_id int,
    @documents_id int
AS
BEGIN
INSERT INTO policy_life_cycle_docs (
    action_no,
    transaction_type,
    gis_scheme_id,
    policy_life_cycle_docs_id,
    documents_id)
VALUES (
    @action_no,
    @transaction_type,
    @gis_scheme_id,
    @policy_life_cycle_docs_id,
    @documents_id)
END

GO

