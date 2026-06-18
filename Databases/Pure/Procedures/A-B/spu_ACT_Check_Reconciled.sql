SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Check_Reconciled'
GO

CREATE PROCEDURE spu_ACT_Check_Reconciled
    @document_id int
AS

SELECT NULL
FROM transdetail
WHERE spare LIKE '%RECONCILED%'
AND document_id = @document_id

GO
