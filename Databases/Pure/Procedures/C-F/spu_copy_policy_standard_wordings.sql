SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_copy_policy_standard_wordings'
GO
CREATE PROCEDURE spu_copy_policy_standard_wordings
    @old_insurance_file_cnt int,
    @new_insurance_file_cnt int
AS
BEGIN
INSERT  INTO policy_standard_wording (
    insurance_file_cnt,
    policy_standard_wording_id,
    document_template_id,
	do_not_merge)
SELECT  @new_insurance_file_cnt,
    policy_standard_wording_id,
    document_template_id,
	do_not_merge
FROM    policy_standard_wording
WHERE   insurance_file_cnt = @old_insurance_file_cnt
END
GO


