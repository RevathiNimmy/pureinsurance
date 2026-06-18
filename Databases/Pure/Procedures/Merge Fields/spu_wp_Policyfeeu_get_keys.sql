SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_Policyfeeu_get_keys'
GO

CREATE PROCEDURE spu_wp_Policyfeeu_get_keys
@PartyCnt INT,
@InsuranceFileCnt INT,
@RiskId INT = NULL,
@ClaimCnt INT,
@DocumentRef VARCHAR(25),
@Instance1 INT,
@Instance2 INT,
@Instance3 INT
AS

BEGIN
SELECT  *  from policy_fee_u
WHERE insurance_file_cnt =@InsuranceFileCnt
END 



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
