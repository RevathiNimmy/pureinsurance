SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_ReceiptPayment_currency'
GO
-- MJ27032002 created from spu_Report_ReceiptPayment
-- DC201101 added bank as extra parameter
-- ECK  06042004 Added Document Type 43 for Installment Receipt.
CREATE PROCEDURE spu_Report_ReceiptPayment_currency
    @branch_id int,
    @start_date datetime,
    @end_date datetime,
    @bank varchar(60) -- DC201101
AS

DECLARE @sStartDate varchar(60),
    @sEndDate varchar(60),
    @sBank varchar(60)

SELECT @sStartDate = CONVERT(varchar(60), @start_date),
    @sEndDate = CONVERT(varchar(60), @end_date)

--DC201101 -check if Bank passed
SELECT @sBank = ISNULL(@Bank, 'ALL')

DECLARE @sBalBF char(25),
    @sBalCF char(25),
-- Empty Temporary table
    @sDelSQL varchar(255),
-- Insert into temporary table
    @sInsSQL1 varchar(255),
    @sInsSQL2 varchar(255),
-- BF if no transactions before start date
    @sBF1SQL1 varchar(255),
    @sBF1SQL2 varchar(255),
    @sBF1SQL3 varchar(255),
    @sBF1SQL4 varchar(255),
    @sBF1SQL5 varchar(255),
    @sBF1SQL6 varchar(255),
    @sBF1SQL6a varchar(255),
    @sBF1SQL7 varchar(255),
-- BF if transactions before start date
    @sBF2SQL1 varchar(255),
    @sBF2SQL2 varchar(255),
    @sBF2SQL3 varchar(255),
    @sBF2SQL4 varchar(255),
    @sBF2SQL5 varchar(255),
    @sBF2SQL6 varchar(255),
    @sBF2SQL7 varchar(255),
    @sBF2SQL8 varchar(255),
-- Transactions in period - Receipts & Payments
    @sDet1SQL1 varchar(255),
    @sDet1SQL1a varchar(255),
    @sDet1SQL2 varchar(255),
    @sDet1SQL2a varchar(255), --PN11448
    @sDet1SQL3 varchar(255),
    @sDet1SQL4 varchar(255),
    @sDet1SQL5 varchar(255),
    @sDet1SQL6 varchar(255),
    @sDet1SQL7 varchar(255),
-- Transactions in period - Journals Bank sequence = 1
    @sDet2SQL1 varchar(255),
    @sDet2SQL2 varchar(255),
    @sDet2SQL3 varchar(255),
    @sDet2SQL4 varchar(255),
    @sDet2SQL5 varchar(255),
    @sDet2SQL6 varchar(255),
    @sDet2SQL7 varchar(255),
-- Transactions in period - Journals Account sequence = 1
    @sDet3SQL1 varchar(255),
    @sDet3SQL2 varchar(255),
    @sDet3SQL3 varchar(255),
    @sDet3SQL4 varchar(255),
    @sDet3SQL5 varchar(255),
    @sDet3SQL6 varchar(255),
    @sDet3SQL7 varchar(255),
-- Transactions in period - Journals Bank sequence = 1, Journals Account amount only
    @sDet4SQL1 varchar(255),
    @sDet4SQL2 varchar(255),
    @sDet4SQL3 varchar(255),
    @sDet4SQL4 varchar(255),
    @sDet4SQL5 varchar(255),
    @sDet4SQL6 varchar(255),
    @sDet4SQL7 varchar(255),
-- Transactions in period - Journals Account sequence = 1, Journals Account amount only
    @sDet5SQL1 varchar(255),
    @sDet5SQL2 varchar(255),
    @sDet5SQL3 varchar(255),
    @sDet5SQL4 varchar(255),
    @sDet5SQL5 varchar(255),
    @sDet5SQL6 varchar(255),
    @sDet5SQL7 varchar(255),
-- CF if no transactions before end date
    @sCF1SQL1 varchar(255),
    @sCF1SQL2 varchar(255),
    @sCF1SQL3 varchar(255),
    @sCF1SQL4 varchar(255),
    @sCF1SQL5 varchar(255),
    @sCF1SQL6 varchar(255),
    @sCF1SQL7 varchar(255),
-- CF if transactions before end date
    @sCF2SQL1 varchar(255),
    @sCF2SQL2 varchar(255),
    @sCF2SQL3 varchar(255),
    @sCF2SQL4 varchar(255),
    @sCF2SQL5 varchar(255),
    @sCF2SQL6 varchar(255),
    @sCF2SQL7 varchar(255),
    @sCF2SQL7a varchar(255),
    @sCF2SQL8 varchar(255),
-- Output SQL
    @sOutSQL1 varchar(255),
    @sOutSQL1A varchar(255),
    @sOutSQL2 varchar(255),
    @sOutSQL2A varchar(255),
    @sOutSQL3 varchar(255),
-- Sort keys
    @sSortSQL varchar(255)

    SET NOCOUNT ON
    SELECT @sBalBF = 'Balance Brought Forward'
    SELECT @sBalCF = 'Balance Carried Forward'

-- Empty Temporary table
    SELECT @sDelSQL = ' DELETE FROM Report_PaymentAndReceipt'

-- Insert into temporary table
    SELECT @sInsSQL1 = ' INSERT INTO' +
               ' Report_PaymentAndReceipt(sort_field, ' +
               ' bank_account_code, ' +
               ' bank_account_name, ' +
               ' bf_bank_amount, ' +
               ' bank_amount, ' +
               ' cf_bank_amount, ' +
               ' account_code, '
    SELECT @sInsSQL2 = ' account_amount, ' +
               ' policy_number, ' +
               ' document_date, ' +
               ' documenttype_id, ' +
               ' document_ref, ' +
               ' bank_branch_id, ' +
               ' bank_branch_name, ' +
               ' account_branch_id, ' +
               ' account_branch_name, ' +
               ' method, ' +
               ' document_sequence, ' +
               ' media_ref)'

-- Construct SQL for BF if no transactions before start date
    SELECT @sBF1SQL1 = ' SELECT 0, ' +
               ' BAA.short_code, ' +
               ' B.bank_name, ' +
               ' 0.0, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' ''' + @sStartDate + ''', ' +
               ' null, ' +
               ' ''' + @sBalBF + ''', ' +
               ' BA.company_id, ' +
               ' BC.description, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null'
    SELECT @sBF1SQL2 = ' FROM BankAccount BA, ' +
               ' Account BAA, ' +
               ' Bank B, ' +
               ' Company BC'
    SELECT @sBF1SQL3 = ' WHERE BAA.account_id = BA.account_id' +
               ' AND B.bank_id = BA.bank_id' +
               ' AND BC.company_id = BA.company_id'

    if @branch_id > 0
        SELECT @sBF1SQL3 = @sBF1SQL3 + ' AND BA.company_id = ' + ltrim(str(@branch_id))
    SELECT @sBF1SQL4 = ' AND BAA.account_id not in (SELECT BTD.account_id'
    SELECT @sBF1SQL5 = ' FROM BankAccount BA, ' +
               ' Account BAA, ' +
               ' Bank B, ' +
               ' TransDetail BTD, ' +
               ' Document D, ' +
               ' Company BC'
    SELECT @sBF1SQL6 = ' WHERE BTD.account_id = BA.account_id' +
               ' AND BAA.account_id = BTD.account_id' +
               ' AND B.bank_id = BA.bank_id' +
               ' AND D.document_id = BTD.document_id' +
               ' AND BC.company_id = BTD.company_id' +
               ' AND D.documenttype_id IN (1, 10, 12, 22, 23, 28, 29, 43)'
    
    if @branch_id > 0
        SELECT @sBF1SQL6a = ' AND BTD.company_id = ' + ltrim(str(@branch_id))
    
    SELECT @sBF1SQL7 = ' AND D.document_date < ''' + @sStartDate + ''')'

-- Construct SQL for BF if transactions before start date
    SELECT @sBF2SQL1 = ' SELECT 0, ' +
               ' BAA.short_code, ' +
               ' B.bank_name, ' +
               ' sum(BTD.currency_amount), ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' ''' + @sStartDate + ''', ' +
               ' null, ' +
               ' ''' + @sBalBF + ''', ' +
               ' BTD.company_id, ' +
               ' BC.description, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null '
    SELECT @sBF2SQL2 = ' FROM BankAccount BA, ' +
               ' Account BAA, ' +
               ' Bank B, ' +
               ' TransDetail BTD, ' +
               ' Document D, ' +
               ' Company BC'
    SELECT @sBF2SQL3 = ' WHERE BTD.account_id = BA.account_id' +
               ' AND BAA.account_id = BTD.account_id' +
               ' AND B.bank_id = BA.bank_id' +
               ' AND D.document_id = BTD.document_id' +
               ' AND BC.company_id = BTD.company_id' +
               ' AND D.documenttype_id IN (1, 10, 12, 22, 23, 28, 29, 43)'
    SELECT @sBF2SQL4 = ' AND D.document_date < ''' + @sStartDate + ''''
    if @branch_id > 0
        SELECT @sBF2SQL4 = @sBF2SQL4 + ' AND BTD.company_id = ' + ltrim(str(@branch_id))
    SELECT @sBF2SQL5 = ' AND BAA.account_id in (SELECT BTD.account_id'
    SELECT @sBF2SQL6 = ' FROM BankAccount BA, ' +
               ' Account BAA, ' +
               ' Bank B, ' +
               ' TransDetail BTD, ' +
               ' Document D, ' +
               ' Company BC'
    SELECT @sBF2SQL7 = ' WHERE BTD.account_id = BA.account_id' +
               ' AND BAA.account_id = BTD.account_id' +
               ' AND B.bank_id = BA.bank_id' +
               ' AND D.document_id = BTD.document_id' +
               ' AND BC.company_id = BTD.company_id' +
               ' AND D.documenttype_id IN (1, 10, 12, 22, 23, 28, 29, 43)'
    if @branch_id > 0
        SELECT @sBF2SQL7 = @sBF2SQL7 + ' AND BTD.company_id = ' + ltrim(str(@branch_id))
    SELECT @sBF2SQL8 = ' AND D.document_date < ''' + @sStartDate + ''')' +
               ' GROUP BY BTD.company_id, ' +
               ' BC.description, ' +
               ' BAA.short_code, ' +
               ' B.bank_name'

-- Construct SQL for Transactions in period - Receipts & Payments
    SELECT @sDet1SQL1 = ' SELECT 1 sort_field, ' +
                ' BAA.short_code bank_account_code, ' +
                ' B.bank_name bank_account_name, ' +
                ' null bf_bank_amount, ' +
                ' BTD.currency_amount bank_amount, '
    SELECT @sDet1SQL1a =
                ' null cf_bank_amount, ' +
                ' A.short_code account_code, ' +
                ' TD.currency_amount account_amount, ' +
                ' TD.insurance_ref policy_number, '
    SELECT @sDet1SQL2 = ' D.document_date, ' +
                ' D.documenttype_id, ' +
                ' D.document_ref, ' +
                ' BTD.company_id bank_branch_id, ' +
                ' BC.description bank_branch_name, ' +
                ' TD.company_id account_branch_id, ' +
                ' C.description account_branch_name, ' +
                ' MT.description method, ' +
                ' TD.document_sequence, '  
    SELECT @sDet1SQL2a = ' SUBSTRING(TD.comment,3, LEN(TD.comment))'  --PN11448

    SELECT @sDet1SQL3 = ' FROM BankAccount BA, ' +
                ' Account BAA, ' +
                ' Bank B, ' +
                ' TransDetail BTD, ' +
                ' Account A, ' +
                ' Document D, ' +

                ' Company BC, ' +
                ' Company C, '
    SELECT @sDet1SQL4 = ' TransDetail TD' +
                ' LEFT OUTER JOIN CashListItem CLI' +
                ' ON TD.transdetail_id = CLI.transdetail_id' +
                ' LEFT OUTER JOIN MediaType MT' +
                ' ON CLI.mediatype_id = MT.mediatype_id'
    SELECT @sDet1SQL5 = ' WHERE BTD.account_id = BA.account_id' +
                ' AND BAA.account_id = BTD.account_id' +
                ' AND B.bank_id = BA.bank_id' +
                ' AND D.document_id = BTD.document_id' +
                ' AND TD.document_id = D.document_id' +
                ' AND A.account_id = TD.account_id' +
                ' AND BC.company_id = BTD.company_id'
    SELECT @sDet1SQL6 = ' AND D.documenttype_id IN (22, 23, 28, 29, 43)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    if @branch_id > 0
        SELECT @sDet1SQL7 = ' AND BTD.company_id = ' + ltrim(str(@branch_id))

-- Construct SQL for Transactions in period - Journals Bank sequence = 1
    SELECT @sDet2SQL1 = ' SELECT 1, ' +
                ' BAA.short_code, ' +
                ' B.bank_name, ' +
                ' null, ' +
                ' BTD.currency_amount, ' +
                ' null, ' +
                ' A.short_code, ' +
                ' TD.currency_amount, ' +
                ' TD.insurance_ref, ' +
                ' D.document_date, ' +
                ' D.documenttype_id, '
    SELECT @sDet2SQL2 = ' D.document_ref, ' +
                ' BTD.company_id, ' +
                ' BC.description, ' +
                ' TD.company_id, ' +
                ' C.description, ' +
                ' MT.description, ' +
                ' BTD.document_sequence, ' +
                ' SUBSTRING(TD.comment,3, lEN(TD.comment))'  --PN11448
    SELECT @sDet2SQL3 = ' FROM BankAccount BA, ' +
                ' Account BAA, ' +
                ' Bank B, ' +
                ' TransDetail BTD, ' +
                ' Account A, ' +
                ' Document D, ' +
                ' Company BC, ' +
                ' Company C, '
    SELECT @sDet2SQL4 = ' TransDetail TD' +
                ' LEFT OUTER JOIN CashListItem CLI' +
                ' ON TD.transdetail_id = CLI.transdetail_id' +
                ' LEFT OUTER JOIN MediaType MT' +
                ' ON CLI.mediatype_id = MT.mediatype_id'
    SELECT @sDet2SQL5 = ' WHERE BTD.account_id = BA.account_id' +
                ' AND BAA.account_id = BTD.account_id' +
                ' AND B.bank_id = BA.bank_id' +

                ' AND D.document_id = BTD.document_id' +
                ' AND TD.document_id = D.document_id' +
                ' AND A.account_id = TD.account_id' +
                ' AND BC.company_id = BTD.company_id'
    SELECT @sDet2SQL6 = ' AND D.documenttype_id IN (1, 10, 12)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    SELECT @sDet2SQL7 = ' AND BTD.document_sequence = 1 AND' +
                ' TD.document_sequence = 2'
    if @branch_id > 0
        SELECT @sDet2SQL7 = @sDet2SQL7 + ' AND BTD.company_id = ' + ltrim(str(@branch_id))

-- Construct SQL for Transactions in period - Journals Account sequence = 1
    SELECT @sDet3SQL1 = ' SELECT 1, ' +
                ' BAA.short_code, ' +
                ' B.bank_name, ' +
                ' null, ' +
                ' BTD.currency_amount, ' +
                ' null, ' +
                ' A.short_code, ' +
                ' TD.currency_amount, ' +
                ' TD.insurance_ref, ' +
                ' D.document_date, ' +
                ' D.documenttype_id, '
    SELECT @sDet3SQL2 = ' D.document_ref, ' +
                ' BTD.company_id, ' +

                ' BC.description, ' +
                ' TD.company_id, ' +
                ' C.description, ' +
                ' MT.description, ' +
                ' TD.document_sequence, ' +
                ' SUBSTRING(TD.comment,3, lEN(TD.comment))'  --PN11448
    SELECT @sDet3SQL3 = ' FROM BankAccount BA, ' +
                ' Account BAA, ' +
                ' Bank B, ' +
                ' TransDetail BTD, ' +
                ' Account A, ' +
                ' Document D, ' +
                ' Company BC, ' +
                ' Company C, '
    SELECT @sDet3SQL4 = ' TransDetail TD' +
                ' LEFT OUTER JOIN CashListItem CLI' +
                ' ON TD.transdetail_id = CLI.transdetail_id' +
                ' LEFT OUTER JOIN MediaType MT' +
                ' ON CLI.mediatype_id = MT.mediatype_id'
    SELECT @sDet3SQL5 = ' WHERE BTD.account_id = BA.account_id' +
                ' AND BAA.account_id = BTD.account_id' +
                ' AND B.bank_id = BA.bank_id' +
                ' AND D.document_id = BTD.document_id' +
                ' AND TD.document_id = D.document_id' +
                ' AND A.account_id = TD.account_id' +
                ' AND BC.company_id = BTD.company_id'

    SELECT @sDet3SQL6 = ' AND D.documenttype_id IN (1, 10, 12)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +

                ' AND TD.transdetail_id <> BTD.transdetail_id'
    SELECT @sDet3SQL7 = ' AND BTD.document_sequence > 1 AND' +
                ' TD.document_sequence = 1'
    if @branch_id > 0
        SELECT @sDet3SQL7 = @sDet3SQL7 + ' AND BTD.company_id = ' + ltrim(str(@branch_id))

-- Construct SQL for Transactions in period - Journals Bank sequence = 1, Journals Account amount only
    SELECT @sDet4SQL1 = ' SELECT 1, ' +
                ' BAA.short_code, ' +
                ' B.bank_name, ' +
                ' null, ' +
                ' null, ' +
                ' null, ' +
                ' A.short_code, ' +
                ' TD.currency_amount, ' +
                ' TD.insurance_ref, ' +
                ' D.document_date, ' +
                ' D.documenttype_id, '
    SELECT @sDet4SQL2 = ' D.document_ref, ' +
                ' BTD.company_id, ' +
                ' BC.description, ' +
                ' TD.company_id, ' +
                ' C.description, ' +
                ' MT.description, ' +
                ' TD.document_sequence, ' +
                ' SUBSTRING(TD.comment,3, lEN(TD.comment))'  --PN11448
    SELECT @sDet4SQL3 = ' FROM BankAccount BA, ' +
                ' Account BAA, ' +
                ' Bank B, ' +
                ' TransDetail BTD, ' +
                ' Account A, ' +
                ' Document D, ' +
                ' Company BC, ' +
                ' Company C, '
    SELECT @sDet4SQL4 = ' TransDetail TD' +
                ' LEFT OUTER JOIN CashListItem CLI' +
                ' ON TD.transdetail_id = CLI.transdetail_id' +
                ' LEFT OUTER JOIN MediaType MT' +
                ' ON CLI.mediatype_id = MT.mediatype_id'
    SELECT @sDet4SQL5 = ' WHERE BTD.account_id = BA.account_id' +
                ' AND BAA.account_id = BTD.account_id' +
                ' AND B.bank_id = BA.bank_id' +
                ' AND D.document_id = BTD.document_id' +
                ' AND TD.document_id = D.document_id' +
                ' AND A.account_id = TD.account_id' +
                ' AND BC.company_id = BTD.company_id'
    SELECT @sDet4SQL6 = ' AND D.documenttype_id IN (1, 10, 12)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    SELECT @sDet4SQL7 = ' AND BTD.document_sequence = 1 AND' +
                ' TD.document_sequence > 2'
    if @branch_id > 0
        SELECT @sDet4SQL7 = @sDet4SQL7 + ' AND BTD.company_id = ' + ltrim(str(@branch_id))

-- Construct SQL for Transactions in period - Journals Account sequence = 1, Journals Account amount only
    SELECT @sDet5SQL1 = ' SELECT 1, ' +
                ' BAA.short_code, ' +
                ' B.bank_name, ' +
                ' null, ' +
                ' null, ' +
                ' null, ' +
                ' A.short_code, ' +
                ' TD.currency_amount, ' +
                ' TD.insurance_ref, ' +
                ' D.document_date, ' +
                ' D.documenttype_id, '
    SELECT @sDet5SQL2 = ' D.document_ref, ' +
                ' BTD.company_id, ' +
                ' BC.description, ' +
                ' TD.company_id, ' +
                ' C.description, ' +
                ' MT.description, ' +
                ' TD.document_sequence, ' +
                ' SUBSTRING(TD.comment,3, lEN(TD.comment))'  --PN11448
    SELECT @sDet5SQL3 = ' FROM BankAccount BA, ' +
                ' Account BAA, ' +
                ' Bank B, ' +
                ' TransDetail BTD, ' +
                ' Account A, ' +
                ' Document D, ' +
                ' Company BC, ' +

                ' Company C, '
    SELECT @sDet5SQL4 = ' TransDetail TD' +
                ' LEFT OUTER JOIN CashListItem CLI' +
                ' ON TD.transdetail_id = CLI.transdetail_id' +
                ' LEFT OUTER JOIN MediaType MT' +
                ' ON CLI.mediatype_id = MT.mediatype_id'
    SELECT @sDet5SQL5 = ' WHERE BTD.account_id = BA.account_id' +
                ' AND BAA.account_id = BTD.account_id' +
                ' AND B.bank_id = BA.bank_id' +
                ' AND D.document_id = BTD.document_id' +
                ' AND TD.document_id = D.document_id' +
                ' AND A.account_id = TD.account_id' +
                ' AND BC.company_id = BTD.company_id'
    SELECT @sDet5SQL6 = ' AND D.documenttype_id IN (1, 10, 12)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    SELECT @sDet5SQL7 = ' AND BTD.document_sequence > 1 AND' +
                ' TD.document_sequence > 1'
    if @branch_id > 0
        SELECT @sDet5SQL7 = @sDet5SQL7 + ' AND BTD.company_id = ' + ltrim(str(@branch_id))

-- Construct SQL for CF if no transactions before end date
    SELECT @sCF1SQL1 = ' SELECT 9, ' +
               ' BAA.short_code, ' +
               ' B.bank_name, ' +
               ' null, ' +
               ' null, ' +
               ' 0.0, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               '''' + @sEndDate + ''', ' +
               ' null, ' +
               ' ''' + @sBalCF + ''', ' +
               ' BA.company_id, ' +
               ' BC.description, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null'
    SELECT @sCF1SQL2 = ' FROM BankAccount BA, ' +
               ' Account BAA, ' +
               ' Bank B, ' +
               ' Company BC'
    SELECT @sCF1SQL3 = ' WHERE BAA.account_id = BA.account_id' +
               ' AND B.bank_id = BA.bank_id' +
               ' AND BC.company_id = BA.company_id'
    if @branch_id > 0
        SELECT @sCF1SQL3 = @sBF1SQL3 + ' AND BA.company_id = ' + ltrim(str(@branch_id))
    SELECT @sCF1SQL4 = ' AND BAA.account_id not in (SELECT BTD.account_id'
    SELECT @sCF1SQL5 = ' FROM BankAccount BA, ' +
               ' Account BAA, ' +
               ' Bank B, ' +
               ' TransDetail BTD, ' +
               ' Document D, ' +
               ' Company BC'
    SELECT @sCF1SQL6 = ' WHERE BTD.account_id = BA.account_id' +
               ' AND BAA.account_id = BTD.account_id' +
               ' AND B.bank_id = BA.bank_id' +
               ' AND D.document_id = BTD.document_id' +
               ' AND BC.company_id = BTD.company_id' +
               ' AND D.documenttype_id IN (1, 10, 12, 22, 23, 28, 29, 43)'
    if @branch_id > 0
        SELECT @sCF1SQL6 = @sCF1SQL6 + ' AND BTD.company_id = ' + ltrim(str(@branch_id))
    SELECT @sCF1SQL7 = ' AND D.document_date <= ''' + @sEndDate + ''')'

-- Construct SQL for CF if transactions before end date
    SELECT @sCF2SQL1 = ' SELECT 9, ' +
               ' BAA.short_code, ' +
               ' B.bank_name, ' +
               ' null, ' +
               ' null, ' +
               ' sum(BTD.currency_amount), ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' ''' + @sEndDate + ''', ' +
               ' null, ' +
               ' ''' + @sBalCF + ''', ' +
               ' BTD.company_id, ' +
               ' BC.description, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null, ' +
               ' null '
    SELECT @sCF2SQL2 = ' FROM BankAccount BA, ' +
               ' Account BAA, ' +
               ' Bank B, ' +
               ' TransDetail BTD, ' +
               ' Document D, ' +
               ' Company BC'
    SELECT @sCF2SQL3 = ' WHERE BTD.account_id = BA.account_id' +
               ' AND BAA.account_id = BTD.account_id' +
               ' AND B.bank_id = BA.bank_id' +
               ' AND D.document_id = BTD.document_id' +
               ' AND BC.company_id = BTD.company_id' +
               ' AND D.documenttype_id IN (1, 10, 12, 22, 23, 28, 29, 43)'
    SELECT @sCF2SQL4 = ' AND D.document_date <= ''' + @sEndDate + ''''
    if @branch_id > 0
        SELECT @sCF2SQL4 = @sCF2SQL4 + ' AND BTD.company_id = ' + ltrim(str(@branch_id))
    SELECT @sCF2SQL5 = ' AND BAA.account_id in (SELECT BTD.account_id'
    SELECT @sCF2SQL6 = ' FROM BankAccount BA, ' +
               ' Account BAA, ' +
               ' Bank B, ' +
               ' TransDetail BTD, ' +
               ' Document D, ' +
               ' Company BC'
    SELECT @sCF2SQL7 = ' WHERE BTD.account_id = BA.account_id' +
               ' AND BAA.account_id = BTD.account_id' +
               ' AND B.bank_id = BA.bank_id' +
               ' AND D.document_id = BTD.document_id' +
               ' AND BC.company_id = BTD.company_id' +
               ' AND D.documenttype_id IN (1, 10, 12, 22, 23, 28, 29, 43)'
    if @branch_id > 0
        SELECT @sCF2SQL7a = ' AND BTD.company_id = ' + ltrim(str(@branch_id))
    SELECT @sCF2SQL8 = ' AND D.document_date <= ''' + @sEndDate + ''')' +
               ' GROUP BY BTD.company_id, ' +
               ' BC.description, ' +
               ' BAA.short_code, ' +
               ' B.bank_name'

-- Construct Sort SQL string
    SELECT @sSortSQL = ' ORDER BY 13, 2, 1, 10, 12 '

-- Empty the temporary table
    EXECUTE (@sDelSQL)
-- Add data to Temporary table
    IF @start_date = @end_date
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet1SQL1 + @sDet1SQL1a + @sDet1SQL2 + @sDet1SQL2a +
		@sDet1SQL3 + @sDet1SQL4 + @sDet1SQL5 + @sDet1SQL6 + @sDet1SQL7) --PN11448
    ELSE
    BEGIN
-- BF if no transactions before start date
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sBF1SQL1 + @sBF1SQL2 + @sBF1SQL3 + @sBF1SQL4 +
              @sBF1SQL5 + @sBF1SQL6 + @sBF1SQL6a + @sBF1SQL7)
-- BF if transactions before start date
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sBF2SQL1 + @sBF2SQL2 + @sBF2SQL3 + @sBF2SQL4 +
              @sBF2SQL5 + @sBF2SQL6 + @sBF2SQL7 + @sBF2SQL8)
-- Transactions in period - Paymenmts & Receips
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet1SQL1 + @sDet1SQL1a + @sDet1SQL2  + @sDet1SQL2a + 
		@sDet1SQL3 + @sDet1SQL4 + @sDet1SQL5 + @sDet1SQL6 + @sDet1SQL7) --PN11448
-- Transactions in period - Journals Bank = sequence 1
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet2SQL1 + @sDet2SQL2 + @sDet2SQL3 + @sDet2SQL4 +
              @sDet2SQL5 + @sDet2SQL6 + @sDet2SQL7)
-- Transactions in period - Journals Account = sequence 1

        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet3SQL1 + @sDet3SQL2 + @sDet3SQL3 + @sDet3SQL4 +
              @sDet3SQL5 + @sDet3SQL6 + @sDet3SQL7)
-- Transactions in period - Journals Bank = sequence 1, Account detail only
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet4SQL1 + @sDet4SQL2 + @sDet4SQL3 + @sDet4SQL4 +
              @sDet4SQL5 + @sDet4SQL6 + @sDet4SQL7)
-- Transactions in period - Journals Account = sequence 1
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet5SQL1 + @sDet5SQL2 + @sDet5SQL3 + @sDet5SQL4 +
              @sDet5SQL5 + @sDet5SQL6 + @sDet5SQL7)
-- CF if no transactions before end date

        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sCF1SQL1 + @sCF1SQL2 + @sCF1SQL3 + @sCF1SQL4 +
              @sCF1SQL5 + @sCF1SQL6 + @sCF1SQL7)
-- CF if transactions before end date
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sCF2SQL1 + @sCF2SQL2 + @sCF2SQL3 + @sCF2SQL4 +
              @sCF2SQL5 + @sCF2SQL6 + @sCF2SQL7 + @sCF2SQL7a + @sCF2SQL8)
    END

-- Construct Output SQL
    SELECT @sOutSQL1 = ' SELECT sort_field, ' +
               ' bank_account_code, ' +
               ' bank_account_name, ' +
               ' ISNULL(bf_bank_amount, 0) bf_bank_amount, ' +
               ' ISNULL(bank_amount, 0) bank_amount, ' +
               ' ISNULL(cf_bank_amount, 0) cf_bank_amount, ' +
               ' ISNULL(account_code, '''') account_code, '
    SELECT @sOutSQL1A = ' ISNULL(account_amount, 0) account_amount, ' +
              ' ISNULL(policy_number, '''') policy_number, '
    SELECT @sOutSQL2 = ' document_date, ' +
               ' ISNULL(documenttype_id, 0) documenttype_id, ' +
               ' document_ref, ' +
               ' bank_branch_id, ' +
               ' bank_branch_name, ' +
               ' ISNULL(account_branch_id, 0) account_branch_id, ' +
               ' ISNULL(account_branch_name, '''') account_branch_name, ' +
               ' ISNULL(method, '''') method, '
    SELECT @sOutSQL2A = ' ISNULL(document_sequence, 0) document_sequence, ' +
               ' ISNULL(media_ref, '''') media_ref'
    SELECT @sOutSQL3 = ' FROM Report_PaymentAndReceipt'

    --DC201101 added check for specific Bank
    IF RTrim(@sBank) <> 'ALL'
        SELECT @sOutSQL3 = @sOutSQL3 + ' WHERE bank_account_name = ''' + RTrim(@sBank) + ''' '

-- Get the data

    SET NOCOUNT OFF

    EXECUTE (@sOutSQL1 + @sOutSQL1A + @sOutSQL2 + @sOutSQL2A + @sOutSQL3 + @sSortSQL)

/*
    PRINT @sBF1SQL1
    Print @sBF1SQL2
    Print @sBF1SQL3
    print @sBF1SQL4
    print @sBF1SQL5
    print @sBF1SQL6
    print @sBF1SQL7
-- BF if transactions before start date
    PRINT @sBF2SQL1
    print @sBF2SQL2

    print @sBF2SQL3
    print @sBF2SQL4
    print @sBF2SQL5
    print @sBF2SQL6
    print @sBF2SQL7
    print @sBF2SQL8
-- Transactions in period - Receipts & Payments
    PRINT @sDet1SQL1
    print @sDet1SQL2
    print @sDet1SQL3
    print @sDet1SQL4
    print @sDet1SQL5
    Print @sDet1SQL6
    print @sDet1SQL7
-- Transactions in period - Journals - Bank sequence = 1
    PRINT @sDet2SQL1
    print @sDet2SQL2
    print @sDet2SQL3
    print @sDet2SQL4
    print @sDet2SQL5
    Print @sDet2SQL6
    print @sDet2SQL7

-- Transactions in period - Journals - Account sequence = 1
    PRINT @sDet3SQL1

    print @sDet3SQL2
    print @sDet3SQL3
    print @sDet3SQL4
    print @sDet3SQL5
    Print @sDet3SQL6
    print @sDet3SQL7
-- Transactions in period - Journals - Bank sequence = 1, Journals Account amount only
    PRINT @sDet4SQL1
    print @sDet4SQL2
    print @sDet4SQL3
    print @sDet4SQL4
    print @sDet4SQL5
    Print @sDet4SQL6
    print @sDet4SQL7
-- Transactions in period - Journals - Account sequence = 1, Journals Account amount only
    PRINT @sDet5SQL1
    print @sDet5SQL2
    print @sDet5SQL3
    print @sDet5SQL4
    print @sDet5SQL5
    Print @sDet5SQL6
    print @sDet5SQL7
-- CF if no transactions before end date
    PRINT @sCF1SQL1
    print @sCF1SQL2
    print @sCF1SQL3
    print @sCF1SQL4
    print @sCF1SQL5
    print @sCF1SQL6
    print @sCF1SQL7
-- CF if transactions before end date
    PRINT @sCF2SQL1
    print @sCF2SQL2
    print @sCF2SQL3
    print @sCF2SQL4
    print @sCF2SQL5
    print @sCF2SQL6
    print @sCF2SQL7
    print @sCF2SQL8
-- Sort keys
    PRINT @sSortSQL
*/

GO

