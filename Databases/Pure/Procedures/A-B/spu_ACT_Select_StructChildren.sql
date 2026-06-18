SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_StructChildren'
GO


CREATE PROCEDURE spu_ACT_Select_StructChildren
    @node_id int, 
    @company_id int = null
AS

IF ISNULL(@company_id, 0) = 0
	EXEC spu_ACT_Select_StructChildren1 @node_id
ELSE
	EXEC spu_ACT_Select_StructChildren2 @node_id, @company_id
GO