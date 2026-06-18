SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_ClientsCorporate'
GO

/*******************************************************************************************************/
/* spu_SAM_Get_ClientsCorporate     */                                                                              
/* Get the Corporate Client Details */
/*******************************************************************************************************/

CREATE PROCEDURE spu_SAM_Get_ClientsCorporate    
 @party_cnt int
AS    

SET NOCOUNT ON
    
SELECT 
	 ClientID
	,TradingName
	,MainContact
	,Business
	,Trade
	,SICCode
	,SICDescription
	,CompanyReg
	,NumberOfOffices
	,NumberOfEmployees
	,TradingSince
	,WageRoll
	,VATCode
	,Turnover
	,FinancialYear
FROM   ClientsCorporate  
WHERE  ClientID = @party_cnt    

SET NOCOUNT OFF 
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO







