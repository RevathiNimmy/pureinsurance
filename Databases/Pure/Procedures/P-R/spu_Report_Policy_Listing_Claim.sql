SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Policy_Listing_Claim'
GO


CREATE PROCEDURE spu_Report_Policy_Listing_Claim
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 15/09/2000
** RSA Reports - PolicyListingLong.rpt
**               (Claims subreport)
**********************************************************************************************************************************
** 06/12/2000 - Jude Killip     rewrite
**
** 19/04/2001 - Jude Killip     re-rewrite
**
** 20/06/2001 - Jude Killip     Split Main Reserve/Others
**                              Claim.risk_type_id is Risk.risk_cnt!!!
**
** 23/06/2001 - Jude Killip     why oh why was  TypeOfLoss not 255??? it is now
**
** 25/06/2001 - Jude Killip     new arrangement for summing up
**
** 05/07/2001 - Jude Killip     base on insurance folder cnt to pick up claims on earlier versions of policy
***********************************************************************************************************************************/
SET NOCOUNT ON
CREATE TABLE #tempRSAPolListLC
(
    FolderCnt int,
    InsuranceCnt int,
    ClaimId int,
    ClaimDate datetime NULL,
    ClaimNo varchar (30) NULL,
    TypeOfLoss varchar (255) NULL,
    Risk varchar (50) NULL,
    ReserveMain decimal (19,4) NULL,
    ReserveOthers decimal (19,4) NULL,
    Settled decimal (19,4) NULL,
    Recovered decimal (19,4) NULL,
    Payment decimal (19,4) NULL
)

INSERT INTO #tempRSAPolListLC
    SELECT (select insurance_folder_cnt from insurance_file where insurance_file_cnt = policy_id),
        c.policy_id,
        c.claim_id,
        c.loss_from_date,
        c.claim_number,
        c.description,
        (SELECT rt.code
            FROM risk r
            JOIN risk_type rt ON rt.risk_type_id = r.risk_type_id
            WHERE c.risk_type_id = r.risk_cnt),
        (SELECT sum(res.initial_reserve)
            FROM Reserve res
            WHERE cp.claim_peril_id = res.claim_peril_id
            AND res.reserve_type_id = 1),
        (SELECT sum(res.initial_reserve)
            FROM Reserve res
            WHERE cp.claim_peril_id = res.claim_peril_id
            AND res.reserve_type_id <> 1),
        (SELECT sum(rpt.amount)
            FROM Receipt rpt
            WHERE cp.claim_peril_id = rpt.claim_peril_id),
        (SELECT sum(rec.initial_reserve)
            FROM [Recovery] rec
            WHERE cp.claim_peril_id = rec.claim_peril_id),
        (SELECT sum(p.amount)
            FROM Payment p
            WHERE cp.claim_peril_id = p.claim_peril_id)
    FROM claim c
    JOIN Claim_Peril cp  ON c.claim_id = cp.claim_id                     -- Claim_Peril link

SELECT * FROM #tempRSAPolListLC
WHERE isnull(ReserveMain,0) <> 0
    OR isnull(ReserveOthers,0) <> 0
    OR isnull(Settled,0) <> 0
    OR isnull(Recovered,0) <> 0
    OR isnull(Payment,0) <> 0
DROP TABLE #tempRSAPolListLC
GO


