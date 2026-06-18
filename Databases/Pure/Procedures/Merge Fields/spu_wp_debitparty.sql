SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitparty'
GO

CREATE PROCEDURE spu_wp_debitparty
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE
    @transdetail_id INT,
    @transaction_amount MONEY,
    @ledger_short_name VARCHAR(2),
    @account_type_code VARCHAR(10),
    @account_key INT,
    @vat_amount MONEY,
    @taxes_total MONEY,
    @shortname VARCHAR(20),
    @spare VARCHAR(20)


DECLARE @SharedIndicator INT

SELECT @SharedIndicator = CHARINDEX('|',@DocumentRef)

If @SharedIndicator <> 0
BEGIN
    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)
END

DECLARE c_debitparty CURSOR SCROLL KEYSET READ_ONLY FOR
    SELECT 
        td.transdetail_id,
        td.amount,
        l.ledger_short_name,
        at.code,
        a.account_key,
        i.vat_amount,
        i.tax_amount,
        p.shortname,
        td.spare      
    FROM document d,
         transdetail td,
         insurance_file i,
         party p,
         party_type pt,
         account a,
         accounttype at,
         ledger l
    WHERE d.document_ref = @DocumentRef
    AND   d.insurance_file_cnt = @InsuranceFileCnt
    AND   i.insurance_file_cnt = @InsuranceFileCnt
    AND   d.document_id = td.document_id
    AND   i.insured_cnt = p.party_cnt
    AND   td.account_id = a.account_id
    AND   at.accounttype_id = a.accounttype_id
    AND   a.ledger_id = l.ledger_id
    AND   p.party_type_id = pt.party_type_id
    AND   pt.code IN ('PC', 'CC', 'GC') 

OPEN c_debitparty

FETCH ABSOLUTE @Instance1 FROM c_debitparty INTO
    @transdetail_id,
    @transaction_amount,
    @ledger_short_name,
    @account_type_code,
    @account_key,
    @vat_amount,
    @taxes_total,
    @shortname,
    @spare

CLOSE c_debitparty
DEALLOCATE c_debitparty

SELECT
    @transdetail_id 'transdetail_id',
    @transaction_amount 'transaction_amount',
    @ledger_short_name 'ledger_short_name',
    @account_type_code 'account_type_code',
    @account_key 'account_key',
    @vat_amount 'vat_amount',
    @taxes_total 'taxes_total',
    @shortname 'shortname',
    @spare 'spare'

GO

