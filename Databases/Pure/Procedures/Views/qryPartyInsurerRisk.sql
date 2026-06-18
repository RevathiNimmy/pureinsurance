SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'qryPartyInsurerRisk'
GO

CREATE VIEW qryPartyInsurerRisk
AS 

SELECT  
    pir.party_cnt,
    pir.risk_code_id,
    rc.description 'RiskCodeDesc',
    pir.risk_transfer_agreement,
    CASE pir.risk_transfer_agreement
        WHEN 0 THEN 'No'
        WHEN 1 THEN 'Yes'
        ELSE ''
    END 'RiskTransferAgreementText',
    pir.delegated_authority,
    CASE pir.delegated_authority
        WHEN 0 THEN 'No'
        WHEN 1 THEN 'Yes'
        ELSE ''
    END 'DelegatedAuthorityText'
FROM party_insurer_risk pir
JOIN risk_code rc
    ON rc.risk_code_id = pir.risk_code_id


GO