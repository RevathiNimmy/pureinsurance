SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Delete_List_user'
GO


CREATE PROCEDURE spu_GEM_Delete_List_user
    @list_user_id int
AS


DELETE FROM List_user
WHERE list_user_id = @list_user_id
GO


