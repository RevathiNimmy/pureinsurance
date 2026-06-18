SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Update_List_custom'
GO


CREATE PROCEDURE spu_GEM_Update_List_custom
    @list_custom_id int,
    @position_id int,
    @value_id int,
    @text varchar(70),
    @abi_code varchar(10),
    @command char,
    @property_id varchar(10)
AS


BEGIN
UPDATE List_custom
    SET
    position_id=@position_id,
    value_id=@value_id,
    text=@text,
    abi_code=@abi_code,
    command=@command,
    property_id=@property_id
WHERE list_custom_id = @list_custom_id
END
GO


