SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Get_PrePaymentOptionValue'
GO


CREATE PROCEDURE spu_Get_PrePaymentOptionValue    
    @product_id int    
AS    
    
SELECT ISNULL(is_enable_PrePayment,0) FROM Product WHERE product_id = @product_id
GO