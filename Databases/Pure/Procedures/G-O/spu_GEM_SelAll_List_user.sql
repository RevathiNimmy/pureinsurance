SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_SelAll_List_user'
GO


CREATE PROCEDURE spu_GEM_SelAll_List_user
AS


SELECT
    list_user_id,
   list_id,
   text,
   abi_code
 FROM List_user
GO


