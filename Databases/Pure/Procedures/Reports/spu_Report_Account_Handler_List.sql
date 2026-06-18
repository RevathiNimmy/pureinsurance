/*
This stored procedure is used by the following reports:

AccountHandlers.rpt
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Account_Handler_List'
GO

CREATE PROCEDURE spu_Report_Account_Handler_List

   @branch_id INT
   
AS

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

DECLARE @party_cnt INT,
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
    phone VARCHAR(255)
) 

INSERT INTO #ReportPartyList
SELECT P.party_cnt,
    P.shortname,
    P.is_prospect,
    '' prospect,
    P.resolved_name,
    PT.code,
    '' address1,
    '' address2,
    '' address3,
    '' address4,
    '' postal_code,
    0 address_cnt,
    ISNULL(
        (
            SELECT ISNULL(description, '')
            FROM Department
            WHERE department_id = PAH.department_id
        )
, '') department,
    '' phone
FROM Party P,
    Party_Type PT,
    Party_Handler PAH               --DC290903 -PS256 -was party_account_handler
WHERE P.party_type_id = PT.party_type_id
AND (PT.code = 'AH' OR PT.code = 'HC')      --DC290903 -PS256 -added executive handler
AND PAH.party_cnt = P.party_cnt
AND P.source_id = ISNULL(@branch_id,P.source_id)    --MKR PN 11363 -Filtered on Branch

DECLARE List_Cursor2 CURSOR FAST_FORWARD FOR
    SELECT party_cnt,
        phone
    FROM #ReportPartyList

OPEN List_Cursor2

FETCH NEXT FROM List_Cursor2 INTO @party_cnt, @phone

WHILE @@FETCH_STATUS = 0 BEGIN
    DECLARE Phone_Cursor CURSOR FAST_FORWARD FOR
        SELECT area_code,
            number,
            extension
        FROM Party_Contact_Usage CU,
            Contact C,
            Contact_Type CT
        WHERE CU.party_cnt = @party_cnt
        AND CU.contact_cnt = C.contact_cnt
        AND C.contact_type_id = CT.contact_type_id
        AND CT.code = 'TELEPHONE'

    OPEN Phone_Cursor

    FETCH NEXT FROM Phone_Cursor INTO @area_code,
                    @number,
                    @extension

    IF @@FETCH_STATUS = 0 BEGIN
        SELECT @phone = NULL

        IF @area_code IS NOT NULL BEGIN
            IF @area_code <> '' BEGIN
                SELECT @phone = RTRIM(@area_code)
            END
        END

        IF @number IS NOT NULL BEGIN
            IF @number <> '' BEGIN
                IF @phone IS NOT NULL BEGIN
                    SELECT @phone = @phone + ' ' + RTRIM(@number)
                END ELSE BEGIN
                    SELECT @phone = @number
                END
            END
        END

        IF @extension IS NOT NULL BEGIN
            IF @extension <> '' BEGIN
                IF @phone IS NOT NULL BEGIN
                    SELECT @phone = @phone + ' Ext. ' + @extension
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

    CLOSE Phone_Cursor

    DEALLOCATE Phone_Cursor

    FETCH NEXT FROM List_Cursor2 INTO @party_cnt,
                        @phone

END

CLOSE List_Cursor2

DEALLOCATE List_Cursor2

SELECT shortname,
    resolved_name,
    department,
    phone
FROM #ReportPartyList
ORDER BY
    shortname

DROP TABLE #ReportPartyList

GO

