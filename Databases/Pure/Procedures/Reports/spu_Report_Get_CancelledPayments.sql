SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Report_Get_CancelledPayments'
GO

CREATE PROCEDURE spu_Report_Get_CancelledPayments
    @branch_id int,
    @start_date datetime,
    @end_date datetime,
    @TypeOfCurrency Varchar(50),
    @GroupByCode Varchar(50),
    @Bank varchar(50)
AS

/*Get System Currency Details*/
DECLARE @SystemCurrencyCode varchar(15)
DECLARE @SystemCurrencyDesc varchar(255)
	SELECT
		@SystemCurrencyCode = c.iso_code,
		@SystemCurrencyDesc = c.description
	FROM PMSystem pms
	JOIN currency c
		ON c.currency_id = pms.currency_id
	WHERE pms.system_id = 1
/*******************/

IF @Bank = 'ALL' OR @Bank=''
SET @Bank = NULL

IF @branch_id =0
SET @branch_id = NULL

BEGIN
SELECT
    P.ShortName,
    P.Resolved_name,
    Case @TypeOfCurrency
        When 'System' Then ISNULL(ROUND(ISNULL(TD.system_amount, 0.0),2), 0.0)
        When 'Base'   Then ISNULL(ROUND(ISNULL(TD.amount, 0.0),2), 0.0)
    END Amount,
    Case @GroupByCode
        	WHEN 'Branch' Then C.company_id
		WHEN 'Branch And Company' Then C.Company_id
                WHEN 'Bank' Then BA.description
		ELSE ' '
		END 'GroupByCode',
    CLI.Transaction_date 'Payment Date',
    CLI.media_ref,
    MT.Code 'Media Type',
    PS.Code,
    --PS.Description 'Cancelled Reason',
	CLRR.Description 'Cancelled Reason',
    CLI.CashListitem_reverse_reason_id,
    CLI.Reversed_date,
    CL.Company_ID,
    D.Document_ref,
    CLIPT.Code 'Payment Type',
    CLI.Payment_name 'Payee Name',
    BA.Bank_Account_name,
    U.UserName,
    CASE @TypeOfCurrency
	         WHEN 'Base' THEN cb.iso_code
		 WHEN 'System' THEN @SystemCurrencyCode
		 END CurrencyCode,
    BK.code

FROM CashListItem CLI

    LEFT JOIN CashList CL
        ON CLI.cashlist_id = CL.cashlist_id
    LEFT JOIN TransDetail TD
        ON CLI.TransDetail_ID = TD.Transdetail_id
    JOIN Document D
        ON D.Document_id = TD.Document_id
    left JOIN Account A
        ON CLI.account_id = A.account_id
    LEFT JOIN CashListItem_Payment_Status PS
        ON CLI.cashlistitem_payment_status_id = PS.cashlistitem_payment_status_id
    LEFT JOIN MediaType MT
        ON CLI.mediaType_id = MT.mediatype_id
    LEFT JOIN CashListItem_Payment_Type CLIPT
        ON CLI.CashListItem_Payment_Type_id = CLIPT.CashListItem_Payment_Type_id
    LEFT JOIN BankAccount BA
        ON CL.BankAccount_id = BA.BankAccount_Id
    LEFT Join Party P
        ON A.Account_key= P.Party_cnt
    LEFT JOIN PMUser U
        ON CLI.pmuser_id= u.user_id
    LEFT JOIN Company C
        ON CL.company_id = C.company_id
    LEFT JOIN Currency CB
	ON CB.Currency_id= C.Base_currency
    LEFT OUTER JOIN Bank BK
	ON BA.bank_id = BK.bank_id
	LEFT OUTER JOIN cashlistitem_reverse_reason CLRR
	ON CLI.cashlistitem_reverse_reason_id = CLRR.cashlistitem_reverse_reason_id
WHERE
     Cl.cashlisttype_id IN ( SELECT  Cashlisttype_id FROM CashListType WHERE Code IN ('P','CP'))
     AND (CLI.Reversed_Date >= @start_date
            AND
            CLI.Reversed_Date <= @end_date)
    --Filter by @branch_id
    AND
       (C.company_id = @branch_id OR @branch_id IS NULL)

    AND( is_reversed IS NOT NULL OR is_Reversed <>0)
    --Filter by @Bank if passed
    AND
    (BA.description = @Bank OR @Bank IS NULL)
END


GO