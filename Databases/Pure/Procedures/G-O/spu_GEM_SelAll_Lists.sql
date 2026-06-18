SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_SelAll_Lists'
GO


CREATE PROCEDURE spu_GEM_SelAll_Lists
AS


SELECT
    list_id,
   property_id,
   description
 FROM Lists
GO


