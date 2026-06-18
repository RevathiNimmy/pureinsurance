
EXECUTE DDLDropProcedure 'spu_Report_Policy_Listing_Short_SFU'
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** 11/09/2000
** RSA Reports - PolicyListingShort.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 22/09/2000   Jude Killip             Fixes
**
** 16/11/2000   Jude Killip             DB change: links between Risk and Insurance_File
**
** 06/12/2000   Jude Killip             Bug: not showing multiple risk
**
** 21/09/2001   Jude Killip             Include Policies under renewal (insurance_file_status_id = 3)
**
** 28/05/2003   Jon Kemp		branch parameter added
**********************************************************************************************************************************
** VER      DATE        WHO     DESC
** 1.01     05/04/2001  JMK     Accumulation Description
** 1.02     08/08/2002  TB      exclude expired policies $Revision:  $ (as of today) 
***********************************************************************************************************************************/

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_Report_Policy_Listing_Short_SFU 
	@branch_id int
	AS


SET NOCOUNT ON

declare @ibranchid int
IF @branch_id IS NULL
		SELECT @iBranchID = 0
	ELSE
		SELECT @iBranchID = @branch_id

CREATE TABLE #tempRSAPolListingS
(
        InsFileCnt int,
        PolicyCode varchar (30),
        CoverFrom datetime NULL,
        CoverTo datetime NULL,
        ClientCode varchar (30),
        AgentAC varchar (20) NULL,
        ProductCode varchar (10) NULL,
        RiskCnt int NULL,
        RiskTypeID int NULL,
        RiskAccumID int NULL,
        RiskDescription varchar (255) NULL,
        RiskTypeCode varchar (10) NULL,
        AccumCode varchar (10) NULL,
        Accumulation varchar (255) NULL
)


-- Get all Insurance_file records with associated Party and Risk details
-- plus id's from Risk for the Updates


INSERT INTO #tempRSAPolListingS
        SELECT ifi.insurance_file_cnt,
                ifi.insurance_ref,
                ifi.cover_start_date,
                ifi.expiry_date,
                pClient.shortname,
                pAgent.shortname,
                p.code,
                ifrl.risk_cnt,
                NULL,
                NULL,
                NULL,
                NULL,   --RiskTypeCode
                NULL,   --AccumCode
                NULL    --AccumDescription
        FROM insurance_file ifi
        LEFT OUTER JOIN Party pClient ON ifi.insured_cnt = pClient.party_cnt
        LEFT OUTER JOIN Party pAgent ON ifi.lead_agent_cnt = pAgent.party_cnt
        LEFT OUTER JOIN Product p ON ifi.product_id = p.product_id
        LEFT OUTER JOIN Insurance_File_Risk_Link ifrl ON ifi.insurance_file_cnt = ifrl.insurance_file_cnt
         WHERE ifi.insurance_file_type_id  in (2,5,6,9)  
        AND  
        (Isnull(ifi.insurance_file_status_id,0) = 0  
        OR insurance_file_status_id = 3)        -- 3 = Under Renewal  
AND ( @iBranchID= 0 or    (   @iBranchID <> 0 and ifi.branch_id = @iBranchID ))  
And  ifi.insurance_file_cnt in (Select insurance_file_cnt FROM (
                                 SELECT MAX (insurance_file_cnt) AS  'insurance_file_cnt',insurance_ref AS 'insurance_ref' FROM insurance_file
                                 WHERE insurance_file_type_id  in (2,5,6,9)                 
                                 AND(Isnull(insurance_file_status_id,0) = 0 OR insurance_file_status_id = 3)      
                                 AND ( @iBranchID= 0 or    (   @iBranchID <> 0 and ifi.branch_id = @iBranchID ))  
                                 GROUP BY insurance_ref )AS T
                                 ) 

-- Update with Risk data
UPDATE #tempRSAPolListingS
        SET RiskTypeID = r.risk_type_id,
                RiskAccumID = r.accumulation_id,
                RiskDescription = r.description
        FROM Insurance_File_Risk_Link ifrl,
                Risk r
        WHERE InsFileCnt = ifrl.insurance_file_cnt
        AND RiskCnt = r.risk_cnt

-- Update with Risk_Type data
UPDATE #tempRSAPolListingS
        SET RiskTypeCode = rt.code
        FROM Risk_Type rt
        WHERE RiskTypeID = rt.risk_type_id

-- Update with Accumulation data
UPDATE #tempRSAPolListingS
        SET AccumCode = ac.code,
            Accumulation = ac.description
        FROM Accumulation ac
        WHERE RiskAccumID = ac.accumulation_id

SET NOCOUNT OFF

-- TB 8/8/02 Exclude expired policies  
DECLARE @dtNow as date 
SELECT @dtNow = GetDate()  

-- Squirt out to report  
SELECT InsFileCnt,  
        ClientCode,  
        PolicyCode,  
        AgentAC,  
        CoverFrom,  
        CoverTo,  
        ProductCode,  
        RiskCnt,  
        RiskDescription,  
        RiskTypeCode,  
        AccumCode,  
        Accumulation  
FROM #tempRSAPolListingS  
where CONVERT(date, CoverTo) >= @dtNow  
  
DROP TABLE #tempRSAPolListingS  
GO
