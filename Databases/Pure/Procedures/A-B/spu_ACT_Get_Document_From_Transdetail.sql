SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Document_From_Transdetail'
GO

CREATE PROCEDURE spu_ACT_Get_Document_From_Transdetail
    @Transdetail_id INT

AS    
   
SELECT d.document_id, d.document_ref  
FROM transdetail t join document d on t.document_id=d.document_id  
WHERE transdetail_id = @Transdetail_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO