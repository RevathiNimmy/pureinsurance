SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claims_recovery_coins_saa'
GO

CREATE PROCEDURE spu_claims_recovery_coins_saa  
    @peril_id int,  
    @is_salvage tinyint  
AS  
  
    Declare @is_share tinyint  
  
    -- Do we share taxes with reinsurers  
    SELECT  @is_share = is_share_with_re_insurers  
    FROM    risk_type rt  
    JOIN    risk r  
            ON r.risk_type_id = rt.risk_type_id  
    JOIN    claim wc  
            ON wc.risk_type_id = r.risk_cnt  
    JOIN    claim_peril wcp  
            ON wcp.claim_id = wc.claim_id  
    WHERE   wcp.claim_peril_id = @peril_id  
  
    -- Get coinsurance  
    SELECT  r.recovery_id,  
            p.Party_cnt,  
            p.name,  
            cp.Share,  
  
           (Select  Sum(wcpi.this_payment * wcpi.payment_loss_xrate)  
            From    claim_payment  
  INNER JOIN claim_payment_item wcpi ON  
   wcpi.claim_payment_id = claim_payment.claim_payment_id  
  
            Where   claim_peril_id = wcp.claim_peril_id  
                And recovery_id = r.recovery_id  
                And party_cnt = cp.party_id) to_date,  
  
            @is_share is_tax_shared  
    FROM    claim_peril wcp  
    JOIN    claim_party cp  
            ON  cp.claim_id = wcp.claim_id  
    JOIN    Party p  
            ON  p.Party_cnt = cp.Party_id  
    JOIN    recovery r  
            ON  r.claim_peril_id = wcp.claim_peril_id  
    JOIN    recovery_type rt  
            ON  rt.recovery_type_id = r.recovery_type_id  
    WHERE   wcp.claim_peril_id = @peril_id  
        AND rt.is_salvage = @is_salvage  
        AND cp.insurer_type = 0  
    ORDER BY  
            r.recovery_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
