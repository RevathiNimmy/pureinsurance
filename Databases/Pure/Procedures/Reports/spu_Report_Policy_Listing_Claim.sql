SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Policy_Listing_Claim'
GO

/********************************************************************************
** RSA Reports - PolicyListingLong.rpt (Claims subreport)
** Created by Jude Killip 15/09/2000
**
*********************************************************************************
** VER      DATE        WHO     WHAT
**          06/12/2000  JMK     rewrite
**
**          19/04/2001  JMK     re-rewrite
**
**          20/06/2001  JMK     Split Main Reserve/Others
**                              Claim.risk_type_id is Risk.risk_cnt!!!
**
**          23/06/2001  JMK     why oh why was TypeOfLoss not 255??? it is now
**
**          25/06/2001  JMK     new arrangement for summing up
**
**          05/07/2001  JMK     base on insurance folder cnt to pick up claims on earlier versions of policy
**
********************************************************************************/
CREATE PROCEDURE spu_Report_Policy_Listing_Claim
AS
SET NOCOUNT ON

CREATE TABLE #tempRSAPolListLC (  
    FolderCnt int,  
    InsuranceCnt int,  
    ClaimId int,  
    ClaimDate datetime NULL,  
    ClaimNo varchar(30) NULL,  
    TypeOfLoss varchar(255) NULL,  
    Risk varchar(255) NULL,
    ReserveMain decimal(19, 4) NULL,  
    ReserveOthers decimal(19, 4) NULL,  
    Settled decimal(19, 4) NULL,  
    Recovered decimal(19, 4) NULL,  
    --Payment decimal(19, 4) NULL  
) 
 
INSERT INTO #tempRSAPolListLC  
SELECT  
    (SELECT insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = policy_id),  
    c.policy_id,  
    c.claim_id,  
    c.loss_from_date,  
    c.claim_number,  
    LEFT(c.description,255),  
   (SELECT r.description FROM risk r WHERE c.risk_type_id = r.risk_cnt),  
    (SELECT sum(res.initial_reserve)  --Initial R
        FROM Reserve res  
        WHERE cp.claim_peril_id = res.claim_peril_id),  
    (SELECT (sum(res.Initial_reserve) +  sum(res.Revised_reserve) - sum(res.paid_to_date))  --Current R
        FROM Reserve res  
        WHERE cp.claim_peril_id = res.claim_peril_id),  
    (SELECT sum(res.paid_to_date)  --settled
        FROM Reserve res  
        WHERE cp.claim_peril_id = res.claim_peril_id),  
    (SELECT sum(rec.initial_reserve)  --recovered
        FROM [Recovery] rec  
        WHERE cp.claim_peril_id = rec.claim_peril_id)  
    --(SELECT (sum(res.Initial_reserve) +  sum(res.Revised_reserve))  --incurred
    --    FROM Reserve res  
    --    WHERE cp.claim_peril_id = res.claim_peril_id)  
    FROM claim c  
    INNER JOIN Claim_Peril cp ON c.claim_id = cp.claim_id  
    WHERE ISNULL(c.is_dirty,0) <> 1  
    AND c.version_id=( ISNULL((SELECT MAX(version_id) FROM  claim WHERE claim_Number= c.claim_Number),1))  

SELECT *  
    FROM #tempRSAPolListLC  
    --WHERE isnull(ReserveMain, 0) <> 0  
    --OR isnull(ReserveOthers, 0) <> 0  
    --OR isnull(Settled, 0) <> 0  
    --OR isnull(Recovered, 0) <> 0  
    --OR isnull(Payment, 0) <> 0  
    WHERE ReserveMain is NOT NULL

DROP TABLE #tempRSAPolListLC  

GO
