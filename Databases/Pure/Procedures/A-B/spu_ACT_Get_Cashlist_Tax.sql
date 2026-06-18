EXEC DDLDropProcedure 'spu_ACT_Get_Cashlist_Tax'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_ACT_Get_Cashlist_Tax 
	@nTransdetailID Int
AS

SELECT  td2.transdetail_id, td2.amount
	From transdetail td1
	Inner Join TransDetail td2 ON td1.account_id = td2.account_id AND td1.document_id = td2.document_id
	Where td1.transdetail_id = @nTransdetailID and td2.transdetail_id <> @nTransdetailID
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

