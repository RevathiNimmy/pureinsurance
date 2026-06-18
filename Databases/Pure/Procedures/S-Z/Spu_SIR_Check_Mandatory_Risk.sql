SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'Spu_SIR_Check_Mandatory_Risk'
GO

CREATE PROCEDURE Spu_SIR_Check_Mandatory_Risk
   @risk_cnt int

AS

	SELECT isnull(is_mandatory_risk,0) 
	FROM   risk
        WHERE risk_cnt = @risk_cnt
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
