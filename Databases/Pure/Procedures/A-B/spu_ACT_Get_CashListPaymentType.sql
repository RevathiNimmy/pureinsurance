SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_CashListPaymentType'
GO

CREATE PROCEDURE spu_ACT_Get_CashListPaymentType
	@transdetail_id int
AS
	SELECT CLT.code
	FROM 
		CashListItem CLI
		JOIN CashList CL
		ON CLI.cashlist_id = CL.cashlist_id
		JOIN CashListType CLT
		ON CL.cashlisttype_id = CLT.cashlisttype_id
        WHERE CLI.transdetail_id = @transdetail_id

GO