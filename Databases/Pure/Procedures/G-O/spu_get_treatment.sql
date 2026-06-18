SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_treatment'
GO

CREATE PROCEDURE spu_get_treatment  
    @ClaimID int  
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
    Select Description from Coinsurance_treatment where coinsurance_treatment_id IN  
        (Select coinsurance_treatment_id from Claim where Claim_id=@ClaimID)  
  
ELSE  
--UNDERWRITING  
    SELECT  CT.Description  
    FROM    Coinsurance_treatment CT,  
        Claim WC  
    WHERE   CT.coinsurance_treatment_id = WC.coinsurance_treatment_id  
    AND WC.Claim_id = @ClaimID  




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
