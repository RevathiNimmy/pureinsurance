SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_Instalments_Paid_To_Date'
GO

CREATE PROCEDURE spu_ACT_Get_Instalments_Paid_To_Date
    @pfpremium_finance_cnt INT,      
    @pfpremium_finance_version INT = NULL,      
 @FirstInstallmentFailureDate DATETIME = NULL OUTPUT    
    
AS      
      
BEGIN      
    	DECLARE @AmountToFinance NUMERIC(20,4)
	DECLARE @AmountCollected NUMERIC(20,4)	
	DECLARE @TotalFinanceVersions NUMERIC(20,4)
	DECLARE @inception_date_tpi DateTime

	CREATE TABLE #temp 
	(pfprem_finance_cnt INT,
	pfprem_finance_version INT,
	TotalCost numeric(20,4))
       
   INSERT INTO #temp (pfprem_finance_cnt,pfprem_finance_version,TotalCost)
	SELECT pf.pfprem_finance_cnt,pf.pfprem_finance_version,pf.TotalCost FROM PFPremiumFinance pf
		INNER JOIN insurance_file inf ON inf.insurance_file_cnt=pf.Insurance_File_Cnt
		INNER JOIN insurance_file i ON i.insurance_folder_cnt=inf.insurance_folder_cnt
		INNER JOIN PFPremiumFinance p ON p.Insurance_File_Cnt=i.insurance_file_cnt
		WHERE p.pfprem_finance_cnt = @pfpremium_finance_cnt AND (@pfpremium_finance_version = NULL OR p.pfprem_finance_version = @pfpremium_finance_version)
		AND pf.StatusInd IN ('040','900','990','999','140') AND inf.inception_date_tpi=i.inception_date_tpi
    

    SELECT @AmountCollected = SUM(pfi.amount)      
      FROM PFInstalments pfi       
	  INNER JOIN #temp ON 
      pfi.pfprem_finance_cnt = #temp.pfprem_finance_cnt AND
       pfi.pfprem_finance_version = #temp.pfprem_finance_version
       AND pfi.status = 3 -- Collected

    DECLARE @premium_changed_amount NUMERIC(20,4) = 0
    DECLARE @ThisTotalCost NUMERIC(20,4) 
	DECLARE @PreviousTotalCost NUMERIC(20,4) = 0
	DECLARE @temp_pfrem_finance_cnt INT
	DECLARE @temp_pfrem_finance_ver INT
	DECLARE @bFirstVersion BIT = 0
	DECLARE @LastVersionCollectedAmount NUMERIC(20,4)


	  DECLARE cur_GetAlteredPremium cursor fast_forward FOR  
			SELECT TotalCost,pfprem_finance_cnt,pfprem_finance_version FROM #temp ORDER BY pfprem_finance_cnt DESC,pfprem_finance_version DESC
			OPEN cur_GetAlteredPremium 
				FETCH NEXT FROM cur_GetAlteredPremium INTO @ThisTotalCost,@temp_pfrem_finance_cnt, @temp_pfrem_finance_ver   
					WHILE (@@FETCH_STATUS = 0) BEGIN 
					IF @PreviousTotalCost<>0
					SELECT @premium_changed_amount = @premium_changed_amount + (@PreviousTotalCost-@ThisTotalCost)
			
					IF @bFirstVersion=0 
					SELECT @LastVersionCollectedAmount = SUM(pfi.amount) FROM PFInstalments pfi WHERE
						pfi.pfprem_finance_cnt = @temp_pfrem_finance_cnt AND
						pfi.pfprem_finance_version = @temp_pfrem_finance_ver
       AND pfi.status = 3 -- Collected      
					SELECT @bFirstVersion = 1
					SELECT @PreviousTotalCost = @ThisTotalCost
						  
    FETCH NEXT FROM cur_GetAlteredPremium  INTO @ThisTotalCost,@temp_pfrem_finance_cnt, @temp_pfrem_finance_ver   
    END  
    CLOSE cur_GetAlteredPremium  

    DEALLOCATE cur_GetAlteredPremium
      
    SELECT @AmountToFinance = ISNULL(@ThisTotalCost,0) + ISNULL(@premium_changed_amount,0) + ISNULL(@AmountCollected,0) - ISNULL(@LastVersionCollectedAmount,0)
    
    SELECT TOP 1 @FirstInstallmentFailureDate = original_DueDate    
      FROM PFInstalments  pfi      
     WHERE pfi.pfprem_finance_cnt = @pfpremium_finance_cnt      
       AND (@pfpremium_finance_version = NULL or pfi.pfprem_finance_version = @pfpremium_finance_version)      
       AND pfi.InstalmentNumber >=1
       AND pfi.status IN (5,6) -- Failed      
    ORDER BY instalmentnumber    
 
	SELECT @inception_date_tpi = inception_date_tpi FROM insurance_file i 
				INNER JOIN PFPremiumFinance pfp ON i.insurance_file_cnt = pfp.Insurance_File_Cnt 
				 WHERE pfp.pfprem_finance_cnt = @pfpremium_finance_cnt
			     AND (@pfpremium_finance_version = NULL or pfp.pfprem_finance_version = @pfpremium_finance_version)

	SELECT	ISNULL(@AmountToFinance,0) ,
		ISNULL(@AmountCollected,0) ,@inception_date_tpi
END      
GO

