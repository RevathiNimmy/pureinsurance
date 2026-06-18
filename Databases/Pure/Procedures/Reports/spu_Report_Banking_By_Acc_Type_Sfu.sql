/*Created for making SFU version of spu_Report_Banking_By_Acc_Type 28-09-04*/
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
-- ECK  06042004 Added Document Type 43 for Installment Receipt.
EXECUTE DDLDropProcedure 'spu_Report_Banking_By_Acc_Type_SFU'
GO
CREATE PROCEDURE spu_Report_Banking_By_Acc_Type_SFU
    @branch_id int,
    @start_date nvarchar(50),    
    @end_date nvarchar(50),
    @TypeOfCurrency	varchar(15),
    @GroupByCode	Varchar(50)
AS

DECLARE @sStartDate varchar(60),
    @sEndDate varchar(60)

SELECT @sStartDate = CONVERT(varchar(60), CONVERT(DATETIME, @start_date, 103)),    
    @sEndDate = CONVERT(varchar(60), CONVERT(DATETIME, @end_date, 103)) 
 
 CREATE TABLE #Report_PaymentAndReceipt(
	sort_field int NOT NULL,
	bank_account_code char(20) NOT NULL,
	bank_account_name varchar(60) NULL,
	bf_bank_amount numeric(19, 4) NULL,
	bank_amount numeric(19, 4) NULL,
	cf_bank_amount numeric(19, 4) NULL,
	account_code char(30) NULL,
	account_amount numeric(19, 4) NULL,
	policy_number varchar(30) NULL,
	document_date datetime NULL,
	documenttype_id int NULL,
	document_ref varchar(25) NULL,
	bank_branch_id int NOT NULL,
	bank_branch_name varchar(255) NULL,
	account_branch_id int NULL,
	account_branch_name varchar(255) NULL,
	method varchar(255) NULL,
	document_sequence smallint NULL,
	media_ref varchar(255) NULL,
	transdetail_id int NULL,
	fee_amount money NULL,
	commission_amount money NULL,
	comment varchar(255) NULL,
	issuer varchar(255) NULL,
	payment_or_receipt_type_flag char(1) NULL,
	payment_or_receipt_type_desc varchar(50) NULL
)     



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
    @sBF1SQL7 varchar(255),
-- BF if transactions before start date
    @sBF2SQL1 varchar(500),
    @sBF2SQL2 varchar(255),
    @sBF2SQL3 varchar(255),
    @sBF2SQL4 varchar(255),
    @sBF2SQL5 varchar(255),
    @sBF2SQL6 varchar(255),
    @sBF2SQL7 varchar(255),
    @sBF2SQL8 varchar(255),
-- Transactions in period - Receipts & Payments
    @sDet1SQL1 varchar(500),
    @sDet1SQL2 varchar(255),
    @sDet1SQL3 varchar(255),
    @sDet1SQL4 varchar(255),
    @sDet1SQL5 varchar(255),
    @sDet1SQL6 varchar(255),
    @sDet1SQL7 varchar(255),
-- Transactions in period - Journals Bank sequence = 1
    @sDet2SQL1 varchar(500),
    @sDet2SQL2 varchar(255),
    @sDet2SQL3 varchar(255),
    @sDet2SQL4 varchar(255),
    @sDet2SQL5 varchar(255),
    @sDet2SQL6 varchar(255),
    @sDet2SQL7 varchar(255),
-- Transactions in period - Journals Account sequence = 1
    @sDet3SQL1 varchar(500),
    @sDet3SQL2 varchar(255),
    @sDet3SQL3 varchar(255),
    @sDet3SQL4 varchar(255),
    @sDet3SQL5 varchar(255),
    @sDet3SQL6 varchar(255),
    @sDet3SQL7 varchar(255),
-- Transactions in period - Journals Bank sequence = 1, Journals Account amount only
    @sDet4SQL1 varchar(500),
    @sDet4SQL2 varchar(255),
    @sDet4SQL3 varchar(255),
    @sDet4SQL4 varchar(255),
    @sDet4SQL5 varchar(255),
    @sDet4SQL6 varchar(255),
    @sDet4SQL7 varchar(255),
-- Transactions in period - Journals Account sequence = 1, Journals Account amount only
    @sDet5SQL1 varchar(500),
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
    @sCF2SQL1 varchar(500),
    @sCF2SQL2 varchar(255),
    @sCF2SQL3 varchar(255),
    @sCF2SQL4 varchar(255),
    @sCF2SQL5 varchar(255),
    @sCF2SQL6 varchar(255),
    @sCF2SQL7 varchar(255),
    @sCF2SQL8 varchar(255),
-- Output SQL
    @sOutSQL1 varchar(255),
    @sOutSQL1A varchar(500),
    @sOutSQL2 varchar(255),
    @sOutSQL2A varchar(1000),
    @sOutSQL3 varchar(500),
-- Sort keys
    @sSortSQL varchar(255)
    
   /*Get System Currency Details*/
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

SET NOCOUNT ON
    SELECT @sBalBF = 'Balance Brought Forward'
    SELECT @sBalCF = 'Balance Carried Forward'
    SELECT @sDelSQL = ' DELETE FROM #Report_PaymentAndReceipt'
    SELECT @sInsSQL1 = ' INSERT INTO' +
               ' #Report_PaymentAndReceipt(sort_field, ' +
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
    SELECT @sBF1SQL1 = ' SELECT 0, ' +
               ' BAA.short_code, ' +
               ' B.bank_name, ' +
               ' 0.0, ' +
               ' null, ' +
               ' null, ' +
               ' BAA.short_code, ' +
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
        SELECT @sBF1SQL3 = @sBF1SQL3 + ' AND BAA.company_id = ' + str(@branch_id)
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
               ' AND D.documenttype_id IN (1, 22, 23, 43)'
    if @branch_id > 0
        SELECT @sBF1SQL6 = @sBF1SQL6 + ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sBF1SQL7 = ' AND D.document_date < ''' + @sStartDate + ''')'
    SELECT @sBF2SQL1 = ' SELECT 0, ' +
               ' BAA.short_code, ' +
               ' B.bank_name, ' +
               --' sum(BTD.amount), ' +
               'CASE ''' +  @TypeOfCurrency + '''' +
	       	       	' WHEN ''Base'' THEN ISNULL(ROUND(SUM(BTD.amount),2), 0.0) ' + 
	       	    	' WHEN ''System'' THEN ISNULL(ROUND(SUM(BTD.system_amount),2), 0.0) ' +
	       ' END,' +
               ' null, ' +
               ' null, ' +
               ' BAA.short_code, ' +
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
               ' AND D.documenttype_id IN (1, 22, 23, 43)'
    SELECT @sBF2SQL4 = ' AND D.document_date < ''' + @sStartDate + ''''
    if @branch_id > 0
        SELECT @sBF2SQL4 = @sBF2SQL4 + ' AND BTD.company_id = ' + str(@branch_id)
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
               ' AND D.documenttype_id IN (1, 22, 23, 43)'
    if @branch_id > 0
        SELECT @sBF2SQL7 = @sBF2SQL7 + ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sBF2SQL8 = ' AND D.document_date < ''' + @sStartDate + ''')' +
               ' GROUP BY BTD.company_id, ' +
               ' BC.description, ' +
               ' BAA.short_code, ' +
               ' B.bank_name'
    SELECT @sDet1SQL1 = ' SELECT 1 sort_field, ' +
                ' BAA.short_code bank_account_code, ' +
                ' B.bank_name bank_account_name, ' +
                ' null bf_bank_amount, ' +
                ' CASE ''' +  @TypeOfCurrency + '''' +
	       	' WHEN ''Base'' THEN ISNULL(ROUND(BTD.amount,2), 0.0) ' + 
	    	' WHEN ''System'' THEN ISNULL(ROUND(BTD.system_amount,2), 0.0) ' +
		' END bank_amount,' +
                ' null cf_bank_amount, ' +
                ' A.short_code account_code, ' +
                ' CASE ''' +  @TypeOfCurrency + '''' +
		       	' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0) ' + 
		    	' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0) ' +
		' END account_amount,' +
                ' TD.insurance_ref policy_number, '
    SELECT @sDet1SQL2 = ' D.document_date, ' +
                ' D.documenttype_id, ' +
                ' D.document_ref, ' +
                ' BTD.company_id bank_branch_id, ' +
                ' BC.description bank_branch_name, ' +
                ' TD.company_id account_branch_id, ' +
                ' C.description account_branch_name, ' +
                ' MT.description method, ' +
                ' TD.document_sequence, ' +
                ' CLI.media_ref'
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
    SELECT @sDet1SQL6 = ' AND D.documenttype_id IN (22, 23, 43)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    if @branch_id > 0
        SELECT @sDet1SQL7 = ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sDet2SQL1 = ' SELECT 1, ' +
                ' BAA.short_code, ' +
                ' B.bank_name, ' +
                ' null, ' +
                ' CASE '' ' + @TypeOfCurrency + ''''  +
			' WHEN ''Base'' THEN ISNULL(ROUND(BTD.amount,2), 0.0)' + 
			' WHEN ''System'' THEN ISNULL(ROUND(BTD.system_amount,2), 0.0)' +
		' END,' +
                ' null, ' +
                ' A.short_code, ' +
                ' CASE  ''' +  @TypeOfCurrency + '''' +
			' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0)' + 
			' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0)' +
		' END, ' +
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
                ' CLI.media_ref'
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
    SELECT @sDet2SQL6 = ' AND D.documenttype_id IN (1)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    SELECT @sDet2SQL7 = ' AND BTD.document_sequence = 1 AND' +
                ' TD.document_sequence = 2'
    if @branch_id > 0
        SELECT @sDet2SQL7 = @sDet2SQL7 + ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sDet3SQL1 = ' SELECT 1, ' +
                ' BAA.short_code, ' +
                ' B.bank_name, ' +
                ' null, ' +
        	' CASE  ''' + @TypeOfCurrency + '''' +
			' WHEN ''Base'' THEN ISNULL(ROUND(BTD.amount,2), 0.0)' + 
			' WHEN ''System'' THEN ISNULL(ROUND(BTD.system_amount,2), 0.0)' +
		' END,' +
                ' null, ' +
                ' A.short_code, ' +
                ' CASE  ''' + @TypeOfCurrency + '''' +
			' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0)' + 
			' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0)' +
		' END,' +
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
                ' CLI.media_ref'
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
    SELECT @sDet3SQL6 = ' AND D.documenttype_id IN (1)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    SELECT @sDet3SQL7 = ' AND BTD.document_sequence > 1 AND' +
                ' TD.document_sequence = 1'
    if @branch_id > 0
        SELECT @sDet3SQL7 = @sDet3SQL7 + ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sDet4SQL1 = ' SELECT 1, ' +
                ' BAA.short_code, ' +
                ' B.bank_name, ' +
                ' null, ' +
                ' null, ' +
                ' null, ' +
                ' A.short_code, ' +
                ' CASE ''' +  @TypeOfCurrency + '''' +
			' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0)' + 
			' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0)' +
		' END,' +
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
                ' CLI.media_ref'
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
    SELECT @sDet4SQL6 = ' AND D.documenttype_id IN (1)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    SELECT @sDet4SQL7 = ' AND BTD.document_sequence = 1 AND' +
                ' TD.document_sequence > 2'
    if @branch_id > 0
        SELECT @sDet4SQL7 = @sDet4SQL7 + ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sDet5SQL1 = ' SELECT 1, ' +
                ' BAA.short_code, ' +
                ' B.bank_name, ' +
                ' null, ' +
                ' null, ' +
                ' null, ' +
                ' A.short_code, ' +
                ' CASE ''' +  @TypeOfCurrency + '''' +
			' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0)' + 
			' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0)' +
		' END,' +
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
                ' CLI.media_ref'
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
    SELECT @sDet5SQL6 = ' AND D.documenttype_id IN (1)' +
                ' AND D.document_date >= ''' + @sStartDate + '''' +
                ' AND D.document_date <= ''' + @sEndDate + '''' +
                ' AND C.company_id = TD.company_id' +
                ' AND TD.transdetail_id <> BTD.transdetail_id'
    SELECT @sDet5SQL7 = ' AND BTD.document_sequence > 1 AND' +
                ' TD.document_sequence > 1'
    if @branch_id > 0
        SELECT @sDet5SQL7 = @sDet5SQL7 + ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sCF1SQL1 = ' SELECT 9, ' +
               ' BAA.short_code, ' +
               ' B.bank_name, ' +
               ' null, ' +
               ' null, ' +
               ' 0.0, ' +
               ' BAA.short_code, ' +
               ' null, ' +
               ' null, ' +
               ' ''' + @sEndDate + ''', ' +
               ' null, ' +
               ' '' + @sBalCF + '', ' +
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
        SELECT @sCF1SQL3 = @sBF1SQL3 + ' AND BAA.company_id = ' + str(@branch_id)
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
               ' AND D.documenttype_id IN (1, 22, 23, 43)'
    if @branch_id > 0
		SELECT @sCF1SQL6 = @sCF1SQL6 + ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sCF1SQL7 = ' AND D.document_date <= ''' + @sEndDate + ''')'
    SELECT @sCF2SQL1 = ' SELECT 9, ' +
               ' BAA.short_code, ' +
               ' B.bank_name, ' +
               ' null, ' +
               ' null, ' +
               ' CASE ''' + @TypeOfCurrency + '''' +
	       		' WHEN ''Base'' THEN sum(ISNULL(BTD.amount,0.0))' + 
	       		' WHEN ''System'' THEN sum(ISNULL(BTD.system_amount, 0.0))' +
		' END,' +
               ' BAA.short_code, ' +
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
               ' AND D.documenttype_id IN (1, 22, 23, 43)'
    SELECT @sCF2SQL4 = ' AND D.document_date <= ''' + @sEndDate + ''''
    if @branch_id > 0
        SELECT @sCF2SQL4 = @sCF2SQL4 + ' AND BTD.company_id = ' + str(@branch_id)
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
               ' AND D.documenttype_id IN (1, 22, 23, 43)'
    if @branch_id > 0
        SELECT @sCF2SQL7 = @sCF2SQL7 + ' AND BTD.company_id = ' + str(@branch_id)
    SELECT @sCF2SQL8 = ' AND D.document_date <= ''' + @sEndDate + ''')' +
               ' GROUP BY BTD.company_id, ' +
               ' BC.description, ' +
               ' BAA.short_code, ' +
               ' B.bank_name'
    SELECT @sSortSQL = ' ORDER BY 13, 2, 1, 10, 12 '
    EXECUTE (@sDelSQL)
    IF @start_date = @end_date
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet1SQL1 + @sDet1SQL2 + @sDet1SQL3 +
              @sDet1SQL4 + @sDet1SQL5 + @sDet1SQL6 + @sDet1SQL7)
    ELSE
    BEGIN
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sBF1SQL1 + @sBF1SQL2 + @sBF1SQL3 + @sBF1SQL4 +
              @sBF1SQL5 + @sBF1SQL6 + @sBF1SQL7)
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sBF2SQL1 + @sBF2SQL2 + @sBF2SQL3 + @sBF2SQL4 +
              @sBF2SQL5 + @sBF2SQL6 + @sBF2SQL7 + @sBF2SQL8)
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet1SQL1 + @sDet1SQL2 + @sDet1SQL3 + @sDet1SQL4 +
              @sDet1SQL5 + @sDet1SQL6 + @sDet1SQL7)
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet2SQL1 + @sDet2SQL2 + @sDet2SQL3 + @sDet2SQL4 +
              @sDet2SQL5 + @sDet2SQL6 + @sDet2SQL7)
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet3SQL1 + @sDet3SQL2 + @sDet3SQL3 + @sDet3SQL4 +
              @sDet3SQL5 + @sDet3SQL6 + @sDet3SQL7)
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet4SQL1 + @sDet4SQL2 + @sDet4SQL3 + @sDet4SQL4 +
              @sDet4SQL5 + @sDet4SQL6 + @sDet4SQL7)
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sDet5SQL1 + @sDet5SQL2 + @sDet5SQL3 + @sDet5SQL4 +
              @sDet5SQL5 + @sDet5SQL6 + @sDet5SQL7)
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sCF1SQL1 + @sCF1SQL2 + @sCF1SQL3 + @sCF1SQL4 +
              @sCF1SQL5 + @sCF1SQL6 + @sCF1SQL7)
        EXECUTE (@sInsSQL1 + @sInsSQL2 + @sCF2SQL1 + @sCF2SQL2 + @sCF2SQL3 + @sCF2SQL4 +
              @sCF2SQL5 + @sCF2SQL6 + @sCF2SQL7 + @sCF2SQL8)

    END
    SELECT @sOutSQL1 = ' SELECT DISTINCT sort_field, ' +
               ' bank_account_code, ' +
               ' bank_account_name, ' +
               ' ISNULL(bf_bank_amount, 0) bf_bank_amount, ' +
               ' ISNULL(bank_amount, 0) bank_amount, ' +
               ' ISNULL(cf_bank_amount, 0) cf_bank_amount, ' +
               ' ISNULL(account_code, '''') account_code, '
    SELECT @sOutSQL1A = ' ISNULL(account_amount, 0) account_amount, ' +
              ' ISNULL(policy_number, '''') policy_number, ' +
              ' ISNULL(ledger_name, '''') ledger_name, '
    SELECT @sOutSQL2 = ' P.document_date, ' +
               ' ISNULL(P.documenttype_id, 0) documenttype_id, ' +
               ' P.document_ref, ' +
               ' bank_branch_id, ' +
               ' bank_branch_name, ' +
               ' ISNULL(account_branch_id, 0) account_branch_id, ' +
               ' ISNULL(account_branch_name, '''') account_branch_name, ' +
               ' ISNULL(method, '''') method, '
    SELECT @sOutSQL2A = ' ISNULL(document_sequence, 0) document_sequence, ' +
               ' ISNULL(media_ref, '''') media_ref, ' +
               ' CASE ''' + @TypeOfCurrency + '''' +
			   '	WHEN ''Base'' THEN cb.iso_code ' + 
			   '   	WHEN ''System'' THEN ''' + @SystemCurrencyCode + ''' END CurrencyCode ,' +
			   ' CASE ''' + @TypeOfCurrency + '''' +
			   '  	WHEN ''Base'' THEN cb.description '+
			   '   	WHEN ''System'' THEN ''' + @SystemCurrencyDesc	+ ''' END CurrrencyDesc, '+
			   ' C.Code,C.Description,' +
               ' CASE ''' + @Groupbycode + '''' +
			   ' 	WHEN ''Branch'' THEN c.Code ' + 
			   '	When ''Branch and Currency'' THEN C.Code '+ 
			   '	ELSE '''' ' +
	 		   ' END "GroupByCode" '
    SELECT @sOutSQL3 = ' FROM #Report_PaymentAndReceipt P LEFT OUTER JOIN ' +  
			' document d ON d.document_ref=P.document_ref  LEFT JOIN ' +
			' Account A ON A.short_code = P.account_code LEFT JOIN ' +  
			' Ledger L ON L.ledger_id = A.ledger_id LEFT JOIN ' +
			' company c ON P.bank_branch_id = c.company_id LEFT JOIN ' +
			' currency cb ON cb.currency_id = c.base_currency ' 

    UPDATE #Report_PaymentAndReceipt SET method= MDT.Description , media_ref= CLIT.media_ref
    FROM #Report_PaymentAndReceipt RPR
	JOIN Document D ON RPR.document_ref = D.document_ref
	JOIN Transdetail TDD ON D.document_id = TDD.document_id
	JOIN pfinstalments PFI ON TDD.transdetail_id = PFI.pftransaction_id
	JOIN CashListItem_Instalments CLII ON PFI.pfinstalments_id = CLII.pfinstalments_id
	JOIN CashListItem CLIT ON CLII.cashlistitem_id = CLIT.cashlistitem_id
	JOIN MediaType MDT ON CLIT.mediatype_id = MDT.mediatype_id
    WHERE RPR.document_ref LIKE 'INC%'
	       
    SET NOCOUNT OFF
    EXEC (@sOutSQL1 + @sOutSQL1A + @sOutSQL2 + @sOutSQL2A + @sOutSQL3 + @sSortSQL)

GO
