SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_SelAll_List_custom'
GO


CREATE PROCEDURE spu_GEM_SelAll_List_custom
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
GO


