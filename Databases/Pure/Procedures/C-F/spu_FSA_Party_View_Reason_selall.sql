SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_FSA_Party_View_Reason_selall'
GO

CREATE PROCEDURE spu_FSA_Party_View_Reason_selall
	@include_complaints int = 0

AS

SELECT
	r.fsa_party_view_reason_id,
	r.code,
	c.caption,
	r.is_logged,
	r.is_question
FROM
	FSA_Party_View_Reason r
INNER JOIN
	PMCaption c ON c.caption_id=r.caption_id
WHERE
	effective_date<=GETDATE()
AND
	is_deleted=0
AND
	(@include_complaints=1) or (@include_complaints=0 and is_complaint=0)
ORDER BY
	c.caption