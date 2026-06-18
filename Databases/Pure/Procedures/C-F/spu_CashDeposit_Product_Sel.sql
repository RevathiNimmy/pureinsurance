SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_Product_Sel'
GO
CREATE  PROCEDURE spu_CashDeposit_Product_Sel 
	@CashDeposit_ID INT 
AS 
BEGIN
	SELECT 
		PDT.Product_ID,
		PDT.Description,
		PDT.CODE,
		PDT.is_deleted 	

	FROM
		CashDeposit_Product_Link CPL
		INNER JOIN Product PDT
			ON PDT.Product_ID=CPL.Product_ID
	WHERE
		PDT.Is_Deleted<>1
		AND CPL.CashDeposit_ID=@CashDeposit_ID
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
