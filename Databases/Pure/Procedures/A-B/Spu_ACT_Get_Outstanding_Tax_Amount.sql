SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_ACT_Get_Outstanding_Tax_Amount'
GO

CREATE PROCEDURE Spu_ACT_Get_Outstanding_Tax_Amount
@Document_id INT,  
@Account_id INT
  
AS  
 
SELECT 	Transdetail_id, (Amount-outstanding_amount) as Amount,(currency_amount-outstanding_currency_amount) as CurrencyAmount 
FROM 	TransDetail TD
	
	INNER JOIN	Document D
	ON		D.Document_id = TD.Document_id

	INNER JOIN	Insurance_File IFL
	ON		IFL.Insurance_File_Cnt = D.Insurance_File_Cnt

	INNER JOIN	Tax_Calculation TC
	ON		TC.Insurance_File_Cnt = IFL.Insurance_File_Cnt

	WHERE 		TD.Document_id=@Document_id  
	AND 		Account_id=@Account_id  
	AND 		TC.include_tax_in_instalments=0  
	AND 		SPARE LIKE '%TAX%'
	
	
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  
