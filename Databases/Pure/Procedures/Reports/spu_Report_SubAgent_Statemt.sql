SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_SubAgent_Statemt'
GO


CREATE PROCEDURE spu_Report_SubAgent_Statemt
    @SubAgentShortName varchar(255),
    @Source_id int
AS

/*
**************************************************************************************
** Created by Jude Killip
** 05/09/2000
** RSA Reports - SubAgent_Statement.rpt
** Created with dummy data to build the report
***************************************************************************************
***************************************************************************************
** NAME         : sp_Report_SubAgent_Statement
** CREATED BY   : Ram Chandrabose
** DATE         : 06-11-2000
** Description  : Used to Fetch Data for SubAgentStatement Report
** Used by      : SubAgentStatement.rpt (RSA Reports)
**
** Parameters   :-
** If Needed    : @AgentShortName (Short Name of the Agent)
**              : @Source_id (sub branch)
***************************************************************************************
**CHANGES
**
** 27/06/2001   Jude Killip     rewrite, based on Agent Statement
** 05/08/2002   CMG/PB          Add source_id parameter to give filtering by Sub Branch
***************************************************************************************
*/
CREATE TABLE #tempRSASubAgentStat
(
    Company                 varchar (255) NULL,
    CompanyAddress1         varchar (40) NULL,
    CompanyAddress2         varchar (40) NULL,
    CompanyAddress3         varchar (40) NULL,
    CompanyAddress4         varchar (40) NULL,
    PhoneAreaCode           varchar (10) NULL,
    PhoneNumber             varchar (15) NULL,
    PhoneExtension          varchar (6) NULL,
    SubAgentResolvedName       varchar (100) NULL,
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
    InsuredName             varchar (100) NULL
)

-- get default sub-branch for supplied source_id
DECLARE @sub_branch_id int
EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

SET NOCOUNT ON
IF @SubAgentShortName = 'ALL'
BEGIN
    --Print 'Select Account details, ALL'
    INSERT INTO #tempRSASubAgentStat
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
            (select t.amount where spare = 'GROSS') gross,
            tm.base_match_amount,
            (select t.amount where spare = 'COMM') commission,
            NULL
        FROM Account a
        JOIN Ledger l ON a.ledger_id = l.ledger_id
        JOIN transdetail t ON a.account_id = t.account_id
        JOIN  Document d ON t.document_id = d.document_id
        LEFT OUTER JOIN  TransMatch tm ON t.transdetail_id = tm.transdetail_id
        WHERE l.ledger_name = 'SubAgent'
        AND a.sub_branch_id = @sub_branch_id

END
ELSE
BEGIN
    --Print 'Select Account details, specific SubAgent'
    INSERT INTO #tempRSASubAgentStat
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
            (select t.amount where spare = 'GROSS') gross,
            tm.base_match_amount,
            (select t.amount where spare = 'COMM') commission,
            NULL
        FROM Account a
        JOIN Ledger l ON a.ledger_id = l.ledger_id
        JOIN transdetail t ON a.account_id = t.account_id
        JOIN  Document d ON t.document_id = d.document_id
        LEFT OUTER JOIN  TransMatch tm ON t.transdetail_id = tm.transdetail_id
        WHERE l.ledger_name = 'SubAgent'
        AND a.short_code = @SubAgentShortName     -- specific SubAgent
        AND a.sub_branch_id = @sub_branch_id

END

--Print 'Update with Company details'
UPDATE #tempRSASubAgentStat
    SET Company = s.Description,
        CompanyAddress1 = s.Address1,
        CompanyAddress2 = s.Address2,
        CompanyAddress3 = s.Address3,
        CompanyAddress4 = s.Address4,
        PhoneAreaCode = s.Phone_Area_Code,
        PhoneNumber = s.Phone_Number,
        PhoneExtension = s.Phone_Extension
    FROM Source s
    WHERE s.Source_Id = 1   -- 1 = Head Office

--Print 'Update with SubAgent Resolved Name, and Insured Resolved Name'
UPDATE #tempRSASubAgentStat
    SET SubAgentResolvedName = pSubAgent.resolved_name,
        InsuredName =  pClient.resolved_name
    FROM party pSubAgent, party pClient
    WHERE pSubAgent.party_id = AccountKey
    AND pClient.party_cnt =
        (SELECT max(insured_cnt)
        FROM Insurance_File WHERE insurance_ref = InsuranceRef
        AND isnull(InsuranceRef, '') <> ''
        )

SET NOCOUNT OFF

--Print 'about to squirt out final details'
SELECT * FROM #tempRSASubAgentStat
WHERE isnull(GrossAmount,0) <> 0 OR isnull(CommissionAmount,0) <> 0

DROP TABLE #tempRSASubAgentStat
GO


