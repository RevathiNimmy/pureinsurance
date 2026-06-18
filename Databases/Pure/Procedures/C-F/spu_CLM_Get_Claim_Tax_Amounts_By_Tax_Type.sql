SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Tax_Amounts_By_Tax_Type'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Tax_Amounts_By_Tax_Type  
 @claim_payment_id int= NULL,  
 @claim_receipt_id int =NULL,
 @claim_receipt_item_id int = NULL  
AS  
/*
	AmendmentDate	AmendedBy		Description
	------------------------------------------------------------------------------------------------------------------------------------------------
	12/03/2020		Vinit Kumar		Changed the proc to use if statements for the params instead of appending to long where clause for performance reasons
*/
BEGIN  
  
	IF @claim_payment_id IS NOT NULL
	BEGIN
		SELECT MIN(tt.description) as description
				, MIN(tt.code) as code
				, SUM(value)as value 
		FROM Tax_Calculation tc
			INNER JOIN Tax_Band tb ON tc.tax_band_id = tb.tax_band_id
			INNER JOIN Tax_Type tt ON tb.tax_type_id = tt.tax_type_id
		WHERE claim_payment_id IS NOT NULL 
			AND	claim_payment_id = @claim_payment_id
		GROUP BY tb.tax_type_id
	End

	IF @claim_receipt_id IS NOT NULL
	BEGIN
		SELECT MIN(tt.description) as description
				, MIN(tt.code) as code
				, SUM(value)as value 
		FROM Tax_Calculation tc
			INNER JOIN Tax_Band tb ON tc.tax_band_id = tb.tax_band_id
			INNER JOIN Tax_Type tt ON tb.tax_type_id = tt.tax_type_id
		WHERE claim_receipt_id IS NOT NULL 
			AND	claim_receipt_id = @claim_receipt_id
		GROUP BY tb.tax_type_id
	END

	IF @claim_receipt_item_id IS NOT NULL
	BEGIN
		SELECT MIN(tt.description) as description
				, MIN(tt.code) as code
				, SUM(value)as value 
		FROM Tax_Calculation tc
			INNER JOIN Tax_Band tb ON tc.tax_band_id = tb.tax_band_id
			INNER JOIN Tax_Type tt ON tb.tax_type_id = tt.tax_type_id
		WHERE claim_receipt_item_id IS NOT NULL 
			AND	claim_receipt_item_id = @claim_receipt_item_id
		GROUP BY tb.tax_type_id
	END

END  

