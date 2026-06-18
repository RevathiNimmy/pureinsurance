EXECUTE DDLDropProcedure 'spu_PF_GetSGPolicyDetails'
GO
--PN12594 add business code
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_PF_GetSGPolicyDetails
    @InsuranceFileCnt int,
    @RiskGroupDesc varchar(255) OUTPUT,
    @InsurerDesc varchar(255) OUTPUT
AS BEGIN

SELECT 	@RiskGroupDesc = rsk.Description,
	@InsurerDesc = par.Name
FROM	risk_group as rsk
JOIN	risk_code as rc
ON	rc.risk_group_id = rsk.risk_group_id
JOIN	insurance_file as ins
ON	ins.risk_code_id = rc.risk_code_id
JOIN	party as par
ON	par.party_cnt = ins.lead_insurer_cnt
WHERE	ins.insurance_file_cnt = @InsuranceFileCnt

END

GO