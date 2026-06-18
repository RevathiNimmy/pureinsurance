SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Reversal_Transaction'
GO


CREATE PROCEDURE spu_ACT_Select_Reversal_Transaction
    @TransdetailID  int
AS

SELECT t2.transdetail_id FROM transdetail t2  
        		JOIN transdetail t ON t.document_id = t2.document_id
					AND t.transdetail_id <> t2.transdetail_id    
        		JOIN transdetail_type ty ON t2.transdetail_type_id = ty.transdetail_type_id  
        
         WHERE t.transdetail_id = @TransdetailID 
        AND ty.code = 'DDREV'
    
 
GO


