SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Product_GetRecoveryInstalmentFlag'
GO

-- ADO #39459: Instalment for Claim Recovery - Scheme Configuration
-- Get recovery_instalments_enabled flag for a given product_id
CREATE PROCEDURE spu_Product_GetRecoveryInstalmentFlag
    @ProductId INT,
    @RecoveryInstalmentsEnabled TINYINT OUTPUT
AS
BEGIN
    SET @RecoveryInstalmentsEnabled = 0

    SELECT @RecoveryInstalmentsEnabled = ISNULL(recovery_instalments_enabled, 0)
    FROM Product
    WHERE product_id = @ProductId
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
