
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_CLM_Get_Total_Payment'
GO


CREATE PROCEDURE spu_CLM_Get_Total_Payment
    @claimid int
AS


SELECT SUM(amount)
FROM payment 
where claim_id= @claimid

GO

