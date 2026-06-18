SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Cheque'
GO


CREATE PROCEDURE spu_ACT_Select_Cheque
    @transaction_date datetime,
    @company_id int
AS

IF @company_id=0
	SELECT @company_id=NULL

if   @company_id is null
BEGIN
SELECT b.bank_account_name,    
       c.cheque_id,    
       c.transdetail_id,    
       t.accounting_date,    
       cl.our_ref,    
       t.currency_amount,    
       cu.currency_id,    
       c.media_ref,    
       a.account_id,    
       a.short_code,    
       p.resolved_name,    
       a.address1,    
       a.address2,    
       a.address3,    
       a.address4,    
       a.postal_code,    
       d.document_ref,    
       a.account_key,    
       cl.our_ref,    
       t.company_id,    
       s.description,    
       b.bankaccount_id    
FROM   cheque c    
JOIN   bankaccount b ON c.bankaccount_id = b.account_id    
JOIN   transdetail t ON c.transdetail_id = t.transdetail_id    
JOIN   account a ON t.account_id = a.account_id    
JOIN   currency cu ON t.currency_id = cu.currency_id    
JOIN   cashlistitem cl ON cl.transdetail_id = t.transdetail_id    
JOIN   party p ON a.account_key = p.party_cnt    
JOIN   document d ON d.document_id=t.document_id    
JOIN   source s ON s.source_id=t.company_id    
WHERE  t.accounting_date <= @transaction_date    
AND    a.account_key <> 0    
AND    c.printed_date IS NULL    
UNION ALL    
SELECT b.bank_account_name,    
       c.cheque_id,    
       c.transdetail_id,    
       t.accounting_date,    
       cl.our_ref,    
       t.currency_amount,    
       cu.currency_id,    
       c.media_ref,    
       a.account_id,    
       a.short_code,    
       CASE WHEN ISNULL(a.payment_name, '') = ''    
            THEN a.account_name    
            ELSE a.payment_name    
            END,    
       a.address1,    
       a.address2,    
       a.address3,    
       a.address4,    
       a.postal_code,    
       d.document_ref,    
       a.account_key,    
       cl.our_ref,    
       t.company_id,    
       s.description,    
       b.bankaccount_id    
FROM   cheque c    
JOIN   bankaccount b ON c.bankaccount_id = b.account_id    
JOIN   transdetail t ON c.transdetail_id = t.transdetail_id    
JOIN   account a ON t.account_id = a.account_id    
JOIN   currency cu ON t.currency_id = cu.currency_id    
JOIN   cashlistitem cl ON cl.transdetail_id = t.transdetail_id    
JOIN   document d ON d.document_id=t.document_id    
JOIN   source s ON s.source_id=t.company_id    
WHERE  t.accounting_date <= @transaction_date    
AND    a.account_key = 0    
AND    c.printed_date IS NULL    
end 
else

begin
SELECT b.bank_account_name,    
       c.cheque_id,    
       c.transdetail_id,    
       t.accounting_date,    
       cl.our_ref,    
       t.currency_amount,    
       cu.currency_id,    
       c.media_ref,    
       a.account_id,    
       a.short_code,    
       p.resolved_name,    
       a.address1,    
       a.address2,    
       a.address3,    
       a.address4,    
       a.postal_code,    
       d.document_ref,    
       a.account_key,    
       cl.our_ref,    
       t.company_id,    
       s.description,    
       b.bankaccount_id    
FROM   cheque c    
JOIN   bankaccount b ON c.bankaccount_id = b.account_id    
JOIN   transdetail t ON c.transdetail_id = t.transdetail_id    
JOIN   account a ON t.account_id = a.account_id    
JOIN   currency cu ON t.currency_id = cu.currency_id    
JOIN   cashlistitem cl ON cl.transdetail_id = t.transdetail_id    
JOIN   party p ON a.account_key = p.party_cnt    
JOIN   document d ON d.document_id=t.document_id    
JOIN   source s ON s.source_id=t.company_id    
WHERE  t.accounting_date <= @transaction_date    
AND    a.account_key <> 0    
AND    t.company_id = @company_id
AND    c.printed_date IS NULL    
UNION ALL    
SELECT b.bank_account_name,    
       c.cheque_id,    
       c.transdetail_id,    
       t.accounting_date,    
       cl.our_ref,    
       t.currency_amount,    
       cu.currency_id,    
       c.media_ref,    
       a.account_id,    
       a.short_code,    
       CASE WHEN ISNULL(a.payment_name, '') = ''    
            THEN a.account_name    
            ELSE a.payment_name    
            END,    
       a.address1,    
       a.address2,    
       a.address3,    
       a.address4,    
       a.postal_code,    
       d.document_ref,    
       a.account_key,    
       cl.our_ref,    
       t.company_id,    
       s.description,    
       b.bankaccount_id    
FROM   cheque c    
JOIN   bankaccount b ON c.bankaccount_id = b.account_id    
JOIN   transdetail t ON c.transdetail_id = t.transdetail_id    
JOIN   account a ON t.account_id = a.account_id    
JOIN   currency cu ON t.currency_id = cu.currency_id    
JOIN   cashlistitem cl ON cl.transdetail_id = t.transdetail_id    
JOIN   document d ON d.document_id=t.document_id    
JOIN   source s ON s.source_id=t.company_id    
WHERE  t.accounting_date <= @transaction_date    
AND    a.account_key = 0    
AND    t.company_id =@company_id
AND    c.printed_date IS NULL  
end 

GO


