SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Written_Status_Used'
GO

CREATE PROCEDURE spu_Get_Written_Status_Used

 @product_Id INT  
AS  
  
SET @Product_Id = ISNULL(@Product_Id, 0)  
  
SELECT  
 product_id  
FROM  
 product  
WHERE  
 allow_written_status = 1  
AND  
 (@Product_Id = 0 OR @product_id = product_id)  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
