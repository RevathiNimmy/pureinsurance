SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_FinanceAmount'
GO

CREATE PROCEDURE spu_ACT_Get_FinanceAmount
    @nTransactionID INT  
	   
AS
    SELECT  amount FROM TransDetail WHERE transdetail_id = @nTransactionID  
GO
