
CREATE PROCEDURE [dbo].[ReinsuranceFacultativeDetails]
	@risk_id			INT,
	@ri_band			INT,
	@insurance_file_cnt INT = null
AS
BEGIN
	SET NOCOUNT ON;
	SELECT		dbo.RI_Band.ri_band_id,
				dbo.RI_Band.code													AS ri_band_code,
				dbo.RI_Band.description												AS ri_band_description,
				dbo.Insurance_File.insurance_ref,
				--dbo.Insurance_File.insured_name,
				dbo.Stats_Folder.insurance_holder_name								AS insured_name,	
				dbo.Insurance_File.inception_date_tpi,
				dbo.Insurance_File.cover_start_date,
				dbo.Insurance_File.expiry_date,
				dbo.Insurance_File.renewal_date,
				dbo.Insurance_File.system_base_date,
				dbo.Insurance_File.anniversary_date,
				dbo.Insurance_File.policy_version,
				dbo.Risk_Type.code													AS risk_type_code,
				dbo.Risk_Type.description											AS risk_type_description,
				dbo.Risk.description												AS risk_description,
				dbo.Stats_Folder.document_ref,
				dbo.Stats_Folder.accounting_date,
				RIGHT(CONVERT(varchar, dbo.Stats_Folder.accounting_date, 103),7)	AS accounting_period,
				dbo.Transaction_Type.code											AS transaction_type_code,
				dbo.Transaction_Type.description									AS transaction_type_description,
				CAST(etana.GetOriginalRiskId(dbo.RI_Arrangement.risk_cnt) AS varchar(10)) +	'/' + CAST(dbo.RI_Band.ri_band_id AS varchar(10)) AS slip_number,
				dbo.Insurance_File.currency_id,
				RTRIM(dbo.Currency.description)										AS Currency
	FROM		dbo.RI_Arrangement
	INNER JOIN	dbo.RI_Band						ON dbo.RI_Band.ri_band_id = dbo.RI_Arrangement.ri_band_id
	INNER JOIN	dbo.Risk						ON dbo.Risk.risk_cnt = dbo.RI_Arrangement.risk_cnt
	INNER JOIN	dbo.Risk_Type					ON dbo.Risk_Type.risk_type_id = dbo.Risk.risk_type_id
	INNER JOIN	dbo.insurance_file_risk_link	ON dbo.insurance_file_risk_link.risk_cnt = dbo.Risk.risk_cnt
	INNER JOIN	dbo.Insurance_File				ON dbo.Insurance_File.insurance_file_cnt = dbo.insurance_file_risk_link.insurance_file_cnt
	INNER JOIN	dbo.Insurance_Folder			ON dbo.Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt
	INNER JOIN  dbo.Stats_Folder				ON dbo.Stats_Folder.insurance_file_cnt = dbo.Insurance_File.insurance_file_cnt AND dbo.Stats_Folder.transaction_type_code IN ('MTR', 'REN', 'MTA', 'MTC','NB') 
	INNER JOIN	dbo.Transaction_Type			ON dbo.Transaction_Type.transaction_type_id = dbo.Stats_Folder.transaction_type_id
	INNER JOIN  dbo.Currency					ON dbo.Currency.currency_id = dbo.Insurance_File.currency_id
	WHERE	dbo.RI_Arrangement.risk_cnt = @risk_id 
	AND		dbo.RI_Arrangement.ri_band_id = @ri_band
	AND		dbo.RI_Arrangement.original_flag=0
	AND     ( @insurance_file_cnt is null or dbo.Insurance_File.insurance_file_cnt = @insurance_file_cnt )
END