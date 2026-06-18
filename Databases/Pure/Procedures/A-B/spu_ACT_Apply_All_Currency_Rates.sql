SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Apply_All_Currency_Rates'
GO

CREATE PROCEDURE spu_ACT_Apply_All_Currency_Rates
@currency_id int
AS

delete from currencyrate
where currency_id = @currency_id
and company_id <> 1

DECLARE @i_CompanyId as int
DECLARE @i_CompanyCurrencyId as int
DECLARE @d_Date as datetime
DECLARE @r_Rate as numeric(12,6) 

DECLARE cCompany CURSOR FAST_FORWARD FOR
SELECT company_id
FROM companycurrency
WHERE company_id <> 1
AND currency_id = @currency_id

DECLARE cRates CURSOR FAST_FORWARD FOR
       
SELECT	rate_against_base, effective_from
FROM 	currencyrate
WHERE 	currency_id = @currency_id
AND 	company_id = 1
       	
OPEN	cRates
FETCH NEXT  FROM cRates INTO @r_Rate, @d_Date

WHILE @@FETCH_STATUS = 0
BEGIN

	OPEN	cCompany

	FETCH NEXT FROM cCompany INTO @i_CompanyId

	WHILE @@FETCH_STATUS = 0
	BEGIN

		INSERT INTO currencyrate(effective_from,rate_against_base,currency_id,company_id)  
		VALUES (@d_Date, @r_Rate, @currency_id, @i_CompanyId)
	
		FETCH NEXT FROM cCompany INTO @i_CompanyId
	END

        FETCH NEXT FROM cRates INTO @r_Rate, @d_Date

	CLOSE cCompany

END

CLOSE 		cRates
DEALLOCATE 	cRates
DEALLOCATE 	cCompany

SET NOCOUNT OFF

GO

