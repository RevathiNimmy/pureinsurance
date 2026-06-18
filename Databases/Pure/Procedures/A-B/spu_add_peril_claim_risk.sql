SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_peril_claim_risk'
GO
CREATE PROCEDURE spu_add_peril_claim_risk  
    @PolicyID integer,  
    @RiskId integer,  
    @Claimid integer  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--*******************************************************************************************  
DECLARE @AgentUnderwriter varchar(1)  
DECLARE @claim_peril_id int  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF ISNULL(@AgentUnderwriter, ' ') = ' '  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
    INSERT INTO Claim_Peril (  
            Claim_id,  
            Peril_type_id,  
            Description)  
    (SELECT DISTINCT @Claimid,  
            peril.peril_type_id,  
            peril_type.Description  
    FROM    peril,  
            peril_Type  
    WHERE   peril.peril_type_id = peril_Type.peril_Type_id  
    --  and insurance_file_cnt = @PolicyID  
    AND     risk_cnt = @RiskId And ISNULL(peril.is_levy_tax,0)<>1)  
  
ELSE  
    -- underwriting  
    INSERT INTO claim_peril (  
            Claim_ID,  
            Peril_Type_ID,  
            Description,  
            Sum_Insured,  
            RI_Band,
			gis_screen_id)  
    SELECT  @Claimid,  
            p.peril_type_id,  
            pt.description,  
            sum(p.rating_sum_insured),  
            p.ri_band,
			pt.gis_screen_id  
    FROM    peril p  
    JOIN    peril_type pt     ON pt.peril_type_id = p.peril_type_id  
    JOIN    rating_section rs ON rs.rating_section_id = p.rating_section_id  
                             AND rs.risk_cnt = p.risk_cnt  
    WHERE   p.risk_cnt = @RiskID  
    AND     rs.original_flag = 0  and ISNULL(p.is_levy_tax,0)<>1
    GROUP BY  
            p.peril_type_id,  
            pt.description,  
            p.ri_band,
			pt.gis_screen_id    
  
 
UPDATE claim_peril  
SET base_claim_peril_id = claim_peril.claim_peril_id,  
     version_id = claim.version_id  
FROM claim_peril  
 INNER JOIN Claim ON  
  claim_peril.claim_id = claim.claim_id  
WHERE claim_peril.claim_id = @claimid
And Base_claim_peril_id IS NULL


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
