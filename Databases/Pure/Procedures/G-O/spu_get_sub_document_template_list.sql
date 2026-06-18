SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_sub_document_template_list'
GO

Create Procedure spu_get_sub_document_template_list 
    @source_id INT
AS
BEGIN
    SELECT
        DT.document_template_id,
        DT.code,
        DT.description,
        DT.source_id,
        DT.document_type_id,
        DT.created_by_id,
        DT.date_created,
        DT.modified_by_id,
        DT.last_modified,
        DT.is_deleted,
        DT.slot_number,
        DT.risk_code_id,
        DT.risk_group_id,
        DT.is_editable_after_merging,
        DT.pmwrk_task_instance_temp_cnt,
        DT.printer,
        DT.chaser_description
    FROM document_template AS DT INNER JOIN document_type AS DTY
        ON  dt.document_type_id = DTY.document_type_id
    WHERE DTY.code = 'LETTER'
    AND DT.is_deleted = 0
AND DT.source_id IN (@source_id, 0)
AND (document_template_id) IN 
	(SELECT MAX(document_template_id) FROM document_template GROUP BY code)

END

GO
