SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_text_file_template'
GO


CREATE PROCEDURE spu_text_file_template
	@DocumentTypeId INT,
	@RiskCodeId INT,
	@RiskGroupId INT,
	@SlotNumber INT,
	@SourceID INT
AS


BEGIN

SELECT
	document_template_id,
	is_editable_after_merging,
	(
		CASE
		WHEN (source_id <> 0 AND risk_group_id IS NOT NULL) THEN 1
		WHEN (source_id <> 0 AND risk_code_id IS NOT NULL) THEN 2
		WHEN (source_id = 0 AND risk_group_id IS NOT NULL) THEN 3
		WHEN (source_id = 0 AND risk_code_id IS NOT NULL) THEN 4
		WHEN (source_id <> 0 AND risk_group_id IS NULL AND risk_code_id IS NULL) THEN 5
		ELSE 6
		END
	) sort_column

FROM document_template
WHERE document_type_id = @DocumentTypeId
AND slot_number = @SlotNumber
AND is_deleted = 0
AND (source_id = @SourceID OR source_id = 0)
AND (risk_group_id = @RiskGroupId OR risk_group_id IS NULL)
AND (risk_code_id = @RiskCodeId OR risk_code_id IS NULL)
ORDER BY sort_column
    
END
GO



