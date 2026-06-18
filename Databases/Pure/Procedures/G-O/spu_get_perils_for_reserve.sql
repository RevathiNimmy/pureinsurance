SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_perils_for_reserve'
GO

CREATE PROCEDURE spu_get_perils_for_reserve  
    @claim_id int,  
    @Reserve_type_id int  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
DECLARE @AgentUnderwriter varchar(1)  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  

	SELECT 
		Reserve_type.Description, 
		Reserve.Reserve_id,  
		Reserve.Initial_reserve, 
		Reserve.Paid_to_date,  
		Reserve.Revised_reserve, 
		Reserve.Sum_insured,  
		Reserve.Average, 
		claim_Peril.claim_Peril_id,  
		claim_Peril.Description AS Peril_Desc,  
		Reserve.Revised_Reserve_Entered  
	FROM claim_Peril 
		INNER JOIN  Reserve ON 
			claim_Peril.claim_Peril_id = Reserve.claim_Peril_id 
		INNER JOIN  Reserve_type ON  
			Reserve.Reserve_type_id = Reserve_type.Reserve_type_id  
	WHERE (claim_Peril.Claim_id = @claim_id) 
	AND (Reserve.Reserve_type_id = @Reserve_type_id)  
	GROUP BY 
	Reserve_type.Description, 
	Reserve.Reserve_id,  
	Reserve.Initial_reserve, 
	Reserve.Paid_to_date,  
	Reserve.Revised_reserve, 
	Reserve.Sum_insured,  
	Reserve.Average, 
	claim_Peril.claim_Peril_id, 
	claim_Peril.Description,  
	Reserve.Revised_Reserve_Entered  

ELSE  
--UNDERWRITING  
    SELECT  rt.Description,  
            wr.Reserve_id,  
            wr.Initial_reserve,  
            wr.Paid_to_date,  
            wr.Revised_reserve,  
            wr.Sum_insured,  
            wr.Average,  
            wcp.claim_Peril_id,  
            wcp.Description AS Peril_Desc  
    FROM    claim_Peril wcp INNER JOIN Reserve wr ON  
            wcp.claim_Peril_id = wr.claim_Peril_id INNER JOIN Reserve_type rt ON  
            wr.Reserve_type_id = rt.Reserve_type_id  
    WHERE   wcp.Claim_id = @claim_id  
    AND     wr.Reserve_type_id = @Reserve_type_id  
    GROUP BY    rt.Description,  
                wr.Reserve_id,  
                wr.Initial_reserve,  
                wr.Paid_to_date,  
                wr.Revised_reserve,  
                wr.Sum_insured,  
                wr.Average,  
                wcp.claim_Peril_id,  
                wcp.Description  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
