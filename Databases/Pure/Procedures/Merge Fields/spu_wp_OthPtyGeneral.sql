SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_OthPtyGeneral'
GO


CREATE PROCEDURE spu_wp_OthPtyGeneral
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  p.shortname         Oth_Pty_Code,
    p.name          Oth_Pty_Name,
    po.date_of_birth    Oth_Pty_DOB,
    po.gender       Oth_Pty_Gender,
    lt.description      Oth_Pty_LicType,
    po.license_number   Oth_Pty_LicNum,
    ds.description      Oth_Pty_Status,
    po.reg_number       Oth_Pty_RegNum

FROM    claim_party_link cpl,
    party p,
    party_other po,
    license_type lt,
    driver_status ds

WHERE   cpl.claim_id = @ClaimCnt
AND cpl.party_cnt = @Instance2
AND po.party_cnt = @instance2
AND p.party_cnt = @instance2
AND po.license_type_id = lt.License_type_id
AND po.party_status = ds.driver_status_id
GO


