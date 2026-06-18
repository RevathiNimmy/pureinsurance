SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Product_Get_No_Of_Policies'
GO


CREATE PROCEDURE spu_Product_Get_No_Of_Policies

@product_id int

AS

BEGIN
	SELECT Count(*) as NoOfPolicies 
	FROM Insurance_File 
	WHERE product_id = @product_id
END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
