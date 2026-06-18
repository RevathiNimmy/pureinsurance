SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON  
GO


EXECUTE DDLDropProcedure 'spu_wp_PFPremiumFinancePolicies'
GO


CREATE PROCEDURE spu_wp_PFPremiumFinancePolicies
    @PartyCnt INT, 
    @InsuranceFileCnt INT, 
    @RiskID INT, 
    @ClaimCnt INT, 
    @DocumentRef VARCHAR(25), 
    @Instance1 INT, 
    @Instance2 INT, 
    @Instance3 INT
AS

DECLARE @pfprem_finance_cnt INT
DECLARE @pfprem_finance_version INT
DECLARE @SchemeNo INT
DECLARE @SchemeVersion INT
DECLARE @SchemeType VARCHAR(50)

IF ISNUMERIC(@DocumentRef) = 1
BEGIN
    /*@DocumentRef contains the pfprem_finance_cnt*/
    SELECT @pfprem_finance_cnt = CAST(@DocumentRef AS INT)
END
ELSE
BEGIN
    /*Try to find the pfprem_finance_cnt from the insurance_file_cnt*/
    SELECT  
        @pfprem_finance_cnt = MAX(pfprem_finance_cnt)
    FROM pfpremiumfinance 
    WHERE insurance_file_cnt = @InsuranceFileCnt
    
    IF @pfprem_finance_cnt IS NULL
    BEGIN
        SELECT  
            @pfprem_finance_cnt = MAX(pfprem_finance_cnt)
        FROM pftransaction_id 
        WHERE insurance_file_cnt = @InsuranceFileCnt
    END
END

/*Now get the latest version of that finance plan*/
SELECT  
    @pfprem_finance_version = MAX(pfprem_finance_version)
FROM pfpremiumfinance 
WHERE pfprem_finance_cnt = @pfprem_finance_cnt

-- Get the scheme details
SELECT  
    @SchemeNo = SchemeNo,@SchemeVersion=SchemeVersion
FROM pfpremiumfinance
WHERE pfprem_finance_cnt = @pfprem_finance_cnt 
AND pfprem_finance_version = @pfprem_finance_version

SELECT @SchemeType=description FROM pfscheme_type pft 
	INNER JOIN pfscheme pf ON pft.pfscheme_type_id=pf.pfscheme_type_id 
WHERE schemeno=@SchemeNo and schemeversion=@SchemeVersion

SELECT  
    CONVERT(VARCHAR(10), iff.cover_start_date, 103) 'cover_start_date', 
    rf.number_of_months, 
    p.shortname 'name', 
    CASE 
        (
            SELECT
                ISNULL(SUM(1), 0)
            FROM source 
            WHERE source_id = iff.source_id 
            AND underwriting_branch_ind = 1 
            AND iff.alternate_reference IS NOT NULL
        )
        WHEN 0 THEN
            iff.insurance_ref
        ELSE
            iff.alternate_reference
    END 'insurance_ref', 
    r.description, 
    (
        SELECT
            CONVERT(VARCHAR(100), SUM(CAST(ISNULL(currency_amount, 0) AS MONEY)), 1)
        FROM transdetail
        WHERE transdetail_id = pf.pftransaction_id
    ) 'currency_amount',    
    CONVERT(VARCHAR(10), iff.expiry_date, 103) 'expiry_date', 
    fol.inception_date, 
    CONVERT(VARCHAR(21), p.shortname) 'name21', 
    CONVERT(VARCHAR(31), r.description) 'description31', 
    CONVERT(VARCHAR(18), r.description) 'description19',
    @SchemeType 'Scheme_type'

FROM pftransaction_id pf 
JOIN insurance_file iff 
    ON iff.insurance_file_cnt = pf.insurance_file_cnt 
JOIN insurance_folder fol 
    ON fol.insurance_folder_cnt = iff.insurance_folder_cnt 
LEFT JOIN party p 
    ON p.party_cnt = iff.lead_insurer_cnt 
JOIN renewal_frequency rf 
    ON rf.renewal_frequency_id = iff.renewal_frequency_id 
LEFT JOIN risk_code r 
    ON r.risk_code_id = iff.risk_code_id 
WHERE pf.pfprem_finance_cnt = @pfprem_finance_cnt 
AND pf.pfprem_finance_version = @pfprem_finance_version
--AND pf.insurance_file_cnt = @Instance2


GO


