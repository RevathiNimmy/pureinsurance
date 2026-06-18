SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_clmriskrepairer'
GO

CREATE PROCEDURE spu_wp_clmriskrepairer
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
    @Repairer_Name VARCHAR(255),
    @Repairer_Address_1 VARCHAR(60),
    @Repairer_Address_2 VARCHAR(60),
    @Repairer_Address_3 VARCHAR(60),
    @Repairer_Address_4 VARCHAR(60),
    @Repairer_PostCode VARCHAR(20),
    @Repairer_Date_Of_Birth DATETIME,
    @Repairer_Phone_Number VARCHAR(300),
    @Repairer_Fax_Number VARCHAR(300),
    @Repairer_Registration_Number VARCHAR(20),
	@Repairer_Date_Passed_Test DATETIME,
	@Repairer_Contact_Name VARCHAR(255),
	@Repairer_Contact_TelNo VARCHAR(60)

DECLARE c_cursor SCROLL CURSOR FOR

    SELECT 
        p.resolved_name,
        a.address1,
        a.address2,
        a.address3,
        a.address4,
        a.postal_code,
        po.date_of_birth,
        RTRIM(LTRIM(RTRIM(c.area_code) + ' ' + RTRIM(c.number)) + ' ' + RTRIM(c.extension)),
        RTRIM(LTRIM(RTRIM(c_f.area_code) + ' ' + RTRIM(c_f.number)) + ' ' + RTRIM(c_f.extension)),
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
    LEFT JOIN contact_address_usage cau_f
        JOIN contact c_f
            ON c_f.contact_cnt = cau_f.contact_cnt
        JOIN contact_type ct_f
            ON ct_f.contact_type_id = c_f.contact_type_id
            AND ct_f.code = 'FAX'
        ON cau_f.address_cnt = a.address_cnt
        AND cau_f.contact_cnt = 
            (
                SELECT
                    MIN(cau.contact_cnt)
                FROM contact_address_usage cau
                JOIN contact c
                    ON c.contact_cnt = cau.contact_cnt
                JOIN contact_type ct
                    ON ct.contact_type_id = c.contact_type_id
                AND ct.code = 'FAX'
                WHERE cau.address_cnt = a.address_cnt
            )
    WHERE cpl.claim_id = @ClaimCnt
    AND ISNULL(aut.code,'3131 XCO') = '3131 XCO'
    AND p.party_type_id IN
        (
            SELECT 
                party_type_id
            FROM party_type
            WHERE code = 'OTREPAIRER'
        )
    AND ISNULL(cpl.risk_type_id,0) > 0        
    
OPEN c_cursor 

FETCH ABSOLUTE @Instance1 FROM c_cursor INTO
    @Repairer_Name,
    @Repairer_Address_1,
    @Repairer_Address_2,
    @Repairer_Address_3,
    @Repairer_Address_4,
    @Repairer_PostCode,
    @Repairer_Date_Of_Birth,
    @Repairer_Phone_Number,
    @Repairer_Fax_Number,
    @Repairer_Registration_Number,
	@Repairer_Date_Passed_Test,
	@Repairer_Contact_Name,
	@Repairer_Contact_TelNo


CLOSE c_cursor
DEALLOCATE c_cursor

SELECT
    @Repairer_Name 'Repairer_Name',
    @Repairer_Address_1 'Repairer_Address_1',
    @Repairer_Address_2 'Repairer_Address_2',
    @Repairer_Address_3 'Repairer_Address_3',
    @Repairer_Address_4 'Repairer_Address_4',
    @Repairer_PostCode 'Repairer_PostCode',
    @Repairer_Date_Of_Birth 'Repairer_Date_Of_Birth',
    @Repairer_Phone_Number 'Repairer_Phone_Number',
    @Repairer_Fax_Number 'Repairer_Fax_Number',
    @Repairer_Registration_Number 'Repairer_Registration_Number',
	@Repairer_Date_Passed_Test 'Repairer_Date_Passed_Test',
	@Repairer_Contact_Name 'Repairer_Contact_Name',
    @Repairer_Contact_TelNo 'Repairer_Contact_TelNo'
    
GO