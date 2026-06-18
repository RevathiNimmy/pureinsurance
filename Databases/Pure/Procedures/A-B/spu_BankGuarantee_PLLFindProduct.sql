SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_BankGuarantee_PLLFindProduct'
GO

CREATE PROCEDURE spu_BankGuarantee_PLLFindProduct
    @WhereClause VARCHAR(50)
AS
BEGIN

 	SELECT
	  P.Product_id,
	  P.description,
	  0 AS Chosen
  	FROM Product P

  	WHERE  is_deleted <> 1
  	AND 	P.Description LIKE @WhereClause

END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
