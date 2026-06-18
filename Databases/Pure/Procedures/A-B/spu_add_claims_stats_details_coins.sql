SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_claims_stats_details_coins'
GO

CREATE PROCEDURE spu_add_claims_stats_details_coins  
    @ClaimID int,  
    @stats_folder_cnt int  
AS  
  
--*****************************************************************************************************  
-- 1.00   Create coinsurance statistics for claims from existing gross stats.             RWH 05/07/01  
-- 1.01   Added in this_premium_home                                                      RWH 10/07/01  
-- 1.02   Make sure party_cnt is used if transaction is payment.                          RWH 12/09/01  
-- 1.03   Pass @stats_folder_cnt in as parameter.                                    RWH 14/09/01  
-- 1.04   Test for coinsurance and reinsurance insufficient                               Tom 28/05/02  
--*****************************************************************************************************  
  
DECLARE  
    -- Working variables  
    @transaction_type_code varchar(10),  
    @transaction_direction int,  
    -- Coinsurer cursor  
    @ci_party_cnt int,  
    @ci_party_type_id smallint,  
    @ci_shortname varchar(20),  
    @ci_share_percent FLOAT,  
    -- Stats detail "GRS" cursor  
    @sd_stats_detail_type varchar(3),  
    @sd_risk_id int,  
    @sd_risk_type_id int,  
    @sd_risk_type_code varchar(10),  
    @sd_peril_id int,  
    @sd_peril_description varchar(30),  
    @sd_peril_type_id int,  
    @sd_peril_type_code varchar(10),  
    @sd_policy_section_type_id int,  
    @sd_policy_section_type_code varchar(10),  
    @sd_class_of_business_id int,  
    @sd_class_of_business_code varchar(10),  
    @sd_tax_type_id int,  
    @sd_tax_type_code varchar(10),  
    @sd_tax_value MONEY,  
    @sd_annual_premium MONEY,  
    @sd_currency_code varchar(10),  
    @sd_currency_rate FLOAT,  
    @sd_this_premium_original MONEY,  
    @sd_this_premium_home MONEY,  
    @sd_this_premium_system MONEY,  
    @sd_sum_insured_home MONEY,  
    -- Calculated output values  
    @out_annual_premium MONEY,  
    @out_annual_premium_home MONEY,  
    @out_annual_premium_system MONEY,  
    @out_sum_insured_home MONEY,  
 -- Tax lines  
 @tax_percent float,  
 @tax_type varchar(3)  
  
-- Check if we have any coinsurers  
IF NOT EXISTS (SELECT  *  
               FROM    Claim_Party  
               WHERE   Claim_id = @ClaimID  
               AND     insurer_type = 0  
               AND     party_id IS NOT NULL)  
    -- No coinsurance so exit  
    RETURN  
  
-- Get the transaction type code  
SELECT  @transaction_type_code = transaction_type_code,  
        -- salvage and reserve are receipts not payments, reverse the amounts  
        @transaction_direction = Case When transaction_type_code In ('C_SA', 'C_RV') Then -1 Else 1 End  
FROM    stats_folder  
WHERE   stats_folder_cnt = @stats_folder_cnt  
  
-- Declare the coinsurer cursor up front, we will need this a few times :-)  
DECLARE CoinsurerCursor CURSOR FAST_FORWARD FOR  
    SELECT  P.Party_cnt,  
            P.party_type_id,  
            P.shortname,  
            CP.Share  
    FROM    Claim_Party CP  
    JOIN    Party P ON P.Party_cnt = CP.Party_id  
    WHERE   CP.Claim_id = @ClaimID  
    AND     insurer_type = 0         -- coinsurer  
  
-- Build cursor of all gross lines  
DECLARE StatsCursor CURSOR FAST_FORWARD FOR  
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
            annual_premium * @transaction_direction,  
            currency_code,  
            currency_rate,  
            this_premium_original * @transaction_direction,  
            this_premium_home * @transaction_direction,  
            this_premium_system * @transaction_direction,  
            sum_insured_home  
    FROM    stats_detail  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
    AND     stats_detail_type = 'GRS'  
  
-- Open stats cursor and get data  
OPEN StatsCursor  
  
FETCH NEXT FROM StatsCursor  
INTO    @sd_stats_detail_type,  
        @sd_risk_id,  
        @sd_risk_type_id,  
        @sd_risk_type_code,  
        @sd_peril_id,  
        @sd_peril_description,  
        @sd_peril_type_id,  
        @sd_peril_type_code,  
        @sd_policy_section_type_id,  
        @sd_policy_section_type_code,  
        @sd_class_of_business_id,  
        @sd_class_of_business_code,  
        @sd_tax_type_id,  
        @sd_tax_type_code,  
        @sd_tax_value,  
        @sd_annual_premium,  
        @sd_currency_code,  
        @sd_currency_rate,  
        @sd_this_premium_original,  
        @sd_this_premium_home,  
        @sd_this_premium_system,  
        @sd_sum_insured_home  
  
-- Loop the cursor  
WHILE (@@FETCH_STATUS = 0)  
BEGIN  
    -- Open the coinsurer cursor  
    OPEN CoinsurerCursor  
  
    FETCH NEXT FROM CoinsurerCursor  
    INTO    @ci_party_cnt,  
            @ci_party_type_id,  
            @ci_shortname,  
            @ci_share_percent  
  
    -- Loop the cursor  
    WHILE (@@FETCH_STATUS = 0)  
    BEGIN  
        -- Calculate this coinsurers premium and si.  
        SELECT  @out_annual_premium = @sd_this_premium_original * @ci_share_percent / 100,  
                @out_annual_premium_home = @sd_this_premium_home * @ci_share_percent / 100,  
                @out_annual_premium_system = @sd_this_premium_system * @ci_share_percent / 100,  
                @out_sum_insured_home = @sd_sum_insured_home * @ci_share_percent / 100  
  
        -- Post current assets account rather than c/i account for open and maintain claim.  
        IF @transaction_type_code IN ('C_CO', 'C_CR')  
            SELECT  @ci_party_cnt = 0,  
                    @ci_shortname = 'CLMOSCI' + @sd_class_of_business_code,  
                    @ci_party_type_id = 0  
  
        INSERT INTO stats_detail  
               (stats_folder_cnt, stats_detail_id, stats_detail_type,  
                risk_id, risk_type_id, risk_type_code,  
                peril_id, peril_description, peril_type_id, peril_type_code,  
                policy_section_type_id, policy_section_type_code,  
                class_of_business_id, class_of_business_code,  
                tax_type_id, tax_type_code, tax_value,  
                ri_party_cnt, ri_shortname, ri_party_type, ri_share_percent,  
                annual_premium, currency_code, currency_rate,  
                this_premium_original, this_premium_home, this_premium_system, sum_insured_home)  
        SELECT  @stats_folder_cnt, MAX(stats_detail_id) + 1, 'COI',  
                @sd_risk_id, @sd_risk_type_id, @sd_risk_type_code,  
                @sd_peril_id, @sd_peril_description, @sd_peril_type_id, @sd_peril_type_code,  
                @sd_policy_section_type_id, @sd_policy_section_type_code,  
                @sd_class_of_business_id, @sd_class_of_business_code,  
                @sd_tax_type_id, @sd_tax_type_code, @sd_tax_value,  
                @ci_party_cnt, @ci_shortname, 'COI', @ci_share_percent,  
                @sd_annual_premium, @sd_currency_code, @sd_currency_rate,  
                @out_annual_premium, @out_annual_premium_home, @out_annual_premium_system, @out_sum_insured_home  
        FROM    stats_detail  
        WHERE   stats_folder_cnt = @stats_folder_cnt  
  
        -- Get next record  
        FETCH NEXT FROM CoinsurerCursor  
        INTO    @ci_party_cnt,  
                @ci_party_type_id,  
                @ci_shortname,  
                @ci_share_percent  
  
    END -- of CoinsurerCursor loop  
  
    -- Close this cursor but don't deallocate, we may need it again  
    CLOSE CoinsurerCursor  
  
    FETCH NEXT FROM StatsCursor  
    INTO    @sd_stats_detail_type,  
            @sd_risk_id,  
            @sd_risk_type_id,  
            @sd_risk_type_code,  
            @sd_peril_id,  
            @sd_peril_description,  
  @sd_peril_type_id,  
            @sd_peril_type_code,  
            @sd_policy_section_type_id,  
            @sd_policy_section_type_code,  
            @sd_class_of_business_id,  
            @sd_class_of_business_code,  
            @sd_tax_type_id,  
            @sd_tax_type_code,  
            @sd_tax_value,  
            @sd_annual_premium,  
            @sd_currency_code,  
            @sd_currency_rate,  
            @sd_this_premium_original,  
            @sd_this_premium_home,  
            @sd_this_premium_system,  
            @sd_sum_insured_home  
  
END -- of StatsCursor loop  
  
-- Close and deallocate stats cursor, also deallocate coinsurer cursor now  
CLOSE StatsCursor  
DEALLOCATE StatsCursor  
DEALLOCATE CoinsurerCursor  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
