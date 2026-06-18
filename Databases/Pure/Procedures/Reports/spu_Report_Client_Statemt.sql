SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF
GO

DDLDropProcedure 'spu_Report_Client_Statemt'
GO


/****** Object:  Stored Procedure dbo.sp_Report_Client_Statemt    Script Date: 16/10/00 12:25:55 ******/
/******************************************************************/
/* NAME         : sp_Report_Client_Statemt                      ***/
/* CREATED BY   : Ram Chandrabose                               ***/
/* DATE         : 02-11-2000                                    ***/
/* Description  : Used to Fetch Data for Client_Statement Report***/
/* Used by      : ClientStatement.rpt (RSA Reports)             ***/
/*                                                              ***/
/* Parameters   :-                                              ***/
/* If Needed    : @sClient (Short Name of the Party)    ***/
/******************************************************************/
/* CHANGES                                                      */
/* 08/12/2000   Jude Killip     rewrite                         */
/*                              use parameters                   */
/*                                                              */
-- 26/06/2001   Jude Killip     get documenttype description to display instead of ledger code
--
/****************************************************************/

CREATE PROCEDURE spu_Report_Client_Statemt
                @PersonalClient varchar (20),
                @GroupClient varchar (20),
                @CorporateClient varchar (20)
AS
/*DECLARE @PersonalClient varchar (20),
        @GroupClient varchar (20),
        @CorporateClient varchar (20)
SELECT @PersonalClient = 'ALL',
        @GroupClient = 'ALL',
        @CorporateClient = 'ALL'*/

/* get Client shortname from one of the 3 Client parameters */
DECLARE @sClient varchar (20)
IF @PersonalClient <> 'ALL'
BEGIN
        SELECT @sClient = @PersonalClient
END
ELSE IF @GroupClient <> 'ALL'
BEGIN
        SELECT @sClient = @GroupClient
END
ELSE
BEGIN
        SELECT @sClient = @CorporateClient
END
CREATE TABLE #tempRSACliStatment
(
        Company                 varchar (255) NULL,
        CompanyAddress1         varchar (40) NULL,
        CompanyAddress2         varchar (40) NULL,
        CompanyAddress3         varchar (40) NULL,
        CompanyAddress4         varchar (40) NULL,
        PhoneAreaCode           varchar (10) NULL,
        PhoneNumber             varchar (15) NULL,
        PhoneExtension          varchar (6) NULL,
        ClientResolvedName      varchar (100) NULL,
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
        Amount                  decimal (19,4) NULL,
        Settled                 decimal (19,4) NULL
)
SET NOCOUNT ON
IF @sClient = 'ALL'
BEGIN
        --Print 'Select Account details, ALL'
        INSERT INTO #tempRSACliStatment
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
                        t.amount,
                        tm.base_match_amount
                FROM Account a
                JOIN Ledger l ON a.ledger_id = l.ledger_id
                JOIN transdetail t ON a.account_id = t.account_id
                JOIN  Document d ON t.document_id = d.document_id
                LEFT OUTER JOIN  TransMatch tm ON t.transdetail_id = tm.transdetail_id
                WHERE l.ledger_name = 'Client'

END
ELSE
BEGIN
        --Print 'Select Account details, specific Client'
        INSERT INTO #tempRSACliStatment
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
                        t.amount,
                        tm.base_match_amount
                FROM Account a
                JOIN Ledger l ON a.ledger_id = l.ledger_id
                JOIN transdetail t ON a.account_id = t.account_id
                JOIN  Document d ON t.document_id = d.document_id
                LEFT OUTER JOIN  TransMatch tm ON t.transdetail_id = tm.transdetail_id
                WHERE l.ledger_name = 'Client'
                AND a.short_code = @sClient     -- specific Client

END
--Print 'Update with Company details'
UPDATE #tempRSACliStatment
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

--Print 'Update with Client Resolved Name'
UPDATE #tempRSACliStatment
        SET ClientResolvedName = p.resolved_name
        FROM Party p
        WHERE p.party_id = AccountKey           -- Account/Party link
SET NOCOUNT OFF
--Print 'about to squirt out final details'
SELECT * FROM #tempRSACliStatment

/* Delete Main Temporary  Table                         */
DROP TABLE #tempRSACliStatment
GO


