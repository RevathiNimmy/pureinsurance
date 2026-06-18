SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_chk_data_defn_prty_exists'
GO

CREATE PROCEDURE spu_chk_data_defn_prty_exists  
    @type_id int,  
    @party_type_id int,  
    @Mode bit  
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
BEGIN  
    if @Mode=0  
        Select COUNT(Party_Type_Id) 
	from claim_party_claim,
		Party_claim,
		party_type,
		Claim  
        where party_type.party_type_id=Party_Claim.Claim_Party_type_id  
        and Party_claim.Party_Claim_id= claim_party_claim.Party_Claim_id  
        and claim_party_claim.Claim_Id = Claim.Claim_id  
        and Claim.Risk_type_id=@type_id  
        and Party_Type_Id = @party_type_id  
    Else  
        Select Count(distinct(party_claim_id)) from Peril_Party where claim_peril_id  
        IN(Select claim_peril_id from claim_peril where peril_type_id=@type_id) AND party_claim_id  
        IN(Select party_claim_id from party_claim where claim_party_type_id =@party_type_id)  
END  
ELSE  
BEGIN  
    IF @Mode = 0  
        SELECT  COUNT(pt.party_type_id)  
        FROM    claim_party_claim wcpc,  
                Party p,  
                party_type pt,  
                claim wc  
        WHERE   pt.party_type_id = p.party_type_id  
        AND     p.party_cnt = wcpc.party_claim_id  
        AND     wc.risk_type_id = @type_id  
        AND     pt.party_type_id = @party_type_id  
  
    ELSE IF @Mode = 1  
        SELECT  COUNT(DISTINCT(wpp.party_claim_id))  
        FROM    peril_party wpp  
        WHERE   claim_peril_id IN  
        (  
            SELECT claim_peril_id FROM claim_peril WHERE peril_type_id = @type_id  
        )  
        AND     wpp.party_claim_id IN  
        (  
            SELECT party_cnt FROM party WHERE party_type_id = @party_type_id  
        )  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
