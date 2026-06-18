EXECUTE DDLDropProcedure 'spu_ACT_Get_Introducer_Trans_For_Reversal'
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
/*
	Created By	: ???
	Creation Date: ???
	Description:	retunr the ddocument id for Internal Commission.  This is definitely called when cancelling a payment
	
	Ammended By:
	
	DATE			By					Description
	--------------------------------------------------------------------------------------------------------------------------------
	6th June 2017	George Harris		Changed the join from transdetail to document as should not join on the spare but rather the following
										d.document_ref = substring(td.spare,10,11)id
*/
CREATE PROC spu_ACT_Get_Introducer_Trans_For_Reversal
(
    @DocumentID int
)
AS BEGIN

	SELECT  DISTINCT d2.document_id 
	FROM	document d 
	JOIN	transdetail td
	ON	d.document_id = td.document_id
        AND	td.company_id = d.company_id
	JOIN	document d2
	ON	d2.document_id = td.document_id
	WHERE	substring(td.spare, 1, 8) = 'INT COMM'
	AND	d.document_id = @DocumentId

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
