SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_PFRFValidDates_Sel_All
GO

CREATE PROCEDURE spu_PFRFValidDates_Sel_All
    @date DATETIME,
    @productFamily VARCHAR(3),
    @has_client_fees BIT,
    @schemeno INT = NULL,
    @pffrequency_id INT = NULL,
    @mediatype_id INT = NULL,
    @bCallFromCM BIT  = 0
AS

SELECT
    CASE f.period
        WHEN 'w' THEN 52 / f.amount
        WHEN 'm' THEN 12 / f.amount
    END AS frequency,
    pfrf.companyno,
    pfrf.schemeno,
    pfrf.schemeversion,
    pfrf.daysdelay,
    pfrf.start_limit,
    pfrf.align_to,
    f.period,
    pfrf.deposit_type,
    pfrf.depositpc,
    pfrf.deposit_charged_to,
    pfrf.pfrf_id,
    pfrf.mininterest,
    pfrf.protection_type,
    pfrf.protectrate,
    pfrf.min1,
    pfrf.max1,
    pfrf.rate1,
    pfrf.r1com,
    NULL obselete1,
    pfrf.min2,
    pfrf.max2,
    pfrf.rate2,
    pfrf.r2com,
    NULL obselete2,
    pfrf.min3,
    pfrf.max3,
    pfrf.rate3,
    pfrf.r3com,
    NULL obselete3,
    pfrf.min4,
    pfrf.max4,
    pfrf.rate4,
    pfrf.r4com,
    NULL obselete4,
    pfrf.min5,
    pfrf.max5,
    pfrf.rate5,
    pfrf.r5com,
    NULL obselete5,
    pfrf.fee_type,
    pfrf.arrangementfee,
    pfrf.fee_charged_to,
    pfrf.fee_charged_to,
    pfrf.protection_charged_to,
    s.schemename,
    pfrf.pffrequency_id,
    f.description,
    mt.mediatype_id,
    mt.description,
    pfrf.productfamily,
    st.code,
    mtv.code,
    p.name,
    f.amount,
    pfrf.existing_days_delay,
    pfrf.advance_instalments,
    pfrf.review_pmuser_group_id,
    pfrf.remainder_amount_threshhold,
    pfrf.remainder_amount_at_end,
    pfrf.maximum_instalments,
    pfrf.minmtainstalments,
    s.currency_id,
    pfrf.backdated_rollup_to,
    s.provider_website,
    p.shortname,
    s.provider_username,
    s.provider_password,
    s.provider_brokerid,
    s.provider_timeout,
    s.plbrokerid,
    s.clbrokerid,
    s.provider_prem_threshold,
    pfrf.minmta,
    s.tax_group_id,
    s.allow_client_fees,
    s.xsl_code,
    s.limittransactions,
    s.transactionlimit,
    pfrf.finance_net_commission,
	s.deposit_as_instalment,  
    s.branch_code_mandatory,  
    s.branch_name_mandatory,  
    s.bank_name_mandatory,  
    s.bank_address_mandatory,pfrf.single_instalment_per_month,pfrf.first_instalment_align_with_day_in_month,
	pfrf.is_deposit_override_allowed ,
	pfrf.apply_fee_percentages_to_fees,
	pfrf.apply_fee_percentages_to_taxes
FROM pfrf pfrf
JOIN pfscheme s
    ON s.companyno = pfrf.companyno
    AND s.schemeno = pfrf.schemeno
    AND s.schemeversion = pfrf.schemeversion
JOIN mediatype mt
    ON mt.mediatype_id = s.mediatype_id
JOIN pffrequency f
    ON f.pffrequency_id = pfrf.pffrequency_id
JOIN mediatype_validation mtv
    ON mtv.mediatype_validation_id = mt.mediatype_validation_id
JOIN pfscheme_type st
    ON st.pfscheme_type_id = s.pfscheme_type_id
LEFT JOIN party p
    ON p.party_cnt = s.party_cnt
WHERE @date BETWEEN s.startdate AND s.enddate
AND @date BETWEEN pfrf.startdate AND pfrf.enddate
AND (
        pfrf.productfamily = @productfamily
        OR  
        (  @bCallFromCM = 1 AND
         pfrf.productfamily IN ('NB', 'REN')  
            ) 
 --PN 44860
        /*OR
        (
            pfrf.productfamily IN ('NB', 'REN')
            AND
            @productfamily <> 'MTA'
        )*/
        OR
        pfrf.productfamily = 'SG')
AND s.quoteableind = 'Y'
AND (
        (
            @has_client_fees = 1
            AND
            s.allow_client_fees = 1
        )
        OR
        @has_client_fees = 0
    )
AND pfrf.schemeno = ISNULL(@schemeno, pfrf.schemeno)
AND pfrf.pffrequency_id = ISNULL(@pffrequency_id, pfrf.pffrequency_id)
AND s.mediatype_id = ISNULL(@mediatype_id, s.mediatype_id)
ORDER BY
    pfrf.companyno,
    pfrf.schemeno ASC,
    pfrf.schemeversion ASC,
    frequency DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

