SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Peril_Link'
GO

CREATE PROCEDURE spu_CLM_Claim_Peril_Link  
    @ClaimId int,  
    @PartyCnt int,  
    @ClaimPerilId int,  
    @PerilPartyId int OUTPUT  
AS  
    DECLARE @ClaimPartyLinkId int  
  
    IF Not Exists(SELECT claim_party_link_id  
              FROM peril_party  
                 WHERE claim_party_link_id in  
                    (SELECT claim_party_link_id  
                     FROM claim_party_link  
                     WHERE claim_id = @claimid  
                     AND party_cnt = @partycnt)  
                    AND claim_peril_id = @claimperilid)  
  
       BEGIN  
  
            -- get claim party link id  
            SELECT @ClaimPartyLinkId = claim_party_link_id  
            FROM claim_party_link  
            WHERE claim_id = @ClaimId  
            AND party_cnt = @PartyCnt  
  
            -- then insert row into peril_party  
            INSERT INTO Peril_Party  
                    (  
                    claim_id,  
                    claim_peril_id,  
                    claim_party_link_id  
                    )  
            VALUES  
                    (  
                    @ClaimId,  
                    @ClaimPerilId,  
                    @ClaimPartyLinkId  
                    )  
        END  
  
IF @@ERROR <> 0  
    BEGIN  
        GOTO Error_Routine  
    END  
  
-- get the identity of the added row  
SELECT @PerilPartyId = @@IDENTITY  
RETURN  
Error_Routine:  
SELECT @PerilPartyId = 0  
RETURN  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
