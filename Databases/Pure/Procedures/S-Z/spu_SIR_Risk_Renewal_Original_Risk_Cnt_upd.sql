SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Risk_Renewal_Original_Risk_Cnt_upd'
GO


CREATE PROCEDURE spu_SIR_Risk_Renewal_Original_Risk_Cnt_upd
(
    @risk_cnt                   int,
    @original_risk_cnt   	int
)
AS 

UPDATE 
    insurance_file_risk_link
SET
    original_risk_cnt = @original_risk_cnt
WHERE
    risk_cnt = @risk_cnt



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
