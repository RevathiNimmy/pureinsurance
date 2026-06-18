SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spe_PFScheme_PLLProduct'
GO

CREATE PROCEDURE spe_PFScheme_PLLProduct
    @CompanyNo INT,
    @SchemeNo INT,
    @SchemeVersion INT,
	@UserID INT,
	@UniqueID VARCHAR(50),
	@ScreenHierarchy VARCHAR(500)
AS
BEGIN

SELECT
    P.product_id,
    P.description,
    CASE
        WHEN PP.product_id IS NULL THEN 0
        ELSE 1
    END Chosen
FROM
    product P
    LEFT OUTER JOIN PFSchemeProducts PP
        ON  P.product_id = PP.product_id
AND PP.CompanyNo = @CompanyNo
AND PP.SchemeNo = @SchemeNo
AND PP.SchemeVersion = @SchemeVersion

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

