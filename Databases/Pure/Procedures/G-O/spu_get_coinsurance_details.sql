SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_coinsurance_details'
GO

CREATE PROCEDURE spu_get_coinsurance_details  
    @PolicyID Int,  
    @ClaimID Int,  
    @Mode Int,  
    @TransactionType varchar(10)  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
-- 1.00.0002    RWH     16/08/2001  Included Begin & End statements in last clause to prevent  
--                                  SELECT being carried out twice resulting in 2 record sets.  
-- 1.01.0001    RWH     30/08/2001  Remove INSERT statement as this is done in update proc.  
--*******************************************************************************************  
Declare  
    @partycnt int,  
    @partyname varchar(100),  
    @sharepercent numeric(12, 8),  
    @sharevalue numeric(19, 4),  
    @tmp_party_cnt int,  
    @BusinessType int,  
    @TotalReserve numeric(19, 4),  
    @AgentUnderwriter varchar(1),
    @BusinessTypeIdCoinsLead int=3 ,
    @BusinessTypeIdCoinsFollow int= 4 
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF ISNULL(@AgentUnderwriter, ' ') = ' '  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
    BEGIN  
    If @Mode=1  
        Begin  
            if not exists (Select Party.Party_cnt, Party.name, Claim_Party.Share, Claim_Party.Share_Value from  
                Claim_Party, Party where Claim_Party.Claim_id=@ClaimID AND Claim_Party.Party_id=  
                Party.Party_cnt AND insurer_type=0)  
            Begin  
  
                Declare claim_party_cursor CURSOR FAST_FORWARD FOR  
                    Select Party.Party_cnt, Party.name, Coi_Value.Share_percent, Coi_Value.Share_Premium from Party,  
                    Coi_Value where Coi_Value.insurance_file_cnt=@PolicyID AND  
                    Coi_Value.Party_cnt=Party.Party_cnt  
  
                Open claim_party_cursor  
                Fetch Next From claim_party_cursor  
                 Into @partycnt, @partyname, @sharepercent, @sharevalue  
                While @@FETCH_STATUS=0  
                BEGIN  
                 exec spu_insert_coinsurance @claimID, @partycnt, @sharepercent, @sharevalue  
                 Fetch Next From claim_party_cursor  
                         Into @partycnt, @partyname, @sharepercent, @sharevalue  
                END  
                Close claim_party_cursor  
                DeAllocate claim_party_cursor  
            End  
  
            Select Party.Party_cnt, Party.name, Claim_Party.Share, Claim_Party.Share_Value from Claim_Party,  
            Party where Claim_Party.Claim_id=@ClaimID AND  
            Claim_Party.Party_id=Party.Party_cnt  
            AND insurer_type=0  
        End  
    Else  
        Begin  
            Select Party.Party_cnt, Party.name, Claim_Party.Share, Claim_Party.Share_Value from Claim_Party,  
            Party where Claim_Party.Claim_id=@ClaimID AND  
            Claim_Party.Party_id=Party.Party_cnt  
            AND insurer_type=0  
        End  
    END  
ELSE  
--UNDERWRITING  
BEGIN  
    -- check to see if we have any coinsurance  
    SELECT @BusinessType = business_type_id  
    FROM   insurance_file  
    WHERE  insurance_file_cnt = @PolicyID  
  
    -- yeap we have coinsurance lead  
      IF @BusinessType =@BusinessTypeIdCoinsLead  or @BusinessType = @BusinessTypeIdCoinsFollow 
    BEGIN  
        -- Get total reserve from for this claim  
        SELECT  @TotalReserve =  SUM(r.initial_reserve + r.revised_reserve - r.paid_to_date)  
        FROM    Reserve r,  
                Claim_Peril cp  
        WHERE   cp.claim_id = @ClaimID  
        AND     cp.claim_peril_id = r.claim_peril_id  
  
        IF @TransactionType = 'C_CO'  
        BEGIN  
            SELECT  @tmp_party_cnt = wcp.Party_id  
            FROM    Claim_Party wcp  
            WHERE   wcp.Claim_id = @ClaimID  
            AND     wcp.insurer_type = 0  
  
            IF ISNULL(@tmp_party_cnt, 0) = 0  
                -- Get coinsurance share  
                SELECT  Party.Party_cnt,  
                        RTRIM(Party.name),  
            Coi_Value.Share_percent,  
                        (@TotalReserve / 100) * coi_value.share_percent Share_Value  
                FROM    Party  
                JOIN    Party_Insurer pin ON pin.party_cnt = party.party_cnt  
                JOIN    Coi_Value ON Coi_Value.Party_cnt = Party.Party_cnt  
                WHERE   Coi_Value.insurance_file_cnt = @PolicyID  
                AND     ISNULL(pin.is_retained, 0) <> 1  
        END  
        ELSE -- not open claim  
        BEGIN  
            -- recalculating share values just in case reserves has been changed  
            UPDATE  Claim_Party  
            SET     share_value = (@TotalReserve / 100) * share  
            WHERE   claim_id = @claimID  
            AND     insurer_type = 0  
  
            -- get coinsurance details  
            SELECT  pty.Party_cnt,  
                    pty.name,  
                    wcp.Share,  
                    wcp.Share_Value  
            FROM    Claim_Party wcp  
            JOIN    Party pty ON wcp.Party_id = pty.Party_cnt  
            WHERE   wcp.Claim_id = @ClaimID  
            AND     wcp.insurer_type = 0  
        END  
    END  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
