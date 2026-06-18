SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_partydet_for_party_type'
GO

CREATE PROCEDURE spu_partydet_for_party_type  
    @ClaimId integer,  
    @PartyTypeId integer  
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
    select  a.Party_Claim_id,a.Claim_Party_type_id,  
        a.Name, a.Address1, a.Address2, a.Address3, a.Address4, a.Postcode,  
        a.License_type ,License_type.description,  
        a.License_Number, a.Date_of_Birth, a.Sex,  
        a.party_status, driver_status.description,  
        a.Phone_Number, a.Fax_Number,  
        a.Reference_Number, a.Reg_Number  
    from    party_claim a left outer join License_type  
        on a.License_type = License_type.License_type_id,  
        party_claim b left outer join Driver_Status  
        on b.Party_Status = Driver_Status.driver_status_id,  
    Claim_Party_claim  
    where   a.party_claim_id = b.party_claim_id  
    and     a.party_claim_id = Claim_Party_claim.party_claim_id  
    and     a.Claim_Party_type_id = @PartyTypeId  
    and     Claim_id = @ClaimId  
ELSE  
--UNDERWRITING  
    SELECT  a.Party_Claim_id,  
            a.Claim_Party_type_id,  
            a.Name, a.Address1, a.Address2, a.Address3, a.Address4, a.Postcode,  
            a.License_type,  
            License_type.description,  
            a.License_Number,  
            a.Date_of_Birth,  
            a.Sex,  
            a.party_status,  
            driver_status.description,  
            a.Phone_Number,  
            a.Fax_Number,  
            a.Reference_Number,  
            a.Reg_Number  
    FROM    party_claim a LEFT OUTER JOIN License_type  
    ON      a.License_type = License_type.License_type_id,  
            party_claim b LEFT OUTER JOIN Driver_Status  
    ON      b.Party_Status = Driver_Status.driver_status_id,  
            Claim_Party_claim wcpc  
    WHERE   a.party_claim_id = b.party_claim_id  
    AND     a.party_claim_id = wcpc.party_claim_id  
    AND     a.Claim_Party_type_id = @PartyTypeId  
    AND     wcpc.Claim_id = @ClaimId  
 



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
