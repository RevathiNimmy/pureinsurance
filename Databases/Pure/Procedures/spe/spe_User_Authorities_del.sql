SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_User_Authorities_del'
GO
CREATE PROCEDURE spe_User_Authorities_del
    @user_id smallint
AS

DELETE FROM User_Authorities

WHERE user_id = @user_id

GO

