SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GetTransactionsExist'
GO

CREATE PROCEDURE spu_GetTransactionsExist
	@TransactionsExist BIT OUTPUT
AS

SELECT @TransactionsExist = 0

SELECT @TransactionsExist = 1
WHERE EXISTS
(
	SELECT NULL
	FROM transdetail
)

GO