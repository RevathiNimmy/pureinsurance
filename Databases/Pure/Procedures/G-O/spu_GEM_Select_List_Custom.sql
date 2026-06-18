SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Select_List_Custom'
GO


CREATE PROCEDURE spu_GEM_Select_List_Custom
    @property_id varchar(10)
AS


SELECT
    list_custom_id,
    position_id,
    value_id,
    text,
    abi_code,
    command,
    property_id
 FROM List_custom
WHERE property_id = @property_id
GO


