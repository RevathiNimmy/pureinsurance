SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_partyBG_Products_Sel'
GO

CREATE PROCEDURE spu_partyBG_Products_Sel  
@BG_Number INT  
  
As  
BEGIN  
 IF ISNULL(@BG_Number,0) <> 0  
  SELECT  
        BGPL.Product_Id,
		P.description			  
  FROM BG_product_link BGPL
	INNER JOIN Product P
		ON BGPL.product_id = p.product_id  
  WHERE BG_id = @BG_Number  
  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

