SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_updstatus'
GO
CREATE PROCEDURE spe_PFInstalments_updstatus
    @pfinstalments_id AS INT,
    @Status AS INT = NULL,
    @TransactionCode AS INT = NULL,
	@isUsingDocRef as BIT = 0  
AS
DECLARE @pfprem_finance_cnt INT
DECLARE @pfprem_finance_version INT
DECLARE @DueDate AS DATETIME
DECLARE @IsParentPlan AS TINYINT
--PN67437
DECLARE @RetryLimit AS INT
SELECT
    @pfprem_finance_cnt = P.pfprem_finance_cnt,
    @pfprem_finance_version = P.pfprem_finance_version,
    @IsParentPlan = P.IsParentPlan,
    @DueDate = I.DueDate
FROM
    PFInstalments I
    INNER JOIN
        PFPremiumFinance P
    ON
        P.pfprem_finance_cnt = I.pfprem_finance_cnt
    AND P.pfprem_finance_version = I.pfprem_finance_version
WHERE
    I.pfinstalments_id=@pfinstalments_id
IF NOT @Status IS NULL
BEGIN
    IF @Status = 7
    BEGIN
        UPDATE
            PFPremiumFinance
        SET
            statusind = '140'
        WHERE
            pfprem_finance_cnt = @pfprem_finance_cnt
        AND pfprem_finance_version = @pfprem_finance_version
        AND (statusind = '040' or statusind = '900')
    END
    IF @Status = 1 OR @Status = 5 OR @Status = 6
    BEGIN
        UPDATE
            PFPremiumFinance
        SET
            statusind = '040'
        WHERE
            pfprem_finance_cnt = @pfprem_finance_cnt
        AND pfprem_finance_version = @pfprem_finance_version
        AND statusind = '900'
    END
    IF @Status=2
    BEGIN
        UPDATE
            PFInstalments
        SET
            BatchExportDate=getdate()
        WHERE
            pfinstalments_id=@pfinstalments_id
        AND BatchExportDate IS NULL
    END

	--PN67437 : If Status passed is Chargeback then set failure count to retrying limit and set as Failed				
    IF @Status=10  
    BEGIN  
		
		SET @Status=6		
		
		SELECT @RetryLimit= ISNULL(pfrf.retry_limit,0)  	  
		FROM pfinstalments  	  
			INNER JOIN pfpremiumfinance ON  
			pfinstalments.pfprem_finance_cnt = pfpremiumfinance.pfprem_finance_cnt  
			AND pfinstalments.pfprem_finance_version = pfpremiumfinance.pfprem_finance_version  	  
			INNER JOIN pfrf ON  
			pfpremiumfinance.pfrf_id = pfrf.pfrf_id  	  
		WHERE pfinstalments.pfinstalments_id = @pfinstalments_id 
 
        UPDATE  
            PFInstalments  
        SET  
            failure_count= @RetryLimit  
        WHERE  
            pfinstalments_id=@pfinstalments_id
	END

    UPDATE
        PFInstalments
    SET
        Status = @Status
    WHERE
        pfinstalments_id=@pfinstalments_id

    IF @IsParentPlan = 1
    BEGIN
        UPDATE
            PFInstalments
        SET
            Status = @Status
        FROM
            PFInstalments I
            INNER JOIN
                PFPremiumFinance P
            ON
                P.pfprem_finance_cnt = I.pfprem_finance_cnt
            AND P.pfprem_finance_version = I.pfprem_finance_version
        WHERE
            P.parent_finance_cnt = @pfprem_finance_cnt
        AND P.parent_finance_version = @pfprem_finance_version
        AND I.DueDate = @DueDate
    END
END
IF NOT @TransactionCode IS NULL
BEGIN
    UPDATE
        PFInstalments
    SET
        TransactionCode = @TransactionCode
    WHERE
        pfinstalments_id=@pfinstalments_id
    IF @IsParentPlan = 1
    BEGIN
        UPDATE
            PFInstalments
        SET
            TransactionCode = @TransactionCode
        FROM
            PFInstalments I
            INNER JOIN
                PFPremiumFinance P
            ON
                P.pfprem_finance_cnt = I.pfprem_finance_cnt
            AND P.pfprem_finance_version = I.pfprem_finance_version
        WHERE
            P.parent_finance_cnt = @pfprem_finance_cnt
        AND P.parent_finance_version = @pfprem_finance_version
        AND I.DueDate = @DueDate
    END
END
IF @isUsingDocRef <> 0 and @Status = 3  
Begin  
  UPDATE  
      PFPremiumFinance  
  SET  
      statusind = '040'  
  WHERE  
      pfprem_finance_cnt = @pfprem_finance_cnt  
  AND pfprem_finance_version = @pfprem_finance_version  
  AND (statusind = '140' or statusind = '900')  
End 
GO
