SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_check_deletion_for_peril'
GO

CREATE PROCEDURE spu_check_deletion_for_peril  
    @ClaimPerilId integer  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--*******************************************************************************************  
Declare @RecCount integer  
declare @CanDelete bit  
  
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
        select  @CanDelete = 1  
  
        -- Check if Peril exists in Payment  
        select @RecCount = count(*)  
        from Claim_Payment  
        where Claim_Peril_id = @ClaimPerilId  
        and amount is not null and amount <> 0  
  
        -- If Peril is present in Payment then Peril cannot be deleted  
        if @RecCount >0  
        begin  
            select  @CanDelete = 0  
            select  @CanDelete  
            return  
        end  
  
        -- Check if Peril exists in Receipt  
        select @RecCount = count(*)  
        from Claim_Receipt  
        where Claim_Peril_id = @ClaimPerilId  
  
        -- If Peril is present in Receipt then Peril cannot be deleted  
        if @RecCount >0  
        begin  
            select  @CanDelete = 0  
            select  @CanDelete  
            return  
        end  
  
        -- Check if Peril exists in Reserve  
        select @RecCount = count(*)  
        from Reserve  
        where Claim_Peril_id = @ClaimPerilId  
        and initial_reserve is not null and initial_reserve <> 0  
        and paid_to_date is not null and paid_to_date <> 0  
        and revised_reserve is not null and revised_reserve <> 0  
  
        -- If Peril is present in Reserve then Peril cannot be deleted  
        if @RecCount >0  
        begin  
            select  @CanDelete = 0  
            select  @CanDelete  
            return  
        end  
  
        -- Check if Peril exists in Recovery  
        select @RecCount = count(*)  
        from Recovery  
        where Claim_Peril_id = @ClaimPerilId  
  
        -- If Peril is present in Recovery then Peril cannot be deleted  
        if @RecCount >0  
        begin  
            select  @CanDelete = 0  
            select  @CanDelete  
            return  
        end  
  
        -- Check if Peril exists in Peril_Party  
        select @RecCount = count(*)  
        from Peril_Party  
        where Claim_Peril_id = @ClaimPerilId  
  
        -- If Peril is present in Peril_Party then Peril cannot be deleted  
        if @RecCount >0  
        begin  
            select  @CanDelete = 0  
            select  @CanDelete  
            return  
        end  
  
        select  @CanDelete  
    END  
ELSE    -- underwriting  
    BEGIN  
        SELECT  @CanDelete = 1  
  
        -- Check if Peril exists in Payment  
        SELECT  @RecCount = COUNT(*)  
        FROM    Claim_Payment  
        WHERE   Claim_Peril_id = @ClaimPerilId  
  
        -- If Peril is present in Payment then Peril cannot be deleted  
        IF @RecCount > 0  
        BEGIN  
            SELECT  @CanDelete = 0  
            SELECT  @CanDelete  
            RETURN  
        END  
  
        -- Check if Peril exists in Receipt  
        SELECT  @RecCount = COUNT(*)  
        FROM    Claim_Receipt  
        WHERE   Claim_Peril_id = @ClaimPerilId  
  
        -- If Peril is present in Receipt then Peril cannot be deleted  
        IF @RecCount > 0  
        BEGIN  
            SELECT  @CanDelete = 0  
            SELECT  @CanDelete  
            RETURN  
        END  
  
        -- Check if Peril exists in Reserve  
        SELECT  @RecCount = COUNT(*)  
        FROM    Reserve  
        WHERE   Claim_Peril_id = @ClaimPerilId  
  
        -- If Peril is present in Reserve then Peril cannot be deleted  
        IF @RecCount > 0  
        BEGIN  
            SELECT  @CanDelete = 0  
            SELECT  @CanDelete  
            RETURN  
        END  
  
        -- Check if Peril exists in Recovery  
        SELECT  @RecCount = COUNT(*)  
        FROM    Recovery  
        WHERE   Claim_Peril_id = @ClaimPerilId  
  
        -- If Peril is present in Recovery then Peril cannot be deleted  
        IF @RecCount > 0  
        BEGIN  
            SELECT  @CanDelete = 0  
            SELECT  @CanDelete  
            RETURN  
        END  
  
        -- Check if Peril exists in Peril_Party  
        SELECT @RecCount = COUNT(*)  
        FROM    Peril_Party  
        WHERE   Claim_Peril_id = @ClaimPerilId  
  
        -- If Peril is present in Peril_Party then Peril cannot be deleted  
        IF @RecCount > 0  
        BEGIN  
            SELECT  @CanDelete = 0  
            SELECT  @CanDelete  
            RETURN  
        END  
  
        SELECT  @CanDelete  
  
    END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
