SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_RiskInceptionDate'
GO

CREATE PROCEDURE spu_SIR_Get_RiskInceptionDate
	@risk_cnt int
AS
BEGIN
	SELECT r.inception_date
	FROM risk r 
	WHERE r.risk_cnt=@risk_cnt
END

GO				
