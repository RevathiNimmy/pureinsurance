SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_clmriskdriver'
GO

CREATE PROCEDURE spu_wp_clmriskdriver
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
    @Driver_Name VARCHAR(255),
    @Driver_Address_1 VARCHAR(60),
    @Driver_Address_2 VARCHAR(60),
    @Driver_Address_3 VARCHAR(60),
    @Driver_Address_4 VARCHAR(60),
    @Driver_PostCode VARCHAR(20),
    @Driver_License_Type VARCHAR(50),
    @Driver_License_Number VARCHAR(20),
    @Driver_Date_Of_Birth DATETIME,
    @Driver_Sex  VARCHAR(70),
    @Driver_Phone_Number VARCHAR(300),
    @Driver_Party_Status VARCHAR(50),
    @Driver_Registration_Number VARCHAR(20),
	@Driver_Date_Passed_Test DATETIME,
	@Driver_Contact_Name VARCHAR(255),
	@Driver_Contact_TelNo VARCHAR(60)
	
DECLARE c_cursor SCROLL CURSOR FOR

    SELECT 
        p.resolved_name,
        a.address1,
        a.address2,
        a.address3,
        a.address4,
        a.postal_code,
        lt.description,
        po.license_number,
        po.date_of_birth,
        po.gender,
        RTRIM(LTRIM(RTRIM(c.area_code) + ' ' + RTRIM(c.number)) + ' ' + RTRIM(c.extension)),
        ds.description,
        po.reg_number,
        po.date_passed_test,
        po.contact_name,
        po.contact_telephone_number
    FROM claim_party_link cpl
    JOIN party p
        ON p.party_cnt = cpl.party_cnt
    LEFT JOIN party_address_usage pau
        ON pau.party_cnt = p.party_cnt
    LEFT JOIN address_usage_type aut
        ON aut.address_usage_type_id = pau.address_usage_type_id
    LEFT JOIN address a
        ON a.address_cnt = pau.address_cnt
    LEFT JOIN party_other po
        ON po.party_cnt = p.party_cnt
    LEFT JOIN license_type lt
        ON lt.license_type_id = po.license_type_id
    LEFT JOIN driver_status ds
        ON ds.driver_status_id = po.party_status
    LEFT JOIN contact_address_usage cau
        JOIN contact c
            ON c.contact_cnt = cau.contact_cnt
        JOIN contact_type ct
            ON ct.contact_type_id = c.contact_type_id
            AND ct.code = 'TELEPHONE'
        ON cau.address_cnt = a.address_cnt
        AND cau.contact_cnt = 
            (
                SELECT
                    MIN(cau.contact_cnt)
                FROM contact_address_usage cau
                JOIN contact c
                    ON c.contact_cnt = cau.contact_cnt
                JOIN contact_type ct
                    ON ct.contact_type_id = c.contact_type_id
                AND ct.code = 'TELEPHONE'
                WHERE cau.address_cnt = a.address_cnt
            )
    WHERE cpl.claim_id = @ClaimCnt
    AND ISNULL(aut.code,'3131 XCO') = '3131 XCO'
    AND p.party_type_id IN
        (
            SELECT 
                party_type_id
            FROM party_type
            WHERE code = 'OTDRIVER'
        )
	AND ISNULL(cpl.risk_type_id,0) > 0        
    
OPEN c_cursor 

FETCH ABSOLUTE @Instance1 FROM c_cursor INTO
    @Driver_Name,
    @Driver_Address_1,
    @Driver_Address_2,
    @Driver_Address_3,
    @Driver_Address_4,
    @Driver_PostCode,
    @Driver_License_Type,
    @Driver_License_Number,
    @Driver_Date_Of_Birth,
    @Driver_Sex,
    @Driver_Phone_Number,
    @Driver_Party_Status,
    @Driver_Registration_Number,
    @Driver_Date_Passed_Test,
    @Driver_Contact_Name,
    @Driver_Contact_TelNo

CLOSE c_cursor
DEALLOCATE c_cursor

SELECT
    @Driver_Name 'Driver_Name',
    @Driver_Address_1 'Driver_Address_1',
    @Driver_Address_2 'Driver_Address_2',
    @Driver_Address_3 'Driver_Address_3',
    @Driver_Address_4 'Driver_Address_4',
    @Driver_PostCode 'Driver_PostCode',
    @Driver_License_Type 'Driver_License_Type',
    @Driver_License_Number 'Driver_License_Number',
    @Driver_Date_Of_Birth 'Driver_Date_Of_Birth',
    @Driver_Sex 'Driver_Sex',
    @Driver_Phone_Number 'Driver_Phone_Number',
    @Driver_Party_Status 'Driver_Party_Status',
	@Driver_Registration_Number 'Driver_Registration_Number',
	@Driver_Date_Passed_Test 'Driver_Date_Passed_Test',
    @Driver_Contact_Name 'Driver_Contact_Name',
    @Driver_Contact_TelNo 'Driver_Contact_TelNo'
    
GO