SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_TRN_risk_transfer_status_select'
GO

CREATE PROCEDURE spu_TRN_risk_transfer_status_select
(
    @DocumentId INT,
    @AccountId INT
)
AS

DECLARE    
    @risk_transfer_agreement BIT,    
    @party_shortname VARCHAR(20),    
    @party_cnt INT,    
    @party_type_code VARCHAR(10)    
    
SELECT    
    @party_cnt = p.party_cnt,    
    @party_type_code = pt.code    
FROM account a    
JOIN party p    
    ON p.party_cnt = a.account_key    
JOIN party_type pt    
    ON pt.party_type_id = p.party_type_id    
WHERE a.account_id = @AccountId    
  
IF @party_type_code = 'IN'    
BEGIN    
IF NOT  EXISTS ( SELECT NULL FROM document WHERE document_id = @documentid and insurance_file_cnt IS NULL)  
BEGIN  
    
    SELECT    
        @risk_transfer_agreement = IFL.risk_transfer_agreement,    
        @party_shortname = P.shortname    
    FROM document D    
    JOIN insurance_file IFL    
        ON D.insurance_file_cnt = IFL.insurance_file_cnt    
    JOIN party P    
        ON IFL.lead_insurer_cnt = P.party_cnt    
    WHERE D.document_id = @DocumentId    
END    
ELSE  
BEGIN  
SELECT  
@risk_transfer_agreement= PIN.risk_transfer_agreement,  
@party_shortname =ACC.short_code  
FROM   
party_insurer PIN  
JOIN Account ACC  
ON PIN.party_cnt = ACC.account_key  
AND ACC.account_id =@AccountId  
END  
  
    IF @party_shortname LIKE 'MULTI%'    
    BEGIN    
    
        SELECT    
            @risk_transfer_agreement = EPC.risk_transfer_agreement    
        FROM Document D    
        JOIN Transaction_Export_Folder TEF    
            ON D.document_ref = TEF.document_ref    
            AND D.company_id = TEF.source_id    
        JOIN Event_Log EL    
            ON EL.event_cnt = TEF.event_log_id    
        JOIN Event_Policy_Coinsurers EPC    
            ON EPC.insurance_file_cnt = EL.event_cnt    
        WHERE D.document_id = @DocumentId    
        AND EPC.party_cnt = @party_cnt    
    
    END    
    
END    
ELSE    
BEGIN    
    
    SELECT    
        @risk_transfer_agreement = pe.risk_transfer_agreement    
    FROM party p    
    JOIN party_extra pe    
        ON pe.party_cnt = p.party_cnt    
    WHERE p.party_cnt = @party_cnt    
    
END    
    
SELECT @risk_transfer_agreement 'risk_transfer_agreement'    
  
GO

