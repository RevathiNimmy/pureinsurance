SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_PreviouslyUsedCCNumbers'
GO

-- Get a list of previously used Credit Card numbers (in reverse date used order).
-- If we don't have an insurance file cnt then source direct from the @account_id
-- else source via all cashlistitem recs for that ins file cnt. We will need to
-- restrict selection to matching cards IF an issuer is passed in though.

-- Note to exclude any cc's that have expired and also link to MediaType_Issuer
-- to return the min and max amount values (for interface validation) and if this
-- is flagged as a claim type payment then check that claims are allowed else for
-- other payments then just check the is_allowed flag. Also exclude any that have
-- a MediaType_Issuer that is deleted.

-- Note that we don't need to return transaction_date but since we used GROUP BY
-- then SQL forces us to! We have used MIN and GROUP BY to ensure we only get one
-- entry back per unique card (if same card is used > once we only want one entry).
-- You will however get one entry per card with different expiry dates or names.

-- We will also return all card related info. for use when doing a payment.

CREATE PROCEDURE spu_ACT_Select_PreviouslyUsedCCNumbers
    @account_id         int,
    @insurance_file_cnt     int,
    @mediatype_issuer_id    int,
    @is_claim_type_payment      tinyint
AS

IF @insurance_file_cnt = 0

    BEGIN

    SELECT DISTINCT
            CLI.cc_number,
            ISNULL(MTI.min_amount,0) AS min_amount,
            ISNULL(MTI.max_amount,0) AS max_amount,
            MIN(CLI.transaction_date) AS transaction_date,
            CLI.cc_expiry_date,
            CLI.cc_start_date,
            CLI.cc_issue,
            CLI.cc_pin,
            CLI.cc_name,
            CLI.cc_customer
    INTO    #CCNumbers
    FROM    CashListItem CLI
    LEFT  JOIN mediatype_issuer MTI ON MTI.mediatype_issuer_id = CLI.mediatype_issuer_id
    INNER JOIN CashList CL ON CL.cashlist_id=CLI.cashlist_id
    INNER JOIN MediaType MT ON MT.mediatype_id=CLI.mediatype_id
    WHERE   CLI.account_id=@account_id
    AND     ((@mediatype_issuer_id = 0)
            OR
            (@mediatype_issuer_id > 0 AND CLI.mediatype_issuer_id = @mediatype_issuer_id))
    AND     (MTI.is_deleted = 0 OR @mediatype_issuer_id = 0)
    AND     (MTI.effective_date <= getdate() OR @mediatype_issuer_id = 0)
    AND     ((@is_claim_type_payment = 0 AND MTI.is_allowed=1)
        OR
            (@is_claim_type_payment = 1 AND MTI.is_allowed=1 AND MTI.is_allowed_for_claims=1)
        OR  @mediatype_issuer_id = 0)
    AND     CL.cashlisttype_id=2            -- Receipts only
    AND     MT.mediatype_validation_id=2    -- Credit Cards
    AND     CLI.cc_expiry_date <> ''
    GROUP BY
            CLI.cc_number,
            MTI.min_amount,
            MTI.max_amount,
            CLI.cc_expiry_date,
            CLI.cc_start_date,
            CLI.cc_issue,
            CLI.cc_pin,
            CLI.cc_name,
            CLI.cc_customer

    SELECT * FROM #CCNumbers
    WHERE   getdate() < DATEADD(MONTH, 1, CAST(LEFT(cc_expiry_date,2) + '/01/' + SUBSTRING(cc_expiry_date,4,2) as datetime))
    ORDER BY transaction_date DESC

    DROP TABLE #CCNumbers

    END

ELSE

    BEGIN

    SELECT DISTINCT
            CLI.cc_number,
            ISNULL(MTI.min_amount,0) AS min_amount,
            ISNULL(MTI.max_amount,0) max_amount,
            MIN(CLI.transaction_date) AS transaction_date,
            CLI.cc_expiry_date,
            CLI.cc_start_date,
            CLI.cc_issue,
            CLI.cc_pin,
            CLI.cc_name,
            CLI.cc_customer
    INTO    #CCNumbersIF
    FROM    CashListItem CLI
    INNER JOIN Insurance_File IFF ON IFF.cashlistitem_id=CLI.cashlistitem_id
    LEFT  JOIN mediatype_issuer MTI ON MTI.mediatype_issuer_id = CLI.mediatype_issuer_id
    INNER JOIN CashList CL ON CL.cashlist_id=CLI.cashlist_id
    INNER JOIN MediaType MT ON MT.mediatype_id=CLI.mediatype_id
    WHERE   IFF.insurance_file_cnt=@insurance_file_cnt
    AND     ((@mediatype_issuer_id = 0)
        OR
            (@mediatype_issuer_id > 0 AND CLI.mediatype_issuer_id = @mediatype_issuer_id))
    AND     cc_expiry_date<>''
    AND     (MTI.is_deleted = 0 OR @mediatype_issuer_id = 0)
    AND     (MTI.effective_date <= getdate() OR @mediatype_issuer_id = 0)
    AND     ((@is_claim_type_payment = 0 AND MTI.is_allowed=1)
        OR
            (@is_claim_type_payment = 1 AND MTI.is_allowed=1 AND MTI.is_allowed_for_claims = 1)
        OR
            @mediatype_issuer_id = 0)
    AND     CL.cashlisttype_id=2            -- Receipts only
    AND     MT.mediatype_validation_id=2    -- Credit Cards
    GROUP BY
            CLI.cc_number,
            MTI.min_amount,
            MTI.max_amount,
            CLI.cc_expiry_date,
            CLI.cc_start_date,
            CLI.cc_issue,
            CLI.cc_pin,
            CLI.cc_name,
            CLI.cc_customer

    SELECT * FROM #CCNumbersIF
    WHERE   getdate() < DATEADD(MONTH, 1, CAST(LEFT(cc_expiry_date,2) + '/01/' + SUBSTRING(cc_expiry_date,4,2) as datetime))
    ORDER BY transaction_date DESC

    DROP TABLE #CCNumbersIF

    END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

