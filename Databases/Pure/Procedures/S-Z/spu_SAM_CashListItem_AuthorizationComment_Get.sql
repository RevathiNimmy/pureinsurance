GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_CashListItem_AuthorizationComment_Get'
GO
CREATE PROCEDURE spu_SAM_CashListItem_AuthorizationComment_Get
		@nCashListItem_Id INT
		
	AS
BEGIN
	SELECT authorization_comment FROM CashListItem   WHERE cashlistitem_id =@nCashListItem_Id

END