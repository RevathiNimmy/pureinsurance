SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_ACT_DeleteUserSessions_TransDetail_Selection'
GO

CREATE PROCEDURE spu_ACT_DeleteUserSessions_TransDetail_Selection
@user_id int
AS

DELETE FROM TRANSDETAIL_SELECTION
WHERE user_id = @user_id

