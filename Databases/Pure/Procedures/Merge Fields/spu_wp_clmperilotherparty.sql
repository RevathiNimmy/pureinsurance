SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_clmperilotherparty'
GO

CREATE PROCEDURE spu_wp_clmperilotherparty
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
    @Other_Party_Name VARCHAR(255),
    @Other_Party_Address_1 VARCHAR(60),
    @Other_Party_Address_2 VARCHAR(60),
    @Other_Party_Address_3 VARCHAR(60),
    @Other_Party_Address_4 VARCHAR(60),
    @Other_Party_PostCode VARCHAR(20),
    @Other_Party_Date_Of_Birth DATETIME,
    @Other_Party_Sex  VARCHAR(70),
    @Other_Party_Phone_Number VARCHAR(300),
    @Other_Party_Reg_Number VARCHAR(20),
    @Other_Party_Date_Passed_Test DATETIME,
    @Other_Party_Contact_Name VARCHAR(255),
    @Other_Party_Contact_Telephone_Number VARCHAR(60),
    @Other_Party_Insurer_Name VARCHAR(255),
    @Other_Party_Insurer_Address_1 VARCHAR(60),
	@Other_Party_Insurer_Address_2 VARCHAR(60),
	@Other_Party_Insurer_Address_3 VARCHAR(60),
	@Other_Party_Insurer_Address_4 VARCHAR(60),
	@Other_Party_Insurer_PostCode VARCHAR(20),
	@Other_Party_Insurer_Telephone_Number VARCHAR(60),
	@Other_Party_Insurer_Fax_Number VARCHAR(60),
	@Other_Party_Insurer_Contact_Name VARCHAR(255),
	@Other_Party_Insurer_Email VARCHAR(255),
	@Other_Party_Insurer_Notes VARCHAR(2000),
	@Other_Party_Company_Notes VARCHAR(2000),
	@Other_Party_Reference_Number VARCHAR(20),
	@Other_Party_Party_Type CHAR(10)
	
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
        po.contact_telephone_number,
        po.insurer_name,
        po.insurer_address1,
        po.insurer_address2,
        po.insurer_address3,
        po.insurer_address4,
        po.insurer_postcode,
        po.insurer_telephone_number,
        po.insurer_fax_number,
        po.insurer_contact_name,
        po.insurer_email,
        po.insurer_notes,
        po.company_notes,
        po.reference_number,
        pt.code
    FROM claim_party_link cpl
    JOIN party p
        ON p.party_cnt = cpl.party_cnt
    JOIN party_type pt
    	ON p.party_type_id = pt.party_type_id
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
    AND ISNULL(cpl.peril_type_id,0) > 0
    
OPEN c_cursor 

FETCH ABSOLUTE @Instance1 FROM c_cursor INTO
    @Other_Party_Name,
    @Other_Party_Address_1,
    @Other_Party_Address_2,
    @Other_Party_Address_3,
    @Other_Party_Address_4,
    @Other_Party_PostCode,
    @Other_Party_Date_Of_Birth,
    @Other_Party_Sex,
    @Other_Party_Phone_Number,
    @Other_Party_Reg_Number,
    @Other_Party_Date_Passed_Test,
	@Other_Party_Contact_Name,
	@Other_Party_Contact_Telephone_Number,
	@Other_Party_Insurer_Name,
	@Other_Party_Insurer_Address_1,
	@Other_Party_Insurer_Address_2,
	@Other_Party_Insurer_Address_3,
	@Other_Party_Insurer_Address_4,
	@Other_Party_Insurer_PostCode,
	@Other_Party_Insurer_Telephone_Number,
	@Other_Party_Insurer_Fax_Number,
	@Other_Party_Insurer_Contact_Name,
	@Other_Party_Insurer_Email,
	@Other_Party_Insurer_Notes,
	@Other_Party_Company_Notes,
	@Other_Party_Reference_Number,
	@Other_Party_Party_Type

CLOSE c_cursor
DEALLOCATE c_cursor

SELECT
    @Other_Party_Name 'Other_Party_Name',
    @Other_Party_Address_1 'Other_Party_Address_1',
    @Other_Party_Address_2 'Other_Party_Address_2',
    @Other_Party_Address_3 'Other_Party_Address_3',
    @Other_Party_Address_4 'Other_Party_Address_4',
    @Other_Party_PostCode 'Other_Party_PostCode',
    @Other_Party_Date_Of_Birth 'Other_Party_Date_Of_Birth',
    @Other_Party_Sex 'Other_Party_Sex',
    @Other_Party_Phone_Number 'Other_Party_Phone_Number',
    @Other_Party_Reg_Number 'Other_Party_Reg_Number',
    @Other_Party_Date_Passed_Test 'Other_Party_Date_Passed_Test',
	@Other_Party_Contact_Name 'Other_Party_Contact_Name',
	@Other_Party_Contact_Telephone_Number 'Other_Party_Contact_Telephone_Number',
	@Other_Party_Insurer_Name 'Other_Party_Insurer_Name',
	@Other_Party_Insurer_Address_1 'Other_Party_Insurer_Address_1',
	@Other_Party_Insurer_Address_2 'Other_Party_Insurer_Address_2',
	@Other_Party_Insurer_Address_3 'Other_Party_Insurer_Address_3',
	@Other_Party_Insurer_Address_4 'Other_Party_Insurer_Address_4',
	@Other_Party_Insurer_PostCode 'Other_Party_Insurer_PostCode',
	@Other_Party_Insurer_Telephone_Number 'Other_Party_Insurer_Telephone_Number',
	@Other_Party_Insurer_Fax_Number 'Other_Party_Insurer_Fax_Number',
	@Other_Party_Insurer_Contact_Name 'Other_Party_Insurer_Contact_Name',
	@Other_Party_Insurer_Email 'Other_Party_Insurer_Email',
	@Other_Party_Insurer_Notes 'Other_Party_Insurer_Notes',
	@Other_Party_Company_Notes 'Other_Party_Company_Notes',
	@Other_Party_Reference_Number 'Other_Party_Reference_Number',
	@Other_Party_Party_Type 'Other_Party_Party_Type'

    
GO