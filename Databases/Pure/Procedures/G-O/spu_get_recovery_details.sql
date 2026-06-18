SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_recovery_details'
GO

CREATE PROCEDURE spu_get_recovery_details  
    @perilid int,  
    @claimid int,  
    @type int  
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
 Select  
  sum(initial_reserve),  
  sum(revised_reserve),  
  sum(received_to_date),
  sum(Tax_Amount) as ReceivedToDateTax  
 from Recovery  
 where recovery_type_id IN  
        (Select recovery_type_id  
  from recovery_type  
  where is_salvage=@type)  
 AND claim_peril_id=@perilid  
  
ELSE  
    SELECT  SUM(initial_reserve) * -1,  
            SUM(revised_reserve) * -1,  
            SUM(received_to_date) * -1  ,
	    sum(Tax_Amount) * -1 as ReceivedToDateTax
    FROM    Recovery  
    WHERE   recovery_type_id IN  
            (  
                SELECT recovery_type_id FROM recovery_type where is_salvage = @type  
            )  
    AND claim_peril_id = @perilid  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
