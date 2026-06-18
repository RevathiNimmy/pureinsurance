SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Report_Cheque_Register'
GO
CREATE PROCEDURE spu_Report_Cheque_Register
    @printed_date VARCHAR(50),
    @printed_by_user_id INT
AS

SELECT a.company_id,
       s.description,
       b.bankaccount_id,
       b.bank_account_name,
       case 
	When len(c.media_ref)<= 6 Then substring('000000',1,6-len(c.media_ref))+c.media_ref
	Else c.media_ref
        End AS media_ref, 
       t.accounting_date,	
       b.bank_account_name,
       a.account_id,
       a.short_code,
       p.resolved_name,
       cl.our_ref,
       t.insurance_ref,
       t.claim_ref, 
       d.document_ref,
       t.currency_amount,
       cu.iso_code	
FROM   cheque c
JOIN   bankaccount b ON c.bankaccount_id = b.account_id
JOIN   transdetail t ON c.transdetail_id = t.transdetail_id
JOIN   account a ON t.account_id = a.account_id
JOIN   currency cu ON t.currency_id = cu.currency_id
JOIN   cashlistitem cl ON cl.transdetail_id = t.transdetail_id
JOIN   party p ON a.account_key = p.party_cnt 
JOIN   document d ON d.document_id=t.document_id
JOIN   source s ON s.source_id=a.company_id
WHERE  a.account_key <> 0
AND c.printed_date=@printed_date 
AND c.printed_by_user_id=@printed_by_user_id 
UNION ALL
SELECT a.company_id,
       s.description,
       b.bankaccount_id,
       b.bank_account_name,
       case 
	When len(c.media_ref)<= 6 Then substring('000000',1,6-len(c.media_ref))+c.media_ref
	Else c.media_ref
        End AS media_ref, 
       t.accounting_date,	
       b.bank_account_name,
       a.account_id,
       a.short_code,
       CASE WHEN ISNULL(a.payment_name, '') = '' 
                   THEN a.account_name 
                   ELSE a.payment_name 
            END,
       cl.our_ref,
       t.insurance_ref,
       t.claim_ref, 
       d.document_ref,
       t.currency_amount,
       cu.iso_code      
FROM   cheque c
JOIN   bankaccount b ON c.bankaccount_id = b.account_id
JOIN   transdetail t ON c.transdetail_id = t.transdetail_id
JOIN   account a ON t.account_id = a.account_id
JOIN   currency cu ON t.currency_id = cu.currency_id
JOIN   cashlistitem cl ON cl.transdetail_id = t.transdetail_id
JOIN   document d ON d.document_id=t.document_id
JOIN   source s ON s.source_id=a.company_id 
WHERE  a.account_key = 0
AND c.printed_date=@printed_date 
AND c.printed_by_user_id=@printed_by_user_id
ORDER BY a.company_id, b.bankaccount_id, media_ref
GO
