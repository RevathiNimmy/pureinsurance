SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_sir_is_risk_a_copy'
GO

CREATE PROCEDURE spu_sir_is_risk_a_copy

	@risk_cnt int 

AS 


BEGIN

	DECLARE @NoOfPerils int
	DECLARE @NoOfRatings int

	SELECT @NoOfPerils = Count(*) FROM peril WHERE risk_cnt = @risk_cnt
	SELECT @NoOfRatings = Count(*) FROM rating_section WHERE risk_cnt = @risk_cnt

	SELECT @NoOfPerils as PerilCnt, @NoOfRatings as RatingsCnt

END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
