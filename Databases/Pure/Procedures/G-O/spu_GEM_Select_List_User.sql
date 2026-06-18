SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Select_List_User'
GO


CREATE PROCEDURE spu_GEM_Select_List_User
    @list_id int
AS


SELECT
    list_user_id,
    list_id,
    text,
    abi_code
 FROM List_user
WHERE list_id = @list_id
GO


