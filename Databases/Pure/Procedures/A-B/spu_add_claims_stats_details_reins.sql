SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_add_claims_stats_details_reins'
GO

CREATE PROCEDURE spu_add_claims_stats_details_reins    
    @ClaimID INT,
    @stats_folder_cnt INT,
    @documenttype_id INT,
    @ri_version_id   INT=1
AS

    -- ****************************************************************************************************
    -- 1.00 Create reinsurance statistics for claims from existing gross stats.              RWH  05/07/01
    -- 1.01 Add this_premium_home.                                                           RWH  10/07/01
    -- 1.02 Store claim_party_id instead of party_cnt in ri_party_cnt.                       RWH  18/07/01
    -- 1.03 Post to current assests account rather than reinsurer themselves if is not
    --      a payment.                                                                       RWH  07/08/01
    -- 1.04 Make sure party_cnt is used if transaction is payment.                           RWH  12/09/01
    -- 1.05 Distinguish between FAC and TTY.                                                 RWH  13/09/01
    -- 1.06 Pass in @stats_folder_cnt.                                                  RWH  14/09/01
    -- 1.07 Put reinsurance_type into ri_party_type.                                         RWH  24/09/01
    -- 2.00 Total rewrite of reinsurance in claims                                           Tomo 29/11/01
    -- 2.01 There was a version 1.8 which had got lost
    --      - reinstate the changes
    --      - Put treaty.code into ri_shortname (TTY only)                                   KB   07/01/02
    -- 2.02 Link wasn't good enough between claims copies of RI tables                       Tomo 23/05/02
    -- 2.03 Test for coinsurance % should ignore retained %                                  Tomo 28/05/02
    -- *****************************************************************************************************
     Declare  
    -- Working variables  
    @old_peril_type_id int,  
    @peril_band_total money,  
    @peril_band_share float,  
    @ri_band int,  
    @is_share_with_reinsurer tinyint,  
    @transaction_type_code varchar(10),  
    -- Stats detail 'Gross' details  
    @grs_stats_detail_type varchar(3),  
    @grs_risk_id int,  
    @grs_risk_type_id int,  
    @grs_risk_type_code varchar(10),  
    @grs_peril_id int,  
    @grs_peril_description varchar(30),  
    @grs_peril_type_id int,  
    @grs_peril_type_code varchar(10),  
    @grs_policy_section_type_id int,  
    @grs_policy_section_type_code varchar(10),  
    @grs_class_of_business_id int,  
    @grs_class_of_business_code varchar(10),  
    @grs_tax_type_id int,  
    @grs_tax_type_code varchar(10),  
    @grs_tax_value numeric(19, 4),  
    @grs_annual_premium MONEY,  
    @grs_currency_code varchar(10),  
    @grs_this_premium_original MONEY,  
    @grs_this_premium_home MONEY,  
    @grs_this_premium_system MONEY,  
    -- RI Line variables  
    @ri_party_cnt int,  
    @ri_shortname varchar(20),  
    @ri_sum_insured MONEY,  
    @ri_share_percent float,  
    @ri_this_reserve money,  
    @ri_this_payment money,  
    @ri_this_salvage money,  
    @ri_this_recovery money,  
    @ri_agreement_code varchar(20),  
    @ri_reinsurance_type char(3),  
    @ri_is_retained tinyint,  
@ri_treaty_id int,  
    -- Output variables  
    @out_party_cnt int,  
    @out_shortname varchar(20),  
    @out_stats_detail_type varchar(3),  
    @out_this_premium_original money,  
    @out_this_premium_home money,  
    @out_this_premium_system money,  
    -- Tax lines  
    @tax_percent float,  
    @tax_type varchar(3),  
    @claim_currency_id int,  
    @claim_currency_rate float,  
    @claim_system_rate float,  
    @payment_currency_id int,  
    @payment_currency_rate float,  
    @payment_system_rate float,  
    @payment_loss_rate float,  
    @company_id int,  
    @return_status int,  
    @net_adjustment_percent float,  
 @reinsurancecode CHAR(4),  
 @RI2007Enabled int,  
 @PerilId int  
  
Select @RI2007Enabled = ISNULL(value,0) From hidden_options Where option_number = 88 --RI2007Enabled  
  
-- Adding variable to get RI_Arrangement_Line_ID  
DECLARE @RI_Arrangement_Line_ID INT  
  
-- Get Payment Details  
IF @documenttype_id = 28 BEGIN  
  
 SELECT @PerilId = cp.claim_peril_id  
 FROM  claim_payment cp  
 INNER JOIN Stats_Folder sf ON  cp.claim_payment_id = sf.payment_id  
 WHERE sf.stats_folder_cnt =  @stats_folder_cnt  
  
    SELECT  @payment_currency_id = MIN(wcpi.currency_id),  
            @payment_currency_rate = ISNULL(MIN(wcpi.currency_base_xrate),0),  
            @payment_system_rate = ISNULL(MIN(wcpi.system_base_xrate),0),  
            @payment_loss_rate = ISNULL(MIN(wcpi.payment_loss_xrate),1)  
    FROM    claim_payment_item wcpi  
   INNER JOIN Claim_payment wcp ON  
     wcpi.claim_payment_id = wcp.claim_payment_id  
   WHERE   wcp.claim_id = @claimid  
   AND     wcp.claim_payment_id = base_claim_payment_id AND wcp.claim_peril_id = @PerilId  
   GROUP BY wcpi.claim_payment_id  
END  
  
-- Get Receipt Details  
IF @documenttype_id = 29 BEGIN  
  
 SELECT @PerilId = cr.claim_peril_id  
 FROM  Claim_Receipt cr  
 INNER JOIN Stats_Folder sf ON  cr.Claim_Receipt_id = sf.Receipt_id  
 WHERE sf.stats_folder_cnt =  @stats_folder_cnt  
  
    SELECT  @payment_currency_id = MIN(wcri.currency_id),  
            @payment_currency_rate = ISNULL(MIN(wcri.currency_base_xrate),0),  
            @payment_system_rate = ISNULL(MIN(wcri.system_base_xrate),0),  
            @payment_loss_rate = ISNULL(MIN(wcri.receipt_loss_xrate),1)  
    FROM    claim_receipt_item wcri  
   INNER JOIN claim_receipt wcr ON  
     wcri.claim_receipt_id = wcr.claim_receipt_id  
   WHERE   wcr.claim_id = @claimid  
   AND     wcr.claim_receipt_id = base_claim_receipt_id AND wcr.claim_peril_id = @PerilId  
   GROUP BY wcri.claim_receipt_id  
END  
  
-- Get Reserve Details  
IF @documenttype_id NOT IN (28,29) BEGIN  
    SELECT  @payment_currency_id = currency_id,  
            @payment_currency_rate = ISNULL(currency_base_xrate,0),  
            @payment_system_rate = ISNULL(system_base_xrate,0),  
            @payment_loss_rate = 1  
    FROM    claim  
    WHERE   claim_id = @claimid  
END  
  
-- Get Claim Details  
SELECT  @claim_currency_id = currency_id,  
        @claim_currency_rate = ISNULL(currency_base_xrate,0),  
        @claim_system_rate = ISNULL(system_base_xrate,0)  
FROM    claim  
WHERE   claim_id = @claimid  
  
-- Get the transaction type code  
SELECT  @company_id = source_id,  
        @transaction_type_code = transaction_type_code  
FROM    stats_folder  
WHERE   stats_folder_cnt = @stats_folder_cnt  
  
-- get share with reinsurer flag  
SELECT  @is_share_with_reinsurer = RT.is_share_with_re_insurers  
FROM    claim WC  
JOIN    risk R  
        ON  R.risk_cnt = WC.risk_type_id  
JOIN    Risk_Type RT  
        ON  RT.risk_type_id = R.risk_type_id  
WHERE   WC.claim_id = @ClaimID  
  
-- Get 'Gross' lines from the stats detail  
DECLARE c_stats_detail CURSOR FAST_FORWARD FOR  
    SELECT  stats_detail_type,  
            risk_id,  
            risk_type_id,  
            risk_type_code,  
            peril_id,  
            peril_description,  
            peril_type_id,  
            peril_type_code,  
            policy_section_type_id,  
            policy_section_type_code,  
            class_of_business_id,  
            class_of_business_code,  
            tax_type_id,  
            tax_type_code,  
            tax_value,  
            annual_premium,  
            currency_code,  
            this_premium_original,  
            this_premium_home  
    FROM    stats_detail  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
    AND     stats_detail_type = 'GRS'  
    ORDER BY  
            peril_type_id  
  
OPEN c_stats_detail  
FETCH NEXT FROM c_stats_detail  
    INTO    @grs_stats_detail_type,  
            @grs_risk_id,  
            @grs_risk_type_id,  
            @grs_risk_type_code,  
@grs_peril_id,  
            @grs_peril_description,  
            @grs_peril_type_id,  
@grs_peril_type_code,  
            @grs_policy_section_type_id,  
            @grs_policy_section_type_code,  
            @grs_class_of_business_id,  
            @grs_class_of_business_code,  
            @grs_tax_type_id,  
            @grs_tax_type_code,  
            @grs_tax_value,  
            @grs_annual_premium,  
            @grs_currency_code,  
            @grs_this_premium_original,  
            @grs_this_premium_home  
  
SELECT @old_peril_type_id = @grs_peril_type_id - 1  
  
WHILE (@@FETCH_STATUS = 0) BEGIN  
    -- Only do this once for each peril_type  
    -- Modifying the reserve more than once will generate this line multiple times,  
    -- however, we only want to post the RI for the total amount per peril_type  
    IF (@old_peril_type_id <> @grs_peril_type_id) BEGIN  
        -- Update for next time  
        SELECT  @old_peril_type_id = @grs_peril_type_id,  
                @net_adjustment_percent = 0 -- Reset adjustment percent for each peril  
  
        -- Get our peril's full annual premium (it may be posted over multiple lines)  
        SELECT  @grs_annual_premium = SUM(annual_premium) -- In Payment/Transaction Currency  
        FROM    stats_detail  
        WHERE   stats_folder_cnt = @stats_folder_cnt  
        AND     peril_type_id = @grs_peril_type_id  
        AND     stats_detail_type = 'GRS'  
  
        -- Get the ri band for this peril  
        SELECT  @ri_band = ri_band  
        FROM    claim_peril wcp  
        WHERE   peril_type_id = @grs_peril_type_id  
        AND     claim_id = @claimid  
  
        -- Get the band share for the peril (required for peril split calculations)  
        SELECT  @peril_band_total = ISNULL(SUM(wsd.this_premium_original), 0) -- In Payment/Transaction Currency  
        FROM    stats_detail wsd  
        JOIN    claim_peril wcp ON wcp.peril_type_id = wsd.peril_type_id  
        WHERE   wsd.stats_folder_cnt = @stats_folder_cnt  
        AND     wsd.stats_detail_type = 'GRS'  
        AND     wcp.ri_band = @ri_band  
        AND     wcp.claim_id = @claimid  
  
        -- If we have multiple perils on the band we must work out a share percentage for them!  
        IF @peril_band_total = 0  
            SELECT @peril_band_share = 0  
        ELSE  
            SELECT @peril_band_share = CONVERT(float, @grs_annual_premium) / CONVERT(float, @peril_band_total)  
  
        -- Get tax percentage, we can group any split TAG entries and assume they are for a single  
        -- peril as currently claims only allows payment of one peril at a time.  
        SELECT  @tax_percent = CONVERT(float, SUM(this_premium_home)) / CONVERT(float, @grs_this_premium_home)  
        FROM    stats_detail  
        WHERE   stats_folder_cnt = @stats_folder_cnt  
        AND     stats_detail_type = 'TAG'  
  
        Declare @RI_Band_ID int  
        Select  @RI_Band_ID = max(RI_Band_Id)from claim_ri_arrangement  
        Where   Claim_id=@ClaimID and ri_Band_iD is NOT NULL  
  
        -- If are in S&R we need to post taxes differently, the TAN lines are posted from the business component  
        -- If tax is not shared with reinsurers post it all to net now  
        IF (ISNULL(@is_share_with_reinsurer, 0) = 0 AND ISNULL(@tax_percent, 0) <> 0 AND @transaction_type_code NOT IN ('C_RV', 'C_SA'))  
        BEGIN  
INSERT INTO stats_detail(  
                    stats_folder_cnt, stats_detail_id, stats_detail_type,  
                    risk_id, risk_type_id, risk_type_code,  
                    peril_id, peril_description, peril_type_id, peril_type_code,  
    policy_section_type_id, policy_section_type_code,  
                    class_of_business_id, class_of_business_code,  
                    tax_type_id, tax_type_code, tax_value,  
                    ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                    annual_premium, currency_code, currency_rate,  
                    this_premium_original, this_premium_home, sum_insured_home)  
SELECT  @stats_folder_cnt, ISNULL(MAX(stats_detail_id), 0) + 1, 'TAN',  
                    @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                    @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
                    @grs_policy_section_type_id, @grs_policy_section_type_code,  
                    @grs_class_of_business_id, @grs_class_of_business_code,  
                    @grs_tax_type_id, @grs_tax_type_code, @grs_tax_value,  
                    @ri_party_cnt, @ri_shortname, @ri_reinsurance_type, @ri_share_percent, @ri_agreement_code,  
                    @grs_annual_premium, @grs_currency_code, @payment_currency_rate,  
                    ABS(@grs_this_premium_original) * @tax_percent,  
                    ABS(@grs_this_premium_home) * @tax_percent,  
                    @ri_sum_insured  
            FROM    stats_detail  
            WHERE   stats_folder_cnt = @stats_folder_cnt  
        END  
  
CREATE Table #RI_Lines (  
 ri_party_cnt int,  
 ri_shortname varchar(20),  
    ri_sum_insured MONEY,  
    ri_share_percent float,  
    ri_this_reserve money,  
    ri_this_payment money,  
    ri_this_salvage money,  
    ri_this_recovery money,  
    ri_agreement_code varchar(255),  
    ri_reinsurance_type char(3),  
    ri_is_retained tinyint,  
 ri_treaty_id int,  
 RI_Arrangement_Line_ID int  
 )  
  
 IF @documenttype_id = 28 BEGIN  
   INSERT INTO #RI_Lines  
  
   SELECT  P.party_cnt,  
     P.shortname,  
     RAL.sum_insured * (TP.share_percent / 100),  
     RAL.this_share_percent * (TP.share_percent / 100),  
     RAL.this_reserve * (TP.share_percent / 100) * @peril_band_share,  
     (RAL.this_payment / @payment_loss_rate) * (TP.share_percent / 100) * @peril_band_share,  
     (RAL.this_salvage / @payment_loss_rate) * (TP.share_percent / 100),  
     (RAL.this_recovery / @payment_loss_rate) * (TP.share_percent / 100),  
     LEFT(RAL.agreement_code,20),  
     RAL.Type,  
     ISNULL(Pin.is_retained,(CASE WHEN RAL.Type = 'R' THEN 1 ELSE 0 END)),  
     RAL.treaty_id,  
     CASE WHEN RAL.Type = 'R' THEN NULL ELSE RAL.ri_arrangement_line_id END  
   FROM    Claim_RI_Arrangement RRA  
   JOIN    Claim_RI_Arrangement_Line RAL  
     ON  RAL.claim_id = RRA.claim_id  
     AND RAL.ri_arrangement_id = RRA.ri_arrangement_id  
   JOIN    Claim_Peril PT -- PWF 11/07/03 - Must link to PT to restrict by band  
     ON  PT.claim_id = RRA.claim_id  
     AND  PT.ri_band = @RI_Band  
   JOIN    Treaty_Party TP  
     ON  TP.treaty_id = RAL.treaty_id  
   JOIN    Party P  
     ON  P.party_cnt = TP.party_cnt  
   JOIN    Party_Insurer Pin  
     ON  P.party_cnt = Pin.party_cnt  
     Join Claim_Payment CP On RRA.claim_id = cp.claim_id inner join Stats_Folder fs on fs.payment_id = cp.claim_payment_id  -- Aded MP  
   WHERE   RRA.Claim_Id = @ClaimID  
    AND     ri_Arrangement_version=@RI_Version_ID  
    AND     PT.peril_type_id = @grs_peril_type_id  
    AND     RRA.ri_band_id = @RI_Band  
    AND     PT.Claim_Peril_id = @PerilId  --- Added for MP  
    AND    (ISNULL(RAL.this_reserve, 0) <> 0 OR ISNULL(RAL.this_payment, 0) <> 0  
    OR  ISNULL(RAL.this_salvage, 0) <> 0 OR ISNULL(RAL.this_recovery, 0) <> 0)  
    AND   cp.claim_id = @ClaimID  And  cp.claim_peril_id  = @PerilId And fs.stats_folder_cnt =@stats_folder_cnt --Added MP  
  
   UNION ALL  
  
   SELECT  P.party_cnt,  
     P.shortname,  
     RAL.sum_insured,  
     RAL.this_share_percent,  
     RAL.this_reserve * @peril_band_share,  
     (RAL.this_payment / @payment_loss_rate) * @peril_band_share,  
     (RAL.this_salvage / @payment_loss_rate),  
     (RAL.this_recovery / @payment_loss_rate),  
     RAL.agreement_code,  
     RAL.Type,  
     CASE WHEN RAL.Type = 'R' THEN 1 ELSE 0 END,  
     RAL.treaty_id,  
     CASE WHEN RAL.Type = 'R' THEN NULL ELSE RAL.ri_arrangement_line_id END  
   FROM    Claim_RI_Arrangement RRA  
   JOIN    Claim_RI_Arrangement_Line RAL  
     ON  RAL.claim_id = RRA.claim_id  
     AND RAL.ri_arrangement_id = RRA.ri_arrangement_id  
   JOIN    Claim_Peril PT -- PWF 11/07/03 - Must link to PT to restrict by band  
     ON  PT.claim_id = RRA.claim_id  
     AND  PT.ri_band = @RI_Band  
   JOIN    Party P  
     ON  P.party_cnt = RAL.party_cnt  
   Join Claim_Payment CP On RRA.claim_id = cp.claim_id inner join Stats_Folder fs on fs.payment_id = cp.claim_payment_id   -- Aded MP  
   WHERE   RRA.Claim_Id = @ClaimID  
    AND     ri_Arrangement_version=@RI_Version_ID  
    AND     PT.peril_type_id = @grs_peril_type_id  
    AND     RRA.ri_band_id = @RI_Band  
    AND     PT.Claim_Peril_id = @PerilId --- Added for MP  
    -- Only return the lines that have changed  
    AND    (ISNULL(RAL.this_reserve, 0) <> 0 OR ISNULL(RAL.this_payment, 0) <> 0  
    OR  ISNULL(RAL.this_salvage, 0) <> 0 OR ISNULL(RAL.this_recovery, 0) <> 0)  
    AND   cp.claim_id = @ClaimID  And cp.claim_peril_id  = @PerilId And fs.stats_folder_cnt =@stats_folder_cnt --Added MP  
  
  END  
 ELSE  
  BEGIN  
   INSERT INTO #RI_Lines  
   SELECT  P.party_cnt,  
                        P.shortname,  
                        RAL.sum_insured * (TP.share_percent / 100),  
                        RAL.this_share_percent * (TP.share_percent / 100),  
                        RAL.this_reserve * (TP.share_percent / 100) * @peril_band_share,  
                        (RAL.this_payment / @payment_loss_rate) * (TP.share_percent / 100) * @peril_band_share,  
      (RAL.this_salvage / @payment_loss_rate) * (TP.share_percent / 100),  
                        (RAL.this_recovery / @payment_loss_rate) * (TP.share_percent / 100),  
                        LEFT(RAL.agreement_code,20),  
                        RAL.Type,  
                        ISNULL(Pin.is_retained,(CASE WHEN RAL.Type = 'R' THEN 1 ELSE 0 END)),  
      RAL.treaty_id,  
            CASE WHEN RAL.Type = 'R' THEN NULL ELSE RAL.ri_arrangement_line_id END  
  
                FROM    Claim_RI_Arrangement RRA  
                JOIN    Claim_RI_Arrangement_Line RAL  
                        ON  RAL.claim_id = RRA.claim_id  
                        AND RAL.ri_arrangement_id = RRA.ri_arrangement_id  
                JOIN    Claim_Peril PT -- PWF 11/07/03 - Must link to PT to restrict by band  
                        ON  PT.claim_id = RRA.claim_id  
      AND  PT.ri_band = @RI_Band  
                JOIN    Treaty_Party TP  
                        ON  TP.treaty_id = RAL.treaty_id  
                JOIN    Party P  
                        ON  P.party_cnt = TP.party_cnt  
                JOIN    Party_Insurer Pin  
      ON  P.party_cnt = Pin.party_cnt  
    WHERE   RRA.Claim_Id = @ClaimID  
      AND ri_Arrangement_version=@RI_Version_ID  
      AND PT.peril_type_id = @grs_peril_type_id  
      AND RRA.ri_band_id = @RI_Band  
      AND (ISNULL(RAL.this_reserve, 0) <> 0 OR ISNULL(RAL.this_payment, 0) <> 0  
      OR  ISNULL(RAL.this_salvage, 0) <> 0 OR ISNULL(RAL.this_recovery, 0) <> 0)  
  
                UNION ALL  
  
                SELECT  P.party_cnt,  
                        P.shortname,  
                        RAL.sum_insured,  
                        RAL.this_share_percent,  
                        RAL.this_reserve * @peril_band_share,  
                       (RAL.this_payment / @payment_loss_rate) * @peril_band_share,  
      (RAL.this_salvage / @payment_loss_rate),  
                        (RAL.this_recovery / @payment_loss_rate),  
      LEFT(RAL.agreement_code,20),  
                        RAL.Type,  
                        CASE WHEN RAL.Type = 'R' THEN 1 ELSE 0 END,  
      RAL.treaty_id,  
         CASE WHEN RAL.Type = 'R' THEN NULL ELSE RAL.ri_arrangement_line_id END  
                FROM    Claim_RI_Arrangement RRA  
                JOIN    Claim_RI_Arrangement_Line RAL  
                        ON  RAL.claim_id = RRA.claim_id  
                        AND RAL.ri_arrangement_id = RRA.ri_arrangement_id  
                JOIN    Claim_Peril PT -- PWF 11/07/03 - Must link to PT to restrict by band  
                        ON  PT.claim_id = RRA.claim_id  
      AND  PT.ri_band = @RI_Band  
                JOIN    Party P  
                        ON  P.party_cnt = RAL.party_cnt  
  
                WHERE   RRA.Claim_Id = @ClaimID  
      AND ri_Arrangement_version=@RI_Version_ID  
      AND PT.peril_type_id = @grs_peril_type_id  
      AND RRA.ri_band_id = @RI_Band  
    -- Only return the lines that have changed  
      AND (ISNULL(RAL.this_reserve, 0) <> 0 OR ISNULL(RAL.this_payment, 0) <> 0  
      OR  ISNULL(RAL.this_salvage, 0) <> 0 OR ISNULL(RAL.this_recovery, 0) <> 0)  
  END  
  
        -- Declare RI Line cursor  
        DECLARE c_reinsurers CURSOR FAST_FORWARD FOR  
            select * from #RI_Lines  
        OPEN c_reinsurers  
        FETCH NEXT FROM c_reinsurers  
            INTO    @ri_party_cnt,  
                    @ri_shortname,  
                    @ri_sum_insured,  
                    @ri_share_percent,  
                    @ri_this_reserve,  
                    @ri_this_payment,  
                    @ri_this_salvage,  
                    @ri_this_recovery,  
                    @ri_agreement_code,  
                    @ri_reinsurance_type,  
                    @ri_is_retained,  
     @ri_treaty_id,  
     @RI_Arrangement_Line_ID  
  
        WHILE (@@FETCH_STATUS = 0) BEGIN  
            -- Post any reserve amendments  
            IF (@ri_this_reserve <> 0) AND (@transaction_type_code <> 'C_CP') BEGIN  
                SELECT  @out_this_premium_original = @ri_this_reserve  
  
                EXEC spu_ACT_Do_Currency_Conversion  
                        @company_id = @company_id,  
                        @currency_id = @claim_currency_id,  
                        @currency_amount_unrounded = @out_this_premium_original,  
                        @mode = 'ALL',  
                        @base_amount = @out_this_premium_home OUTPUT,  
                        @system_amount = @out_this_premium_system OUTPUT,  
                        @currency_base_xrate = @claim_currency_rate OUTPUT,  
                        @system_base_xrate = @claim_system_rate OUTPUT,  
                        @return_status = @return_status OUTPUT  
  
                -- Set appropriate account details for reserve postings  
                IF @ri_is_retained = 1  
                    SELECT  @out_stats_detail_type = 'NET',  
                            @out_party_cnt = 0,  
                            @out_shortname = @ri_shortname  
                ELSE IF @RI2007Enabled = 1  
    BEGIN  
    SELECT @reinsurancecode = UPPER(LTRIM(RTRIM(rt.code)))  
    FROM party_insurer ptyin  
    INNER JOIN reinsurance_type rt ON ptyin.reinsurance_type=rt.reinsurance_type_id  
    WHERE rt.is_deleted =0 AND ptyin.party_cnt = @ri_party_cnt  
  
-- Post current assets account rather than r/i account for reserves.  
SELECT  @out_stats_detail_type = CASE @ri_reinsurance_type  
    WHEN 'F' THEN 'FAC'  
    WHEN 'X' THEN 'XOL'  
    WHEN 'FX' THEN 'FAX'  
    WHEN 'TX' THEN 'TYX'  
    WHEN 'T' THEN 'TTY'  
    WHEN 'TC' THEN 'CAT'  
    WHEN 'TFS' THEN 'TFS'  
    ELSE 'TR'  
    END,  
@out_party_cnt = @ri_party_cnt,  
@out_shortname = CASE  
    WHEN @ri_reinsurance_type = 'F' THEN 'OT'  
    WHEN @ri_reinsurance_type = 'X' THEN 'XL'  
    WHEN @ri_reinsurance_type = 'FX' THEN 'FX'  
    WHEN @ri_reinsurance_type = 'TX' THEN 'TX'  
    WHEN @ri_reinsurance_type = 'T' AND ISNULL(LTRIM(RTRIM(@reinsurancecode)),'') = 'QUO' THEN 'TQ'  
    WHEN @ri_reinsurance_type = 'T' AND (ISNULL(LTRIM(RTRIM(@reinsurancecode)),'') = '001' OR ISNULL(LTRIM(RTRIM(@reinsurancecode)),'') = '002' OR ISNULL(LTRIM(RTRIM(@reinsurancecode)),'') = '003') THEN 'TS  
'  
        WHEN @ri_reinsurance_type = 'TC' THEN 'TC'  
        WHEN @ri_reinsurance_type = 'TFS' THEN 'TS'  
                                                    ELSE 'TR'  
                                                END + @grs_class_of_business_code  
                END  
    ELSE -- RI2007 switched off  
    BEGIN  
        -- Post current assets account rather than r/i account for reserves.  
        SELECT  @out_stats_detail_type = CASE @ri_reinsurance_type  
        WHEN 'F' THEN 'FAC'  
        WHEN 'X' THEN 'XOL'  
        WHEN 'FX' THEN 'FAX'  
        WHEN 'TX' THEN 'TYX'  
        WHEN 'T' THEN 'TTY'  
        WHEN 'TC' THEN 'CAT'  
        WHEN 'TFS' THEN 'TFS'  
        ELSE 'TR'  
        END,  
@out_party_cnt = @ri_party_cnt,  
@out_shortname = CASE @ri_reinsurance_type  
        WHEN 'F' THEN 'OT'  
        WHEN 'X' THEN 'XL'  
        WHEN 'FX' THEN 'FX'  
        WHEN 'TX' THEN 'TTY'  
        WHEN 'TC' THEN 'CAT'  
        WHEN 'TFS' THEN 'TFS'  
        ELSE 'TR'  
        END + @grs_class_of_business_code  
END  
  
                -- Insert reserve line  
  
                INSERT INTO stats_detail(  
                stats_folder_cnt, stats_detail_id, stats_detail_type,  
                        risk_id, risk_type_id, risk_type_code,  
                        peril_id, peril_description, peril_type_id, peril_type_code,  
                        policy_section_type_id, policy_section_type_code,  
                        class_of_business_id, class_of_business_code,  
                        tax_type_id, tax_type_code, tax_value,  
                        ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
    annual_premium, currency_code, currency_rate,  
                        this_premium_original, this_premium_home, this_premium_system, sum_insured_home,  
        ri_arrangement_line_id  
  
    )  
                SELECT  @stats_folder_cnt, ISNULL(MAX(stats_detail_id), 0) + 1, @out_stats_detail_type,  
                        @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                        @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
                        @grs_policy_section_type_id, @grs_policy_section_type_code,  
    @grs_class_of_business_id, @grs_class_of_business_code,  
    @grs_tax_type_id, @grs_tax_type_code, @grs_tax_value,  
                        @out_party_cnt, @out_shortname, @ri_reinsurance_type, @ri_share_percent, @ri_agreement_code,  
                        @grs_annual_premium, @grs_currency_code, @claim_currency_rate,  
                        @out_this_premium_original, @out_this_premium_home, @out_this_premium_system, @ri_sum_insured,  
        @RI_Arrangement_Line_ID  
                FROM    stats_detail  
                WHERE   stats_folder_cnt = @stats_folder_cnt  
            END  
  
            -- Post any payment amendments  
            IF (@ri_this_payment <> 0) AND (@transaction_type_code <> 'C_CR') BEGIN  
                -- Get premiums based on actual values, not percentages  
                SELECT  @out_this_premium_original = @ri_this_payment  
  
                EXEC spu_ACT_Do_Currency_Conversion  
                        @company_id = @company_id,  
                        @currency_id = @payment_currency_id,  
                        @currency_amount_unrounded = @out_this_premium_original,  
                        @mode = 'ALL',  
                        @base_amount = @out_this_premium_home OUTPUT,  
                        @system_amount = @out_this_premium_system OUTPUT,  
                        @currency_base_xrate = @payment_currency_rate OUTPUT,  
                        @system_base_xrate = @payment_system_rate OUTPUT,  
                        @return_status = @return_status OUTPUT  
  
                IF @ri_is_retained = 1  
                    SELECT  @out_stats_detail_type = 'NET'  
                ELSE  
                    SELECT  @out_stats_detail_type = CASE @ri_reinsurance_type  
            WHEN 'F' THEN 'FAC'  
            WHEN 'X' THEN 'XOL'  
            WHEN 'FX' THEN 'FAX'  
            WHEN 'TX' THEN 'TYX'  
            WHEN 'T' THEN 'TTY'  
            WHEN 'TC' THEN 'CAT'  
            WHEN 'TFS' THEN 'TFS'  
            ELSE 'TR'  
            END  
  
                -- Insert payment line  
                INSERT INTO stats_detail(  
                        stats_folder_cnt, stats_detail_id, stats_detail_type,  
                        risk_id, risk_type_id, risk_type_code,  
                        peril_id, peril_description, peril_type_id, peril_type_code,  
                        policy_section_type_id, policy_section_type_code,  
    class_of_business_id, class_of_business_code,  
                        tax_type_id, tax_type_code, tax_value,  
 ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                        annual_premium, currency_code, currency_rate,  
                        this_premium_original, this_premium_home, this_premium_system, sum_insured_home,  
    ri_arrangement_line_id  
                            )  
                SELECT  @stats_folder_cnt, ISNULL(MAX(stats_detail_id), 0) + 1, @out_stats_detail_type,  
                        @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                        @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
                        @grs_policy_section_type_id, @grs_policy_section_type_code,  
                        @grs_class_of_business_id, @grs_class_of_business_code,  
                        @grs_tax_type_id, @grs_tax_type_code, @grs_tax_value,  
                        @ri_party_cnt, @ri_shortname, @ri_reinsurance_type, @ri_share_percent, @ri_agreement_code,  
                        @grs_annual_premium, @grs_currency_code, @payment_currency_rate,  
                        @out_this_premium_original, @out_this_premium_home, @out_this_premium_system, @ri_sum_insured,  
    @RI_Arrangement_Line_ID  
  
                FROM    stats_detail  
                WHERE   stats_folder_cnt = @stats_folder_cnt  
  
                IF (@is_share_with_reinsurer = 1) AND (ISNULL(@tax_percent, 0) <> 0) BEGIN  
  
                    -- Set stats type for tax line  
                SELECT @tax_type = CASE  
                        WHEN @out_stats_detail_type = 'NET' THEN 'TAN'  
                        WHEN @out_stats_detail_type IN ('TTY','TYX') THEN 'TAT'  
        WHEN @out_stats_detail_type IN ('FAC','FAX') THEN 'TAF'  
                        WHEN @out_stats_detail_type IN ('XOL','CAT') THEN 'TAL'  
                    END  
    if isnull(@tax_type,'')<>''  
    BEGIN  
  
                    INSERT INTO stats_detail(  
                            stats_folder_cnt, stats_detail_id, stats_detail_type,  
                            risk_id, risk_type_id, risk_type_code,  
                            peril_id, peril_description, peril_type_id, peril_type_code,  
                            policy_section_type_id, policy_section_type_code,  
                            class_of_business_id, class_of_business_code,  
                            tax_type_id, tax_type_code, tax_value,  
                            ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                            annual_premium, currency_code, currency_rate,  
                            this_premium_original, this_premium_home, this_premium_system, sum_insured_home,  
    ri_arrangement_line_id  
        )  
                    SELECT  @stats_folder_cnt, ISNULL(MAX(stats_detail_id), 0) + 1, @tax_type,  
                            @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                            @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
    @grs_policy_section_type_id, @grs_policy_section_type_code,  
                            @grs_class_of_business_id, @grs_class_of_business_code,  
                            @grs_tax_type_id, @grs_tax_type_code, @grs_tax_value,  
                            @ri_party_cnt, @ri_shortname, @ri_reinsurance_type, @ri_share_percent, @ri_agreement_code,  
                            @grs_annual_premium, @grs_currency_code, @payment_currency_rate,  
                            @out_this_premium_original * @tax_percent,  
                            @out_this_premium_home * @tax_percent,  
                            @out_this_premium_system * @tax_percent,  
                            @ri_sum_insured,  
    @RI_Arrangement_Line_ID  
  
                    FROM    stats_detail  
                    WHERE   stats_folder_cnt = @stats_folder_cnt  
    END  
                END  
            END  
  
            -- Post any salvage amendments  
            IF (@ri_this_salvage <> 0) BEGIN  
                -- Get premiums based on actual values, not percentages  
                SELECT  @out_this_premium_original = @ri_this_salvage  --* -1   --PN-71895  
  
                EXEC spu_ACT_Do_Currency_Conversion  
                        @company_id = @company_id,  
                        @currency_id = @payment_currency_id,  
                        @currency_amount_unrounded = @out_this_premium_original,  
                        @mode = 'ALL',  
                        @base_amount = @out_this_premium_home OUTPUT,  
                        @system_amount = @out_this_premium_system OUTPUT,  
                        @currency_base_xrate = @payment_currency_rate OUTPUT,  
                        @system_base_xrate = @payment_system_rate OUTPUT,  
                        @return_status = @return_status OUTPUT  
  
                IF @ri_is_retained = 1  
                    SELECT  @out_stats_detail_type = 'NET'  
                ELSE  
                    SELECT  @out_stats_detail_type = CASE @ri_reinsurance_type  
            WHEN 'F' THEN 'FAC'  
            WHEN 'X' THEN 'XOL'  
            WHEN 'FX' THEN 'FAX'  
            WHEN 'TX' THEN 'TYX'  
            WHEN 'T' THEN 'TTY'  
            WHEN 'TC' THEN 'CAT'  
            WHEN 'TFS' THEN 'TFS'  
            ELSE 'TR'  
            END  
  
            -- Insert payment line  
                INSERT INTO stats_detail(  
                        stats_folder_cnt, stats_detail_id, stats_detail_type,  
                        risk_id, risk_type_id, risk_type_code,  
                        peril_id, peril_description, peril_type_id, peril_type_code,  
                        policy_section_type_id, policy_section_type_code,  
                        class_of_business_id, class_of_business_code,  
                        tax_type_id, tax_type_code, tax_value,  
                        ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                        annual_premium, currency_code, currency_rate,  
                        this_premium_original, this_premium_home, this_premium_system, sum_insured_home,  
    ri_arrangement_line_id  
            )  
                SELECT  @stats_folder_cnt, ISNULL(MAX(stats_detail_id), 0) + 1, @out_stats_detail_type,  
                        @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                        @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
                @grs_policy_section_type_id, @grs_policy_section_type_code,  
                        @grs_class_of_business_id, @grs_class_of_business_code,  
                        @grs_tax_type_id, @grs_tax_type_code, @grs_tax_value,  
                        @ri_party_cnt, @ri_shortname, @ri_reinsurance_type, @ri_share_percent, @ri_agreement_code,  
                        @grs_annual_premium, @grs_currency_code, @payment_currency_rate,  
                        @out_this_premium_original, @out_this_premium_home, @out_this_premium_system, @ri_sum_insured,  
            @RI_Arrangement_Line_ID  
  
                FROM    stats_detail  
                WHERE   stats_folder_cnt = @stats_folder_cnt  
  
                IF (@is_share_with_reinsurer = 1) AND (@out_stats_detail_type <> 'NET') AND (ISNULL(@tax_percent, 0) <> 0)  
                BEGIN  
                    -- Set stats type for tax line  
                    SELECT @tax_type = CASE  
                        WHEN @out_stats_detail_type IN ('TTY','TYX') THEN 'TAT'  
                        WHEN @out_stats_detail_type IN ('FAC','FAX') THEN 'TAF'  
                        WHEN @out_stats_detail_type IN ('XOL','CAT') THEN 'TAL'  
                    END  
    if isnull(@tax_type,'')<>''  
BEGIN  
                    INSERT INTO stats_detail(  
                            stats_folder_cnt, stats_detail_id, stats_detail_type,  
                            risk_id, risk_type_id, risk_type_code,  
                            peril_id, peril_description, peril_type_id, peril_type_code,  
                            policy_section_type_id, policy_section_type_code,  
                            class_of_business_id, class_of_business_code,  
                            tax_type_id, tax_type_code, tax_value,  
                            ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                            annual_premium, currency_code, currency_rate,  
                            this_premium_original, this_premium_home, this_premium_system, sum_insured_home,  
    ri_arrangement_line_id  
    )  
                    SELECT  @stats_folder_cnt, ISNULL(MAX(stats_detail_id), 0) + 1, @tax_type,  
                            @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                            @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
                            @grs_policy_section_type_id, @grs_policy_section_type_code,  
                            @grs_class_of_business_id, @grs_class_of_business_code,  
                        @grs_tax_type_id, @grs_tax_type_code, @grs_tax_value,  
                            @ri_party_cnt, @ri_shortname, @ri_reinsurance_type, @ri_share_percent, @ri_agreement_code,  
                            @grs_annual_premium, @grs_currency_code, @payment_currency_rate,  
                            @out_this_premium_original * @tax_percent,  
                            @out_this_premium_home * @tax_percent,  
                            @out_this_premium_system * @tax_percent,  
                            @ri_sum_insured,  
    --Start (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)  
    @RI_Arrangement_Line_ID  
--End (Prakash Varghese) - (Tech Spec - TRAC 4761 Stats Detail to RI Line.docx) - (6.2)  
                    FROM    stats_detail  
                    WHERE   stats_folder_cnt = @stats_folder_cnt  
END  
                    -- Add the tax percentage to our net adjustment total  
    SELECT  @net_adjustment_percent = @net_adjustment_percent + (ISNULL(@ri_share_percent, 0) / 100)  
                END -- (@is_share_with_reinsurer = 1)  
            END -- (@ri_this_salvage <> 0)  
  
            -- Post any recovery amendments  
            IF (@ri_this_recovery <> 0) BEGIN  
                -- Get premiums based on actual values, not percentages  
                SELECT  @out_this_premium_original = @ri_this_recovery  --* -1  --PN-71895  
  
                EXEC spu_ACT_Do_Currency_Conversion  
                        @company_id = @company_id,  
                        @currency_id = @payment_currency_id,  
                        @currency_amount_unrounded = @out_this_premium_original,  
                        @mode = 'ALL',  
                        @base_amount = @out_this_premium_home OUTPUT,  
                        @system_amount = @out_this_premium_system OUTPUT,  
                        @currency_base_xrate = @payment_currency_rate OUTPUT,  
                        @system_base_xrate = @payment_system_rate OUTPUT,  
                        @return_status = @return_status OUTPUT  
  
                IF @ri_is_retained = 1  
                    SELECT  @out_stats_detail_type = 'NET'  
                ELSE  
                    SELECT  @out_stats_detail_type = CASE @ri_reinsurance_type  
            WHEN 'F' THEN 'FAC'  
            WHEN 'X' THEN 'XOL'  
            WHEN 'FX' THEN 'FAX'  
            WHEN 'TX' THEN 'TYX'  
            WHEN 'T' THEN 'TTY'  
            WHEN 'TC' THEN 'CAT'  
            WHEN 'TFS' THEN 'TFS'  
            ELSE 'TR'  
            END  
  
                -- Insert payment line  
                INSERT INTO stats_detail(  
                        stats_folder_cnt, stats_detail_id, stats_detail_type,  
                        risk_id, risk_type_id, risk_type_code,  
                        peril_id, peril_description, peril_type_id, peril_type_code,  
                        policy_section_type_id, policy_section_type_code,  
                        class_of_business_id, class_of_business_code,  
    tax_type_id, tax_type_code, tax_value,  
                        ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                        annual_premium, currency_code, currency_rate,  
                        this_premium_original, this_premium_home, this_premium_system, sum_insured_home,  
    ri_arrangement_line_id  
)  
                SELECT  @stats_folder_cnt, ISNULL(MAX(stats_detail_id), 0) + 1, @out_stats_detail_type,  
                        @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                        @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
                        @grs_policy_section_type_id, @grs_policy_section_type_code,  
                        @grs_class_of_business_id, @grs_class_of_business_code,  
                        @grs_tax_type_id, @grs_tax_type_code, @grs_tax_value,  
                        @ri_party_cnt, @ri_shortname, @ri_reinsurance_type, @ri_share_percent, @ri_agreement_code,  
                        @grs_annual_premium, @grs_currency_code, @payment_currency_rate,  
                        @out_this_premium_original, @out_this_premium_home, @out_this_premium_system, @ri_sum_insured  ,  
    @RI_Arrangement_Line_ID  
  
                FROM    stats_detail  
                WHERE   stats_folder_cnt = @stats_folder_cnt  
  
                IF (@is_share_with_reinsurer = 1) AND (@out_stats_detail_type <> 'NET') AND (ISNULL(@tax_percent, 0) <> 0)  
                BEGIN  
                    -- Set stats type for tax line  
                    SELECT @tax_type = CASE  
                        WHEN @out_stats_detail_type IN ('TTY','TYX') THEN 'TAT'  
                        WHEN @out_stats_detail_type IN ('FAC','FAX') THEN 'TAF'  
                        WHEN @out_stats_detail_type IN ('XOL','CAT') THEN 'TAL'  
                    END  
                IF ISNULL(@tax_type,'')<>''  
    BEGIN  
                    INSERT INTO stats_detail(  
                        stats_folder_cnt, stats_detail_id, stats_detail_type,  
                            risk_id, risk_type_id, risk_type_code,  
    peril_id, peril_description, peril_type_id, peril_type_code,  
                            policy_section_type_id, policy_section_type_code,  
                            class_of_business_id, class_of_business_code,  
                            tax_type_id, tax_type_code, tax_value,  
                            ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                            annual_premium, currency_code, currency_rate,  
                            this_premium_original, this_premium_home, this_premium_system, sum_insured_home,  
    ri_arrangement_line_id  
        )  
                    SELECT  @stats_folder_cnt, ISNULL(MAX(stats_detail_id), 0) + 1, @tax_type,  
                            @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                            @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
                            @grs_policy_section_type_id, @grs_policy_section_type_code,  
                            @grs_class_of_business_id, @grs_class_of_business_code,  
                            @grs_tax_type_id, @grs_tax_type_code, @grs_tax_value,  
                            @ri_party_cnt, @ri_shortname, @ri_reinsurance_type, @ri_share_percent, @ri_agreement_code,  
                            @grs_annual_premium, @grs_currency_code, @payment_currency_rate,  
                            @out_this_premium_original * @tax_percent,  
                            @out_this_premium_home * @tax_percent,  
                            @out_this_premium_system * @tax_percent,  
                            @ri_sum_insured,  
    @RI_Arrangement_Line_ID  
                            FROM    stats_detail  
                    WHERE   stats_folder_cnt = @stats_folder_cnt  
  
                    -- Add the tax percentage to our net adjustment total  
                    SELECT  @net_adjustment_percent = @net_adjustment_percent + (ISNULL(@ri_share_percent, 0) / 100)  
    END  
                END  
            END  
  
            -- Get next ri line  
            FETCH NEXT FROM c_reinsurers  
                INTO    @ri_party_cnt,  
    @ri_shortname,  
@ri_sum_insured,  
                        @ri_share_percent,  
                        @ri_this_reserve,  
                        @ri_this_payment,  
                        @ri_this_salvage,  
                        @ri_this_recovery,  
                        @ri_agreement_code,  
                        @ri_reinsurance_type,  
                        @ri_is_retained,  
    @ri_treaty_id,  
        @RI_Arrangement_Line_ID  
  
        END  
  
        CLOSE c_reinsurers  
        DEALLOCATE c_reinsurers  
    END  
  
    -- Post contra tax net lines for this peril according to salvage or recovery tax postings  
    IF (@net_adjustment_percent <> 0) BEGIN  
        -- Set contra stats type for all RI tax lines  
        INSERT INTO stats_detail(  
                stats_folder_cnt, stats_detail_id, stats_detail_type,  
                risk_id, risk_type_id, risk_type_code,  
                peril_id, peril_description, peril_type_id, peril_type_code,  
                policy_section_type_id, policy_section_type_code,  
                class_of_business_id, class_of_business_code,  
                tax_type_id, tax_type_code, tax_value,  
                ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                annual_premium, currency_code, currency_rate,  
                this_premium_original, this_premium_home, this_premium_system, sum_insured_home)  
        SELECT  @stats_folder_cnt,  
                (SELECT ISNULL(MAX(stats_detail_id), 0) + 1 FROM stats_detail WHERE stats_folder_cnt = @stats_folder_cnt) + stats_detail_id,  
                'TAN',  
                @grs_risk_id, @grs_risk_type_id, @grs_risk_type_code,  
                @grs_peril_id, @grs_peril_description, @grs_peril_type_id, @grs_peril_type_code,  
                @grs_policy_section_type_id, @grs_policy_section_type_code,  
                @grs_class_of_business_id, @grs_class_of_business_code,  
                tax_type_id, tax_type_code, tax_value,  
                ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent, ri_agreement_code,  
                annual_premium, currency_code, currency_rate,  
                -this_premium_original * @net_adjustment_percent,  
                -this_premium_home * @net_adjustment_percent,  
                -this_premium_system * @net_adjustment_percent,  
                sum_insured_home  
        FROM    stats_detail  
        WHERE   stats_folder_cnt = @stats_folder_cnt  
        AND     risk_id = @grs_risk_id  
        AND     peril_type_id = @grs_peril_type_id  
        AND     stats_detail_type = 'TAN'  
    END  
  
    -- Get next peril  
    FETCH NEXT FROM c_stats_detail  
        INTO    @grs_stats_detail_type,  
                @grs_risk_id,  
                @grs_risk_type_id,  
                @grs_risk_type_code,  
                @grs_peril_id,  
                @grs_peril_description,  
                @grs_peril_type_id,  
                @grs_peril_type_code,  
                @grs_policy_section_type_id,  
                @grs_policy_section_type_code,  
                @grs_class_of_business_id,  
                @grs_class_of_business_code,  
                @grs_tax_type_id,  
                @grs_tax_type_code,  
                @grs_tax_value,  
                @grs_annual_premium,  
                @grs_currency_code,  
                @grs_this_premium_original,  
                @grs_this_premium_home  
 DROP TABLE #RI_Lines  
END  
  
CLOSE c_stats_detail  
DEALLOCATE c_stats_detail 
