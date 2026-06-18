/*
This stored procedure is used by the following reports:

Client_List.rpt
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_List'
GO

CREATE PROCEDURE spu_Report_Client_List

    @branch_id INT,
    @Live VARCHAR(50)
    
AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

DECLARE 
    @party_cnt INT,
    @address_cnt INT,
    @phone VARCHAR(255),
    @area_code VARCHAR(10),
    @number VARCHAR(255),
    @extension VARCHAR(6)

CREATE TABLE #ReportPartyList
(
    party_cnt INT,
    shortname CHAR(20),
    is_prospect TINYINT,
    prospect CHAR(1),
    resolved_name VARCHAR(255),
    code CHAR(10),
    address1 VARCHAR(60),
    address2 VARCHAR(60),
    address3 VARCHAR(60),
    address4 VARCHAR(60),
    postal_code VARCHAR(20),
    address_cnt INT,
    department VARCHAR(255),
    phone VARCHAR(255),
    branch_name VARCHAR(255)
) 

/*Select all clients*/
INSERT INTO #ReportPartyList
SELECT 
    P.party_cnt,
    P.shortname,
    p.is_prospect,
    'C',
    P.resolved_name,
    PT.code,
    A.address1,
    A.address2,
    A.address3,
    A.address4,
    A.postal_code,
    A.address_cnt,
    '',
    '',
    s.description
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
WHERE PT.code IN ('PC', 'GC', 'CC')
AND AUT.code = "3131 XCO"
AND P.is_deleted = 0
AND P.source_id = ISNULL(@branch_id,P.source_id)

/*Remove clients that don't have a live policy*/
DELETE FROM #ReportPartyList    
WHERE @Live <> 'ALL'
AND NOT EXISTS
    (
        SELECT 
            I.insurance_file_cnt 
        FROM Insurance_File I
        WHERE I.insured_cnt = party_cnt
        AND I.insurance_file_status_id IS NULL
        AND I.insurance_file_type_id <> 1
    )


DECLARE List_Cursor CURSOR FAST_FORWARD FOR
    SELECT party_cnt,
        address_cnt
    FROM #ReportPartyList
    WHERE code = "CC"

OPEN List_Cursor

FETCH NEXT FROM List_Cursor INTO @party_cnt,
                    @address_cnt

WHILE (@@FETCH_STATUS = 0)
BEGIN

    SELECT @address_cnt = NULL

    SELECT @address_cnt = PAU.address_cnt
    FROM Party_Address_Usage PAU,
        Address_Usage_Type AUT
    WHERE PAU.party_cnt = @party_cnt
    AND PAU.address_usage_type_id = AUT.address_usage_type_id
    AND AUT.code = "3131 002"

    IF @address_cnt IS NOT NULL
    BEGIN
        UPDATE #ReportPartyList
        SET address_cnt = @address_cnt
        WHERE party_cnt = @party_cnt
    END

    FETCH NEXT FROM List_Cursor INTO @party_cnt,
                        @address_cnt

END

CLOSE List_Cursor
DEALLOCATE List_Cursor


DECLARE List_Cursor2 CURSOR FAST_FORWARD FOR
    SELECT party_cnt,
        address_cnt,
        phone
    FROM #ReportPartyList

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

    IF ISNULL(@number,'') <> ''
    BEGIN
        SELECT @phone = NULL

        IF @area_code IS NOT NULL BEGIN
            IF @area_code <> "" BEGIN
                SELECT @phone = @area_code
            END
        END

        IF @number IS NOT NULL BEGIN
            IF @number <> "" BEGIN
                IF @phone IS NOT NULL BEGIN
                    SELECT @phone = @phone + " " + @number
                END ELSE BEGIN
                    SELECT @phone = @number
                END
            END
        END

        IF @extension IS NOT NULL BEGIN
            IF @extension <> "" BEGIN
                IF @phone IS NOT NULL BEGIN
                    SELECT @phone = @phone + " ex " + @extension
                END ELSE BEGIN
                    SELECT @phone = @extension
                END
            END
        END

        IF @phone IS NOT NULL BEGIN
            UPDATE #ReportPartyList
            SET phone = @phone
            WHERE party_cnt = @party_cnt
        END

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

UPDATE #ReportPartyList
SET code = "C"
WHERE code = "CC"

UPDATE #ReportPartyList
SET code = "P"
WHERE code = "PC"

UPDATE #ReportPartyList
SET code = "G"
WHERE code = "GC"

UPDATE #ReportPartyList
SET prospect = "P"
WHERE is_prospect = 1

SELECT 
    shortname,
    resolved_name,
    prospect,
    code,
    address1,
    address2,
    address3,
    address4,
    postal_code,
    phone,
    branch_name
FROM #ReportPartyList
ORDER BY shortname

DROP TABLE #ReportPartyList

GO

