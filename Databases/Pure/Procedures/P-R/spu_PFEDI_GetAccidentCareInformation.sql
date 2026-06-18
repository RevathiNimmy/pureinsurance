SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFEDI_GetAccidentCareInformation'
GO

CREATE PROCEDURE spu_PFEDI_GetAccidentCareInformation
    @insurance_file_cnt INT
AS
--DC270304 PN11062 added new fields for policy premium and start date
--DC150404 PN11570 added new fields for address line 2 to 4
--DC300604 PN12139 added new fields for telephone numbers in GII
--DC050404 PN13913 added new field for date of birth in GII
--DC240804 Added check for GII as there are GII specific tables which would cause sp to fail if didnt exist
DECLARE @title AS VARCHAR(20)
DECLARE @forenames AS VARCHAR(20)
DECLARE @surname AS VARCHAR(20)
DECLARE @dob AS DATETIME
DECLARE @address1 AS VARCHAR(60)
DECLARE @address2 As VARCHAR(60)
DECLARE @address3 AS VARCHAR(60)
DECLARE @address4 AS VARCHAR(60)
DECLARE @postcode AS VARCHAR(20)
DECLARE @telephone AS VARCHAR(255)
DECLARE @insurer_name AS VARCHAR(100)
DECLARE @policy_number AS VARCHAR(20)
DECLARE @ACPremium AS NUMERIC(19,4)
DECLARE @ACType AS VARCHAR(50)
DECLARE @policy_premium AS NUMERIC(19,4)
DECLARE @policy_start_date AS DATETIME
DECLARE @GIIhometelephone AS VARCHAR(255)
DECLARE @GIIworktelephone AS VARCHAR(255)
DECLARE @GIIdob AS DATETIME

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'GIIMGemPolicy')
    AND EXISTS (SELECT * FROM sysobjects WHERE name = 'GIIMProposer_PolicyHolder')
BEGIN
    SELECT
        @title=PPC.party_title_code,
        @forenames=PPC.forename,
        @surname=P.name,
        @dob=PL.date_of_birth,
        @address1=A.address1,
        @postcode=A.postal_code,
        @telephone=RTRIM(C.area_code)+' '+RTRIM(C.number),
        @insurer_name=INS.name,
        @policy_number=I.insurance_ref,
        @policy_premium=I.this_premium,
        @ACType=ES.Description,
        @ACPremium=PFee.fee_amount,
        @policy_start_date=I.cover_start_date,
        @address2=A.address2,
        @address3=A.address3,
        @address4=A.address4,
        @GIIhometelephone=GPP.tel_no_home,
        @GIIworktelephone=GPP.tel_no_work,
        @GIIdob=GPP.date_of_birth
    FROM Party P
    INNER JOIN Party_Personal_Client PPC ON PPC.party_cnt=P.party_cnt
    INNER JOIN Party_Lifestyle PL ON PL.party_cnt=P.party_cnt
    INNER JOIN Insurance_File I ON I.insured_cnt=P.party_cnt
    INNER JOIN Party INS ON INS.party_cnt=I.lead_insurer_cnt
    LEFT JOIN Party_Address_Usage PAU ON PAU.party_cnt=P.party_cnt
    LEFT JOIN Address_Usage_Type AUT ON AUT.address_usage_type_id=PAU.address_usage_type_id
        AND AUT.code='3131 XCO'     
    LEFT JOIN Address A ON A.address_cnt=PAU.address_cnt
    LEFT JOIN Party_Contact_Usage PCU ON PCU.party_cnt=P.party_cnt
    LEFT JOIN Contact C ON C.contact_cnt=PCU.contact_cnt
    LEFT JOIN Contact_Type CT ON CT.contact_type_id=C.contact_type_id
        AND CT.code='TELEPHONE'     
    INNER JOIN Policy_Fee PFee ON PFee.insurance_file_cnt=I.insurance_file_cnt
    INNER JOIN Party PARTY_AC ON PARTY_AC.party_cnt=PFee.party_cnt
        AND RTRIM(PARTY_AC.shortname)='ACCCA'
    INNER JOIN Extra_Scheme ES ON ES.extra_scheme_id=PFee.extra_scheme_id
    LEFT JOIN Gis_Policy_Link GPL ON I.insurance_file_cnt=GPL.insurance_file_cnt
    LEFT JOIN GIIMGemPolicy GP ON GPL.gis_policy_link_id=GP.gis_policy_link_id
    LEFT JOIN GIIMProposer_PolicyHolder GPP ON GP.GIIMGemPolicy_Id = GPP.GIIMGemPolicy_Id
    WHERE I.insurance_file_cnt=@insurance_file_cnt
        
END
ELSE
BEGIN
    IF EXISTS (SELECT * FROM sysobjects WHERE name = 'GIIMOTOR_Policy_Binder')
        AND EXISTS (SELECT * FROM sysobjects WHERE name = 'GIIMProposer_PolicyHolder')
    BEGIN
        SELECT
            @title=PPC.party_title_code,
            @forenames=PPC.forename,
            @surname=P.name,
            @dob=PL.date_of_birth,
            @address1=A.address1,
            @postcode=A.postal_code,
            @telephone=RTRIM(C.area_code)+' '+RTRIM(C.number),
            @insurer_name=INS.name,
            @policy_number=I.insurance_ref,
            @policy_premium=I.this_premium,
            @ACType=ES.Description,
            @ACPremium=PFee.fee_amount,
            @policy_start_date=I.cover_start_date,
            @address2=A.address2,
            @address3=A.address3,
            @address4=A.address4,
            @GIIhometelephone=GPP.tel_no_home,
            @GIIworktelephone=GPP.tel_no_work,
            @GIIdob=GPP.date_of_birth
        FROM Party P
        INNER JOIN Party_Personal_Client PPC ON PPC.party_cnt=P.party_cnt
        INNER JOIN Party_Lifestyle PL ON PL.party_cnt=P.party_cnt
        INNER JOIN Insurance_File I ON I.insured_cnt=P.party_cnt
        INNER JOIN Party INS ON INS.party_cnt=I.lead_insurer_cnt
        LEFT JOIN Party_Address_Usage PAU ON PAU.party_cnt=P.party_cnt
        LEFT JOIN Address_Usage_Type AUT ON AUT.address_usage_type_id=PAU.address_usage_type_id
            AND AUT.code='3131 XCO'     
        LEFT JOIN Address A ON A.address_cnt=PAU.address_cnt
        LEFT JOIN Party_Contact_Usage PCU ON PCU.party_cnt=P.party_cnt
        LEFT JOIN Contact C ON C.contact_cnt=PCU.contact_cnt
        LEFT JOIN Contact_Type CT ON CT.contact_type_id=C.contact_type_id
            AND CT.code='TELEPHONE'     
        INNER JOIN Policy_Fee PFee ON PFee.insurance_file_cnt=I.insurance_file_cnt
        INNER JOIN Party PARTY_AC ON PARTY_AC.party_cnt=PFee.party_cnt
            AND RTRIM(PARTY_AC.shortname)='ACCCA'
        INNER JOIN Extra_Scheme ES ON ES.extra_scheme_id=PFee.extra_scheme_id
        LEFT JOIN Gis_Policy_Link GPL ON I.insurance_file_cnt=GPL.insurance_file_cnt
        LEFT JOIN GIIMOTOR_Policy_Binder GP ON GPL.gis_policy_link_id=GP.gis_policy_link_id
        LEFT JOIN GIIMProposer_PolicyHolder GPP ON GP.GIIMOTOR_Policy_Binder_Id = GPP.GIIMOTOR_Policy_Binder_Id
        WHERE I.insurance_file_cnt=@insurance_file_cnt

    END
    ELSE
    BEGIN

        SELECT
            @title=PPC.party_title_code,
            @forenames=PPC.forename,
            @surname=P.name,
            @dob=PL.date_of_birth,
            @address1=A.address1,
            @postcode=A.postal_code,
            @telephone=RTRIM(C.area_code)+' '+RTRIM(C.number),
            @insurer_name=INS.name,
            @policy_number=I.insurance_ref,
            @policy_premium=I.this_premium,
            @ACType=ES.Description,
            @ACPremium=PFee.fee_amount,
            @policy_start_date=I.cover_start_date,
            @address2=A.address2,
            @address3=A.address3,
            @address4=A.address4,
            @GIIhometelephone=NULL,
            @GIIworktelephone=NULL,
            @GIIdob=NULL
        FROM Party P
        INNER JOIN Party_Personal_Client PPC ON PPC.party_cnt=P.party_cnt
        INNER JOIN Party_Lifestyle PL ON PL.party_cnt=P.party_cnt
        INNER JOIN Insurance_File I ON I.insured_cnt=P.party_cnt
        INNER JOIN Party INS ON INS.party_cnt=I.lead_insurer_cnt
        LEFT JOIN Party_Address_Usage PAU ON PAU.party_cnt=P.party_cnt
        LEFT JOIN Address_Usage_Type AUT ON AUT.address_usage_type_id=PAU.address_usage_type_id
            AND AUT.code='3131 XCO'     
        LEFT JOIN Address A ON A.address_cnt=PAU.address_cnt
        LEFT JOIN Party_Contact_Usage PCU ON PCU.party_cnt=P.party_cnt
        LEFT JOIN Contact C ON C.contact_cnt=PCU.contact_cnt
        LEFT JOIN Contact_Type CT ON CT.contact_type_id=C.contact_type_id
            AND CT.code='TELEPHONE'     
        INNER JOIN Policy_Fee PFee ON PFee.insurance_file_cnt=I.insurance_file_cnt
        INNER JOIN Party PARTY_AC ON PARTY_AC.party_cnt=PFee.party_cnt
            AND RTRIM(PARTY_AC.shortname)='ACCCA'
        INNER JOIN Extra_Scheme ES ON ES.extra_scheme_id=PFee.extra_scheme_id
        WHERE I.insurance_file_cnt=@insurance_file_cnt
            
    END
END

SELECT
    @title 'Title',
    @forenames 'Forenames',
    @surname 'Surname',
    @dob 'Date of Birth',
    @address1 'Address1',
    @postcode 'Postcode',
    @telephone 'Telephone',
    @insurer_name 'Insurer',
    @policy_number 'Policy Number',
    @policy_premium 'Policy Premium',
    @ACType 'Accidentcare Type',
    @ACPremium 'Accidentcare Premium',
    @policy_start_date 'Policy Start Date',
    @address2 'Address2',
    @address3 'Address3',
    @address4 'Address4',
    @GIIhometelephone 'GIIHomeTelephone',
    @GIIworktelephone 'GIIWorkTelephone',
    @GIIdob 'GII Date Of Birth'
    
    
GO
