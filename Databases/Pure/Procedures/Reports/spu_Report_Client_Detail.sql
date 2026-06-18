SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_Detail'
GO

CREATE PROCEDURE spu_Report_Client_Detail

    @branch_id INT,
    @status VARCHAR(50),
    @unique_report_name VARCHAR(300)

AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @status = '' OR @status is null
BEGIN
    SELECT @status = 'All'
END

SET NOCOUNT ON
DECLARE
    @party_cnt INT,
    @address_cnt INT,
    @phone VARCHAR(255),
    @area_code VARCHAR(10),
    @number VARCHAR(255),
    @extension VARCHAR(6)

CREATE TABLE #ReportPartyDetail
(
    party_cnt INT,
    shortname CHAR(20),
    resolved_name VARCHAR(255),
    Title VARCHAR (70),
    Initials VARCHAR (20),
    forename VARCHAR(60),
    surname VARCHAR(255),
    is_prospect TINYINT,
    prospect CHAR(1),
    code CHAR(10),
    address1 VARCHAR(60),
    address2 VARCHAR(60),
    address3 VARCHAR(60),
    address4 VARCHAR(60),
    postal_code VARCHAR(20),
    address_cnt INT,
    phone VARCHAR(255), 
    email VARCHAR(50),
    Account_Exec_cnt INT,
    Account_Exec VARCHAR(255), 
    branch_name VARCHAR(255),
    client_type VARCHAR(50),
    policy_count INT
)

/*Select all clients*/
INSERT INTO #ReportPartyDetail
SELECT
    P.party_cnt,
    P.shortname,
    P.resolved_name,
    '',
    '',
    '',
    p.name,
    p.is_prospect,
    'C',
    PT.code,
    A.address1,
    A.address2,
    A.address3,
    A.address4,
    A.postal_code,
    A.address_cnt,
    '',
    '',
    ISNULL(PAE.party_cnt,0),
    ISNULL(PAE.resolved_name, ''),
    s.description,
    '',
    ''
FROM Party P
JOIN Party_Type PT
    ON PT.party_type_id = P.party_type_id
JOIN Party_Address_Usage PAU
    ON PAU.party_cnt = P.party_cnt
JOIN Address_Usage_Type AUT
    ON AUT.address_usage_type_id = PAU.address_usage_type_id
JOIN Address A
    ON A.address_cnt = PAU.address_cnt
JOIN source s
    ON s.source_id = P.source_id
LEFT OUTER JOIN Party PAE
    ON P.consultant_cnt = PAE.party_cnt
WHERE PT.code IN ('PC', 'GC', 'CC')
AND AUT.code = '3131 XCO'
AND P.is_deleted = 0
AND P.source_id = ISNULL(@branch_id,P.source_id)

/*Remove clients that don't have a live policy*/

DELETE FROM #ReportPartyDetail
WHERE @status <> 'ALL'
AND NOT EXISTS
    (
        SELECT
            I.insurance_file_cnt
        FROM Insurance_File I
        WHERE I.insured_cnt = party_cnt
        AND I.insurance_file_status_id IS NULL
    )


UPDATE  R SET
R.address_cnt = PAU.address_cnt
FROM #ReportPartyDetail R 
JOIN Party_Address_Usage PAU
ON R.party_cnt=PAU.party_cnt 
JOIN Address_Usage_Type AUT
ON PAU.address_usage_type_id = AUT.address_usage_type_id
AND AUT.code = '3131 002'
WHERE R.code = 'CC'

DECLARE List_Cursor2 CURSOR FAST_FORWARD FOR
    SELECT party_cnt,
        address_cnt,
        phone
    FROM #ReportPartyDetail

OPEN List_Cursor2

FETCH NEXT FROM List_Cursor2 INTO
    @party_cnt,
    @address_cnt,
    @phone

WHILE @@FETCH_STATUS = 0
BEGIN

    IF EXISTS
        (
            SELECT
                NULL
            FROM Contact_Address_Usage CAU
            JOIN Contact C
                ON C.contact_cnt = CAU.contact_cnt
            JOIN Contact_Type CT
                ON CT.contact_type_id = C.contact_type_id
            WHERE CAU.address_cnt = @address_cnt
            AND CT.code = 'TELEPHONE'
        )
    BEGIN
        SELECT
            @area_code = area_code,
            @number = number,
            @extension = extension
        FROM Contact_Address_Usage CAU
        JOIN Contact C
            ON C.contact_cnt = CAU.contact_cnt
        JOIN Contact_Type CT
            ON CT.contact_type_id = C.contact_type_id
        WHERE CAU.address_cnt = @address_cnt
        AND CT.code = 'TELEPHONE'
    END
    ELSE
    BEGIN
        SELECT
            @area_code = area_code,
            @number = number,
            @extension = extension
        FROM Party_Contact_Usage PCU
        JOIN Contact C
            ON C.contact_cnt = PCU.contact_cnt
        JOIN Contact_Type CT
            ON CT.contact_type_id = C.contact_type_id
        WHERE PCU.party_cnt = @party_cnt
        AND CT.code = 'TELEPHONE'
    END
	
	IF @extension IS NOT NULL AND  @extension <>''
		SELECT @phone=rtrim(@area_code) + ' ' + rtrim(@number) + ' ex ' + rtrim(@extension)
	ELSE
		SELECT @phone=rtrim(@area_code) + ' ' + rtrim(@number)

	IF @phone IS NOT NULL BEGIN
            UPDATE #ReportPartyDetail
            SET phone = ltrim(rtrim(@phone))
            WHERE party_cnt = @party_cnt
        END
    --Clearup variables
        SELECT
            @area_code = NULL,
            @number = NULL,
            @extension = NULL

    FETCH NEXT FROM List_Cursor2 INTO
        @party_cnt,
        @address_cnt,
        @phone

END

CLOSE List_Cursor2
DEALLOCATE List_Cursor2

-- Updating Client Type-----------------------------------------------------
UPDATE #ReportPartyDetail
SET client_type = 'Corporate Client'
WHERE code = 'CC'

UPDATE #ReportPartyDetail
SET client_type = 'Personal Client'
WHERE code = 'PC'

UPDATE #ReportPartyDetail
SET client_type = 'Group Client'
WHERE code = 'GC'

-- Updating Client Status---------------------------------------------------
UPDATE #ReportPartyDetail
SET prospect = 'P'
WHERE is_prospect = 1

-- Updating title Initials and forename of PC-------------------------------
UPDATE  R SET
R.Title = PC.party_title_code,
R.Initials= PC.initials,
R.forename=PC.forename
FROM #ReportPartyDetail R 
JOIN party_personal_client PC
ON R.party_cnt=pc.party_cnt 

UPDATE #ReportPartyDetail 
SET Resolved_name= Null 
WHERE code = 'PC'

UPDATE #ReportPartyDetail 
SET surname= Null,title=Null, 
Initials=null,forename=null
WHERE code IN ('CC','GC')

-- Updating Email Id---------------------------------------------------------
UPDATE  R SET
R.email = a.address1
FROM #ReportPartyDetail R 
JOIN Party_Address_Usage PAU
ON R.party_cnt=PAU.party_cnt 
JOIN Address A 
ON A.address_cnt=PAU.address_cnt
JOIN Address_Usage_Type AUT
ON PAU.address_usage_type_id = AUT.address_usage_type_id
AND AUT.code = '3131 ECK'

UPDATE R SET  
R.email = C.description 
FROM #ReportPartyDetail R
JOIN Party_Address_Usage PAU  
ON R.party_cnt=PAU.party_cnt 
JOIN contact_address_usage CAU 
ON PAU.address_cnt = CAU.address_cnt 
JOIN Contact C
ON CAU.contact_cnt= C.contact_cnt
JOIN contact_type CT
ON c.contact_type_id = CT.contact_type_id
AND CT.code='E-MAIL'
WHERE R.email='' OR R.email IS NULL

-- Exclude Account Executive-------------------------------------------------
DELETE #ReportPartyDetail 
FROM #ReportPartyDetail 
WHERE party_cnt IN
(SELECT t.party_cnt FROM #ReportPartyDetail t JOIN
(SELECT t2.party_cnt
	FROM #ReportPartyDetail  t2 WHERE t2.account_exec_cnt IN 
        (SELECT tp_ex.ID FROM Temp_Report_Exclude tp_ex 
         WHERE  unique_report_name = @unique_report_name AND type IN ('AE'))) as t2
ON t.party_cnt=t2.party_cnt
WHERE NOT((t.account_exec_cnt IS NULL) OR LTRIM(RTRIM(t.account_exec_cnt))=''  OR LTRIM(RTRIM(t.account_exec_cnt))=0))



--updating policy  count-----------------------------------------------------
IF @status ='All'
	BEGIN
		UPDATE  R SET
		policy_count=(SELECT COUNT(*)
		FROM insurance_file i
		WHERE
		i.policy_version=1 AND i.insured_cnt= R .party_cnt AND i.risk_code_id NOT IN
		(
	         SELECT tp_ex.ID 
	         FROM Temp_Report_Exclude tp_ex 
                 WHERE  unique_report_name = @unique_report_name AND type='RC'
	   	)
		AND i.lead_insurer_cnt NOT IN
		(
	         SELECT tp_ex.ID 
	         FROM Temp_Report_Exclude tp_ex 
                 WHERE  unique_report_name = @unique_report_name AND type='IN'
	   	)
		GROUP BY shortname)
		FROM #ReportPartyDetail R
	END
ELSE
	BEGIN
		UPDATE  R SET
		policy_count=(SELECT COUNT(*)
		FROM insurance_file i
		WHERE
                i.policy_version= 
                (
                    SELECT 
                        ISNULL(MAX(I1.policy_version), 1)
                    FROM insurance_file I1
        	    JOIN insurance_file_type IFT1
        	    ON IFT1.insurance_file_type_id = I1.insurance_file_type_id
        	    WHERE I1.insurance_folder_cnt = i.insurance_folder_cnt 
		)
		AND i.insured_cnt= R .party_cnt  
                and i.insurance_file_type_id in (2,5,6) AND (insurance_file_status_id not in (1,2,4) OR insurance_file_status_id is null) AND i.risk_code_id NOT IN  
		(
	         SELECT tp_ex.ID 
	         FROM Temp_Report_Exclude tp_ex 
                 WHERE  unique_report_name = @unique_report_name AND type='RC'
	   	)
		AND i.lead_insurer_cnt NOT IN
		(
	         SELECT tp_ex.ID 
	         FROM Temp_Report_Exclude tp_ex 
                 WHERE  unique_report_name = @unique_report_name AND type='IN'
	   	)
		GROUP BY shortname)
		FROM #ReportPartyDetail R 
	END


UPDATE #ReportPartyDetail SET policy_count=0 where policy_count is null

SET NOCOUNT OFF

SELECT
 *
FROM #ReportPartyDetail 
ORDER BY shortname

DROP TABLE #ReportPartyDetail
           
GO

