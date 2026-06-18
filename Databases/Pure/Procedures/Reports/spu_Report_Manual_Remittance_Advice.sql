SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Manual_Remittance_Advice'
GO

CREATE PROCEDURE spu_Report_Manual_Remittance_Advice
	@branch_id 	int,
	@document_ref	varchar(225)
AS

DECLARE @cashlistid int

SELECT @cashlistid = cli.cashlist_id
FROM document d
JOIN transdetail td
ON td.document_id = d.document_id
JOIN cashlistitem cli
ON cli.transdetail_id = td.transdetail_id
WHERE d.document_ref = @document_ref
AND d.company_id = @branch_id

EXEC spu_Report_Remittance_Advice @cashlistid

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

