SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PFPremiumFinancePolicies_get_keys'
GO


CREATE PROCEDURE spu_wp_PFPremiumFinancePolicies_get_keys
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
    
    
SELECT
    insurance_file_cnt
FROM pftransaction_id 
WHERE pfprem_finance_cnt=@pfprem_finance_cnt
AND pfprem_finance_version=@pfprem_finance_version
AND insurance_file_cnt IS NOT NULL /*Do not include client fee lines at the moment.*/
GROUP BY insurance_file_cnt

GO

