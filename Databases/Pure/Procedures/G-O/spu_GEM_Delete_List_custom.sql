SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Delete_List_custom'
GO


CREATE PROCEDURE spu_GEM_Delete_List_custom
    @list_custom_id int
AS


DELETE FROM List_custom
WHERE list_custom_id = @list_custom_id
GO


