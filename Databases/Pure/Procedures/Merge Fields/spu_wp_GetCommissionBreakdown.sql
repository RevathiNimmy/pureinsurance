SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_GetCommissionBreakdown'
GO

--will return the commission type and percentage for this policy for lead agent  
--JW 20170719 Amended to return RIsk Type to enable commissions by risk type  
CREATE   PROCEDURE spu_wp_GetCommissionBreakdown  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
DECLARE  
    @AgentCommissionPercentage FLOAT,  
    @AgentCommissionValue FLOAT,  
    @CommissionType VARCHAR(50),  
@CommissionTaxDescription VARCHAR(100),  
@AgentCommissionTaxValue FLOAT,  
@AgentCommissionRiskType VARCHAR(50) 
DECLARE AgentCommissionBreakdown_Cursor SCROLL CURSOR FOR  
    SELECT  
        ac.commission_percentage,  
        ac.commission_value,  
        cb.description,  
  ac.tax_amount,  
  tg.description,  
  rt.code  
    FROM Agent_Commission ac  
    JOIN Commission_Band cb  
        ON ac.commission_band_id = cb.commission_band_id  
JOIN Tax_Group tg  
  ON tg.tax_group_id = ac.tax_group_id  
  JOIN Risk_Type rt 
ON ac.risk_type_id = rt.risk_type_id 
    WHERE insurance_file_cnt = @InsuranceFileCnt  
    AND ac.is_lead_agent = 1  

OPEN AgentCommissionBreakdown_Cursor  
FETCH ABSOLUTE @Instance1 FROM AgentCommissionBreakdown_Cursor INTO  
    @AgentCommissionPercentage,  
    @AgentCommissionValue,  
    @CommissionType,  
@AgentCommissionTaxValue,  
@CommissionTaxDescription,  
@AgentCommissionRiskType  
CLOSE AgentCommissionBreakdown_Cursor  
DEALLOCATE AgentCommissionBreakdown_Cursor  
SELECT  
    @AgentCommissionPercentage 'Agent_Commission_Percentage',  
    @AgentCommissionValue  'Agent_Commission_Value',  
    @CommissionType 'Commission_Type',  
@AgentCommissionTaxValue 'Agent_Commission_Tax_Value',  
@CommissionTaxDescription 'Commission_Tax_Description',  
@AgentCommissionRiskType 'Agent_Commission_Risk_Type'   
GO
