SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Agent_Statemt_T'
GO


CREATE PROCEDURE spu_Report_Agent_Statemt_T
        @company_id int, 
        @AgentShortName as varchar(255)
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 12/03/2002
** Same as spu_Report_Agent_Statemt but with Tax split out - additional report required by AUA
** v1.00
**********************************************************************************************************************************
** VER  DATE        WHO     DESC
** 1.01 5/6/2       Tomo    We want the allocated amount, not that which was marked.
**
** 1.02 10/Jun/02   JMK     Add second Settled Amount column to pick up Data Transfer generated amounts
**                          Add postcodes
**                          Display amended Document Codes instead of Document Description
**                          Add FirstTransDate column so report can group policies together within date order
**
** 1.03 12/Jun/02   JMK     Get Policy and Insured values from CashListItem for Receipts
**                          2 extra Fax fields added
** 01/08/2002   AMJ - branch specific change
***********************************************************************************************************************************/

/*
declare @AgentShortName varchar (20)
select @AgentShortName = 'AUAH'
*/
CREATE TABLE #tempRSAAgentStat
(
    Company                 varchar (255) NULL,
    CompanyAddress1         varchar (40) NULL,
    CompanyAddress2         varchar (40) NULL,
    CompanyAddress3         varchar (40) NULL,
    CompanyAddress4         varchar (40) NULL,
    CompanyPostCode         varchar (40) NULL,
    PhoneAreaCode           varchar (10) NULL,
    PhoneNumber             varchar (15) NULL,
    PhoneExtension          varchar (6) NULL,
    FaxAreaCode               varchar (10) NULL,
    FaxNumber                  varchar (15) NULL,
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
    AccountPostCode         varchar (40) NULL,
    DocRef                  varchar (25) NULL,
    DocDate                 datetime NULL,
    CreateDate              datetime NULL,
    InsuranceRef            varchar (30) NULL,
    FirstTransDate          datetime NULL,
    GrossAmount             decimal (19,4) NULL,
    Settled                 decimal (19,4) NULL,
    Settled2                decimal (19,4) NULL,
    CommissionAmount        decimal (19,4) NULL,
    TaxAmount               decimal (19,4) NULL,
    InsuredName             varchar (100) NULL,
    FromSirius              tinyint NULL,
    ExportStatus            char (1) NULL
)

SET NOCOUNT ON
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
        NULL,
        NULL,
        NULL,
        l.ledger_id,
        (SELECT CASE dt.code
            WHEN 'JN' THEN 'JRN'
            WHEN 'SND' THEN 'NBD'
            WHEN 'SNC' THEN 'NBC'
            WHEN 'SWD' THEN 'WO'
            WHEN 'SRD' THEN 'RND'
            WHEN 'SRC' THEN 'RNC'
            WHEN 'SED' THEN 'END'
            WHEN 'SEC' THEN 'END'
            WHEN 'SRP' THEN 'REC'
            WHEN 'SPY' THEN 'PAY'
            WHEN 'CLP' THEN 'CLMP'
            WHEN 'CLR' THEN 'CLMR'
            ELSE dt.code
            END
        FROM orion_for_broking..documenttype dt
        WHERE dt.documenttype_id = d.documenttype_id
        ),
        a.account_key,
        a.account_id,
        a.short_code,
        a.account_name,
        a.address1,
        a.address2,
        a.address3,
        a.address4,
        a.postal_code,
        d.document_ref,
        d.document_date,
        d.created_date,
        CASE d.documenttype_id
            WHEN 22 THEN c.our_ref
            ELSE t.insurance_ref
        END,
        NULL,
        (select t.amount where spare <> 'COMM' AND spare <> 'TAX' and spare <> 'ALLOCATED'),
            (SELECT SUM(tm.base_match_amount)
                FROM Orion_for_broking..TransMatch tm
                WHERE t.transdetail_id = tm.transdetail_id),
--        (SELECT SUM(ad.alloc_base_amount)
--            FROM Orion_for_broking..allocationdetail ad
--            WHERE t.transdetail_id = ad.transdetail_id),
--        (SELECT SUM(-ad2.alloc_base_amount)
--            FROM Orion_for_broking..allocationdetail ad1,
--      Orion_for_broking..allocationdetail ad2
--            WHERE t.transdetail_id = ad1.transdetail_id
--      AND ad1.allocation_id = ad2.allocation_id),
        (select t.amount where spare = 'ALLOCATED'),        -- Settled2
        (select t.amount where spare = 'COMM'),
        (select t.amount where spare = 'TAX'),
        CASE d.documenttype_id
            WHEN 22 THEN c.media_ref
            ELSE NULL
        END,
        (SELECT dt.from_sirius FROM orion_for_broking..documenttype dt WHERE d.documenttype_id = dt.documenttype_id),
        (SELECT max(tef.accounts_export_status) FROM transaction_export_folder tef WHERE tef.document_ref = d.document_ref)
    FROM orion_for_broking..Account a
    JOIN orion_for_broking..Ledger l ON a.ledger_id = l.ledger_id
    JOIN orion_for_broking..transdetail t ON a.account_id = t.account_id
        AND t.fully_matched <> 1
    LEFT OUTER JOIN orion_for_broking..CashListItem c on c.transdetail_id = t.transdetail_id
    JOIN  orion_for_broking..Document d ON t.document_id = d.document_id
        AND d.document_date <= getdate()
    WHERE l.ledger_name = 'Agent'
    AND (a.short_code = @AgentShortName     -- specific Agent
        OR
        @AgentShortName = 'ALL')            -- all Agents
    and (   d.company_id = @company_id
        or
        @company_id = 0
    )

-- Get together policy numbers and their earliest doc date
CREATE TABLE #tempDocDates
(
    FirstDocDate    datetime,
    InsRef          varchar (30)
)

INSERT INTO #tempDocDates
    SELECT min(DocDate), InsuranceRef
    FROM #tempRSAAgentStat
    GROUP BY InsuranceRef


--Print 'Update with earliest DocDate for each InsuranceRef'
UPDATE #tempRSAAgentStat
    SET FirstTransDate = FirstDocDate
    FROM #tempDocDates
    WHERE InsuranceRef = InsRef

--Print 'Use DocDate for Transactions with no InsuranceRef'
UPDATE #tempRSAAgentStat
    SET FirstTransDate = DocDate
    WHERE isnull(InsuranceRef,'') = ''

--Print 'Update with Company details'
UPDATE #tempRSAAgentStat
    SET Company = s.Description,
        CompanyAddress1 = s.Address1,
        CompanyAddress2 = s.Address2,
        CompanyAddress3 = s.Address3,
        CompanyAddress4 = s.Address4,
        CompanyPostCode = s.postal_code,
        PhoneAreaCode = s.Phone_Area_Code,
        PhoneNumber = s.Phone_Number,
        PhoneExtension = s.Phone_Extension,
        FaxAreaCode = s.Fax_Area_Code,
        FaxNumber = s.Fax_Number
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
WHERE  FromSirius = 1 and isnull(ExportStatus, 'c') = 'c'
    OR FromSirius = 0

DROP TABLE #tempDocDates
DROP TABLE #tempRSAAgentStat

GO
