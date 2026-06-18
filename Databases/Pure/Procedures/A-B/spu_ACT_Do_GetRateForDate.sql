SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_GetRateForDate'
GO


CREATE PROCEDURE spu_ACT_Do_GetRateForDate
 @currency_id int,
 @rate_date datetime
 
 AS
 
 SELECT 
 	currency_id,
 	effective_from,
 	rate_against_base
 
 FROM  	CurrencyRate
 WHERE 	currency_id = @currency_id
 AND	effective_from =
 		(SELECT 	max (effective_from)
 		FROM 	CurrencyRate
 		WHERE 	currency_id = @currency_id
 		AND	effective_from <= @rate_date )
 GO



