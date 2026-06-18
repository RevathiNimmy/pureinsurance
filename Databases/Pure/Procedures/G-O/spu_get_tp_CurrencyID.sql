SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_tp_CurrencyID'
GO

CREATE PROCEDURE spu_get_tp_CurrencyID  
    @ClaimID INT  
AS  
  
DECLARE @AgentUnderwriter varchar(1)  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
    Select  Currency_id,  
        coinsurance_treatment_id  
    from    Claim  
    where   Claim_id = @ClaimID  
ELSE  
    Select  Currency_id,  
        coinsurance_treatment_id  
    from    Claim  
    where   Claim_id = @ClaimID  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
