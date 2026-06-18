SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_Branch_Sel'
GO
 
CREATE  PROCEDURE spu_CashDeposit_Branch_Sel 
	@CashDeposit_ID INT 
AS 
BEGIN
	SELECT 
		SRC.Source_ID,
		SRC.Description,
		SRC.CODE,
		SRC.is_deleted 	
	FROM
		CashDeposit_Branch_Link CBL
		INNER JOIN Source SRC
			ON SRC.Source_ID=CBL.Branch_ID
	WHERE
		SRC.Is_Deleted<>1
		AND CBL.CashDeposit_ID=@CashDeposit_ID
END
 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
