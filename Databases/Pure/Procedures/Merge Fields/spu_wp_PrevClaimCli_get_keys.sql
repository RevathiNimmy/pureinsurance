SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PrevClaimCli_get_keys'
GO


CREATE PROCEDURE spu_wp_PrevClaimCli_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


-- CREATE TEMP TABLE TO HOLD INSURANCE_FOLDER FOR THIS CLIENT
CREATE TABLE #TempInsuranceFolder
    (
    insurance_folder_cnt INT NULL
    )

INSERT INTO #TempInsuranceFolder
    SELECT  ifi.insurance_folder_cnt
    FROM    insurance_file ifi,
        insurance_folder ifo
    WHERE   ifo.insurance_holder_cnt =
        (
        SELECT  ifo.insurance_holder_cnt
        FROM    claim c,
            insurance_file ifi,
            insurance_folder ifo
        WHERE   c.claim_id = @ClaimCnt
        AND c.policy_id = ifi.insurance_file_cnt
        AND ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
        )
    AND ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
    GROUP BY ifi.insurance_folder_cnt

-- GET ALL CLAIM_ID FOR THIS CLIENT
SELECT  c.claim_id
FROM    claim c,
    insurance_file i,
    #TempInsuranceFolder t
WHERE   t.insurance_folder_cnt = i.insurance_folder_cnt
AND i.insurance_file_cnt = c.policy_id

-- DELETE TEMP TABLES
DROP TABLE #TempInsuranceFolder
GO


