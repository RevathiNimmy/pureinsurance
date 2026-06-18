
EXECUTE DDLDropProcedure 'spu_Report_Party_Listing_SFU'
GO

--*******************************************************************************************
-- Created by Jude Killip
-- 25/09/2000
-- RSA Reports - Party_Listing.rpt
--
--*******************************************************************************************
-- 26/09/2000 - JMK - script now includes at least one
--                      record for each valid Party
--
-- 05/10/2000 - JMK - #48 change party_type_id condition
--
-- 25/04/2001 - JMK - add parameter, to take record selection out of report
--                      take out party type filter (makes no difference)
--                      amend contact type
--
-- 21/09/2001   JMK     add - party_other
--                          - party_conviction
--                          - previous_accidents
--                      limit to party_type at start instead of end
--
-- 01/11/2001   JMK     move accidents and convictions out to subreports
--                      speed up short listing - add reporttype parameter
--                      add Other Party Limit parameter
--                      get address contacts and party contacts
--
-- 02/11/2001   JMK     two reports:    Other Party Listing     @OtherPartyLimit = 'yes'
--                                      Party Listing           @OtherPartyLimit <> 'yes'
--
-- 12/11/2001   JMK     get Underwriting Type flag (display Insurer/Reinsurer)
--
-- 19/11/2001   JMK             get UWType using sp_Report_GetUnderwritingType
-- 06/07/2005	JT	insured name is now 255 long
--*******************************************************************************************
CREATE PROCEDURE spu_Report_Party_Listing_SFU
        (@PartyType varchar (255), @ReportType varchar(15), @OtherPartyLimit varchar (15))
AS
/*
 -- for testing
DECLARE @PartyType varchar (255), @ReportType varchar(15), @OtherPartyLimit varchar (15)
SELECT @PartyType = 'driver', @ReportType = 'long format', @OtherPartyLimit = 'yes'
*/
-- get UWType
DECLARE @UWType char(1)
EXECUTE spu_Report_GetUnderwritingType_SFU @UWType OUTPUT

 --print 'party type'
CREATE TABLE #tempParty
(
    PartyCnt int,
    PartyTypeID smallint NULL,
    PartyTypeCode varchar (10) NULL,
    PartyTypeDesc varchar (255) NULL,
    PartyCode varchar (20) NULL,
    PartyName varchar (255) NULL,
    UWType char (1)
)

IF isnull(@PartyType,'') IN ('ALL','')
BEGIN
    IF @OtherPartyLimit = 'yes'
    BEGIN
        INSERT INTO #tempParty
            SELECT p.party_cnt,
                p.party_type_id,
                pt.code,
                pt.description,
                p.shortname,
                p.resolved_name,
                @UWType
            FROM Party p
            JOIN Party_Type pt ON p.party_type_id = pt.party_type_id
            WHERE p.is_deleted <> 1               -- not deleted
            AND pt.code LIKE 'OT%'
    END
    ELSE
    BEGIN
        INSERT INTO #tempParty
            SELECT p.party_cnt,
                p.party_type_id,
                pt.code,
                pt.description,
                p.shortname,
                p.resolved_name,
                @UWType
            FROM Party p
            JOIN Party_Type pt ON p.party_type_id = pt.party_type_id
            WHERE p.is_deleted <> 1               -- not deleted
            AND pt.code NOT LIKE 'OT%'
    END
END
ELSE
BEGIN
    IF @OtherPartyLimit = 'yes'
    BEGIN
        INSERT INTO #tempParty
            SELECT p.party_cnt,
                p.party_type_id,
                pt.code,
                pt.description,
                p.shortname,
                p.resolved_name,
                @UWType
            FROM Party p
            JOIN Party_Type pt ON p.party_type_id = pt.party_type_id
            WHERE p.is_deleted <> 1               -- not deleted
            AND pt.description = @PartyType
            AND pt.code LIKE 'OT%'
    END
    ELSE
    BEGIN
        INSERT INTO #tempParty
            SELECT p.party_cnt,
                p.party_type_id,
                pt.code,
                pt.description,
                p.shortname,
                p.resolved_name,
                @UWType
            FROM Party p
            JOIN Party_Type pt ON p.party_type_id = pt.party_type_id
            WHERE p.is_deleted <> 1               -- not deleted
            AND pt.description = @PartyType
            AND pt.code NOT LIKE 'OT%'
    END
END

 --print 'get Correspondence Address details'
CREATE TABLE #tempCorrespond
(
    PartyCnt int,
    AddressCnt int NULL,
    PartyAddress1 varchar (60) NULL,
    PartyAddress2 varchar (60) NULL,
    PartyAddress3 varchar (60) NULL,
    PartyAddress4 varchar (60) NULL,
    PostalCode varchar (20) NULL,
    Country varchar (50) NULL
)

INSERT INTO #tempCorrespond
    SELECT pau.party_cnt,
        a.address_cnt,
        a.address1,
        a.address2,
        a.address3,
        a.address4,
        a.postal_code,
        c.[description]
    FROM Party_Address_Usage pau
    JOIN #tempParty tp ON tp.PartyCnt = pau.party_cnt
    JOIN Address a ON pau.address_cnt = a.address_cnt
    JOIN Country c ON a.country_id = c.country_id
    WHERE pau.address_usage_type_id = 4   -- Correspondence

IF @ReportType = 'LONG FORMAT'
BEGIN

 --print 'get Contact details'
CREATE TABLE #tempContact
(
    PartyCnt int,
    ContactCnt int NULL,
    AreaCode varchar (10) NULL,
    Number varchar (255) NULL,
    Extension varchar (6) NULL,
    ContactTypeID smallint NULL,
    ContactTypeCode varchar (10) NULL
)

INSERT INTO #tempContact
    SELECT pcu.party_cnt,
        pcu.contact_cnt,
        con.area_code,
        con.number,
        con.extension,
        ct.contact_type_id,
        ct.code
    FROM #tempParty tp
    JOIN Party_Contact_Usage pcu    ON tp.PartyCnt = pcu.party_cnt
    JOIN Contact con                ON pcu.contact_cnt = con.contact_cnt
    JOIN Contact_Type ct            ON con.contact_type_id = ct.contact_type_id
UNION
    SELECT pau.party_cnt,
        cau.contact_cnt,
        con.area_code,
        con.number,
        con.extension,
        ct.contact_type_id,
        ct.code
    FROM #tempParty tp
    JOIN Party_Address_Usage pau    ON tp.PartyCnt = pau.party_cnt
    JOIN Address a                  ON pau.address_cnt = a.address_cnt
    JOIN Contact_Address_Usage cau  ON cau.address_cnt = a.address_cnt
    JOIN Contact con                ON cau.contact_cnt = con.contact_cnt
    JOIN Contact_Type ct            ON con.contact_type_id = ct.contact_type_id
    WHERE pau.address_usage_type_id = 4   -- Correspondence

--print 'get Other Party (license) details'
CREATE TABLE #tempPartyOther
(
    PartyCnt int,
    LicenseType varchar (50) NULL,
    DriverStatus varchar (50) NULL,
    LicenseNumber varchar (20) NULL,
    RegNumber varchar (20) NULL
)

INSERT INTO #tempPartyOther
    SELECT party_cnt,
        (SELECT lt.description FROM license_type lt WHERE po.license_type_id = lt.license_type_id),
        (SELECT ds.description FROM driver_status ds WHERE po.party_status = ds.driver_status_id),
        license_number,
        reg_number
    FROM party_other po
    JOIN #tempParty tp ON tp.PartyCnt = po.party_cnt
    WHERE isnull(po.license_type_id,0) <> 0
    OR isnull(po.license_number,'') <> ''
    OR isnull(po.reg_number,'') <> ''

     --print  'get all the LONG REPORT details together'

    SELECT tp.PartyCnt,
        tp.PartyTypeID,
        tp.PartyTypeCode,
        tp.PartyTypeDesc,
        tp.PartyCode,
        tp.PartyName,
        tp.UWType,
        tCor.PartyAddress1,
        tCor.PartyAddress2,
        tCor.PartyAddress3,
        tCor.PartyAddress4,
        tCor.PostalCode,
        tCor.Country,
        tCon.ContactCnt,
        tCon.AreaCode,
        tCon.Number,
        tCon.Extension,
        tCon.ContactTypeID,
        tCon.ContactTypeCode,
        tPartyO.LicenseType,
        tPartyO.DriverStatus,
        tPartyO.LicenseNumber,
        tPartyO.RegNumber
    FROM #tempParty tp
    LEFT OUTER JOIN #tempCorrespond tCor        ON tp.PartyCnt = tCor.PartyCnt
    LEFT OUTER JOIN #tempContact tCon           ON tp.PartyCnt = tCon.PartyCnt
    LEFT OUTER JOIN #tempPartyOther tPartyO     ON tp.PartyCnt = tPartyO.PartyCnt

    DROP TABLE #tempParty
    DROP TABLE #tempCorrespond
    DROP TABLE #tempContact
    DROP TABLE #tempPartyOther

END
ELSE
BEGIN
     --print  'get all the SHORT REPORT details together'
    SELECT tp.PartyCnt,
        tp.PartyTypeID,
        tp.PartyTypeCode,
        tp.PartyTypeDesc,
        tp.PartyCode,
        tp.PartyName,
        tp.UWType,
        tCor.PartyAddress1,
        tCor.PartyAddress2,
        tCor.PartyAddress3,
        tCor.PartyAddress4,
        tCor.PostalCode,
        tCor.Country,
        0 'ContactCnt',
        '' 'AreaCode',
        '' 'Number',
        '' 'Extension',
        0 'ContactTypeID',
        '' 'ContactTypeCode',
        '' 'LicenseType',
        '' 'DriverStatus',
        '' 'LicenseNumber',
        '' 'RegNumber'
    FROM #tempParty tp
    LEFT OUTER JOIN #tempCorrespond tCor ON tp.PartyCnt = tCor.PartyCnt

    DROP TABLE #tempParty
    DROP TABLE #tempCorrespond

END
GO