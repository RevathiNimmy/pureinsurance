SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Update_List_user'
GO


CREATE PROCEDURE spu_GEM_Update_List_user
    @list_user_id int,
    @list_id int,
    @text varchar(70),
    @abi_code varchar(10)
AS


BEGIN
UPDATE List_user
    SET
    list_id=@list_id,
    text=@text,
    abi_code=@abi_code
WHERE list_user_id = @list_user_id
END
GO


