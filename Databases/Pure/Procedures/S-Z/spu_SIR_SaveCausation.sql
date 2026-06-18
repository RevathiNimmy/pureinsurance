

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_SaveCausation'
GO

CREATE PROCEDURE spu_SIR_SaveCausation
	@Sources TPRODUCT readonly,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS
DECLARE @product_id INT
SELECT 	@product_id = product_id from @Sources
Begin

UPDATE Product_Allowed_Causation
		SET UserId = @UserId,
			UniqueId = @UniqueId,
			ScreenHierarchy =  @ScreenHierarchy
WHERE primary_cause_id not in (Select Linked_id from @Sources) and product_id = @product_id

Delete from Product_Allowed_Causation where primary_cause_id not in (Select Linked_id from @Sources) and product_id = @product_id;

Insert into Product_Allowed_Causation(product_id, primary_cause_id,UserId,UniqueId,ScreenHierarchy) 
(select s.product_id,
 s.Linked_id,
 @UserId,
 @UniqueId,
 @ScreenHierarchy
 FROM  @Sources s where Linked_id not in (select primary_cause_id from Product_Allowed_Causation where product_id = @product_id))

end
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO