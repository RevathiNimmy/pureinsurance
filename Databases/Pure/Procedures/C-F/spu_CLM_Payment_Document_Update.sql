SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Payment_Document_Update'
GO

CREATE PROCEDURE spu_CLM_Payment_Document_Update

@stats_folder_cnt int

AS

BEGIN

	UPDATE cp 
		SET cp.document_id = doc.document_id

	FROM claim_payment cp
		INNER JOIN stats_folder sf ON 
			sf.payment_id = cp.base_claim_payment_id

			INNER JOIN document doc ON
				sf.document_ref = doc.document_ref
			AND 	sf.source_id = doc.company_id

	WHERE sf.stats_folder_cnt =@stats_folder_cnt

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
