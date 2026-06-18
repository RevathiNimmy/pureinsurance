
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_BankGuarantee_PLLProduct'
GO
 CREATE PROCEDURE spu_BankGuarantee_PLLProduct
    @BG_Id INT
AS

SELECT
    P.product_id,
    P.description,
    CASE
        WHEN BGPL.product_id IS NULL THEN 0
        ELSE 1
    END Chosen

FROM    product P
        INNER JOIN BG_Product_Link BGPL
              ON BGPL.product_id = P.product_id
WHERE   P.Is_Deleted <>1
AND  BGPL.Bg_Id = @Bg_Id

  GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
