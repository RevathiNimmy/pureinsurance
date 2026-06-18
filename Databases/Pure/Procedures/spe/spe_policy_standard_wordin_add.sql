SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_policy_standard_wordin_add'
GO

CREATE PROCEDURE spe_policy_standard_wordin_add
    @insurance_file_cnt int,
    @policy_standard_wording_id int,
    @document_template_id int,
	@do_not_merge int=0

AS

BEGIN
INSERT INTO policy_standard_wording (
    insurance_file_cnt ,
    policy_standard_wording_id ,
    document_template_id,
do_not_merge	)
VALUES (
    @insurance_file_cnt,
    @policy_standard_wording_id,
    @document_template_id,
	@do_not_merge)
END

GO

