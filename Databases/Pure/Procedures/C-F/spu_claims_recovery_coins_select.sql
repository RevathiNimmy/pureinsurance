SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claims_recovery_coins_select'
GO

CREATE PROCEDURE spu_claims_recovery_coins_select  
    @peril_id int,  
    @is_salvage tinyint  
AS  
  
    DECLARE @is_share tinyint  
    DECLARE @insurance_file_cnt int  
    DECLARE @AgentUnderwriter varchar(1)  
  
    SELECT  @AgentUnderwriter = ISNULL(value,'A')  
    FROM    hidden_options  
    WHERE   branch_id = 1 and option_number = 1  
  
    IF @AgentUnderwriter = ''  
        SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
 BEGIN  
  
 SELECT @insurance_file_cnt = c.policy_id  
 FROM claim_peril cp  
 INNER JOIN claim c ON c.claim_id = cp.claim_id  
 WHERE cp.claim_peril_id = @peril_id  
  
 CREATE TABLE #tmp_claims_recovery_coins_select  
 (party_cnt int NULL, shortname char(20) NULL, name varchar(50) NULL, share float NULL, claim_peril_id int NULL)  
  
 IF EXISTS(SELECT NULL FROM policy_coinsurers WHERE insurance_file_cnt = @insurance_file_cnt)  
  INSERT INTO #tmp_claims_recovery_coins_select  
  SELECT  
  pc.party_cnt,  
  p.shortname,  
  p.name,  
  pc.coinsurer_cover_percentage,  
  @peril_id  
  FROM policy_coinsurers pc  
  INNER JOIN party p ON pc.party_cnt = p.party_cnt  
  WHERE pc.insurance_file_cnt = @insurance_file_cnt  
 ELSE  
  INSERT INTO #tmp_claims_recovery_coins_select  
  SELECT  
  inf.lead_insurer_cnt,  
  p.shortname,  
  p.name,  
  100,  
  @peril_id  
  FROM insurance_file inf  
  INNER JOIN party p ON inf.lead_insurer_cnt = p.party_cnt  
  WHERE inf.insurance_file_cnt = @insurance_file_cnt  
  
 SELECT 
  r.recovery_id,  
  rt.description,  
  tmp.party_cnt,  
  tmp.name,  
  tmp.share,  
  (  
   SELECT SUM(cpi.this_payment * cpi.payment_loss_xrate)  
   FROM  
   claim_payment cp  
   INNER JOIN claim_payment_item cpi ON cpi.claim_payment_id = cp.claim_payment_id  
   WHERE  
   cp.claim_peril_id = @peril_id AND  
   cpi.recovery_id = r.recovery_id AND  
   cp.party_cnt = tmp.party_cnt  
  ) as to_date,  
  0 as is_tax_shared,  
  tmp.shortname,
 rt.code RecoveryTypeCode  
 FROM  
  #tmp_claims_recovery_coins_select tmp  
  INNER JOIN claim_peril cp ON tmp.claim_peril_id = cp.claim_peril_id  
  INNER JOIN recovery r ON cp.claim_peril_id = r.claim_peril_id  
  INNER JOIN recovery_type rt ON r.recovery_type_id = rt.recovery_type_id  
  WHERE rt.is_salvage = @is_salvage  
  ORDER BY r.recovery_id  
  
 DROP TABLE #tmp_claims_recovery_coins_select  
  
 END  
ELSE  
 BEGIN  
  
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
--Start (Sriram P) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurance - Recoveries.doc) - (6.2.1)  
    SELECT  r.recovery_id RecoveryKey,  
       rt.description RecoveryType,  
            p.Party_cnt PartyKey,  
            p.name Coinsurer,  
            cp.Share SharePercent,  
--End (Sriram P) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurance - Recoveries.doc) - (6.2.1)  
  
           (Select  Sum(wcpi.this_payment * wcpi.payment_loss_xrate)  
            From    claim_payment  
  INNER JOIN claim_payment_item wcpi ON  
   wcpi.claim_payment_id = claim_payment.claim_payment_id  
  
            Where   claim_peril_id = wcp.claim_peril_id  
                And recovery_id = r.recovery_id 
--Start (Sriram P) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurance - Recoveries.doc) - (6.2.1)   
                And party_cnt = cp.party_id) RecoveryToDate,  
  --End (Sriram P) - (Tech Spec - UIIC WR24 - OpenClaim - Reserve Process - Coinsurance - Recoveries.doc) - (6.2.1)    
            @is_share is_tax_shared  ,
			'',
			 rt.code RecoveryTypeCode
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
  
 END  



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
