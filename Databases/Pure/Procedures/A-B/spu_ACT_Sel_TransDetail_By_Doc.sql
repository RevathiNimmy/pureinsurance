SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Sel_TransDetail_By_Doc'
GO

CREATE PROCEDURE spu_ACT_Sel_TransDetail_By_Doc  
    @document_id int  
AS  
  
SELECT  
    transdetail_id,  
    account_id,  
    amount
FROM TransDetail  
    WHERE document_id = @document_id  
Order By document_sequence

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
