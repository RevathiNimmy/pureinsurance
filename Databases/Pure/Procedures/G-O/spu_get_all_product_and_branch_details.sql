SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_get_all_product_and_branch_details
GO

CREATE PROCEDURE spu_get_all_product_and_branch_details  
 @nGetProduct INT,
 @nGetBranch INT  
AS  
  Declare @nGetproductTemp INT
  Declare @nGetBranchTemp int

  SET @nGetproductTemp = @nGetProduct 
  SET @nGetBranchTemp = @nGetBranch
  If @nGetproductTemp =1 
  BEGIN
  SELECT product_id ,LTRIM(RTRIM(description))  FROM Product WHERE is_deleted =0
  END
  ELSE 
  IF @nGetBranchTemp = 1
  BEGIN
  SELECT source_id ,LTRIM(RTRIM(description))   FROM Source WHERE is_deleted =0
  END 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO