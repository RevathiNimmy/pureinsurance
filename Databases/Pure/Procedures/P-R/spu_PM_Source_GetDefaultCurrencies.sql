EXECUTE DDLDropProcedure 'spu_PM_Source_GetDefaultCurrencies'
GO

CREATE PROCEDURE spu_PM_Source_GetDefaultCurrencies
AS
/****************************************************************************************/
/* spu_PM_Source_GetDefaultCurrencies returns default currencies for the*/
/*branch                                                                */
/****************************************************************************************/
/* Revision Description of Modification		Date       	Who   */
/* -------- ---------------------------         ----  		---   */
/* 1.0      Original                            27/06/1997  	AG    */
/****************************************************************************************/

CREATE TABLE #Currencies
(
	currency_id SMALLINT,
	description VARCHAR(50)
)

INSERT INTO #Currencies
SELECT C.currency_id, C.description
FROM 	Currency C
INNER JOIN PMSystem P ON C.currency_id = P.currency_id AND P.system_id = 1

SELECT description FROM #currencies
GROUP BY description

DROP TABLE #currencies

