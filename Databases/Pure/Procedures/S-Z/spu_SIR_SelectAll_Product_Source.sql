
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_SelectAll_Product_Source'
GO

CREATE PROCEDURE spu_SIR_SelectAll_Product_Source    
    @product_id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
    
AS    
    
 SELECT s.source_id,    
  s.description,    
  CASE WHEN p.product_id IS Null THEN 0 ELSE 1 END As Chosen    
 FROM source s    
 LEFT JOIN product_source p    
  ON p.source_id = s.source_id    
  AND p.product_id = @product_id    
 ORDER BY p.product_id 

GO
  
