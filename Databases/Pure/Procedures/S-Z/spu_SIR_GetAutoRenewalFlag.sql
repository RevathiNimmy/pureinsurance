SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_GetAutoRenewalFlag'
GO
Create Procedure spu_SIR_GetAutoRenewalFlag
@InsFileCnt int
AS
BEGIN
Select is_auto_renewable from Product 
INNER JOIN Insurance_file on Insurance_file.product_id = Product.Product_id
Where Insurance_file_cnt=@InsFileCnt 
END
