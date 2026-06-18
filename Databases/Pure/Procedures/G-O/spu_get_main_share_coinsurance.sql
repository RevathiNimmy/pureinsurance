SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_main_share_coinsurance'
GO

CREATE PROCEDURE spu_get_main_share_coinsurance  
    @PolicyID INT,  
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
  
    Select  sum(Sum_insured)  
    from    Peril p,  
        Insurance_File_Risk_Link ifr  
    where   ifr.Insurance_file_cnt = @policyID  
    and ifr.status_flag <> 'D'  
    and ifr.risk_cnt = p.risk_cnt  
  
ELSE  
  
    -- Get total reserve from for this claim  
    SELECT  SUM(r.initial_reserve + r.revised_reserve - r.paid_to_date)  
    FROM    Reserve r,  
            Claim_Peril cp  
    WHERE   cp.claim_id = @ClaimID  
    AND     cp.claim_peril_id = r.claim_peril_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
