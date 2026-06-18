SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PM_Source_ValidateCurrencies'
GO

CREATE PROCEDURE spu_PM_Source_ValidateCurrencies
    @source_id integer
AS

CREATE TABLE #Currencies
(
	currency_id SMALLINT,
	description VARCHAR(50)
)

INSERT INTO #Currencies 
SELECT 	DISTINCT C.currency_id, C.description
FROM 	Currency C
JOIN	TransDetail T ON T.currency_id=C.currency_id
WHERE	T.company_id=@source_id
AND		C.currency_id NOT IN (SELECT currency_id 
				FROM CompanyCurrency 
				WHERE company_id=@source_id)

INSERT INTO #Currencies 
SELECT 	DISTINCT C.currency_id, C.description
FROM 	Currency C
JOIN	Insurance_File iff ON iff.currency_id=C.currency_id
WHERE	iff.source_id=@source_id
AND		C.currency_id NOT IN (SELECT currency_id 
				FROM CompanyCurrency 
				WHERE company_id=@source_id)

INSERT INTO #Currencies 
SELECT 	DISTINCT C.currency_id, C.description
FROM 	Currency C
JOIN	Claim CL ON CL.currency_id=C.currency_id
JOIN	Insurance_File iff ON iff.insurance_file_cnt=CL.policy_id
WHERE	iff.source_id=@source_id
AND		C.currency_id NOT IN (SELECT currency_id 
				FROM CompanyCurrency 
				WHERE company_id=@source_id)

INSERT INTO #Currencies 
SELECT 	DISTINCT C.currency_id, C.description
FROM 	Currency C
JOIN	Account A ON A.currency_id=C.currency_id
WHERE	A.company_id=@source_id
AND		C.currency_id NOT IN (SELECT currency_id 
				FROM CompanyCurrency 
				WHERE company_id=@source_id)

--AG - 08/10/2004 - PN 15638
--To insert system currency as default to each branch
--**************************START
INSERT INTO #Currencies
SELECT 	DISTINCT C.currency_id, C.description
FROM 	Currency C
JOIN 	PMSystem P ON P.system_id = 1 
AND	C.currency_id = P.currency_id
AND	C.currency_id NOT IN (SELECT currency_id
				FROM CompanyCurrency
				WHERE company_id = @source_id)
--**************************END

INSERT INTO CompanyCurrency (currency_id, company_id)
SELECT currency_id, @source_id
FROM #currencies
GROUP BY currency_id

SELECT description FROM #currencies
GROUP BY description

DROP TABLE #currencies

GO