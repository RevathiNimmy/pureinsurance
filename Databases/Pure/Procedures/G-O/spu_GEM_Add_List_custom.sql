SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Add_List_custom'
GO


CREATE PROCEDURE spu_GEM_Add_List_custom
    @list_custom_id int OUTPUT,
    @position_id int,
    @value_id int,
    @text varchar(70),
    @abi_code varchar(10),
    @command char,
    @property_id varchar(10)
AS


BEGIN
INSERT INTO List_custom (
    position_id,
    value_id,
    text,
    abi_code,
    command,
    property_id)
VALUES (
    @position_id,
    @value_id,
    @text,
    @abi_code,
    @command,
    @property_id)
END
BEGIN
SELECT @list_custom_id = @@IDENTITY
END
GO


