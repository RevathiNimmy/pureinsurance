SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Add_Claim_Peril'
GO

CREATE PROCEDURE spu_Add_Claim_Peril  
    @ClaimId integer,  
    @PerilTypeId integer,  
    @RiskID integer,  
    @Description varchar(50)  
AS  
BEGIN  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--              JMK     23/05/2001  (UW only) - add full details for peril  
--    RVH 18/01/2005  Changed to use claimsbuilder flag rather than loss schedule  
--*******************************************************************************************  
DECLARE @OrigDescription varchar(50)  
DECLARE @AgentUnderwriter varchar(1)  
DECLARE @ClaimsBuilder varchar(1)  
DECLARE @NewDescription varchar(50)  
DECLARE @claim_peril_id int  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
SELECT  @ClaimsBuilder = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 12  
  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
BEGIN  
    select @OrigDescription = Description  
    from Peril_Type  
    where Peril_Type_Id = @PerilTypeId  

    UPDATE Claim 
    SET Last_modified_date = Getdate()
    WHERE Claim_id = @ClaimId	

insert into claim_Peril (claim_id,peril_type_id,description )  
    values (@ClaimId , @PerilTypeId, @OrigDescription )  
END  
ELSE  
  
BEGIN  
    IF ((not exists(SELECT Claim_ID FROM claim_peril WHERE Claim_ID = @ClaimId AND peril_type_id = @PerilTypeId)) or (@ClaimsBuilder = 1))  
    BEGIN  
        IF @Description <> ''  
        BEGIN  
            SELECT @NewDescription = @Description  
        END  
        ELSE  
        BEGIN  
            select @NewDescription = Description  
            from Peril_Type  
            where Peril_Type_Id = @PerilTypeId  
        END  
  
        INSERT INTO claim_peril  
        (  
         Claim_ID,  
         Peril_Type_ID,  
         Description,  
         Sum_Insured,  
         RI_Band  
        )  
        SELECT  @Claimid,  
             prl.peril_type_id,  
             @NewDescription,  
             sum(prl.rating_sum_insured),  
             prl.ri_band  
        FROM    peril prl,  
                peril_type prlt  
        WHERE   prl.peril_type_id = prlt.peril_type_id  
        AND  prl.risk_cnt = @RiskID  
        AND     prl.peril_type_id = @PerilTypeId  
        GROUP BY prl.peril_type_id, prlt.description, prl.ri_band  
    END  
END  
  
SELECT @claim_peril_id = @@IDENTITY  
  
UPDATE claim_peril  
SET base_claim_peril_id = @claim_peril_id,  
     version_id = claim.version_id  
FROM claim_peril  
 INNER JOIN Claim ON  
  claim_peril.claim_id = claim.claim_id  
WHERE claim_peril_id = @claim_peril_id  
  
-- return the new claim peril id
SELECT @claim_peril_id

END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
