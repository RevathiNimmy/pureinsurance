SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_all_risk_status_sel'
GO

CREATE PROCEDURE spu_all_risk_status_sel
@insurance_file_cnt INT
AS
SELECT risk_cnt FROM Risk WHERE risk_cnt in (SELECT risk_cnt FROM insurance_file_risk_link WHERE insurance_file_cnt=@insurance_file_cnt)
and risk_status_id in (4,8) and is_risk_selected=1

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 