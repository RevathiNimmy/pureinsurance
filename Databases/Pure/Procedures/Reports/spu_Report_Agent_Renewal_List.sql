SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Agent_Renewal_List'
GO


CREATE PROCEDURE spu_Report_Agent_Renewal_List
				@UserID int
AS

------------------------------------------------
-- Created by Jude Killip
-- 10/10/2000
-- RSA Reports - AgentRenewalList.rpt
--
------------------------------------------------
-- 11/07/2001   JMK     tidy up
--                      calc commission
--
-- 13/07/2001   JMK     forgot to remove agent_cnt...
--                      no, leave that, remove address_cnt!
--
-- 23/07/2001   JMK     use lead_commission table to get agent commission
--
-- 10/08/2001   JMK     change to agent commission table
--                      ifi.cover_start_date = DueDate
--
-- 28/09/2001   JMK     change back to lead_commission....
--
-- 24/10/2001   JMK     guess what.....(back to agent_commission)
--
-- 25/10/2001   JMK     red face - link agent_commission to renewal_insurance_file_cnt
------------------------------------------------

CREATE TABLE #tempRSAAgntRnwList
(
    AgentCnt int NULL,
    AgentName varchar (100) NULL,
    AddressCnt int NULL,
    Address1 varchar (60) NULL,
    Address2 varchar (60) NULL,
    Address3 varchar (60) NULL,
    Address4 varchar (60) NULL,
    PostalCode varchar (60) NULL,
    ContactCode varchar (10) NULL,
    ContactNumber varchar (255) NULL,
    ContactExt varchar (6) NULL,
    CompanyName varchar (255) NULL,
    CoAddress1 varchar (60) NULL,
    CoAddress2 varchar (60) NULL,
    CoAddress3 varchar (60) NULL,
    CoAddress4 varchar (60) NULL,
    CoPostalCode varchar (60) NULL,
    Client varchar (100) NULL,
    Policy varchar (30) NULL,
    DueDate datetime NULL,
    Premium decimal (19,4) NULL,
    Commission decimal (19,4) NULL
)
SET NOCOUNT ON

INSERT INTO #tempRSAAgntRnwList
    SELECT 
    MAX(rs.lead_agent_cnt)              AS AgentCnt,
    MAX(pAgent.resolved_name)           AS AgentName,
    MAX(a.address_cnt)                  AS AddressCnt,
    MAX(a.address1)                     AS Address1,
    MAX(a.address2)                     AS Address2,
    MAX(a.address3)                     AS Address3,
    MAX(a.address4)                     AS Address4,
    MAX(a.postal_code)                  AS PostalCode,
    NULL,
    NULL,
    NULL,
    MAX(s.Description)                  AS CompanyName,
    MAX(s.Address1)                     AS CoAddress1,
    MAX(s.Address2)                     AS CoAddress2,
    MAX(s.Address3)                     AS CoAddress3,
    MAX(s.Address4)                     AS CoAddress4,
    MAX(s.postal_code)                  AS CoPostalCode,
    MAX(pClient.resolved_name)          AS Client,
    MAX(ifi.insurance_ref)              AS Policy,
    MAX(ifi.cover_start_date)           AS DueDate,
    MAX(ISNULL(ifi.this_premium,0)) + ISNULL(MAX(X.SumValue),0) AS Premium,  
    SUM(ac.commission_value)            AS Commission 
    FROM Source s, Renewal_Status rs  
    JOIN Last_Print_Run lpr             ON rs.renewal_status_cnt = lpr.renewal_status_cnt           -- available for printing  
    JOIN Party pAgent                   ON rs.lead_agent_cnt = pAgent.party_cnt                     -- agent name  
    JOIN Party pClient                  ON rs.insurance_holder_cnt = pClient.party_cnt              -- client name  
    JOIN Insurance_File ifi             ON rs.renewal_insurance_file_cnt = ifi.insurance_file_cnt   -- for insurance_ref  
    LEFT JOIN (SELECT insurance_file_cnt, SUM(tc.Value) AS SumValue  
               FROM tax_calculation tc  
      GROUP BY insurance_file_cnt) AS X  
 ON rs.renewal_insurance_file_cnt = X.insurance_file_cnt  
-- Above join to get sum of tax calculation.value for an insurance_file_cnt  
-- JOIN tax_calculation tc    ON rs.renewal_insurance_file_cnt = tc.insurance_file_cnt -- for tax on policy  
    --left outer JOIN Lead_Commission lc  ON rs.renewal_insurance_file_cnt = lc.insurance_file_cnt    -- for premium and commission  
    left outer JOIN Agent_Commission ac  ON rs.renewal_insurance_file_cnt = ac.insurance_file_cnt    -- for premium and commission  
    JOIN Party_Address_Usage pau        ON pau.party_cnt = rs.lead_agent_cnt  
        AND pau.address_usage_type_id = 4                                                           -- Correspondence  
    JOIN Address a                      ON pau.address_cnt = a.address_cnt  
    WHERE s.source_id = pAgent.source_id  
 AND rs.created_by_id = @UserID  
 Group by ifi.insurance_file_cnt
	
UPDATE #tempRSAAgntRnwList
    SET ContactCode = con.area_code,
    ContactNumber = con.number,
    ContactExt = con.extension
    FROM #tempRSAAgntRnwList t1
    JOIN Contact_Address_Usage cau  ON t1.AddressCnt = cau.address_cnt
    JOIN Contact con                ON cau.contact_cnt = con.contact_cnt
        AND con.contact_type_id = 1                                                                 -- Public ISDN Telephone Line

SET NOCOUNT OFF
Select * from #tempRSAAgntRnwList

DROP TABLE #tempRSAAgntRnwList
GO