SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
drop proc spu_wp_insurance_file_3
go

--DC190404 relaised RiskId should not be in parameters 
CREATE PROCEDURE spu_wp_insurance_file_3
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @this_premium MONEY OUTPUT,
    @lead_agent_percent FLOAT OUTPUT,
    @lead_agent_amount MONEY OUTPUT
AS
SET ROWCOUNT 1
SELECT  @this_premium = I.this_premium,
    @lead_agent_percent = A.agent_rate1
FROM    Insurance_File          I,
    Party               P,
    Agent_Group_Rate        A,
    Risk_Code           R
WHERE   I.insurance_file_cnt = @InsuranceFileCnt
AND A.party_cnt = I.lead_agent_cnt
AND A.risk_group_id = R.risk_group_id
AND R.risk_code_id = I.risk_code_id
AND A.effective_date <= I.cover_start_date
ORDER BY A.effective_date DESC
SET ROWCOUNT 0
SELECT @lead_agent_amount = (@this_premium * convert(decimal,@lead_agent_Percent)/100)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

