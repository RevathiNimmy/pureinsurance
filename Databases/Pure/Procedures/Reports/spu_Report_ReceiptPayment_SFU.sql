--SET QUOTED_IDENTIFIER on SET ANSI_NULLS ON
--GO

EXECUTE DDLDropProcedure 'spu_Report_ReceiptPayment_SFU'
GO
CREATE PROCEDURE spu_Report_ReceiptPayment_SFU  
    @branch_id  int,  
    @start_date datetime,  
    @end_date   datetime,  
    @bank       varchar(60),     --DC201101  
    @TypeofCurrency varchar(15), --added for multicurrency feature  
    @GroupByCode Varchar(50)  
AS  
  
DECLARE @sStartDate varchar(60),  
    @sEndDate   varchar(60),  
    @sBank  varchar(60)  
  
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
  
  
SELECT  @sStartDate = CONVERT(varchar(60), @start_date),  
    @sEndDate = CONVERT(varchar(60), @end_date)  
  
--DC201101 -check if Bank passed  
SELECT @sBank = ISNULL(@Bank, 'ALL')  
  
DECLARE @sBalBF char(25),  
    @sBalCF char(25),  
-- Empty Temporary table  
    @sDelSQL varchar(500),  
-- Insert into temporary table  
    @sInsSQL1 varchar(500),  
    @sInsSQL2 varchar(500),  
-- BF if no transactions before start date  
    @sBF1SQL1 varchar(500),  
    @sBF1SQL2 varchar(500),  
    @sBF1SQL3 varchar(500),  
    @sBF1SQL4 varchar(500),  
    @sBF1SQL5 varchar(500),  
    @sBF1SQL6 varchar(500),  
    @sBF1SQL7 varchar(500),  
-- BF if transactions before start date  
    @sBF2SQL1 varchar(500),  
    @sBF2SQL2 varchar(500),  
    @sBF2SQL3 varchar(500),  
    @sBF2SQL4 varchar(500),  
    @sBF2SQL5 varchar(500),  
    @sBF2SQL6 varchar(500),  
    @sBF2SQL7 varchar(500),  
    @sBF2SQL8 varchar(500),  
-- Transactions in period - Receipts & Payments  
    @sDet1SQL1 varchar(1500),  
    @sDet1SQL2 varchar(1500),  
    @sDet1SQL3 varchar(1500),  
    @sDet1SQL4 varchar(1500),  
    @sDet1SQL5 varchar(1500),  
    @sDet1SQL6 varchar(1500),  
    @sDet1SQL7 varchar(1500),  
-- Transactions in period - Journals Bank sequence = 1  
    @sDet2SQL1 varchar(1500),  
    @sDet2SQL2 varchar(1500),  
    @sDet2SQL3 varchar(1500),  
    @sDet2SQL4 varchar(1500),  
    @sDet2SQL5 varchar(1500),  
    @sDet2SQL6 varchar(1500),  
    @sDet2SQL7 varchar(1500),  
-- Transactions in period - Journals Account sequence = 1  
    @sDet3SQL1 varchar(1500),  
    @sDet3SQL2 varchar(1500),  
    @sDet3SQL3 varchar(1500),  
    @sDet3SQL4 varchar(1500),  
    @sDet3SQL5 varchar(1500),  
    @sDet3SQL6 varchar(1500),  
    @sDet3SQL7 varchar(1500),  
-- Transactions in period - Journals Bank sequence = 1, Journals Account amount only  
    @sDet4SQL1 varchar(1500),  
    @sDet4SQL2 varchar(1500),  
    @sDet4SQL3 varchar(1500),  
    @sDet4SQL4 varchar(1500),  
    @sDet4SQL5 varchar(1500),  
    @sDet4SQL6 varchar(1500),  
    @sDet4SQL7 varchar(1500),  
-- Transactions in period - Journals Account sequence = 1, Journals Account amount only  
    @sDet5SQL1 varchar(1500),  
    @sDet5SQL2 varchar(1500),  
    @sDet5SQL3 varchar(1500),  
    @sDet5SQL4 varchar(1500),  
    @sDet5SQL5 varchar(1500),  
    @sDet5SQL6 varchar(1500),  
    @sDet5SQL7 varchar(1500),  
-- CF if no transactions before end date  
    @sCF1SQL1 varchar(500),  
    @sCF1SQL2 varchar(500),  
    @sCF1SQL3 varchar(500),  
    @sCF1SQL4 varchar(500),  
    @sCF1SQL5 varchar(500),  
    @sCF1SQL6 varchar(500),  
    @sCF1SQL7 varchar(500),  
-- CF if transactions before end date  
    @sCF2SQL1 varchar(500),  
    @sCF2SQL2 varchar(500),  
    @sCF2SQL3 varchar(500),  
    @sCF2SQL4 varchar(500),  
    @sCF2SQL5 varchar(500),  
    @sCF2SQL6 varchar(500),  
    @sCF2SQL7 varchar(500),  
    @sCF2SQL8 varchar(500),  
-- Output SQL  
    @sOutSQL1 varchar(500),  
    @sOutSQL1A varchar(500),  
    @sOutSQL2 varchar(500),  
    @sOutSQL2A varchar(1000),  
    @sOutSQL3 varchar(500),  
-- Sort keys  
    @sSortSQL varchar(500)  
  
    SET NOCOUNT ON  
    SELECT @sBalBF = 'Balance Brought Forward'  
    SELECT @sBalCF = 'Balance Carried Forward'  
  
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
  
-- Empty Temporary table  
    SELECT @sDelSQL = ' DELETE FROM #Report_PaymentAndReceipt'  
  
-- Insert into temporary table  
    SELECT @sInsSQL1 = ' INSERT INTO' +  
               ' #Report_PaymentAndReceipt(sort_field,' +  
               ' bank_account_code,' +  
               ' bank_account_name,' +  
               ' bf_bank_amount,' +  
               ' bank_amount,' +  
               ' cf_bank_amount,' +  
               ' account_code,'  
    SELECT @sInsSQL2 = ' account_amount,' +  
               ' policy_number,' +  
               ' document_date,' +  
               ' documenttype_id,' +  
               ' document_ref,' +  
               ' bank_branch_id,' +  
               ' bank_branch_name,' +  
               ' account_branch_id,' +  
               ' account_branch_name,' +  
               ' method,' +  
               ' document_sequence,' +  
               ' media_ref,' +  
      ' issuer)'  
  
-- Construct SQL for BF if no transactions before start date  
    SELECT @sBF1SQL1 = ' SELECT 0,' +  
               ' BAA.short_code,' +  
               ' B.bank_name,' +  
               ' 0.0,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' ''' + @sStartDate + ''',' +  
               ' null,' +  
               ' '' + @sBalBF + '',' +  
               ' BC.company_id,' +  
               ' BC.description,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null'  
    SELECT @sBF1SQL2 = ' FROM BankAccount BA,' +  
               ' Account BAA,' +  
               ' Bank B,' +  
               ' Company BC'  
    SELECT @sBF1SQL3 = ' WHERE BAA.account_id = BA.account_id' +  
               ' AND B.bank_id = BA.bank_id' +  
               ' AND BC.company_id = BAA.company_id'  
  
    if @branch_id > 0  
        SELECT @sBF1SQL3 = @sBF1SQL3 + ' AND BC.company_id = ' + str(@branch_id)  
    SELECT @sBF1SQL4 = ' AND BAA.account_id not in (SELECT BTD.account_id'  
    SELECT @sBF1SQL5 = ' FROM BankAccount BA,' +  
               ' Account BAA,' +  
               ' Bank B,' +  
               ' TransDetail BTD,' +  
               ' Document D,' +  
               ' Company BC'  
    SELECT @sBF1SQL6 = ' WHERE BTD.account_id = BA.account_id' +  
               ' AND BAA.account_id = BTD.account_id' +  
               ' AND B.bank_id = BA.bank_id' +  
               ' AND D.document_id = BTD.document_id' +  
               ' AND BC.company_id = BTD.company_id' +  
               ' AND D.documenttype_id IN (1, 12, 22, 23, 28, 29, 43, 45, 47)'  
    if @branch_id > 0  
        SELECT @sBF1SQL6 = @sBF1SQL6 + ' AND BTD.company_id = ' + str(@branch_id)  
  
    SELECT @sBF1SQL7 = ' AND D.document_date < ''' + @sStartDate + ''')'  
  
 -- Construct SQL for BF if transactions before start date  
    SELECT @sBF2SQL1 = ' SELECT 0,' +  
               ' BAA.short_code,' +  
               ' B.bank_name,' +  
               --' sum(BTD.amount),' +  
            ' CASE ''' + @TypeOfCurrency +'''' +  
            ' WHEN ''Base'' THEN ISNULL(ROUND(sum(BTD.amount),2), 0.0) ' +  
            ' WHEN ''System'' THEN ISNULL(ROUND(sum(BTD.system_amount),2), 0.0) ' +  
      ' END , ' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' ''' + @sStartDate + ''',' +  
               ' null,' +  
               ' ''' + @sBalBF + ''',' +  
               ' BTD.company_id,' +  
               ' BC.description,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null '  
    SELECT @sBF2SQL2 = ' FROM BankAccount BA,' +  
               ' Account BAA,' +  
               ' Bank B,' +  
               ' TransDetail BTD,' +  
               ' Document D,' +  
               ' Company BC'  
    SELECT @sBF2SQL3 = ' WHERE BTD.account_id = BA.account_id' +  
               ' AND BAA.account_id = BTD.account_id' +  
               ' AND B.bank_id = BA.bank_id' +  
               ' AND D.document_id = BTD.document_id' +  
               ' AND BC.company_id = BTD.company_id' +  
               ' AND D.documenttype_id IN (1, 12, 22, 23, 28, 29, 43, 45, 47)'  
    SELECT @sBF2SQL4 = ' AND D.document_date < ''' + @sStartDate + ''''  
    if @branch_id > 0  
        SELECT @sBF2SQL4 = @sBF2SQL4 + ' AND BTD.company_id = ' + str(@branch_id)  
    SELECT @sBF2SQL5 = ' AND BAA.account_id in (SELECT BTD.account_id'  
    SELECT @sBF2SQL6 = ' FROM BankAccount BA,' +  
               ' Account BAA,' +  
               ' Bank B,' +  
               ' TransDetail BTD,' +  
               ' Document D,' +  
               ' Company BC'  
    SELECT @sBF2SQL7 = ' WHERE BTD.account_id = BA.account_id' +  
               ' AND BAA.account_id = BTD.account_id' +  
               ' AND B.bank_id = BA.bank_id' +  
               ' AND D.document_id = BTD.document_id' +  
               ' AND BC.company_id = BTD.company_id' +  
               ' AND D.documenttype_id IN (1, 12, 22, 23, 28, 29, 43, 45, 47)'  
    if @branch_id > 0  
        SELECT @sBF2SQL7 = @sBF2SQL7 + ' AND BTD.company_id = ' + str(@branch_id)  
    SELECT @sBF2SQL8 = ' AND D.document_date < ''' + @sStartDate + ''')' +  
               ' GROUP BY BTD.company_id,' +  
               ' BC.description,' +  
               ' BAA.short_code,' +  
               ' B.bank_name'  
  
-- Construct SQL for Transactions in period - Receipts & Payments  
    SELECT @sDet1SQL1 = ' SELECT 1 sort_field,' +  
                ' BAA.short_code bank_account_code,' +  
                ' B.bank_name bank_account_name,' +  
                ' null  bf_bank_amount,' +  
                ' CASE ''' +  @TypeOfCurrency + '''' +  
          ' WHEN ''Base'' THEN ISNULL(ROUND(BTD.amount,2), 0.0) ' +  
       ' WHEN ''System'' THEN ISNULL(ROUND(BTD.system_amount,2), 0.0) ' +  
    ' END bank_amount,' +  
    ' null cf_bank_amount,' +  
             ' A.short_code account_code,' +  
                ' CASE ''' +  @TypeOfCurrency + '''' +  
          ' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0) ' +  
       ' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0) ' +  
    ' END account_amount,' +  
                --' TD.insurance_ref policy_number,'  
  ' CASE  ISNULL(TD.insurance_ref,'''')' +  
      ' WHEN '''' THEN (SELECT TD1.insurance_ref FROM transdetail TD1 WHERE TD1.transdetail_id in ( SELECT TOP 1 AD.transdetail_id FROM AllocationDetail ALD LEFT JOIN AllocationDetail AD ON ALD.allocation_id=AD.allocation_id WHERE ALD.transdetail_id=TD.transdetail_id AND AD.transdetail_id <> TD.transdetail_id))' +  
      ' ELSE TD.insurance_ref ' +  
      ' END, '  
    SELECT @sDet1SQL2 = ' D.document_date,' +  
                ' D.documenttype_id,' +  
                ' D.document_ref,' +  
                ' BTD.company_id bank_branch_id,' +  
                ' BC.description bank_branch_name,' +  
                ' TD.company_id account_branch_id,' +  
                ' C.description account_branch_name,' +  
                ' ISNULL(MT.description, (CASE WHEN TD.transdetail_id IN' +  
				' (SELECT  innerpfi.PFTransaction_id FROM PFInstalments innerpfi INNER JOIN CashListItem_Instalments innercli' +  
				' ON innercli.pfinstalments_id = innerpfi.pfinstalments_id INNER JOIN CashListItem innerclsli' +  
				' ON innerclsli.cashlistitem_id = innercli.cashlistitem_id INNER JOIN MediaType innermd' +  
				' ON innermd.mediatype_id = innerclsli.mediatype_id' +  
				' WHERE innerpfi.PFTransaction_id = TD.transdetail_id' +  
				' )' +  
				' THEN (' +  
				' SELECT MIN(innermd.description)' +  
				' FROM PFInstalments innerpfi INNER JOIN CashListItem_Instalments innercli' +  
				' ON innercli.pfinstalments_id = innerpfi.pfinstalments_id INNER JOIN CashListItem innerclsli' +  
				' ON innerclsli.cashlistitem_id = innercli.cashlistitem_id INNER JOIN MediaType innermd' +   
				' ON innermd.mediatype_id = innerclsli.mediatype_id' +  
				' WHERE innerpfi.PFTransaction_id = TD.transdetail_id)' +  
				' ELSE NULL END)' +  
				' ) method,' +  
                ' TD.document_sequence,' +  
                ' CLI.media_ref,' +  
    ' MTI.description issuer'  
  
    SELECT @sDet1SQL3 = ' FROM BankAccount BA,' +  
                ' Account BAA,' +  
                ' Bank B,' +  
                ' TransDetail BTD,' +  
                ' Account A,' +  
                ' Document D,' +  
                ' Company BC,' +  
                ' Company C,'  
    SELECT @sDet1SQL4 = ' TransDetail TD' +  
                ' LEFT OUTER JOIN CashListItem CLI' +  
                ' ON TD.transdetail_id = CLI.transdetail_id' +  
                ' LEFT OUTER JOIN MediaType MT' +  
                ' ON CLI.mediatype_id = MT.mediatype_id' +  
                ' LEFT OUTER JOIN MediaType_Issuer MTI' +  
                ' ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id'  
    SELECT @sDet1SQL5 = ' WHERE BTD.account_id = BA.account_id' +  
                ' AND BAA.account_id = BTD.account_id' +  
                ' AND B.bank_id = BA.bank_id' +  
                ' AND D.document_id = BTD.document_id' +  
                ' AND TD.document_id = D.document_id' +  
                ' AND A.account_id = TD.account_id' +  
                ' AND BC.company_id = BTD.company_id'  
    SELECT @sDet1SQL6 = ' AND D.documenttype_id IN (22, 23, 28, 29, 43, 45, 47)' +  
                ' AND D.document_date >= ''' + @sStartDate + '''' +  
                ' AND D.document_date <= ''' + @sEndDate + '''' +  
                ' AND C.company_id = TD.company_id' +  
                ' AND TD.transdetail_id <> BTD.transdetail_id'  
  
    if @branch_id > 0  
        SELECT @sDet1SQL7 = ' AND BTD.company_id = ' + str(@branch_id)  
  
-- Construct SQL for Transactions in period - Journals Bank sequence = 1  
    SELECT @sDet2SQL1 = ' SELECT 1,' +  
                ' BAA.short_code,' +  
                ' B.bank_name,' +  
                ' null,' +  
                ' CASE ''' + @TypeOfCurrency + ''''  +  
    ' WHEN ''Base'' THEN ISNULL(ROUND(BTD.amount,2), 0.0)' +  
    ' WHEN ''System'' THEN ISNULL(ROUND(BTD.system_amount,2), 0.0)' +  
    ' END,' +  
                ' null,' +  
                ' A.short_code,' +  
                ' CASE  ''' +  @TypeOfCurrency + '''' +  
    ' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0)' +  
    ' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0)' +  
    ' END, ' +  
  ' CASE  ISNULL(TD.insurance_ref,'''')' +  
      ' WHEN '''' THEN (SELECT TD1.insurance_ref FROM transdetail TD1 WHERE TD1.transdetail_id in ( SELECT TOP 1 AD.transdetail_id FROM AllocationDetail ALD LEFT JOIN AllocationDetail AD ON ALD.allocation_id=AD.allocation_id WHERE ALD.transdetail_id=TD.transdetail_id AND AD.transdetail_id <> TD.transdetail_id))' +  
      ' ELSE TD.insurance_ref ' +  
      ' END, ' +  
                --" TD.insurance_ref,' +  
                ' D.document_date,' +  
                ' D.documenttype_id,'  
    SELECT @sDet2SQL2 = ' D.document_ref,' +  
                ' BTD.company_id,' +  
                ' BC.description,' +  
                ' TD.company_id,' +  
                ' C.description,' +  
                ' MT.description,' +  
                ' BTD.document_sequence,' +  
                ' CLI.media_ref,' +  
    ' MTI.description issuer'  
    SELECT @sDet2SQL3 = ' FROM BankAccount BA,' +  
                ' Account BAA,' +  
                ' Bank B,' +  
                ' TransDetail BTD,' +  
                ' Account A,' +  
                ' Document D,' +  
                ' Company BC,' +  
                ' Company C,'  
    SELECT @sDet2SQL4 = ' TransDetail TD' +  
                ' LEFT OUTER JOIN CashListItem CLI' +  
                ' ON TD.transdetail_id = CLI.transdetail_id' +  
                ' LEFT OUTER JOIN MediaType MT' +  
                ' ON CLI.mediatype_id = MT.mediatype_id' +  
                ' LEFT OUTER JOIN MediaType_Issuer MTI' +  
                ' ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id'  
    SELECT @sDet2SQL5 = ' WHERE BTD.account_id = BA.account_id' +  
                ' AND BAA.account_id = BTD.account_id' +  
                ' AND B.bank_id = BA.bank_id' +  
  
                ' AND D.document_id = BTD.document_id' +  
                ' AND TD.document_id = D.document_id' +  
                ' AND A.account_id = TD.account_id' +  
                ' AND BC.company_id = BTD.company_id'  
    SELECT @sDet2SQL6 = ' AND D.documenttype_id IN (1, 12)' +  
                ' AND D.document_date >= ''' + @sStartDate + '''' +  
                ' AND D.document_date <= ''' + @sEndDate + '''' +  
                ' AND C.company_id = TD.company_id' +  
                ' AND TD.transdetail_id <> BTD.transdetail_id'  
    SELECT @sDet2SQL7 = ' AND BTD.document_sequence = 1 AND' +  
                ' TD.document_sequence = 2'  
    if @branch_id > 0  
        SELECT @sDet2SQL7 = @sDet2SQL7 + ' AND BTD.company_id = ' + str(@branch_id)  
  
-- Construct SQL for Transactions in period - Journals Account sequence = 1  
    SELECT @sDet3SQL1 = ' SELECT 1,' +  
                ' BAA.short_code,' +  
                ' B.bank_name,' +  
                ' null,' +  
                ' CASE  ''' + @TypeOfCurrency + '''' +  
    ' WHEN ''Base'' THEN ISNULL(ROUND(BTD.amount,2), 0.0)' +  
    ' WHEN ''System'' THEN ISNULL(ROUND(BTD.system_amount,2), 0.0)' +  
    ' END,' +  
                ' null,' +  
                ' A.short_code,' +  
                ' CASE  ''' + @TypeOfCurrency + '''' +  
    ' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0)' +  
    ' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0)' +  
    ' END,' +  
  ' CASE ISNULL(TD.insurance_ref,'''') ' +  
      ' WHEN '''' THEN (SELECT TD1.insurance_ref FROM transdetail TD1 WHERE TD1.transdetail_id in ( SELECT TOP 1 AD.transdetail_id FROM AllocationDetail ALD LEFT JOIN AllocationDetail AD ON ALD.allocation_id=AD.allocation_id WHERE ALD.transdetail_id=TD.transdetail_id AND AD.transdetail_id <> TD.transdetail_id))' +  
      ' ELSE TD.insurance_ref ' +  
      ' END, ' +  
                --' TD.insurance_ref,' +  
                ' D.document_date,' +  
                ' D.documenttype_id,'  
    SELECT @sDet3SQL2 = ' D.document_ref,' +  
                ' BTD.company_id,' +  
  
                ' BC.description,' +  
                ' TD.company_id,' +  
                ' C.description,' +  
                ' MT.description,' +  
                ' TD.document_sequence,' +  
                ' CLI.media_ref,' +  
    ' MTI.description issuer'  
    SELECT @sDet3SQL3 = ' FROM BankAccount BA,' +  
                ' Account BAA,' +  
                ' Bank B,' +  
                ' TransDetail BTD,' +  
                ' Account A,' +  
                ' Document D,' +  
                ' Company BC,' +  
                ' Company C,'  
    SELECT @sDet3SQL4 = ' TransDetail TD' +  
                ' LEFT OUTER JOIN CashListItem CLI' +  
                ' ON TD.transdetail_id = CLI.transdetail_id' +  
                ' LEFT OUTER JOIN MediaType MT' +  
                ' ON CLI.mediatype_id = MT.mediatype_id' +  
                ' LEFT OUTER JOIN MediaType_Issuer MTI' +  
                ' ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id'  
    SELECT @sDet3SQL5 = ' WHERE BTD.account_id = BA.account_id' +  
                ' AND BAA.account_id = BTD.account_id' +  
                ' AND B.bank_id = BA.bank_id' +  
                ' AND D.document_id = BTD.document_id' +  
                ' AND TD.document_id = D.document_id' +  
                ' AND A.account_id = TD.account_id' +  
                ' AND BC.company_id = BTD.company_id'  
  
    SELECT @sDet3SQL6 = ' AND D.documenttype_id IN (1, 12)' +  
                ' AND D.document_date >= ''' + @sStartDate + '''' +  
                ' AND D.document_date <= ''' + @sEndDate + '''' +  
                ' AND C.company_id = TD.company_id' +   
                ' AND TD.transdetail_id <> BTD.transdetail_id'  
    SELECT @sDet3SQL7 = ' AND BTD.document_sequence > 1 AND' +  
                ' TD.document_sequence = 1'  
    if @branch_id > 0  
        SELECT @sDet3SQL7 = @sDet3SQL7 + ' AND BTD.company_id = ' + str(@branch_id)  
  
 -- Construct SQL for Transactions in period - Journals Bank sequence = 1, Journals Account amount only  
    SELECT @sDet4SQL1 = ' SELECT 1,' +  
                ' BAA.short_code,' +  
                ' B.bank_name,' +  
                ' null,' +  
                ' null,' +  
                ' null,' +  
                ' A.short_code,' +  
                ' CASE ''' +  @TypeOfCurrency + '''' +  
    ' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0)' +  
    ' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0)' +  
    ' END,' +  
  ' CASE ISNULL(TD.insurance_ref,'''') ' +  
      ' WHEN '''' THEN (SELECT TD1.insurance_ref FROM transdetail TD1 WHERE TD1.transdetail_id in ( SELECT TOP 1 AD.transdetail_id FROM AllocationDetail ALD LEFT JOIN AllocationDetail AD ON ALD.allocation_id=AD.allocation_id WHERE ALD.transdetail_id=TD.transdetail_id AND AD.transdetail_id <> TD.transdetail_id))' +  
      ' ELSE TD.insurance_ref ' +  
      ' END, ' +  
                --' TD.insurance_ref,' +  
                ' D.document_date,' +  
                ' D.documenttype_id,'  
    SELECT @sDet4SQL2 = ' D.document_ref,' +  
                ' BTD.company_id,' +  
                ' BC.description,' +  
                ' TD.company_id,' +  
                ' C.description,' +  
                ' MT.description,' +  
                ' TD.document_sequence,' +  
                ' CLI.media_ref,' +  
    ' MTI.description issuer'  
    SELECT @sDet4SQL3 = ' FROM BankAccount BA,' +  
                ' Account BAA,' +  
                ' Bank B,' +  
                ' TransDetail BTD,' +  
                ' Account A,' +  
                ' Document D,' +  
                ' Company BC,' +  
                ' Company C,'  
    SELECT @sDet4SQL4 = ' TransDetail TD' +  
                ' LEFT OUTER JOIN CashListItem CLI' +  
                ' ON TD.transdetail_id = CLI.transdetail_id' +  
                ' LEFT OUTER JOIN MediaType MT' +  
                ' ON CLI.mediatype_id = MT.mediatype_id' +  
                ' LEFT OUTER JOIN MediaType_Issuer MTI' +  
                ' ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id'  
    SELECT @sDet4SQL5 = ' WHERE BTD.account_id = BA.account_id' +  
                ' AND BAA.account_id = BTD.account_id' +  
                ' AND B.bank_id = BA.bank_id' +  
                ' AND D.document_id = BTD.document_id' +  
                ' AND TD.document_id = D.document_id' +  
                ' AND A.account_id = TD.account_id' +  
                ' AND BC.company_id = BTD.company_id'  
    SELECT @sDet4SQL6 = ' AND D.documenttype_id IN (1, 12)' +  
                ' AND D.document_date >= ''' + @sStartDate + '''' +  
                ' AND D.document_date <= ''' + @sEndDate + '''' +  
                ' AND C.company_id = TD.company_id' +  
                ' AND TD.transdetail_id <> BTD.transdetail_id'  
    SELECT @sDet4SQL7 = ' AND BTD.document_sequence = 1 AND' +  
                ' TD.document_sequence > 2'  
    if @branch_id > 0  
        SELECT @sDet4SQL7 = @sDet4SQL7 + ' AND BTD.company_id = ' + str(@branch_id)  
  
 -- Construct SQL for Transactions in period - Journals Account sequence = 1, Journals Account amount only  
    SELECT @sDet5SQL1 = ' SELECT 1,' +  
                ' BAA.short_code,' +  
                ' B.bank_name,' +  
                ' null,' +  
                ' null,' +  
                ' null,' +  
                ' A.short_code,' +  
                ' CASE ''' + @TypeOfCurrency +  '''' +  
    ' WHEN ''Base'' THEN ISNULL(ROUND(TD.amount,2), 0.0)' +  
    ' WHEN ''System'' THEN ISNULL(ROUND(TD.system_amount,2), 0.0)' +  
    ' END,' +  
  ' CASE ISNULL(TD.insurance_ref,'''') ' +  
      ' WHEN '''' THEN (SELECT TD1.insurance_ref FROM transdetail TD1 WHERE TD1.transdetail_id in ( SELECT TOP 1 AD.transdetail_id FROM AllocationDetail ALD LEFT JOIN AllocationDetail AD ON ALD.allocation_id=AD.allocation_id WHERE ALD.transdetail_id=TD.transdetail_id AND AD.transdetail_id <> TD.transdetail_id))' +  
      ' ELSE TD.insurance_ref ' +  
      ' END, ' +  
                --' TD.insurance_ref,' +  
                ' D.document_date,' +  
                ' D.documenttype_id,'  
    SELECT @sDet5SQL2 = ' D.document_ref,' +  
                ' BTD.company_id,' +  
                ' BC.description,' +  
                ' TD.company_id,' +  
                ' C.description,' +  
                ' MT.description,' +  
                ' TD.document_sequence,' +  
                ' CLI.media_ref,' +  
    ' MTI.description issuer'  
    SELECT @sDet5SQL3 = ' FROM BankAccount BA,' +  
                ' Account BAA,' +  
                ' Bank B,' +  
                ' TransDetail BTD,' +  
                ' Account A,' +  
                ' Document D,' +  
                ' Company BC,' +  
                ' Company C,'  
    SELECT @sDet5SQL4 = ' TransDetail TD' +  
                ' LEFT OUTER JOIN CashListItem CLI' +  
                ' ON TD.transdetail_id = CLI.transdetail_id' +  
                ' LEFT OUTER JOIN MediaType MT' +  
       ' ON CLI.mediatype_id = MT.mediatype_id' +  
                ' LEFT OUTER JOIN MediaType_Issuer MTI' +  
                ' ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id'  
    SELECT @sDet5SQL5 = ' WHERE BTD.account_id = BA.account_id' +  
                ' AND BAA.account_id = BTD.account_id' +  
                ' AND B.bank_id = BA.bank_id' +  
                ' AND D.document_id = BTD.document_id' +  
                ' AND TD.document_id = D.document_id' +  
                ' AND A.account_id = TD.account_id' +  
                ' AND BC.company_id = BTD.company_id'  
    SELECT @sDet5SQL6 = ' AND D.documenttype_id IN (1, 12)' +  
                ' AND D.document_date >= ''' + @sStartDate + '''' +  
                ' AND D.document_date <= ''' + @sEndDate + '''' +  
                ' AND C.company_id = TD.company_id' +  
                ' AND TD.transdetail_id <> BTD.transdetail_id'  
    SELECT @sDet5SQL7 = ' AND BTD.document_sequence > 1 AND' +  
                ' TD.document_sequence > 1'  
    if @branch_id > 0  
        SELECT @sDet5SQL7 = @sDet5SQL7 + ' AND BTD.company_id = ' + str(@branch_id)  
  
 -- Construct SQL for CF if no transactions before end date  
    SELECT @sCF1SQL1 = ' SELECT 9,' +  
               ' BAA.short_code,' +  
               ' B.bank_name,' +  
               ' null,' +  
               ' null,' +  
               ' 0.0,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               '''' +  @sEndDate + ''',' +  
               ' null,' +  
               ' ''' + @sBalCF + ''',' +  
               ' BC.company_id,' +  
               ' BC.description,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null,' +  
               ' null'  
    SELECT @sCF1SQL2 = ' FROM BankAccount BA,' +  
               ' Account BAA,' +  
               ' Bank B,' +  
               ' Company BC'  
    SELECT @sCF1SQL3 = ' WHERE BAA.account_id = BA.account_id' +  
               ' AND B.bank_id = BA.bank_id' +  
               ' AND BC.company_id = BAA.company_id'  
    if @branch_id > 0  
        SELECT @sCF1SQL3 = @sBF1SQL3 + ' AND BC.company_id = ' + str(@branch_id)  
    SELECT @sCF1SQL4 = ' AND BAA.account_id not in (SELECT BTD.account_id'  
    SELECT @sCF1SQL5 = ' FROM BankAccount BA,' +  
               ' Account BAA,' +  
               ' Bank B,' +  
               ' TransDetail BTD,' +  
               ' Document D,' +  
               ' Company BC'  
    SELECT @sCF1SQL6 = ' WHERE BTD.account_id = BA.account_id' +  
               ' AND BAA.account_id = BTD.account_id' +  
               ' AND B.bank_id = BA.bank_id' +  
               ' AND D.document_id = BTD.document_id' +  
               ' AND BC.company_id = BTD.company_id' +  
               ' AND D.documenttype_id IN (1, 12, 22, 23, 28, 29, 43, 47)'  
    if @branch_id > 0  
  
    SELECT @sCF1SQL6 = @sCF1SQL6 + ' AND BTD.company_id = ' + str(@branch_id)  
    SELECT @sCF1SQL7 = ' AND D.document_date <= ''' + @sEndDate + ''')'  
  
 -- Construct SQL for CF if transactions before end date  
    SELECT @sCF2SQL1 = ' SELECT 9,' +  
    ' BAA.short_code,' +  
    ' B.bank_name,' +  
    ' null,' +  
    ' null,' +  
    ' CASE ''' + @TypeOfCurrency + '''' +  
    ' WHEN ''Base'' THEN sum(ISNULL(BTD.amount,0.0))' +  
    ' WHEN ''System'' THEN sum(ISNULL(BTD.system_amount, 0.0))' +  
    ' END,' +  
    ' null,' +  
    ' null,' +  
    ' null,' +  
    ' ''' + @sEndDate + ''',' +  
    ' null,' +  
    ' ''' + @sBalCF + ''',' +  
    ' BTD.company_id,' +  
    ' BC.description,' +  
    ' null,' +  
    ' null,' +  
    ' null,' +  
    ' null,' +  
                ' null,' +  
    ' null '  
    SELECT @sCF2SQL2 = ' FROM BankAccount BA,' +  
               ' Account BAA,' +  
               ' Bank B,' +  
               ' TransDetail BTD,' +  
               ' Document D,' +  
               ' Company BC'  
    SELECT @sCF2SQL3 = ' WHERE BTD.account_id = BA.account_id' +  
               ' AND BAA.account_id = BTD.account_id' +  
               ' AND B.bank_id = BA.bank_id' +  
               ' AND D.document_id = BTD.document_id' +  
               ' AND BC.company_id = BTD.company_id' +  
               ' AND D.documenttype_id IN (1, 12, 22, 23, 28, 29, 43, 45, 47)'  
    SELECT @sCF2SQL4 = ' AND D.document_date <= ''' + @sEndDate + ''''  
    if @branch_id > 0  
        SELECT @sCF2SQL4 = @sCF2SQL4 + ' AND BTD.company_id = ' + str(@branch_id)  
    SELECT @sCF2SQL5 = ' AND BAA.account_id in (SELECT BTD.account_id'  
    SELECT @sCF2SQL6 = ' FROM BankAccount BA,' +  
               ' Account BAA,' +  
               ' Bank B,' +  
               ' TransDetail BTD,' +  
               ' Document D,' +  
               ' Company BC'  
    SELECT @sCF2SQL7 = ' WHERE BTD.account_id = BA.account_id' +  
               ' AND BAA.account_id = BTD.account_id' +  
               ' AND B.bank_id = BA.bank_id' +  
               ' AND D.document_id = BTD.document_id' +  
               ' AND BC.company_id = BTD.company_id' +  
               ' AND D.documenttype_id IN (1, 12, 22, 23, 28, 29, 43, 45, 47)'  
    if @branch_id > 0  
        SELECT @sCF2SQL7 = @sCF2SQL7 + ' AND BTD.company_id = ' + str(@branch_id)  
    SELECT @sCF2SQL8 = ' AND D.document_date <= ''' + @sEndDate + ''')' +  
               ' GROUP BY BTD.company_id,' +  
               ' BC.description,' +  
               ' BAA.short_code,' +  
               ' B.bank_name'  
  
-- Construct Sort SQL string  
    SELECT @sSortSQL = ' ORDER BY 13, 2, 1, 10, 12 '  
  
-- Empty the temporary table  
   -- EXEC (@sDelSQL)  
   EXEC (@sDelSQL)
-- Add data to Temporary table  
    IF @start_date = @end_date  
 begin  
         EXEC (@sInsSQL1 + @sInsSQL2 + @sDet1SQL1 + @sDet1SQL2 + @sDet1SQL3 +  
              @sDet1SQL4 + @sDet1SQL5 + @sDet1SQL6 + @sDet1SQL7)  
--print (@sInsSQL1 + @sInsSQL2 + @sDet1SQL1 + @sDet1SQL2 + @sDet1SQL3 +   @sDet1SQL4 + @sDet1SQL5 + @sDet1SQL6 + @sDet1SQL7)  
--print @sDet1SQL1 + @sDet1SQL2  
 end  
    ELSE  
    BEGIN  
-- BF if no transactions before start date  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sBF1SQL1 + @sBF1SQL2 + @sBF1SQL3 + @sBF1SQL4 +  
              @sBF1SQL5 + @sBF1SQL6 + @sBF1SQL7)  
-- Print @sInsSQL1 + @sInsSQL2 + @sBF1SQL1 + @sBF1SQL2 + @sBF1SQL3 + @sBF1SQL4 +@sBF1SQL5 + @sBF1SQL6 + @sBF1SQL7  
-- BF if transactions before start date  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sBF2SQL1 + @sBF2SQL2 + @sBF2SQL3 + @sBF2SQL4 +  
              @sBF2SQL5 + @sBF2SQL6 + @sBF2SQL7 + @sBF2SQL8)  
  
 --Print @sInsSQL1 + @sInsSQL2 + @sBF2SQL1 + @sBF2SQL2 + @sBF2SQL3 + @sBF2SQL4 +@sBF2SQL5 + @sBF2SQL6 + @sBF2SQL7 + @sBF2SQL8  
-- Transactions in period - Paymenmts & Receips  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sDet1SQL1 + @sDet1SQL2 + @sDet1SQL3 + @sDet1SQL4 +  
              @sDet1SQL5 + @sDet1SQL6 + @sDet1SQL7)  
  
 --Print @sInsSQL1 + @sInsSQL2 + @sDet1SQL1 + @sDet1SQL2 + @sDet1SQL3 + @sDet1SQL4 +@sDet1SQL5 + @sDet1SQL6 + @sDet1SQL7  
  
-- Transactions in period - Journals Bank = sequence 1  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sDet2SQL1 + @sDet2SQL2 + @sDet2SQL3 + @sDet2SQL4 +  
              @sDet2SQL5 + @sDet2SQL6 + @sDet2SQL7)  
  
--Print @sInsSQL1 + @sInsSQL2 + @sDet2SQL1 + @sDet2SQL2 + @sDet2SQL3 + @sDet2SQL4 +@sDet2SQL5 + @sDet2SQL6 + @sDet2SQL7  
-- Transactions in period - Journals Account = sequence 1  
  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sDet3SQL1 + @sDet3SQL2 + @sDet3SQL3 + @sDet3SQL4 +  
              @sDet3SQL5 + @sDet3SQL6 + @sDet3SQL7)  
  
 --Print @sInsSQL1 + @sInsSQL2 + @sDet3SQL1 + @sDet3SQL2 + @sDet3SQL3 + @sDet3SQL4 +@sDet3SQL5 + @sDet3SQL6 + @sDet3SQL7  
  
-- Transactions in period - Journals Bank = sequence 1, Account detail only  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sDet4SQL1 + @sDet4SQL2 + @sDet4SQL3 + @sDet4SQL4 +  
              @sDet4SQL5 + @sDet4SQL6 + @sDet4SQL7)  
  
 --Print @sInsSQL1 + @sInsSQL2 + @sDet4SQL1 + @sDet4SQL2 + @sDet4SQL3 + @sDet4SQL4 +@sDet4SQL5 + @sDet4SQL6 + @sDet4SQL7  
  
-- Transactions in period - Journals Account = sequence 1  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sDet5SQL1 + @sDet5SQL2 + @sDet5SQL3 + @sDet5SQL4 +  
              @sDet5SQL5 + @sDet5SQL6 + @sDet5SQL7)  
  
-- Print @sInsSQL1 + @sInsSQL2 + @sDet5SQL1 + @sDet5SQL2 + @sDet5SQL3 + @sDet5SQL4 + @sDet5SQL5 + @sDet5SQL6 + @sDet5SQL7  
  
-- CF if no transactions before end date  
  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sCF1SQL1 + @sCF1SQL2 + @sCF1SQL3 + @sCF1SQL4 +  
              @sCF1SQL5 + @sCF1SQL6 + @sCF1SQL7)  
  
 --Print @sInsSQL1 + @sInsSQL2 + @sCF1SQL1 + @sCF1SQL2 + @sCF1SQL3 + @sCF1SQL4 + @sCF1SQL5 + @sCF1SQL6 + @sCF1SQL7  
  
-- CF if transactions before end date  
        EXEC (@sInsSQL1 + @sInsSQL2 + @sCF2SQL1 + @sCF2SQL2 + @sCF2SQL3 + @sCF2SQL4 +  
              @sCF2SQL5 + @sCF2SQL6 + @sCF2SQL7 + @sCF2SQL8)  
  
 --Print @sInsSQL1 + @sInsSQL2 + @sCF2SQL1 + @sCF2SQL2 + @sCF2SQL3 + @sCF2SQL4 + @sCF2SQL5 + @sCF2SQL6 + @sCF2SQL7 + @sCF2SQL8  
    END  
-- Construct Output SQL  
    SELECT @sOutSQL1 = ' SELECT sort_field,' +  
               ' bank_account_code,' +  
               ' bank_account_name,' +  
               ' ISNULL(bf_bank_amount, 0) bf_bank_amount,' +  
               ' ISNULL(bank_amount, 0) bank_amount,' +  
               ' ISNULL(cf_bank_amount, 0) cf_bank_amount,' +  
               ' ISNULL(account_code, '''') account_code,'  
    SELECT @sOutSQL1A = ' ISNULL(account_amount, 0) account_amount,'  +  
              ' ISNULL(policy_number, '''') policy_number,'  
    SELECT @sOutSQL2 = ' #Report_PaymentAndReceipt.document_date,' +  
               ' ISNULL(#Report_PaymentAndReceipt.documenttype_id, 0) documenttype_id,' +  
               ' #Report_PaymentAndReceipt.document_ref,' +  
               ' bank_branch_id,' +  
               ' bank_branch_name,' +  
               ' ISNULL(account_branch_id, 0) account_branch_id,' +  
               ' ISNULL(account_branch_name, '''') account_branch_name,' +  
               ' ISNULL(method, '''') method,'  
    SELECT @sOutSQL2A = ' ISNULL(document_sequence, 0) document_sequence,'  +  
               ' ISNULL(media_ref, '''') media_ref,' +  
               'CASE ''' + @TypeOfCurrency + '''' +  
			   '	WHEN ''Base'' THEN cb.iso_code ' + 
			   '   	WHEN ''System'' THEN ''' + @SystemCurrencyCode +	''' END CurrencyCode,' +
      ' CASE ''' + @TypeOfCurrency + '''' +  
			   '  	WHEN ''Base'' THEN cb.description '+
			   '   	WHEN ''System'' THEN ''' + @SystemCurrencyDesc	+ ''' END CurrenyDesc,' +
               '	C.Code,C.Description, ' +
               ' CASE ''' + @Groupbycode + '''' +  
               ' 	WHEN ''Branch'' THEN c.Code ' + 
               '	When ''Branch and Currency'' THEN C.Code '+ 
	 		   '	ELSE '''' ' +
       ' END "GroupByCode", ' +  
      ' ISNULL(issuer,'''') Issuer'  
  
    SELECT @sOutSQL3 = ' FROM #Report_PaymentAndReceipt,currency cb ,company c'  
  
    --DC201101 added check for specific Bank  
    IF RTrim(@sBank) <> 'ALL'  
    Begin  
        SELECT @sOutSQL3 = @sOutSQL3 + ' WHERE #Report_PaymentAndReceipt.bank_branch_id = c.company_id '  
        SELECT @sOutSQL3 = @sOutSQL3 + ' and cb.currency_id = c.base_currency '  
 SELECT @sOutSQL3 = @sOutSQL3 + ' and bank_account_name = ''' + RTrim(@sBank) + ''' '  
    End  
    ELSE  
    Begin  
     SELECT @sOutSQL3 = @sOutSQL3 + ' WHERE '  
 SELECT @sOutSQL3 = @sOutSQL3 + ' #Report_PaymentAndReceipt.bank_branch_id = c.company_id '  
 SELECT @sOutSQL3 = @sOutSQL3 + ' and cb.currency_id = c.base_currency '  
    End  
  
-- Get the data  
  
 SET NOCOUNT OFF  
 --print (@sOutSQL1 + @sOutSQL1A + @sOutSQL2 + @sOutSQL2A + @sOutSQL3 + @sSortSQL)  
   EXEC (@sOutSQL1 + @sOutSQL1A + @sOutSQL2 + @sOutSQL2A + @sOutSQL3 + @sSortSQL)  
  
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
