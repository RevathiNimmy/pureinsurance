SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO


EXECUTE DDLDropProcedure 'spu_wp_PFPremiumFinancePolicies_get_parent_key'
GO


CREATE PROCEDURE spu_wp_PFPremiumFinancePolicies_get_parent_key
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

SELECT @pfprem_finance_cnt

GO


