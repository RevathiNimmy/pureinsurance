SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GEM_Select_Lists'
GO


CREATE PROCEDURE spu_GEM_Select_Lists
    @list_id int
AS


SELECT
    list_id,
    property_id,
    description
 FROM Lists
WHERE list_id = @list_id
GO


