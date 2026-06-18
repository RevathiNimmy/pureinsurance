SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_DeleteSession_TransDetail_Selection'
GO

Create Procedure spu_ACT_DeleteSession_TransDetail_Selection
	@Session_guid varchar(40)
AS
	DELETE FROM TransDetail_selection WHERE session_guid = @session_guid
 





