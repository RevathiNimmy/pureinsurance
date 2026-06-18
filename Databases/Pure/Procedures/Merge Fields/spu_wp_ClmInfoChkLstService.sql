EXEC DDLDropProcedure 'spu_wp_ClmInfoChkLstService'
GO

CREATE PROCEDURE spu_wp_ClmInfoChkLstService
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE
    @Service_Description VARCHAR(255), 
    @Service VARCHAR(50), 
    @Party VARCHAR(50),
    @Contact VARCHAR(255), 
    @Date_Requested DATETIME, 
    @Date_Critical DATETIME, 
    @Date_Received DATETIME,
    @Address1 VARCHAR(60),
    @Address2 VARCHAR(60),
    @Address3 VARCHAR(60),
    @Address4 VARCHAR(60),
    @PostCode VARCHAR(20),
    @PartyType VARCHAR(255),
    @PartyTypeCode CHAR(10)

DECLARE c_service CURSOR SCROLL KEYSET READ_ONLY FOR
    SELECT 
        ces.Description, 
        ces.Service, 
        ces.Reference,
        ces.Contact, 
        ces.Date_requested, 
        ces.Date_critical, 
        ces.Date_received,
        a.address1,
        a.address2,
        a.address3,
        a.address4,
        a.postal_code,
        pt.description as PartyType,
        pt.code PartyTypeCode
        
    FROM Claim_Expert_Service ces
    JOIN party p
    	ON p.party_cnt = ces.Party_Claim_id
    JOIN party_type pt
    	ON pt.party_type_id = p.party_type_id
    LEFT OUTER JOIN party_address_usage pau
        ON pau.party_cnt = ces.Party_Claim_id
    LEFT OUTER JOIN address_usage_type aut
        ON aut.address_usage_type_id = pau.address_usage_type_id
    LEFT OUTER JOIN address a
        ON a.address_cnt = pau.address_cnt
    WHERE service_type_id=2
    AND ISNULL(aut.code,'3131 XCO') = '3131 XCO'
    AND ces.Claim_id = @ClaimCnt
     
OPEN c_service

FETCH ABSOLUTE @Instance1 FROM c_service INTO
    @Service_Description, 
    @Service, 
    @Party,
    @Contact, 
    @Date_Requested, 
    @Date_Critical, 
    @Date_Received,
    @Address1,
    @Address2,
    @Address3,
    @Address4,
    @PostCode,
    @PartyType,
    @PartyTypeCode

CLOSE c_service
DEALLOCATE c_service

SELECT
    @Service_Description 'Service_Description', 
    @Service 'Service', 
    @Party 'Party',
    @Contact 'Contact', 
    @Date_Requested 'Date_Requested', 
    @Date_Critical 'Date_Critical', 
    @Date_Received 'Date_Received',
    @Address1 'Address1',
    @Address2 'Address2',
    @Address3 'Address3',
    @Address4 'Address4',
    @PostCode 'PostCode',
    @PartyType 'PartyType',
    @PartyTypeCode 'PartyTypeCode'

GO

