SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Add_List_user'
GO


CREATE PROCEDURE spu_GEM_Add_List_user
    @list_user_id int OUTPUT,
    @list_id int,
    @text varchar(70),
    @abi_code varchar(10)
AS


BEGIN
INSERT INTO List_user (
    list_id,
    text,
    abi_code)
VALUES (
    @list_id,
    @text,
    @abi_code)
END
BEGIN
SELECT @list_user_id = @@IDENTITY
END
GO


