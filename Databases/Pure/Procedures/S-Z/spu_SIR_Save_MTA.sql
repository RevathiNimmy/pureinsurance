SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Save_MTA'
GO

CREATE PROCEDURE spu_SIR_Save_MTA
    		
    @product_id INT,
    @mta_event_description_id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS    
    
INSERT INTO    
    Product_MTA_Events (mta_event_description_id,product_id,UserId,UniqueId,ScreenHierarchy)
VALUES    
    (@mta_event_description_id,@product_id,@UserId,@UniqueId,@ScreenHierarchy)  

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO