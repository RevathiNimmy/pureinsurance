SET quoted_identifier  off

GO

SET ansi_nulls  ON

GO

EXECUTE ddldropprocedure
   'spu_Get_Peril_For_claim_Risk'

GO

CREATE PROCEDURE spu_get_peril_for_claim_risk
                @SiriusProduct VARCHAR(1),
                @ClaimId       INTEGER,
                @RiskTypeId    INTEGER
AS
  DECLARE  @RiskDescription VARCHAR(255)
  
  IF @SiriusProduct = 'A'
    BEGIN
    
      SELECT @RiskDescription = description
      FROM   risk_code
      WHERE  risk_code_id = @RiskTypeId
                            
      CREATE TABLE #claim_peril_details (
        claimid               INT,
        claimperilid          INT,
        riskdescription       VARCHAR(255),
        periltypedesc         VARCHAR(255),
        suminsured            NUMERIC(19,4),
        currentreserve        NUMERIC(19,4),
        revisedreserveentered INT,
        paidtodate            NUMERIC(19,4),
        policycurrency        VARCHAR(10) NULL,
        losscurrency          VARCHAR(10) NULL,
  	peril_type_id 	      INT NULL)
      
      INSERT INTO #claim_peril_details
                 (claimid,
                  claimperilid,
                  riskdescription,
                  periltypedesc,
                  suminsured,
                  currentreserve,
                  revisedreserveentered,
                  paidtodate,
                  policycurrency,
                  losscurrency,
                  peril_type_id)
      SELECT claim.claim_id,
             claim_peril.claim_peril_id,
             @RiskDescription,
             peril_type.description,
             isnull(reserve.sum_insured,0),
             isnull(reserve.initial_reserve,0) - isnull(reserve.paid_to_date,0),
             reserve.revised_reserve_entered,
             reserve.paid_to_date,
             pc.code,
             lc.code,
      	     claim_peril.peril_type_id             
      FROM   reserve
             RIGHT OUTER JOIN claim_peril
               ON reserve.claim_peril_id = claim_peril.claim_peril_id
                  AND reserve.reserve_type_id IN (SELECT reserve_type_id
                                                  FROM   reserve_type
                                                  WHERE  include_in_total = 1),
             claim,
             peril_type,
             currency lc,
             currency pc,
             insurance_file ifi
      WHERE  claim_peril.claim_id = claim.claim_id
             AND claim.policy_id = ifi.insurance_file_cnt
             AND claim.currency_id = lc.currency_id
             AND ifi.currency_id = pc.currency_id
             AND claim_peril.peril_type_id = peril_type.peril_type_id
             AND claim.claim_id = @ClaimId
             AND (reserve.revised_reserve = 0
                   OR reserve.revised_reserve IS NULL)
             AND (reserve.revised_reserve_entered = 0
                   OR reserve.revised_reserve_entered IS NULL)
             AND reserve.initial_reserve <> 0
                                            
      INSERT INTO #claim_peril_details
                 (claimid,
                  claimperilid,
                  riskdescription,
                  periltypedesc,
                  suminsured,
                  currentreserve,
                  revisedreserveentered,
                  paidtodate,
                  policycurrency,
                  losscurrency,
                  peril_type_id)
                 
      SELECT claim.claim_id,
             claim_peril.claim_peril_id,
             @RiskDescription,
             peril_type.description,
             0,
             ((isnull(recovery.initial_reserve,0) + isnull(recovery.revised_reserve,0)) - isnull(recovery.received_to_date,0)) * -1,
             0,
             0,
             pc.code,
             lc.code,
  	     claim_peril.peril_type_id             
      FROM   recovery
             RIGHT OUTER JOIN claim_peril
               ON recovery.claim_peril_id = claim_peril.claim_peril_id,
             claim,
             peril_type,
             currency lc,
             currency pc,
             insurance_file ifi
      WHERE  claim_peril.claim_id = claim.claim_id
             AND claim.policy_id = ifi.insurance_file_cnt
             AND claim.currency_id = lc.currency_id
             AND ifi.currency_id = pc.currency_id
             AND claim_peril.peril_type_id = peril_type.peril_type_id
             AND claim.claim_id = @ClaimId
                                  
      INSERT INTO #claim_peril_details
                 (claimid,
                  claimperilid,
                  riskdescription,
                  periltypedesc,
                  suminsured,
                  currentreserve,
                  revisedreserveentered,
                  paidtodate,
                  policycurrency,
                  losscurrency,
                  peril_type_id)
                 
      SELECT claim.claim_id,
             claim_peril.claim_peril_id,
             @RiskDescription,
             peril_type.description,
             isnull(reserve.sum_insured,0),
             isnull(reserve.initial_reserve,0) + isnull(reserve.revised_reserve,0) - isnull(reserve.paid_to_date,0),
             reserve.revised_reserve_entered,
             reserve.paid_to_date,
             pc.code,
             lc.code,
             claim_peril.peril_type_id
      FROM   reserve
             RIGHT OUTER JOIN claim_peril
               ON reserve.claim_peril_id = claim_peril.claim_peril_id
                  AND reserve.reserve_type_id IN (SELECT reserve_type_id
                                                  FROM   reserve_type
                                                  WHERE  include_in_total = 1),
             claim,
             peril_type,
             insurance_file ifi,
             currency lc,
             currency pc
      WHERE  claim_peril.claim_id = claim.claim_id
             AND claim.policy_id = ifi.insurance_file_cnt
             AND claim.currency_id = lc.currency_id
             AND ifi.currency_id = pc.currency_id
             AND claim_peril.peril_type_id = peril_type.peril_type_id
             AND claim.claim_id = @ClaimId
             AND reserve.revised_reserve <> 0
      
      --and (reserve.initial_reserve = 0 OR reserve.initial_reserve IS NULL)
      INSERT INTO #claim_peril_details
                 (claimid,
                  claimperilid,
                  riskdescription,
                  periltypedesc,
                  suminsured,
                  currentreserve,
                  revisedreserveentered,
                  paidtodate,
                  policycurrency,
                  losscurrency,
                  peril_type_id)
      SELECT claim.claim_id,
             claim_peril.claim_peril_id,
             @RiskDescription,
             peril_type.description,
             0,
             0,
             0,
             0,
             pc.code,
             lc.code,
             claim_peril.peril_type_id
      FROM   reserve
             RIGHT OUTER JOIN claim_peril
               ON reserve.claim_peril_id = claim_peril.claim_peril_id
                  AND reserve.reserve_type_id IN (SELECT reserve_type_id
                                                  FROM   reserve_type
                                                  WHERE  include_in_total = 1),
             claim,
             peril_type,
             insurance_file ifi,
             currency lc,
             currency pc
      WHERE  claim_peril.claim_id = claim.claim_id
             AND claim.policy_id = ifi.insurance_file_cnt
             AND claim.currency_id = lc.currency_id
             AND ifi.currency_id = pc.currency_id
             AND claim_peril.peril_type_id = peril_type.peril_type_id
             AND claim.claim_id = @ClaimId
             AND (reserve.revised_reserve = 0
                   OR reserve.revised_reserve IS NULL)
             AND (reserve.initial_reserve = 0
                   OR reserve.initial_reserve IS NULL)
                 
      INSERT INTO #claim_peril_details
                 (claimid,
                  claimperilid,
                  riskdescription,
                  periltypedesc,
                  suminsured,
                  currentreserve,
                  revisedreserveentered,
                  paidtodate,
                  policycurrency,
                  losscurrency,
                  peril_type_id)
      SELECT cp.claim_id,
             cp.claim_peril_id,
             @riskDescription,
             pt.description,
             0,
             0,
             0,
             0,
             pc.code,
             lc.code,
             cp.peril_type_id
      FROM   claim c
             JOIN claim_peril cp
               ON c.claim_id = cp.claim_id
             JOIN peril_type pt
               ON pt.peril_type_id = cp.peril_type_id
             JOIN insurance_file ifi
               ON c.policy_id = ifi.insurance_file_cnt
             JOIN currency pc
               ON ifi.currency_id = pc.currency_id
             JOIN currency lc
               ON c.currency_id = lc.currency_id
      WHERE  cp.claim_id = @claimid
             AND NOT EXISTS (SELECT *
                             FROM   reserve
                             WHERE  claim_peril_id = cp.claim_peril_id)
                            
      -- Extract the data
      SET nocount  off
      
      SELECT   claimid,
               claimperilid,
               riskdescription,
               periltypedesc,
               SUM(suminsured) sum_insured,
               SUM(currentreserve) current_reserve,
               MAX(revisedreserveentered) revised_entered,
               SUM(paidtodate) paid_to_date,
               policycurrency,
               losscurrency,
               peril_type_id
      FROM     #claim_peril_details
      GROUP BY claimid,claimperilid,peril_type_id,periltypedesc,riskdescription,
               policycurrency,losscurrency
               
      SET nocount  ON
      
      DELETE FROM #claim_peril_details
      
      DROP TABLE #claim_peril_details
      
      SET nocount  off
      
    END
    
  ELSE
  
    -- UNDERWRITING
    BEGIN
    
      SELECT   clmp.claim_id,
               clmp.claim_peril_id,
               clmp.description,
               prlt.description,
               clmp.sum_insured,
               SUM(rsv.initial_reserve + rsv.revised_reserve - rsv.paid_to_date),
               prlt.gis_screen_id,
               clmp.base_claim_peril_id,
               prlt.loss_schedule_type_id,
               prlt.peril_type_id,
               pc.code,
               lc.code,
               SUM(rsv.initial_reserve + rsv.revised_reserve) - ISNull(salvage.received_to_date,0) AS incurred,
               thirdparty.received_to_date AS tp_recovery_received_to_date,
               salvage.received_to_date AS salvage_recovery_received_to_date,
               SUM(rsv.paid_to_date)
               
      FROM     claim_peril clmp
               
               LEFT JOIN (SELECT   claim_peril_id,
                                   SUM(received_to_date) AS received_to_date
                          FROM     recovery
                          WHERE    recovery_type_id IN (SELECT recovery_type_id
                                                        FROM   recovery_type
                                                        WHERE  is_salvage = 1)
                          GROUP BY claim_peril_id) salvage
                 ON clmp.claim_peril_id = salvage.claim_peril_id
                                          
               LEFT JOIN (SELECT   claim_peril_id,
                                   SUM(received_to_date) AS received_to_date
                          FROM     recovery
                          WHERE    recovery_type_id IN (SELECT recovery_type_id
                                                        FROM   recovery_type
                                                        WHERE  is_salvage = 0)
                          GROUP BY claim_peril_id) thirdparty
                 ON clmp.claim_peril_id = thirdparty.claim_peril_id
                                          
               INNER JOIN peril_type prlt
                 ON clmp.peril_type_id = prlt.peril_type_id
                                         
               LEFT JOIN reserve rsv
                 ON clmp.claim_peril_id = rsv.claim_peril_id
                                          
               INNER JOIN claim clw
                 ON clw.claim_id = clmp.claim_id
                                   
               INNER JOIN currency lc
                 ON clw.currency_id = lc.currency_id
                                      
               INNER JOIN insurance_file ifi
                 ON clw.policy_id = ifi.insurance_file_cnt
                                    
               INNER JOIN currency pc
                 ON ifi.currency_id = pc.currency_id
                                      
      --LEFT JOIN (SELECT SUM(amount * payment_loss_xrate) as loss_amount, claim_peril_id    
      --    FROM payment    
      --    WHERE recovery_Type_id IS NULL    
      --    GROUP BY claim_peril_id) payment ON    
      --     payment.claim_peril_id = clmp.claim_peril_id
      WHERE clmp.claim_id = @ClaimId
                               
               AND ((rsv.reserve_type_id IS NULL)
                     OR (rsv.reserve_type_id IN (SELECT reserve_type_id
                                                 FROM   reserve_type
                                                 WHERE  include_in_total = 1)))
      AND 	ISNULL(prlt.is_levy_tax, 0) <> 1                
      AND  	ISNULL(prlt.is_stamp_duty_insurer,0)<>1
      AND  	ISNULL(prlt.is_stamp_duty_insured,0)<>1                    
      GROUP BY clmp.claim_id,clmp.claim_peril_id,prlt.description,
               clmp.description,clmp.sum_insured,prlt.gis_screen_id,
               clmp.base_claim_peril_id,prlt.loss_schedule_type_id,
               prlt.peril_type_id,pc.code,lc.code,salvage.received_to_date,
               thirdparty.received_to_date              
    END

GO

SET quoted_identifier  off

GO

SET ansi_nulls  ON

GO

