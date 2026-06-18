SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Agent_Statemt'
GO

CREATE PROCEDURE spu_Report_Agent_Statemt
    @company_id int, 
    @AgentShortName varchar(255)

AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 04/09/2000
** RSA Reports - Agent_Statement.rpt
**  Created with dummy data to build the report
***********************************************************************************************************************************/
/******************************************************************/
/* NAME         : sp_Report_Agent_Statement                     ***/
/* CREATED BY   : Ram Chandrabose                               ***/
/* DATE         : 07-11-2000                                    ***/
/* Description  : Used to Fetch Data for AgentStatement Report  ***/
/* Used by      : AgentStatement.rpt (RSA Reports)              ***/
/*                                                              ***/
/* Parameters   :-                                              ***/
/* If Needed    : @AgentShortName (Short Name of the Agent)     ***/
/******************************************************************/
/* CHANGES                                                      */
/* 01/12/2000   Jude Killip     amend address/country join      */
/*                              use parameter                   */
/*                              bug fixes                       */
--
-- 27/06/2001   Jude Killip     rewrite, based on Agent Statement (which is working)
--                              can't think why this wasn't done in the first place
--
-- 27/09/2001   Jude Killip     filter out dodgy exported account details
--                              transmatch join was creating duplicates - sum in subquery instead
--                              filter out fully matched details
--                              filter up to today's document date
/****************************************************************/
CREATE TABLE #tempRSAAgentStat
(
    Company                 varchar (255) NULL,
    CompanyAddress1         varchar (40) NULL,
    CompanyAddress2         varchar (40) NULL,
    CompanyAddress3         varchar (40) NULL,
    CompanyAddress4         varchar (40) NULL,
    PhoneAreaCode           varchar (10) NULL,
    PhoneNumber             varchar (15) NULL,
    PhoneExtension          varchar (6) NULL,
    AgentResolvedName       varchar (100) NULL,
    LedgerID                smallint NULL,
    TransType               varchar (255) NULL,
    AccountKey              int NULL,
    AccountID               int,
    AccountCode             varchar (30) NULL,
    AccountName             varchar (60) NULL,
    AccountAddress1         varchar (40) NULL,
    AccountAddress2         varchar (40) NULL,
    AccountAddress3         varchar (40) NULL,
    AccountAddress4         varchar (40) NULL,
    DocRef                  varchar (25) NULL,
    DocDate                 datetime NULL,
    CreateDate              datetime NULL,
    InsuranceRef            varchar (30) NULL,
    GrossAmount             decimal (19,4) NULL,
    Settled                 decimal (19,4) NULL,
    CommissionAmount        decimal (19,4) NULL,
    InsuredName             varchar (100) NULL,
    FromSirius              tinyint NULL,
    ExportStatus            char (1) NULL
)

select @company_id = isnull(@company_id,0)

SET NOCOUNT ON
IF @AgentShortName = 'ALL'
BEGIN
    --Print 'Select Account details, ALL'
    INSERT INTO #tempRSAAgentStat
        SELECT NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            l.ledger_id,
            (SELECT dt.description FROM documenttype dt
                WHERE dt.documenttype_id = d.documenttype_id),
            a.account_key,
            a.account_id,
            a.short_code,
            a.account_name,
            a.address1,
            a.address2,
            a.address3,
            a.address4,
            d.document_ref,
            d.document_date,
            d.created_date,
            t.insurance_ref,
            (select t.amount where spare <> 'COMM'),
            (SELECT SUM(tm.base_match_amount)
                FROM TransMatch tm
                WHERE t.transdetail_id = tm.transdetail_id),
            (select t.amount where spare = 'COMM'),
            NULL,
            (SELECT dt.from_sirius FROM documenttype dt WHERE d.documenttype_id = dt.documenttype_id),
            (SELECT max(tef.accounts_export_status) FROM transaction_export_folder tef WHERE tef.document_ref = d.document_ref)
        FROM Account a
        JOIN Ledger l ON a.ledger_id = l.ledger_id
        JOIN transdetail t ON a.account_id = t.account_id
            AND t.fully_matched <> 1
        JOIN  Document d ON t.document_id = d.document_id
            AND d.document_date <= getdate()
        WHERE l.ledger_name = 'Agent'
    and     
    (   d.company_id = @company_id
        or
        @company_id = 0
    )

END
ELSE
BEGIN
    --Print 'Select Account details, specific Agent'
    INSERT INTO #tempRSAAgentStat
        SELECT NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL,
            l.ledger_id,
            (SELECT dt.description FROM documenttype dt
                WHERE dt.documenttype_id = d.documenttype_id),
            a.account_key,
            a.account_id,
            a.short_code,
            a.account_name,
            a.address1,
            a.address2,
            a.address3,
            a.address4,
            d.document_ref,
            d.document_date,
            d.created_date,
            t.insurance_ref,
            (select t.amount where spare <> 'COMM'),
            (SELECT SUM(tm.base_match_amount)
                FROM TransMatch tm
                WHERE t.transdetail_id = tm.transdetail_id),
            (select t.amount where spare = 'COMM'),
            NULL,
            (SELECT dt.from_sirius FROM documenttype dt WHERE d.documenttype_id = dt.documenttype_id),
            (SELECT max(tef.accounts_export_status) FROM transaction_export_folder tef WHERE tef.document_ref = d.document_ref)
        FROM Account a
        JOIN Ledger l ON a.ledger_id = l.ledger_id
        JOIN transdetail t ON a.account_id = t.account_id
            AND t.fully_matched <> 1
        JOIN  Document d ON t.document_id = d.document_id
        WHERE l.ledger_name = 'Agent'
        AND a.short_code = @AgentShortName     -- specific Agent
    and     
    (   d.company_id = @company_id
        or
        @company_id = 0
    )
END

--Print 'Update with Company details'
UPDATE #tempRSAAgentStat
    SET Company = s.Description,
        CompanyAddress1 = s.Address1,
        CompanyAddress2 = s.Address2,
        CompanyAddress3 = s.Address3,
        CompanyAddress4 = s.Address4,
        PhoneAreaCode = s.Phone_Area_Code,
        PhoneNumber = s.Phone_Number,
        PhoneExtension = s.Phone_Extension
    FROM Source s
    WHERE s.Source_Id = @company_id   -- 1 = Head Office

--Print 'Update with Agent Resolved Name, and Insured Resolved Name'
UPDATE #tempRSAAgentStat
    SET AgentResolvedName = pAgent.resolved_name,
        InsuredName =  pClient.resolved_name
    FROM party pAgent, party pClient
    WHERE pAgent.party_id = AccountKey
    AND pClient.party_cnt =
        (SELECT max(insured_cnt)
        FROM Insurance_File WHERE insurance_ref = InsuranceRef
        AND isnull(InsuranceRef, '') <> ''
        )

SET NOCOUNT OFF

 --print 'Extract the data - minus dodgy transactions'
SELECT * FROM #tempRSAAgentStat
WHERE  FromSirius = 1 and ExportStatus = 'c'
    OR FromSirius = 0
DROP TABLE #tempRSAAgentStat
GO
