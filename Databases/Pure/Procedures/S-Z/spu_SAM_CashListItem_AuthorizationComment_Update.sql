
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_CashListItem_AuthorizationComment_Update'
GO
CREATE PROCEDURE spu_SAM_CashListItem_AuthorizationComment_Update
		@nCashListItem_Id INT,
		@sDescription VARCHAR(MAX)
		
	AS
BEGIN
	UPDATE CashListItem SET authorization_comment =@sDescription + CHAR(10) + ISNULL(authorization_comment,'')  WHERE cashlistitem_id =@nCashListItem_Id

END