SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Payment_Advice'
GO


CREATE PROCEDURE spu_Report_Payment_Advice
    @allocation_id INT
AS

SELECT 
    CASE
        WHEN d.documenttype_id IN (22,23) THEN 'Cash'
        ELSE 'Business'
    END TransactionType,
    d.document_date DocumentDate,
    d.document_ref DocumentRef,
    dt.documenttype_id DocumentTypeID,
    dt.description DocTypeDesc,
    ISNULL(i.insurance_ref,'') PolicyRef,
    (
        SELECT ISNULL(cli.media_ref, '') 
        FROM cashlistitem cli
        JOIN transdetail td
            ON td.transdetail_id = cli.transdetail_id
        WHERE td.document_id = d.document_id
        
    ) MediaRef,
    a.Insurer,
    a.Address1,
    a.Address2,
    a.Address3,
    a.Address4,
    a.PostalCode,    
    (   
        SELECT ISNULL(a.account_name, '')
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        WHERE td.document_id = d.document_id
        AND td.document_sequence = 
            (   
                SELECT MIN(document_sequence)
                FROM transdetail        
                WHERE document_id = td.document_id 
                AND ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            )
    ) Client,
    (
        SELECT ISNULL(a.short_code, '')
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        WHERE td.document_id = d.document_id
        AND td.document_sequence = 
            (   
                SELECT MIN(document_sequence)
                FROM transdetail        
                WHERE document_id = td.document_id 
                AND ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            )
    ) ClientCode, 
    (
        SELECT ISNULL(SUM(ISNULL(tm.base_match_amount,0)),0)
        FROM allocationdetail ad
        JOIN transmatch tm
            ON tm.allocationdetail_id = ad.allocationdetail_id 
        JOIN transdetail td
            ON td.transdetail_id = tm.transdetail_id
        WHERE td.document_id = d.document_id
        AND ad.allocation_Id = @allocation_id
    ) ThisPayment 
FROM document d
JOIN documenttype dt
    ON dt.documenttype_id = d.documenttype_id
LEFT JOIN insurance_file i
    ON i.insurance_file_cnt = d.insurance_file_cnt
CROSS JOIN
    (
        SELECT 
            ISNULL(p.name, '') Insurer,
            ISNULL(ad.address1, '') Address1,
            ISNULL(ad.address2, '') Address2,
            ISNULL(ad.address3, '') Address3,
            ISNULL(ad.address4, '') Address4,
            ISNULL(ad.postal_code, '') PostalCode
        FROM allocation al
        JOIN account a
            ON a.account_id = al.account_id
        JOIN party p
            ON p.party_cnt = a.account_key            
        LEFT JOIN party_address_usage pau
            ON pau.party_cnt = p.party_cnt 
            AND pau.address_usage_type_id = 4
        LEFT JOIN address ad
            ON ad.address_cnt = pau.address_cnt
        WHERE al.allocation_id = @allocation_id
    ) a
WHERE EXISTS
    (
        SELECT NULL
        FROM allocationdetail ad
        JOIN transmatch tm
            ON tm.allocationdetail_id = ad.allocationdetail_id 
        JOIN transdetail td
            ON td.transdetail_id = tm.transdetail_id
        WHERE ad.allocation_id = @allocation_id
        AND td.document_id = d.document_id
    )
ORDER BY 
    DocumentTypeId,
    ClientCode


GO

