
/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select details from pfpremiumfinance table
Test Code     : Exec spu_GetSchemeNosForOriginalAndRenewalPolicies
 */
SET QUOTED_IDENTIFIER OFF
GO

Execute DDLDropProcedure 'spu_GetSchemeNosForOriginalAndRenewalPolicies'
GO

CREATE PROCEDURE spu_GetSchemeNosForOriginalAndRenewalPolicies

	@nOriginal_Insurance_File_Cnt INT,
	@nRenewal_Insurance_File_Cnt INT

AS
BEGIN

	SELECT SchemeNo, ISNULL(noofinstallments,0) 
	FROM pfpremiumfinance 
	WHERE insurance_file_cnt in 
	(@nOriginal_Insurance_File_Cnt,@nRenewal_Insurance_File_Cnt) 

END   

