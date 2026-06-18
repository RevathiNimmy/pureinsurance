
CREATE PROCEDURE [dbo].[ReinsuranceFacultativeParticularsOfInsurance]
	@risk_id			INT,
	@ri_band			INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	COALESCE(perils.peril_type_id, perils_Original.peril_type_id)						AS peril_type_id,
			COALESCE(perils.peril_type_code, perils_Original.peril_type_code)					AS peril_type_code,
			COALESCE(perils.peril_type_description, perils_Original.peril_type_description)		AS peril_type_description,
			COALESCE(perils.sum_insured, perils_Original.sum_insured)							AS sum_insured,
			COALESCE(perils.this_premium,0)														AS debit_premium,
			COALESCE(perils_Original.this_premium,0)											AS credit_premium,
			COALESCE(perils.this_premium,0)	 - COALESCE(perils_Original.this_premium,0)			AS transaction_premium,
			COALESCE(perils.total_ri_premium,perils_Original.total_ri_premium)	 				AS transaction_ri_premium,
			COALESCE(perils.total_ri_sum_insured,perils_Original.total_ri_sum_insured) 			AS transaction_sum_insured
			
	FROM  (SELECT  dbo.ReinsurancePerilView.peril_type_id,
			dbo.ReinsurancePerilView.peril_type_code,
			dbo.ReinsurancePerilView.peril_type_description,
			dbo.ReinsurancePerilView.sum_insured,
			dbo.ReinsurancePerilView.this_premium,
			dbo.ReinsurancePerilView.total_ri_premium,
			dbo.ReinsurancePerilView.total_ri_sum_insured 
	FROM	dbo.ReinsurancePerilView
	WHERE	dbo.ReinsurancePerilView.risk_cnt = @risk_id AND dbo.ReinsurancePerilView.ri_band =@ri_band  ) AS perils
	FULL OUTER JOIN 
	(SELECT  dbo.ReinsurancePerilView.peril_type_id,
			dbo.ReinsurancePerilView.peril_type_code,
			dbo.ReinsurancePerilView.peril_type_description,
			dbo.ReinsurancePerilView.sum_insured,
			dbo.ReinsurancePerilView.this_premium,
			dbo.ReinsurancePerilView.total_ri_premium,
			dbo.ReinsurancePerilView.total_ri_sum_insured  
	FROM	dbo.ReinsurancePerilView
	INNER JOIN dbo.Insurance_File_Risk_Link ON dbo.ReinsurancePerilView.risk_cnt = dbo.insurance_file_risk_link.original_risk_cnt  
	WHERE	dbo.Insurance_File_Risk_Link.Risk_cnt  = @risk_id AND dbo.ReinsurancePerilView.ri_band =@ri_band  ) AS perils_Original ON  perils.peril_type_id = perils_Original.peril_type_id

END





GO


