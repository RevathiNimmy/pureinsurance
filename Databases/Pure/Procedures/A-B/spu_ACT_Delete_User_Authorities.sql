SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_User_Authorities'
GO


CREATE PROCEDURE spu_ACT_Delete_User_Authorities
    @user_id smallint
AS


DELETE FROM User_Authorities
WHERE user_id = @user_id
GO


