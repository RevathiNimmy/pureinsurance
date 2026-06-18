SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetPolicyGIS_U'
GO


CREATE PROCEDURE spu_GetPolicyGIS_U
    @PolicyNo varchar(30) = NULL,
    @ClaimDate datetime,
    @PartyShortName varchar(20) = NULL,
    @PostCode varchar(20) = NULL,
    @PolicyStartDate datetime = NULL,
    @PolicyEndDate datetime = NULL,
    @InsuranceFileCnt int = NULL,
    @GISValue varchar(100) = NULL
AS

/*
- Jude Killip
- 11/04/2001    Created based on sp_GetPolicy_U
-               to enable GIS Index Search
*/
-- create temporary table to store latest version of policy
CREATE TABLE #tempLatestPolicyCnt
(
Insurance_File_Cnt int
)

INSERT INTO #tempLatestPolicyCnt
SELECT insurance_file_cnt FROM insurance_file WHERE insurance_file_cnt in
        (SELECT max(insurance_file_cnt) FROM insurance_file
        WHERE insurance_file_type_id IN
                (SELECT insurance_file_type_id FROM insurance_file_type
                WHERE code IN ('POLICY','MTA PERM'))
        AND insurance_file_cnt = @InsuranceFileCnt
        GROUP BY insurance_folder_cnt)

-- create temporary table to store unfiltered policy details
CREATE TABLE #tempLatestPolicyDetails
(
        InsFileCnt int,
        PartyName varchar(60) NULL,
        PartyShortname varchar(20) NULL,
        InsRef varchar(30) NULL,
        GISIndex varchar(100) NULL,
        ProdCode varchar(10) NULL,
        CoverStart datetime NULL,
        Expiry datetime NULL,
        PostCode varchar(20) NULL
        )

INSERT INTO #tempLatestPolicyDetails
SELECT  ifi.insurance_file_cnt,
        pty.name,
        pty.shortname,
        ifi.insurance_ref,
        @GISValue,
        prd.code,
        ifi.cover_start_date,
        ifi.expiry_date,
        adr.postal_code
        FROM Address adr,
        Party_Address_Usage pau,
        Product prd,
        Party pty,
        Insurance_File ifi,
        Insurance_Folder ifo
        WHERE   ifi.insurance_file_type_id in (2,3,5,6)
        AND pau.address_usage_type_id = 4
        AND ifi.product_id = prd.product_id
        AND ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
        AND ifo.insurance_holder_cnt = pty.party_cnt
        AND pty.party_cnt = pau.party_cnt
        AND pau.address_cnt = adr.address_cnt
        AND ifi.insurance_file_cnt IN
                (SELECT insurance_file_cnt FROM #tempLatestPolicyCnt)
        AND @ClaimDate BETWEEN  ifi.cover_start_date AND ifi.expiry_date        -- there will always be @ClaimDate

DROP TABLE #tempLatestPolicyCnt

-- Optional filters:
-- do we have policy number?
        IF @PolicyNo IS NOT null
        delete FROM #tempLatestPolicyDetails
        WHERE InsRef NOT like @PolicyNo
-- do we have party short name?
        IF @PartyShortName IS NOT null
        delete FROM #tempLatestPolicyDetails
        WHERE PartyShortname NOT like @PartyShortName
-- do we have post code?
        IF @PostCode IS NOT null
        delete FROM #tempLatestPolicyDetails
        WHERE PostCode NOT like @PostCode
-- policy start date?
        IF @PolicyStartDate IS NOT null
        delete FROM #tempLatestPolicyDetails
        WHERE CoverStart <> @PolicyStartDate
-- policy end date?
        IF @PolicyEndDate IS NOT null
        delete FROM #tempLatestPolicyDetails
        WHERE Expiry <> @PolicyEndDate

-- SELECT final result (exclude party shortname)
SELECT InsFileCnt,
        PartyName,
        InsRef,
        GISIndex,
        ProdCode,
        CoverStart,
        Expiry,
        PostCode
FROM #tempLatestPolicyDetails

DROP TABLE #tempLatestPolicyDetails
GO


