EXECUTE DDLDropProcedure 'spu_get_latest_document_version_template_id'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_get_latest_document_version_template_id
	@doc_template_id INT,
	@effective_date DATETIME
AS

DECLARE @code VARCHAR(20), @copy_of_original tinyint

SELECT @code = code, @copy_of_original = copy_of_original from Document_Template where document_template_id = @doc_template_id

IF @copy_of_original = 1
	SELECT document_template_id  FROM document_template dtmp
	WHERE dtmp.code = @code ORDER BY document_template_id ASC
ELSE BEGIN
	SELECT document_template_id  FROM document_template dtmp
	WHERE dtmp.effective_date =
		(SELECT MAX(effective_date)
		FROM document_template
		WHERE document_template.code = @code
			AND CONVERT(DATETIME,(CONVERT(VARCHAR,document_template.effective_date,106))) <= @effective_date
			AND is_deleted=0
		)
	AND  dtmp.code = @code
END

GO
