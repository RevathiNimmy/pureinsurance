SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Save_Causation'
GO

CREATE PROCEDURE spu_SIR_Save_Causation
    @product_id INT,
    @primary_cause_id INT 	
AS    
    
INSERT INTO    
    Product_Allowed_Causation (product_id,primary_cause_id)
VALUES    
    (@product_id,@primary_cause_id)  

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO