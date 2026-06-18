SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Policy_Details_From_Claim_Payment'
GO

CREATE PROCEDURE spu_CLM_Get_Policy_Details_From_Claim_Payment

@claim_payment_id int

AS 

BEGIN
	SELECT insurance_file_cnt, document_ref
	FROM Document
	INNER JOIN Claim_Payment ON Claim_Payment.document_id = Document.document_id
	WHERE Claim_Payment.claim_payment_id = @claim_payment_id
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO