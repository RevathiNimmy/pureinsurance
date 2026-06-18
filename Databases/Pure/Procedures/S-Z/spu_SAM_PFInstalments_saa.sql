EXECUTE DDLDropProcedure 'spu_SAM_PFInstalments_saa'
GO
CREATE  PROCEDURE spu_SAM_PFInstalments_saa
    @nPfprem_finance_cnt INT,
    @nPfprem_finance_version INT
AS
BEGIN

    DECLARE @InstalmentCount INT
    DECLARE @InstalmentsProcessed INT

    SELECT  @InstalmentCount = NoOfInstallments
    FROM    PFPremiumFinance WITH(NOLOCK)
    WHERE   pfprem_finance_cnt = @npfprem_finance_cnt
    AND     pfprem_finance_version = @npfprem_finance_version

    SELECT  @InstalmentsProcessed = COUNT(*)
    FROM    PFInstalments WITH(NOLOCK)
    WHERE   pfprem_finance_cnt = @npfprem_finance_cnt
    AND     pfprem_finance_version = @npfprem_finance_version
    AND     Status <> 1
    AND     TransactionCode > 2

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
    PFIS.description as status_description,
    PFIS.code as status_Code,
    PFInstalments_History.posted_date,
    PFInstalments_History.pfinstalments_history_id,
    pfinstalments_status.description,
    CASE WHEN  PFI.Status = 3 THEN 'Collected' ELSE  PFIR.description END As pfinstalments_resultDesc ,
    CASE WHEN PFI.write_off_reason_id > 0 THEN WOR.[description]
	ELSE CASE WHEN PFI.Status = 3 THEN 'Collected'
	ELSE PFIR.description END END AS reason_description,
    pfinstalments_result.code As pfinstalments_resultCode,
	Curr.[description] as currency_desc
    FROM
    PFInstalments PFI WITH(NOLOCK)
    LEFT JOIN
    PFInstalments_result PFIR WITH(NOLOCK) ON PFIR.pfinstalments_result_id=PFI.pfinstalments_result_id

    LEFT JOIN pfInstalments_transaction PFIT WITH(NOLOCK) ON
    PFI.transactioncode = PFIT.pfinstalments_transaction_id

    LEFT JOIN pfInstalments_status PFIS WITH(NOLOCK) ON
    PFI.status = PFIS.pfinstalments_status_id


	LEFT JOIN PFPremiumFinance pfp  WITH(NOLOCK) ON
	pfi.pfprem_finance_cnt = pfp.pfprem_finance_cnt
	AND pfi.pfprem_finance_version = pfp.pfprem_finance_version

	LEFT JOIN Insurance_File ifi WITH(NOLOCK) ON
	ifi.insurance_file_cnt = pfp.Insurance_File_Cnt

	LEFT JOIN Currency Curr WITH(NOLOCK) ON
	ifi.currency_id = Curr.currency_id 

    LEFT JOIN Batch WITH(NOLOCK) ON
    PFI.batch_Id = Batch.batch_Id

    LEFT JOIN PFInstalments_History ON
    PFInstalments_History.pfinstalments_id = PFI.pfinstalments_id
    LEFT JOIN pfinstalments_status ON
    pfinstalments_status.pfinstalments_status_id = pfinstalments_history.pfinstalments_status_id

    LEFT JOIN pfinstalments_result ON
    pfinstalments_result.pfinstalments_result_id = pfinstalments_history.pfinstalments_result_id

	LEFT JOIN Write_Off_Reason WOR ON
	WOR.write_off_reason_id = PFI.write_off_reason_id

    WHERE
    PFI.pfprem_finance_cnt = @npfprem_finance_cnt
    AND
    PFI.pfprem_finance_version = @npfprem_finance_version
    AND
    PFI.TransactionCode > 2
    ORDER BY
    PFI.InstalmentNumber,
    PFInstalments_History.pfinstalments_history_id

END


GO
