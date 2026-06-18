SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'qryFSA'
GO

CREATE VIEW qryFSA
AS 

SELECT  
    i.insurance_file_cnt,
    i.fsa_contract_location_id,
    CASE i.fsa_contract_location_id
        WHEN 0 THEN 'Non Distance'
        WHEN 1 THEN 'Distance with info provided'
        WHEN 2 THEN 'Distance - unable to provide info'
        ELSE ''
    END 'FSAContractLocation',
    i.fsa_customer_category_id,
    CASE i.fsa_customer_category_id
        WHEN 0 THEN 'Commercial'
        WHEN 1 THEN 'Retail'
        ELSE ''
    END 'FSACustomerCategory',
    i.fsa_renewal_consent,
    i.fsa_type_of_sale_id,
    CASE i.fsa_type_of_sale_id
        WHEN 0 THEN 'Advised'
        WHEN 1 THEN 'Non advised'
        ELSE ''
    END 'FSATypeOfSale',
    i.fsa_underwriter_cnt,
    p.resolved_name 'FSAUnderwriter',
    i.terms_agreed, 
    CASE i.terms_agreed
        WHEN 0 THEN 'No'
        WHEN 1 THEN 'Yes'
        ELSE ''
    END 'TermsAgreedText',
    i.terms_agreed_date,
    i.inception_date,
    i.policy_documents_issued_date,
    i.policy_documents_correct,
    CASE i.policy_documents_correct
        WHEN 0 THEN 'No'
        WHEN 1 THEN 'Yes'
        ELSE ''
    END 'PolicyDocumentsCorrectText',
    i.error_notification_date,
    i.risk_transfer_agreement,
    CASE i.risk_transfer_agreement
        WHEN 0 THEN 'No'
        WHEN 1 THEN 'Yes'
        ELSE ''
    END 'RiskTransferAgreementText'
FROM insurance_file i
LEFT JOIN party p 
    ON p.party_cnt = i.fsa_underwriter_cnt


GO
