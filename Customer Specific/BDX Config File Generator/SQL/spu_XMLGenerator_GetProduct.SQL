/****** Object:  StoredProcedure [dbo].[spu_XMLGenerator_GetProduct]    Script Date: 18-05-2016 5:22:06 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spu_XMLGenerator_GetProduct]

AS
BEGIN

SELECT  code as ProductCode, product_id   
FROM    Product 
WHERE (is_deleted = 0)
END