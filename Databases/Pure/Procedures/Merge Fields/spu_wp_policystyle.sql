SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_policystyle'
GO


CREATE PROCEDURE spu_wp_policystyle
	@PartyCnt INT,
	@InsuranceFileCnt INT,
	@RiskId INT,
	@ClaimCnt INT,
	@DocumentRef VARCHAR(25),
	@Instance1 INT,
	@Instance2 INT,
	@Instance3 INT
AS


SELECT ps.code, ps.description
FROM policy_style ps
JOIN insurance_file i
ON i.policy_style_id = ps.policy_style_id
WHERE i.insurance_file_cnt = @InsuranceFileCnt

GO