SET QUOTED_IDENTIFIER OFF 
GO

Execute DDLDropProcedure 'spu_GetRiskStatusFlag'
GO

CREATE PROCEDURE spu_GetRiskStatusFlag
  @Risk_Cnt INT,
  @OriginalRiskCnt INT OUTPUT,
  @StatusFlag varchar(10) OUTPUT
AS

SELECT
        @OriginalRiskCnt = ifrl.original_risk_cnt,
        @StatusFlag = ifrl.status_flag
FROM risk
INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt =risk.risk_cnt
WHERE risk.risk_cnt = @Risk_Cnt

GO
SET QUOTED_IDENTIFIER OFF
GO
