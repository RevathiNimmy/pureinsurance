SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CheckIsMarketplacePolicy'
GO

CREATE PROCEDURE spu_CheckIsMarketplacePolicy @nInsuranceFileKey INT
AS
    SELECT is_marketplace_policy,
	CASE WHEN tmp.insurance_file_cnt is not null  THEN 1
	Else 0
	END RiskStatus
    FROM   insurance_file ifi
	LEFT JOIN (SELECT ifrl.insurance_file_cnt from risk rsk join insurance_file_risk_link ifrl on rsk.risk_cnt=ifrl.risk_cnt where risk_status_id =1) tmp
	ON tmp.insurance_file_cnt=ifi.insurance_file_cnt
	WHERE  ifi.insurance_file_cnt = @nInsuranceFileKey
GO  