SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Bank_Account_Balance'
GO
CREATE PROCEDURE spu_Report_Bank_Account_Balance
    @company_id int, 
    @start_date datetime,
    @end_date datetime
AS

/* Bank Account Balance */
SELECT DISTINCT
    account_id = Account2.account_id,
    bank_name = Bank.bank_name,
    bank_account_no = BankAccount.bank_account_no,
    bank_account_name = BankAccount.bank_account_name,
    Date = TransDetail.accounting_date,
    amount = TransDetail.amount,
    sum_amount = NULL,
    Reference = Document.document_ref,
    Type = DocumentType.code,
    Type_Desc = DocumentType.description,
    document_sequence = TransDetail2.document_sequence,
    Broker_Ac_Name = Account2.account_name,
    Ac_Code = Account2.short_code,
    Details = NULL
FROM
    Bank Bank,
    BankAccount BankAccount,
    TransDetail TransDetail,
    Document Document,
    TransDetail TransDetail2,
    DocumentType DocumentType,
    Account Account2,
    Ledger Ledger
WHERE
    Bank.bank_id = BankAccount.bank_id
    AND BankAccount.account_id = TransDetail.account_id
    AND TransDetail.document_id = Document.document_id
    AND Document.document_id = TransDetail2.document_id
    AND Document.documenttype_id = DocumentType.documenttype_id
    AND TransDetail2.account_id = Account2.account_id
    AND Transdetail2.accounting_date > @start_date
    AND Transdetail2.accounting_date < @end_date
    AND Account2.ledger_id = Ledger.ledger_id
    AND Ledger.ledger_short_name = 'SA'
    AND Document.company_id = @company_id
UNION
SELECT DISTINCT
    account_id = Account2.account_id,
    bank_name = MAX(Bank.bank_name),
    bank_account_no = MAX(BankAccount.bank_account_no),
    bank_account_name = MAX(BankAccount.bank_account_name),
    Date = DATEADD(day, -1, @start_date),
    amount = NULL,
    sum_amount = sum(transdetail.amount),
    Reference = NULL,
    Type = NULL,
    Type_Desc = NULL,
    document_sequence = NULL,

    Broker_Ac_Name = NULL,
    Ac_Code = NULL,
    Details = NULL
FROM
        BankAccount BankAccount,
        TransDetail TransDetail,
        Account Account2,
    Bank Bank

WHERE
    BankAccount.account_id = TransDetail.account_id
    AND TransDetail.account_id = Account2.account_id
        AND TransDetail.accounting_date < @start_date
    AND BankAccount.bank_id = Bank.bank_id
    AND Transdetail.company_id = @company_id
GROUP BY Account2.account_id

GO

