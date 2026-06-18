SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Client'
GO

CREATE PROCEDURE spu_Report_Client
AS
BEGIN
    DECLARE @lID INTEGER,
        @party_cnt INTEGER,
        @home_areacode VARCHAR(10),
        @home_telno VARCHAR(255),
        @work_areacode VARCHAR(10),
        @work_telno VARCHAR(255),
        @work_ext VARCHAR(10),
        @MobilePhoneNo VARCHAR(255),
        @EmailAddress VARCHAR(255),
        @WebAddress VARCHAR(255)

    CREATE TABLE #TempAddressDetails (
        ID integer IDENTITY PRIMARY KEY,
        party_id integer NOT NULL,
        party_cnt integer NOT NULL,
        party_short_name varchar(20) NOT NULL,
        party_name varchar(60) NOT NULL,
        corres_address_line1 varchar(60) NOT NULL,
        corres_address_line2 varchar(60) NOT NULL,
        corres_address_line3 varchar(60) NOT NULL,
        corres_address_line4 varchar(60) NOT NULL,
        corres_address_postcode varchar(20) NOT NULL,
        corres_country varchar(50) NOT NULL,
        home_area_code varchar(10) NOT NULL,
        home_telno varchar(255) NOT NULL,
        work_area_Code varchar(10) NOT NULL,
        work_telno varchar(255) NOT NULL,
        work_ext varchar(10) NOT NULL,
        mobile_phoneno varchar(255) NOT NULL,
        emailaddress varchar(255) NOT NULL,
        webaddress varchar(255) NOT NULL,
        balance_outstanding decimal(19, 4) NOT NULL
    )

    INSERT INTO #TempAddressDetails (
        party_id,
        party_cnt,
        party_short_name,
        party_name,
        corres_address_line1,
        corres_address_line2,
        corres_address_line3,
        corres_address_line4,
        corres_address_postcode,
        corres_country,
        home_area_code,
        home_telno,
        work_area_Code,
        work_telno,
        work_Ext,
        mobile_phoneno,
        emailaddress,
        webaddress,
        balance_outstanding
    ) SELECT
        P.party_id,
        P.party_cnt,
        P.shortname,
        P.resolved_name,
        A.address1,
        A.address2,
        A.address3,
        A.address4,
        A.postal_code,
        C.description,
        '',
        '',
        '',
        '',
        '',
        '',
        '',
        '',
        0
        FROM Party AS P
        INNER JOIN Party_Address_Usage AS PAU ON P.party_cnt = PAU.party_cnt
        INNER JOIN Address AS A ON PAU.address_cnt = A.address_cnt
        INNER JOIN Address_Usage_Type AS AU ON PAU.address_usage_type_id = AU.address_usage_type_id
        INNER JOIN Country AS C ON A.country_id = C.country_id
        WHERE P.is_deleted = 0

    DECLARE curAddressDetails CURSOR FAST_FORWARD FOR
        SELECT ID,
        party_cnt,
        home_area_code,
        home_telno,
        work_area_code,
        work_telno,
        work_ext,
        mobile_phoneno,
        emailaddress,
        webaddress
        FROM #TempAddressDetails

    OPEN curAddressDetails
    FETCH NEXT FROM curAddressDetails INTO
        @lID,
        @party_cnt,
        @home_areacode,
        @home_telno,
        @work_areacode,
        @work_telno,
        @work_ext,
        @MobilePhoneNo,
        @EmailAddress,
        @WebAddress

    WHILE @@FETCH_STATUS = 0 BEGIN
        SELECT @home_areacode = null,
            @home_telno = null,
            @work_areacode = null,
            @work_telno = null,
            @work_ext = null,
            @MobilePhoneNo = null,
            @EmailAddress = null,
            @WebAddress = null

        SELECT @home_areacode = C.area_code,
            @home_telno = C.number
            FROM Party AS P
            INNER JOIN Party_Address_Usage AS PAU ON P.party_cnt = PAU.Party_cnt
            INNER JOIN Party_Contact_Usage AS PCU ON P.party_cnt = PCU.Party_cnt
            INNER JOIN contact AS C ON PCU.contact_cnt = C.contact_cnt
            WHERE P.party_cnt = @party_cnt
            AND PAU.address_usage_type_id = 5
            AND C.Contact_Type_id = 1

        SELECT @work_areacode = C.area_code,
            @work_telno = C.number,
            @work_ext = C.extension
            FROM Party AS P
            INNER JOIN Party_Address_Usage AS PAU ON P.party_cnt = PAU.Party_cnt
            INNER JOIN Party_Contact_Usage AS PCU ON P.party_cnt = PCU.Party_cnt
            INNER JOIN contact AS C ON PCU.contact_cnt = C.contact_cnt
            WHERE P.party_cnt = @party_cnt
            AND PAU.address_usage_type_id = 3
            AND C.Contact_Type_id = 1

        SELECT @MobilePhoneNo = C.number
            FROM Party AS P
            INNER JOIN Party_Address_Usage AS PAU ON P.party_cnt = PAU.Party_cnt
            INNER JOIN Party_Contact_Usage AS PCU ON P.party_cnt = PCU.Party_cnt
            INNER JOIN contact AS C ON PCU.contact_cnt = C.contact_cnt
            WHERE P.party_cnt = @party_cnt
            AND PAU.address_usage_type_id = 4
            AND C.Contact_Type_id = 4

        SELECT @EmailAddress = C.number
            FROM Party AS P
            INNER JOIN Party_Address_Usage AS PAU ON P.party_cnt = PAU.Party_cnt
            INNER JOIN Party_Contact_Usage AS PCU ON P.party_cnt = PCU.Party_cnt
            INNER JOIN contact AS C ON PCU.contact_cnt = C.contact_cnt
            WHERE P.party_cnt = @party_cnt
            AND PAU.address_usage_type_id = 4
            AND C.Contact_Type_id = 3

        SELECT @WebAddress = C.number
            FROM Party AS P
            INNER JOIN Party_Address_Usage AS PAU ON P.party_cnt = PAU.Party_cnt
            INNER JOIN Party_Contact_Usage AS PCU ON P.party_cnt = PCU.Party_cnt
            INNER JOIN contact AS C ON PCU.contact_cnt = C.contact_cnt
            WHERE P.party_cnt = @party_cnt
            AND PAU.address_usage_type_id = 4
            AND C.Contact_Type_id = 5

        IF @home_areacode IS NOT NULL OR
            @home_telno IS NOT NULL BEGIN
            UPDATE #TempAddressDetails
                SET home_area_code = @home_areacode,
                home_telno = @home_telno
                WHERE ID = @lID
        END

        IF @work_areacode IS NOT NULL OR
           @work_telno IS NOT NULL OR
           @work_ext IS NOT NULL BEGIN
            UPDATE #TempAddressDetails
                SET work_area_code = @work_areacode,
                work_telno = @work_telno,
                work_ext = @work_ext
                WHERE ID = @lID
        END

        IF @MobilePhoneNo IS NOT NULL BEGIN
            UPDATE #TempAddressDetails
                SET mobile_phoneno = @MobilePhoneNo
                WHERE ID = @lID
        END

        IF @EmailAddress IS NOT NULL BEGIN
            UPDATE #TempAddressDetails
                SET emailaddress = @EmailAddress
                WHERE ID = @lID
        END

        IF @WebAddress IS NOT NULL BEGIN
            UPDATE #TempAddressDetails
                SET webaddress = @WebAddress
                WHERE ID = @lID
        END

        FETCH NEXT FROM curAddressDetails INTO
            @lID,
            @party_cnt,
            @home_areacode,
            @home_telno,
            @work_areacode,
            @work_telno,
            @work_ext,
            @MobilePhoneNo,
            @EmailAddress,
            @WebAddress
    END

    CLOSE curAddressDetails
    DEALLOCATE curAddressDetails

    SELECT party_id,
        party_cnt,
        party_short_name,
        party_name,
        corres_address_line1,
        corres_address_line2,
        corres_address_line3,
        corres_address_line4,
        corres_address_postcode,
        corres_country,
        home_area_code,
        home_telno,
        work_area_Code,
        work_telno,
        work_ext,
        mobile_phoneno,
        emailaddress,
        webaddress,
        balance_outstanding
        FROM #TempAddressDetails

    DROP TABLE #TempAddressDetails
END
GO

