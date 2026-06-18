

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Save_ProductSource'
GO

CREATE PROCEDURE spu_SIR_Save_ProductSource
	@Sources TPRODUCT readonly,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS
DECLARE @product_id INT
SELECT 	@product_id = product_id from @Sources
Begin

UPDATE Product_Source
		SET UserId = @UserId,
			UniqueId = @UniqueId,
			ScreenHierarchy =  @ScreenHierarchy
WHERE source_id not in (Select Linked_id from @Sources) and product_id = @product_id

Delete from Product_Source where source_id not in (Select Linked_id from @Sources) and product_id = @product_id;

Insert into Product_Source(product_id, source_id,UserId,UniqueId,ScreenHierarchy) 
(select s.product_id,
 s.Linked_id,
 @UserId,
 @UniqueId,
 @ScreenHierarchy
 FROM  @Sources s where Linked_id not in (select source_id from Product_Source where product_id = @product_id))

end

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO