SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Existing_Instalment_Plan_Payment_Details' 
GO

/*******************************************************************************************************/
/* spu_SAM_Get_Existing_Instalment_Plan_Payment_Details     */                                                                              
/* Get Plan Details for Existing Installment plans */
/*******************************************************************************************************/

CREATE PROCEDURE spu_SAM_Get_Existing_Instalment_Plan_Payment_Details 
  @Renewal_Insurance_File_Cnt INT
AS

SET NOCOUNT ON

DECLARE @Ins_File_Cnt INT

If Exists(Select null 
	  From 
		PFPremiumFinance
	  Where
		Insurance_File_Cnt = @Renewal_Insurance_File_Cnt
	 )	
BEGIN
	SET @Ins_File_Cnt = @Renewal_Insurance_File_Cnt
END
Else
BEGIN
	SELECT  
		@Ins_File_Cnt = Insurance_File_Cnt 
	FROM 
		Renewal_status
	WHERE
		Renewal_Insurance_File_Cnt = @Renewal_Insurance_File_Cnt
END

SELECT  TOP 1 
	BankName,
	BankSortCode,
	BankAccountNo,
	BankAccountName,
	BankBranch,
	BankAddr1,
	BankAddr2,
	BankAddr3,
	BankAreaCode,
	BankFaxAreaCode,
	BankFaxNo,
	BankTown,
	BankPCode,
	BankPhoneNo,
	BankRegion,
	BankCountry,
	BankExtension,
	cc_issue,
	cc_number,
	cc_expiry_date,
	cc_start_date,
	cc_pin,
	cardholder_name,
	cardholder_address1,
	cardholder_address2,
	cardholder_address3,
	cardholder_address4,
	cardholder_postcode,
	card_type,
	(SELECT TOP 1 Code
	 FROM
	        Country 
	 WHERE
		Country.description = PFPremiumFinance.ClientCountry
	) Client_CountryCode
	,(SELECT TOP 1 Code
	 FROM
	        Country 
	 WHERE
		Country.description = PFPremiumFinance.BankCountry
	) Bank_CountryCode,
	pfrf_id,
	SchemeNo,
	SchemeVersion
FROM
	PFPremiumFinance
WHERE
	Insurance_File_Cnt	= @Ins_File_Cnt	

ORDER BY pfprem_finance_cnt DESC

SET NOCOUNT OFF

GO



