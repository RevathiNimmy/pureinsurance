SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_claim_current_reserve'
GO

CREATE PROCEDURE spu_get_claim_current_reserve  
    @claim_id int  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
-- 1.00.0002    AJM     25/07/2001  Get total reserve for reinsurance screen less coinsurer share value.  
--  
--*******************************************************************************************  
DECLARE @AgentUnderwriter varchar(1)  
DECLARE @TotalCoinsurerPercent money  
DECLARE @TotalReserve money  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
    Select sum(initial_reserve + revised_reserve - paid_to_date)  
    from    Reserve r,  
        Claim_Peril cp  
    where   cp.claim_id = @claim_id  
    and cp.claim_peril_id = r.claim_peril_id  
  
ELSE  
  
    SELECT  @TotalReserve = SUM(wr.initial_reserve + wr.revised_reserve - wr.paid_to_date )  
    from        Reserve wr,  
            Claim_Peril wcp  
    WHERE   wcp.claim_id = @claim_id  
    AND     wcp.claim_peril_id = wr.claim_peril_id  
  
    IF      @TotalReserve IS NULL  
            SELECT  @TotalReserve = 0  
  
--AJM (24/07/2001) - Get total reserve less coinsurer share  
    SELECT  @TotalCoinsurerPercent = SUM(share)  
    FROM        claim_party  
    WHERE   insurer_type = 0  
    AND     claim_id = @claim_id  
    AND     party_id NOT IN  
            (  
            SELECT party_cnt FROM party WHERE shortname = 'RETAINED'  
            )  
    IF      @TotalCoinsurerPercent IS NULL  
            SELECT  @TotalCoinsurerPercent = 0  
  
    IF @TotalCoinsurerPercent > 0  
            SELECT  @TotalReserve = (@TotalReserve * @TotalCoinsurerPercent) / 100  
  
    SELECT  @TotalReserve  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
