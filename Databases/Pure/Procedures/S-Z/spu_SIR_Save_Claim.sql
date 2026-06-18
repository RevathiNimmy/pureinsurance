SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Save_Claim'
GO

CREATE PROCEDURE spu_SIR_Save_Claim
    	
    @product_id INT,
    @claim_event_description_id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL		
AS    
    
INSERT INTO    
    Product_Claim_Events (claim_event_description_id,product_id,UserId,UniqueId,ScreenHierarchy)
VALUES    
    (@claim_event_description_id,@product_id,@UserId,@UniqueId,@ScreenHierarchy)  

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO