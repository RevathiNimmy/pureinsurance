SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFInstalments_saa'
GO


CREATE  PROCEDURE spu_PFInstalments_saa  
    @pfprem_finance_cnt int,  
    @pfprem_finance_version int,  
    @batchnumber int = Null,  
    @duedate datetime = Null,  
    @filter int = 0  
AS BEGIN  

    DECLARE @InstalmentCount int  
    DECLARE @InstalmentsProcessed int  
	DECLARE @RemainingInstalments int
  
    SELECT  @InstalmentCount = NoOfInstallments  
    FROM    PFPremiumFinance WITH(NOLOCK)  
    WHERE   pfprem_finance_cnt = @pfprem_finance_cnt  
    AND     pfprem_finance_version = @pfprem_finance_version  
  
    SELECT  @InstalmentsProcessed = Count(*)  
    FROM    PFInstalments WITH(NOLOCK) 
    WHERE   pfprem_finance_cnt = @pfprem_finance_cnt  
    AND     pfprem_finance_version = @pfprem_finance_version  
    AND     Status NOT IN (1,5,6)--1 for new,5 for retrying and 6 for failed so no need to consider as processed
    AND     TransactionCode > 2
	AND     InstalmentNumber <>0
	
    DECLARE @USE_TRANS_CURRENCY AS INT 
    DECLARE @INS_FILE_CNT INT
    DECLARE @CurrencyID int
    DECLARE @CurrencyISOCode VARCHAR(10)
    
    SELECT @USE_TRANS_CURRENCY = ISNULL(use_trans_currency,0),@INS_FILE_CNT = Insurance_File_Cnt   FROM PFPremiumFinance 
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt and pfprem_finance_version = @pfprem_finance_version
    
    SET @CurrencyISOCode = ''
    SET @CurrencyID = 0
    
	SELECT @CurrencyID=currency_id, @CurrencyISOCode = iso_code FROM Currency 
	WHERE currency_id = (Select currency_id  from insurance_file where insurance_file_cnt = @INS_FILE_CNT) 
	
	IF @USE_TRANS_CURRENCY = 0 BEGIN
		SET @CurrencyISOCode = ''
    END
	
  SELECT @RemainingInstalments=Count(*)
  FROM    PFInstalments WITH(NOLOCK)  
    WHERE   pfprem_finance_cnt = @pfprem_finance_cnt  
    AND     pfprem_finance_version = @pfprem_finance_version  
    AND     Status <> 3  
    AND     TransactionCode <> 7 
	AND InstalmentNumber > 0
	
    SELECT  
        PFI.pfprem_finance_cnt,  
        PFI.pfprem_finance_version,  
        PFI.InstalmentNumber,  
        PFI.DueDate,  
        PFI.Fee,  
        PFI.Amount,  
        PFI.TransactionCode,  
        PFI.Status,  
        Batch.Batch_Ref,  
        Batch.Export_Date,  
        PFI.PostedDate,  
        @InstalmentCount As NoOfInstallments,  
        @InstalmentsProcessed As InstalmentsProcessed,  
        PFI.PFTransaction_id,  
        PFI.tax,  
        PFI.commission,  
        PFI.PFInstalments_result_id,  
        PFI.Batch_id,  
        PFIR.description, 
        PFI.PfInstalments_id,
	PFIT.description as transaction_description, 
	CASE WHEN PFI.write_off_reason_id > 0 THEN WOR.[description] ELSE PFIS.description END AS status_description,
	@RemainingInstalments as NoOfRemainingInstalments,
	  PFI.write_off_reason_id,
	   PFIS.description
    FROM  
        PFInstalments PFI WITH(NOLOCK) 
    LEFT JOIN  
        PFInstalments_result PFIR WITH(NOLOCK) ON PFIR.pfinstalments_result_id=PFI.pfinstalments_result_id  

   LEFT JOIN pfInstalments_transaction PFIT WITH(NOLOCK) ON 
	PFI.transactioncode = PFIT.pfinstalments_transaction_id

   LEFT JOIN pfInstalments_status PFIS WITH(NOLOCK) ON 
	PFI.status = PFIS.pfinstalments_status_id

   LEFT JOIN Batch WITH(NOLOCK) ON 
	PFI.batch_Id = Batch.batch_Id

	LEFT JOIN Write_Off_Reason WOR ON
	WOR.write_off_reason_id = PFI.write_off_reason_id

    WHERE  
        PFI.pfprem_finance_cnt = @pfprem_finance_cnt  
    AND  
        PFI.pfprem_finance_version = @pfprem_finance_version  
    AND (  
        (@filter = 0)  
    OR  
        (@filter = 1 AND PFI.TransactionCode > 2)  
    OR  
        (@filter = 2 AND PFI.TransactionCode > 2 AND PFI.Status IN (1,5))  
    OR  
        (@filter = 3 AND PFI.TransactionCode > 2 AND PFI.Status <> 1))  
    AND  
        (PFI.BatchNumber = @batchnumber OR @batchnumber IS NULL)  
    AND  
        (PFI.DueDate < = @duedate OR @duedate IS NULL)  
    ORDER BY  
        PFI.InstalmentNumber asc, PFI.PostedDate desc,  PFI.PFTransaction_id  asc
  
END  




GO
