SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Delete_Lists'
GO


CREATE PROCEDURE spu_GEM_Delete_Lists
    @list_id int
AS


DELETE FROM Lists
WHERE list_id = @list_id
GO


