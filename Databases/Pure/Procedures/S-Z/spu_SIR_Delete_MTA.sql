SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_Delete_MTA'
GO

CREATE PROCEDURE spu_SIR_Delete_MTA
    @product_id INT, 
    @mta_event_description_id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS    

UPDATE Product_MTA_Events
		SET UserId = @UserId,
			UniqueId = @UniqueId,
			ScreenHierarchy =  @ScreenHierarchy
WHERE product_id = @product_id 

DELETE FROM    
    Product_MTA_Events  
WHERE    
   product_id = @product_id    

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO