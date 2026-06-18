SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_SAM_SelectAll_Product_Source'
GO

CREATE PROCEDURE spu_SAM_SelectAll_Product_Source 
    @product_id INT,        
    @user_id INT   =NULL
AS        
        
 SELECT s.code,        
  s.description       
FROM source s        
 JOIN product_source p        
  ON p.source_id = s.source_id        
  AND p.product_id = @product_id 
  WHERE (p.source_id not in (SELECT source_id from PMUser_Source where user_id=@user_id) or @user_id is null)
  ORDER BY p.product_id 

Go