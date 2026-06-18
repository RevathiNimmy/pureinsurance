SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_DelByList_List_user'
GO


CREATE PROCEDURE spu_GEM_DelByList_List_user
    @list_id int
AS


DELETE FROM List_user
WHERE list_id = @list_id
GO


