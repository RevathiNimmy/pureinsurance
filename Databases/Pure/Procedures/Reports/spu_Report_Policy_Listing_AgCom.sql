SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Policy_Listing_AgCom'
GO


CREATE PROCEDURE spu_Report_Policy_Listing_AgCom
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 23/09/2000
** RSA Reports - Policy_Listing_Long.rpt
**              (Agent Commission subreport)
**********************************************************************************************************************************
** 17/11/2000 - Jude Killip     Lead_Commission.percent: not a valid column name in SQL7
**                              ... use "0" for now, to be resolved.
**
** 05/12/2000 - Jude Killip     use [percent]
**
** 19/04/2001 - Jude Killip     update selection in line with sp_report_policy_listing_long
**
** 23/06/2001 - Jude Killip     LeadAgCode was inexplicably defined as varchar(10), now it's 20 as it should be
**
** 06/07/2001 - Jude Killip     base on insurance folder cnt to pick up claims on earlier versions of policy
***********************************************************************************************************************************/
SET NOCOUNT ON
DECLARE @CurrentDate AS datetime
SELECT @CurrentDate = getdate()

-- Lead Agent Commission
CREATE TABLE #tempRSAPolAgCom1
(
        InsuranceCnt int,
        FolderCnt int,
        AgentType int,                          --1=Lead Agent, 2=Sub Agent
        AgentCode varchar (20) NULL,
        LeadAgGrossPrem decimal (19,4) NULL,
        AgentPerc decimal (19,4) NULL,
        AgentComm decimal (19,4) NULL
)

/*INSERT INTO #tempRSAPolAgCom1
        SELECT ifi.insurance_file_cnt,
                ifi.insurance_folder_cnt,
                1,
                p.shortname,
                lc.premium,
                lc.[percent],
                lc.value
        FROM Insurance_file ifi
        JOIN Lead_Commission lc ON ifi.insurance_file_cnt = lc.insurance_file_cnt
            AND (isnull(lc.premium,0) <> 0
            OR isnull(lc.[percent],0) <> 0
            OR isnull(lc.value,0) <> 0)
        JOIN Party p ON ifi.lead_agent_cnt = p.party_cnt
        WHERE ifi.insurance_file_cnt IN  
            (  
            SELECT  max(insurance_file_cnt)  
            FROM    insurance_file  
            WHERE insurance_folder_cnt = ifi.insurance_folder_cnt     
            AND insurance_file_type_id in (2, 5, 6, 8, 9)  
            ) */

-- SubAgent Commission
INSERT INTO #tempRSAPolAgCom1
        SELECT ifi.insurance_file_cnt,
                ifi.insurance_folder_cnt,
                2,
                p.shortname,
                ac.premium,
                ac.commission_percentage,
                ac.commission_value 
        FROM Insurance_file ifi
        JOIN agent_commission ac ON ifi.insurance_file_cnt = ac.insurance_file_cnt
            AND (isnull(ac.premium,0) <> 0 
            OR isnull(ac.commission_percentage,0) <> 0
            OR isnull(ac.commission_value,0) <> 0)
        JOIN Party p ON ac.party_cnt = p.party_cnt
        WHERE ifi.insurance_file_cnt IN  
            (  
            SELECT  max(insurance_file_cnt)  
            FROM    insurance_file  
            WHERE insurance_folder_cnt = ifi.insurance_folder_cnt     
            AND insurance_file_type_id in (2, 5, 6, 8, 9)  
            ) 

SET NOCOUNT OFF
-- Squirt it out for the report
SELECT * FROM #tempRSAPolAgCom1
--WHERE isnull(LeadAgGrossPrem,0) <> 0
--OR isnull(AgentPerc,0) <> 0
--OR isnull(AgentComm,0) <> 0

DROP TABLE #tempRSAPolAgCom1
GO


