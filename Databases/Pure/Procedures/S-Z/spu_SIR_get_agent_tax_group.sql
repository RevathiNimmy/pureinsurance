SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_get_Agent_tax_group'
GO

CREATE PROCEDURE spu_SIR_get_agent_tax_group
	@AgentCnt int,
    	@InsuranceFileCnt int,
    	@AgentTaxGroupId int OUTPUT
AS


 
BEGIN
DECLARE	@EffectiveDate datetime 
DECLARE @RiskGroupId integer
 

 
SELECT @effectivedate = getdate()
SELECT @RiskGroupId = (SELECT RC.risk_group_id FROM Risk_Code RC
				JOIN insurance_file I ON I.risk_code_id = RC.risk_Code_id
				WHERE I.insurance_file_Cnt = @InsuranceFileCnt)
 
SELECT @AgentTaxGroupId =(SELECT ISNULL(AGR.tax_group_id,0)
				FROM Agent_Group_Rate AGR
 				WHERE AGR.party_cnt = @AgentCnt
				AND AGR.risk_group_id = @RiskGroupId
				AND AGR.effective_date = 
					(SELECT MAX(effective_date) from agent_group_rate
					WHERE party_cnt = @AgentCnt
					AND risk_group_id = @RiskGroupId
					AND effective_date <= @effectivedate)
			  )
 
 
END
GO