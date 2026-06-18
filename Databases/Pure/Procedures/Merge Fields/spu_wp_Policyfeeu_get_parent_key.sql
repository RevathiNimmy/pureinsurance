SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_Policyfeeu_get_parent_key'
GO

CREATE PROCEDURE spu_wp_Policyfeeu_get_parent_key
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
SELECT  @InsuranceFileCnt
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
