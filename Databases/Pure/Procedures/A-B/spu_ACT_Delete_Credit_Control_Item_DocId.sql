SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Delete_Credit_Control_Item_DocId'
GO

CREATE PROCEDURE spu_ACT_Delete_Credit_Control_Item_DocId
    @document_id INT 
AS

BEGIN
    
    Update Credit_Control_Item	WITH (ROWLOCK)
    Set is_deleted=1
    WHERE document_id = @document_id
    
END
GO