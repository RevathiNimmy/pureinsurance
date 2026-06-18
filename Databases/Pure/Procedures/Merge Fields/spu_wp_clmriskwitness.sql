SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_clmriskwitness'
GO

CREATE PROCEDURE spu_wp_clmriskwitness
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
    @Witness_Name VARCHAR(255),
    @Witness_Address_1 VARCHAR(60),
    @Witness_Address_2 VARCHAR(60),
    @Witness_Address_3 VARCHAR(60),
    @Witness_Address_4 VARCHAR(60),
    @Witness_PostCode VARCHAR(20),
    @Witness_Date_Of_Birth DATETIME,
    @Witness_Sex  VARCHAR(70),
    @Witness_Phone_Number VARCHAR(300),
    @Witness_Registration_Number VARCHAR(20),
	@Witness_Date_Passed_Test DATETIME,
	@Witness_Contact_Name VARCHAR(255),
	@Witness_Contact_TelNo VARCHAR(60)

DECLARE c_cursor SCROLL CURSOR FOR

    SELECT 
        p.resolved_name,
        a.address1,
        a.address2,
        a.address3,
        a.address4,
        a.postal_code,
        po.date_of_birth,
        po.gender,
        RTRIM(LTRIM(RTRIM(c.area_code) + ' ' + RTRIM(c.number)) + ' ' + RTRIM(c.extension)),
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
            WHERE code = 'OTWITNESS'
        )
    AND ISNULL(cpl.risk_type_id,0) > 0        
    
OPEN c_cursor 

FETCH ABSOLUTE @Instance1 FROM c_cursor INTO
    @Witness_Name,
    @Witness_Address_1,
    @Witness_Address_2,
    @Witness_Address_3,
    @Witness_Address_4,
    @Witness_PostCode,
    @Witness_Date_Of_Birth,
    @Witness_Sex,
    @Witness_Phone_Number,
    @Witness_Registration_Number,
	@Witness_Date_Passed_Test,
	@Witness_Contact_Name,
    @Witness_Contact_TelNo

CLOSE c_cursor
DEALLOCATE c_cursor

SELECT
    @Witness_Name 'Witness_Name',
    @Witness_Address_1 'Witness_Address_1',
    @Witness_Address_2 'Witness_Address_2',
    @Witness_Address_3 'Witness_Address_3',
    @Witness_Address_4 'Witness_Address_4',
    @Witness_PostCode 'Witness_PostCode',
    @Witness_Date_Of_Birth 'Witness_Date_Of_Birth',
    @Witness_Sex 'Witness_Sex',
    @Witness_Phone_Number 'Witness_Phone_Number',
    @Witness_Registration_Number 'Witness_Registration_Number',
	@Witness_Date_Passed_Test 'Witness_Date_Passed_Test',
	@Witness_Contact_Name 'Witness_Contact_Name',
    @Witness_Contact_TelNo 'Witness_Contact_TelNo'
    
GO