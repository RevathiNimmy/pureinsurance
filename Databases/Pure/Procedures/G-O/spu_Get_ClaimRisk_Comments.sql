SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_ClaimRisk_Comments'
GO

CREATE PROCEDURE spu_Get_ClaimRisk_Comments  
    @ClaimId integer,  
    @RiskTypeId integer  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
Declare @Count integer  
  
DECLARE @AgentUnderwriter varchar(1)  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
    BEGIN  
        select @Count = count(*) from Claim_Risk  
                where Claim_Id = @ClaimId  
                and Risk_type_Id = @RiskTypeId  
        if @count > 0  
        begin  
         select comments,description  
         from Claim_Risk  
         where Claim_Id = @ClaimId  
         and Risk_type_Id = @RiskTypeId  
        end  
        else  
        begin  
         select comments,description  
         from Claim  
         where Claim_Id = @ClaimId  
        end  
    END  
ELSE  
    SELECT  comments,  
            description  
    FROM    Claim_Risk  
    WHERE   Claim_Id = @ClaimId  
    AND     Risk_type_Id = @RiskTypeId  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
