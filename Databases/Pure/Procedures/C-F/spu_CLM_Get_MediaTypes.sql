SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_MediaTypes'
GO

CREATE PROCEDURE spu_CLM_Get_MediaTypes
@PaymentsOnly TINYINT = 0

AS

BEGIN

	SELECT mt.mediatype_id, mt.description, mt.code, mtv.code, mt.is_validation_enabled

	FROM mediatype mt

	INNER JOIN mediatype_validation mtv
		ON mt.mediatype_validation_id =mtv.mediatype_validation_id
	WHERE mt.effective_date < GETDATE()
    AND mt.is_deleted=0 
    AND (@PaymentsOnly = 0 OR (@PaymentsOnly = 1 AND ISNULL(is_payment, 0) = 1))
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
