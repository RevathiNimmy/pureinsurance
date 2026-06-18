SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_PF_CashFlow_SFU'
GO



/**********************************************************************************************************************************
** Created by Kerry Butler
** 03/10/01
**
** NAME:        spu_Report_Report_PF_CashFlow_SFU
**
** PARAMETERS:  NONE IDENTIFIED
**
** 
** DESCRIPTION: Created for Premium Finance Instalments
**              Reports Projected Cash Flow from Instalments on a Monthly basis.
**		Shows 12 months from the current month
**              PMU report PIP used as a basis for contents and layout
**              Currently the only method available for PF is Direct Debit so the CASH (non DD)
**              column values will be zero.
**              I've assumed that entries which go into the cash column are those which are marked for
**              manual collection and so have status of 4
**              We also need to exlude any instalments which have already been paid or which
**              are otherwise N/A.
**              Status types excluded are:
**	 		3.	Collected
**			8.	Write-off
**			9.	Transferred
**		The splitting into months can be done most easily within Crystal
**
** Modified  JT
** 20-10-04
** MultiCurrency Changes
**********************************************************************************************************************************
**
***********************************************************************************************************************************/ 
CREATE PROCEDURE spu_Report_PF_CashFlow_SFU
	@TypeofCurrency	Varchar(30),
	@GroupByCode		Varchar(30)
AS
SET NOCOUNT ON
	
CREATE TABLE #tempPFCashFlow
(
	InstDueDate		Datetime NULL,
	CashValue		decimal(19,4) NULL,
	DDAmount		decimal (19,4) NULL,
	CurrencyCode	Varchar(30) NULL,
	CurrencyDesc	Varchar(255) NULL,
	CompanyCode	Varchar(30) NULL,
	CompanyDesc	Varchar(255) NULL,
--	GroupByCode	Varchar(30) NULL
)
declare @system_currency_id int
-- Get the Consolidated Currency (this is always from System 1)
SELECT
	@system_currency_id=currency_id
FROM
	PMSystem
WHERE
system_id=1


/*Get System Currency Details--jitendra*/
	declare @SystemCurrencyCode varchar(10)
	declare @SystemCurrencyDesc varchar(255)
    SELECT
    	@SystemCurrencyCode = c.iso_code,
    	@SystemCurrencyDesc = c.description
    FROM PMSystem pms
    JOIN currency c
    	ON c.currency_id = pms.currency_id
    WHERE pms.system_id = 1
/*end  Get System Currency*/

	
INSERT into #tempPFCashFlow
	SELECT 
	
		DueDate, 
		Case @Typeofcurrency 
			WHEN 'Base'  THEN 
			(SELECT Amount where p.status like '4')
			WHEN 'System' THEN 
			(SELECT  Amount/ISNULL(CR.rate_against_base,1)
			where p.status like '4')
		END Amount
		,
		Case @Typeofcurrency 
			WHEN 'Base'  THEN 
			(SELECT Amount where p.status NOT like '4')
			WHEN 'System' THEN 
			(SELECT  Amount/ISNULL(CR.rate_against_base,1)
 			 where p.status NOT like '4')
		END Amount1,
		CASE @TypeOfCurrency 
			WHEN 'Base' THEN C.Code
			 WHEN  'System' THEN @SystemCurrencyCode
		END CurrencyCode,
		CASE @TypeOfCurrency 
			WHEN 'Base' THEN C.description
			WHEN  'System' THEN @SystemCurrencyDesc
		END CurrencyDesc,
		S.Code CompanyCode,
		S.Description CompanyName
/*		Case @GroupByCode 
			WHEN 'Branch' THEN S.Code
			WHEN 'Branch And Currency' THEN S.Code
			Else
			''
		END GroupByCode*/
		
	FROM
		PFInstalments p
		JOIN PFPremiuMFinance PF 
		ON p.pfprem_finance_cnt = PF.pfprem_finance_cnt
		JOIN Source S 		ON S.Source_id = PF.Source_id
		JOIN CurrencyRate CR ON CR.Company_id=PF.Source_id
		JOIN Currency C 	ON C.currency_id = CR.currency_id
		WHERE CR.currency_id=@system_currency_id
		AND CR.effective_from IN
			(
				SELECT MAX(effective_from)
				FROM CurrencyRate
				WHERE effective_from <= p.DueDate
				AND	currency_id = CR.currency_id
				AND company_id = CR.company_id
			)
		AND p.status not in (3, 8, 9)

SELECT * FROM #tempPFCashFlow
DROP TABLE #tempPFCashFlow





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

