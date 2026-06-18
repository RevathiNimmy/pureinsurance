SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Coinsurance_Details_Update'
GO

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurer Breakdown.doc) - (6.2.1)
CREATE PROCEDURE spu_SAM_Coinsurance_Details_Update
    @claim_id int,
    @transaction_type varchar(10)
AS
DECLARE
    @businesstype int,
    @totalreserve numeric(19, 4) ,
    @insurance_file_cnt int
BEGIN
    -- get insurance_file_cnt
    SELECT @insurance_file_cnt=policy_id
    FROM claim
    WHERE claim_id=@claim_id

    -- check to see if we have any coinsurance
    SELECT @BusinessType = business_type_id
    FROM   insurance_file
    WHERE  insurance_file_cnt = @insurance_file_cnt

    DECLARE @businessType_CoinsuranceLead int
    SET @businessType_CoinsuranceLead = 3

    -- if business type is "coinsurance lead"
    IF @businessType = @businessType_CoinsuranceLead
    BEGIN
        -- Get total reserve from for this claim
        SELECT  @TotalReserve =  SUM(r.initial_reserve + r.revised_reserve - r.paid_to_date)
        FROM    reserve r,
                claim_peril cp
        WHERE   cp.claim_id = @claim_id
            AND cp.claim_peril_id = r.claim_peril_id

          -- if this is "Open Claim"
        IF @transaction_type = 'C_CO'
        BEGIN
            -- check if there are any coinsurers already in
	   	    -- the claim party table
  	    	IF NOT EXISTS(SELECT wcp.Party_id
		                  FROM   Claim_Party wcp
	              	      WHERE  wcp.Claim_id = @claim_id
		                     AND wcp.insurer_type = 0)
	    	BEGIN
		        -- create the coinsurance entries
			    INSERT INTO claim_party(claim_id, party_id, share, share_value, insurer_type)
                    SELECT  @claim_id,
				            party.party_cnt,
		                    coi_Value.share_percent,
	                        (@TotalReserve / 100) * coi_value.share_percent Share_Value,
				            0
	                FROM    Party
	                	INNER JOIN Party_Insurer pin ON
					               pin.party_cnt = party.party_cnt
		                INNER JOIN Coi_Value ON
					               Coi_Value.Party_cnt = Party.Party_cnt
                    WHERE   Coi_Value.insurance_file_cnt = @insurance_file_cnt
	                    AND ISNULL(pin.is_retained, 0) <> 1
            END
			ELSE
			BEGIN
				UPDATE  Claim_Party
				SET     share_value = (@totalreserve / 100) * share
				WHERE   claim_id = @claim_id
                AND insurer_type = 0
			END
    	END
        ELSE -- not open claim
        BEGIN
            -- recalculate the share values on the coinsurance entries
            UPDATE  Claim_Party
            SET     share_value = (@totalreserve / 100) * share
            WHERE   claim_id = @claim_id
                AND insurer_type = 0
        END
    END
END
--End (Prakash C Varghese) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurer Breakdown.doc) - (6.2.1)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
