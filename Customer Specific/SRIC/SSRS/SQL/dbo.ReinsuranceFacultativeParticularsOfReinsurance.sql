CREATE PROCEDURE [dbo].[ReinsuranceFacultativeParticularsOfReinsurance]
	@risk_id			INT,
	@ri_band			INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	COALESCE(fac.Type, fac_original.type) AS type,
			COALESCE(fac.reinsurance_company_name,fac_original.reinsurance_company_name) AS reinsurance_company_name,
			fac.this_share_percent,
			fac.commission_percent,
			COALESCE(fac.agreement_code,fac_original.agreement_code) AS agreement_code,
			fac.sum_insured,
			fac.premium_value,
			fac.commission_value,
			fac.lower_limit,
			fac.line_limit,
			CASE 
				WHEN fac_original.premium_value IS NULL THEN NULL
				WHEN fac_original.premium_value =0 THEN NULL
				ELSE fac_original.this_share_percent 
			END AS this_share_percent_original,
			CASE 
				WHEN fac_original.premium_value IS NULL THEN NULL
				WHEN fac_original.premium_value =0 THEN NULL
				ELSE fac_original.commission_percent	
			END AS commission_percent_original,
			fac_original.sum_insured		AS sum_insured_original,
			CASE 
				WHEN fac_original.premium_value IS NULL THEN NULL
				WHEN fac_original.premium_value =0 THEN NULL
				ELSE fac_original.premium_value		
			END AS premium_value_original,
			fac_original.commission_value	AS commission_value_original,
			fac_original.lower_limit		AS lower_limit_original,
			fac_original.line_limit			AS line_limit_original
			
	FROM
	(
		SELECT		dbo.ReinsuranceFacultativeView.*
		FROM		dbo.ReinsuranceFacultativeView
		INNER JOIN	dbo.RI_Arrangement ON dbo.ReinsuranceFacultativeView.ri_arrangement_id =  dbo.RI_Arrangement.ri_arrangement_id
		WHERE		dbo.RI_Arrangement.risk_cnt = @risk_id AND dbo.RI_Arrangement.ri_band_id = @ri_band AND dbo.RI_Arrangement.original_flag = 0
		) AS fac
	FULL JOIN
	(
		SELECT		dbo.ReinsuranceFacultativeView.*
		FROM		dbo.ReinsuranceFacultativeView
		INNER JOIN	dbo.RI_Arrangement ON dbo.ReinsuranceFacultativeView.ri_arrangement_id =  dbo.RI_Arrangement.ri_arrangement_id
		INNER JOIN	dbo.Insurance_File_Risk_Link ON dbo.RI_Arrangement.risk_cnt = dbo.insurance_file_risk_link.original_risk_cnt AND dbo.Insurance_File_Risk_Link.risk_cnt = @risk_id 
		WHERE		 dbo.RI_Arrangement.ri_band_id = @ri_band AND dbo.RI_Arrangement.original_flag = 0
	) AS fac_original ON fac.type = fac_original.type AND fac.party_cnt =fac_original.party_cnt
	ORDER BY reinsurance_company_name ASC

END





GO


