/****** Object:  Stored Procedure dbo.sp_Report_SubAgent_Statemt    Script Date: 16/10/00 12:26:04 ******/

EXECUTE DDLDropProcedure 'spu_Report_SubAgent_Statemt_SFU'
GO
/**********************************************************************************************************************************  
** Created by Jude Killip  
** 05/09/2000  
** RSA Reports - SubAgent_Statement.rpt  
**  Created with dummy data to build the report  
**********************************************************************************************************************************  
** *Make sure Orion is referenced as "ORION_FOR_BROKING" before release!!!  
  
***********************************************************************************************************************************/  
/******************************************************************/  
/* NAME         : sp_Report_SubAgent_Statement                     ***/  
/* CREATED BY   : Ram Chandrabose                               ***/  
/* DATE         : 06-11-2000                                    ***/  
/* Description  : Used to Fetch Data for SubAgentStatement Report  ***/  
/* Used by      : SubAgentStatement.rpt (RSA Reports)              ***/  
/*                                                              ***/  
/* Parameters   :-                                              ***/  
/* If Needed    : @AgentShortName (Short Name of the Agent)     ***/  
/******************************************************************/  
--CHANGES  
--  
-- 27/06/2001   Jude Killip     rewrite, based on Agent Statement  
-- 03-11-2004 JT  PN-16032 Added From Date To date Parameter  
-- 21-10-2005   Puneet  PN-24872 Currency and Group By parameters are Added  
-- 14-06-2006   Rajesh  Filter by Agent Group  
-- 28-01-2010 By Nitesh (Changes amount check condition in select to show all trasaction)  
/******************************************************************/  
  
CREATE PROCEDURE spu_Report_SubAgent_Statemt_SFU  
 @SubAgentShortName varchar (30),  
 @Underwriting_Year char(10),  
 @start_date DateTime,  
 @end_date DateTime,  
 @TypeOfCurrency VARCHAR(15) = NULL,  
 @GroupBy VARCHAR(20) = NULL,  
 @AgentGroupCode Varchar(30)  
  
AS  
DECLARE @CompanyCode VARCHAR(10)  
DECLARE @CompanyDesc VARCHAR(255)  
DECLARE @SystemCurrencyCode VARCHAR(10)  
DECLARE @SystemCurrencyDesc VARCHAR(255)  
  
-- If underwriting year is not passed in, set to ALL  
if isnull(@Underwriting_Year, '') = ''  
BEGIN  
    set @Underwriting_Year = 'ALL'  
END  
  
IF ISNULL(@TypeOfCurrency,'') = ''  
BEGIN  
    SELECT @TypeOfCurrency = 'Base'  
END  
  
IF ISNULL(@GroupBy,'') = ''  
BEGIN  
    SELECT @GroupBy = 'No Grouping'  
END  
  
/*Get System Currency*/  
SELECT  
    @SystemCurrencyCode = c.iso_code,  
    @SystemCurrencyDesc = c.description  
FROM PMSystem pms  
JOIN currency c  
    ON c.currency_id = pms.currency_id  
WHERE pms.system_id = 1  
  
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
    SubAgentResolvedName    varchar (255) NULL,  
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
    InsuranceRef            varchar (100) NULL,  
    GrossAmount             decimal (19,4) NULL,  
    Settled                 decimal (19,4) NULL,  
    CommissionAmount        decimal (19,4) NULL,  
    InsuredName             varchar (255) NULL,  
    Underwriting_year       char (10) NULL,  
    CompanyCode VARCHAR(10),  
    CompanyDesc VARCHAR(255),  
    CurrencyCode VARCHAR(10),  
    CurrencyDesc VARCHAR(255)  
)  
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
            CASE @TypeOfCurrency  
			 WHEN 'Account' THEN (select t.account_amount where spare <> 'COMM')  
			 WHEN 'Base' THEN (select t.amount where spare <> 'COMM') 
			 WHEN 'System' THEN (select t.system_amount where spare <> 'COMM')  
			 WHEN 'Transaction' THEN (select t.currency_amount where spare <> 'COMM')  
		   END gross,  
		    CASE @TypeOfCurrency  
                        WHEN 'Account' THEN (ad.alloc_account_amount)  
                        WHEN 'Base' THEN (ad.alloc_base_amount)  
                        WHEN 'System' THEN (ad.alloc_system_amount)  
                        WHEN 'Transaction' THEN (ad.alloc_ccy_amount)  
            END  ,
            CASE @TypeOfCurrency  
			 WHEN 'Account' THEN (select t.account_amount where spare = 'COMM')  
			 WHEN 'Base' THEN (select t.amount where spare = 'COMM') 
			 WHEN 'System' THEN (select t.system_amount where spare = 'COMM')  
			 WHEN 'Transaction' THEN (select t.currency_amount where spare = 'COMM') 
		   END commission,
            NULL,  
            underwriting_year.code,  
     c.code,  
     c.description,  
     CASE @TypeOfCurrency  
         WHEN 'Account' THEN ca.iso_code  
         WHEN 'Base' THEN cb.iso_code  
         WHEN 'System' THEN @SystemCurrencyCode  
         WHEN 'Transaction' THEN ct.iso_code  
     END,  
     CASE @TypeOfCurrency  
         WHEN 'Account' THEN ca.description  
         WHEN 'Base' THEN cb.description  
         WHEN 'System' THEN @SystemCurrencyDesc  
         WHEN 'Transaction' THEN ct.description  
     END  
  
        FROM Account a  
        JOIN Ledger l ON a.ledger_id = l.ledger_id  
        JOIN transdetail t ON a.account_id = t.account_id  
        JOIN Document d ON t.document_id = d.document_id  
 JOIN company c  
     ON c.company_id = t.company_id  
 JOIN currency cb /*Base Currency*/  
     ON cb.currency_id = c.base_currency  
 JOIN currency ca /*Account Currency*/  
     ON ca.currency_id = a.currency_id  
 JOIN currency ct /*Transaction Currency*/  
     ON ct.currency_id = t.currency_id  
        LEFT OUTER JOIN  AllocationDetail ad ON t.transdetail_id = ad.transdetail_id  
 LEFT OUTER JOIN underwriting_year ON t.underwriting_year_id = underwriting_year.underwriting_year_id  
        WHERE l.ledger_short_name = 'UB' -- subagent  
  AND (underwriting_year.code = @Underwriting_Year OR rtrim(@Underwriting_Year) = 'ALL')  
  AND (D.document_date >= @start_date AND D.document_date <= @end_date)  
  
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
            CASE @TypeOfCurrency  
			 WHEN 'Account' THEN (select t.account_amount where spare <> 'COMM')  
			 WHEN 'Base' THEN (select t.amount where spare <> 'COMM') 
			 WHEN 'System' THEN (select t.system_amount where spare <> 'COMM')  
			 WHEN 'Transaction' THEN (select t.currency_amount where spare <> 'COMM')  
		    END gross, 
			CASE @TypeOfCurrency  
                        WHEN 'Account' THEN (ad.alloc_account_amount)  
                        WHEN 'Base' THEN (ad.alloc_base_amount)  
                        WHEN 'System' THEN (ad.alloc_system_amount)  
                        WHEN 'Transaction' THEN (ad.alloc_ccy_amount)  
            END  ,
           CASE @TypeOfCurrency  
			 WHEN 'Account' THEN (select t.account_amount where spare = 'COMM')  
			 WHEN 'Base' THEN (select t.amount where spare = 'COMM') 
			 WHEN 'System' THEN (select t.system_amount where spare = 'COMM')  
			 WHEN 'Transaction' THEN (select t.currency_amount where spare = 'COMM') 
		   END commission, 
            NULL,  
     underwriting_year.code,  
  
     c.code,  
     c.description,  
     CASE @TypeOfCurrency  
         WHEN 'Account' THEN ca.iso_code  
         WHEN 'Base' THEN cb.iso_code  
         WHEN 'System' THEN @SystemCurrencyCode  
         WHEN 'Transaction' THEN ct.iso_code  
     END,  
     CASE @TypeOfCurrency  
         WHEN 'Account' THEN ca.description  
         WHEN 'Base' THEN cb.description  
         WHEN 'System' THEN @SystemCurrencyDesc  
         WHEN 'Transaction' THEN ct.description  
     END  
        FROM Account a  
        JOIN Ledger l ON a.ledger_id = l.ledger_id  
        JOIN transdetail t ON a.account_id = t.account_id  
        JOIN Document d ON t.document_id = d.document_id  
 JOIN company c  
     ON c.company_id = t.company_id  
 JOIN currency cb /*Base Currency*/  
     ON cb.currency_id = c.base_currency  
 JOIN currency ca /*Account Currency*/  
     ON ca.currency_id = a.currency_id  
 JOIN currency ct /*Transaction Currency*/  
     ON ct.currency_id = t.currency_id  
  
        LEFT OUTER JOIN  AllocationDetail ad ON t.transdetail_id = ad.transdetail_id  
 LEFT OUTER JOIN underwriting_year ON t.underwriting_year_id = underwriting_year.underwriting_year_id  
  
 WHERE l.ledger_short_name = 'UB' -- subagent  
        AND a.short_code = @SubAgentShortName     -- specific SubAgent  
  AND (underwriting_year.code = @Underwriting_Year OR rtrim(@Underwriting_Year) = 'ALL')  
  AND (D.document_date >= @start_date AND D.document_date <= @end_date)  
  
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
SET SubAgentResolvedName =  LEFT(ISNULL(p.resolved_name, ''), 100)  
FROM #tempRSASubAgentStat TST INNER JOIN Party P  
ON TST.AccountKey = P.Party_cnt  
  
UPDATE #tempRSASubAgentStat  
SET InsuredName =  LEFT(ISNULL(INF.Insured_Name, ''), 100)  
FROM #tempRSASubAgentStat TST  
INNER JOIN Document DOC  
ON TST.DocRef = DOC.document_ref  
INNER JOIN Insurance_File INF  
ON DOC.insurance_file_cnt = INF.insurance_file_cnt  
Where InsuredName Is Null  
  
  

SET NOCOUNT OFF  
--Main select statement  
/*  
SELECT  
    *,  
    CASE @GroupBy  
        WHEN 'Branch' THEN CompanyCode  
        WHEN 'Branch and Currency' THEN CompanyCode  
        WHEN 'Currency' THEN CurrencyCode  
        ELSE ''  
    END 'GroupByCode1'  
FROM #tempRSASubAgentStat  
*/  
--Print 'about to squirt out final details'  
  
IF LOWER(@AgentGroupCode) = 'all'  
BEGIN  
  
 --PRINT 'ENTER1'  
  
 SELECT *,  
    CASE @GroupBy  
        WHEN 'Branch' THEN CompanyCode  
        WHEN 'Branch and Currency' THEN CompanyCode  
        WHEN 'Currency' THEN CurrencyCode  
        ELSE ''  
    END 'GroupByCode1'  
 FROM #tempRSASubAgentStat  
--PN(61787)->Condition changed by Nitesh to display all transaction in the report(28-Jan-2010)  
--isnull(GrossAmount,0) <> 0 OR isnull(CommissionAmount,0) <> 0  
WHERE (isnull(GrossAmount,0) <> 0 or isnull(CommissionAmount,0) <> 0)  or isnull(Settled,0)<>0  
  
END  
  
IF LOWER(@AgentGroupCode) <> 'all'  
BEGIN  
  
 --PRINT 'ENTER2'  
  
 SELECT *,  
    CASE @GroupBy  
        WHEN 'Branch' THEN CompanyCode  
        WHEN 'Branch and Currency' THEN CompanyCode  
        WHEN 'Currency' THEN CurrencyCode  
        ELSE ''  
    END 'GroupByCode1'  
 FROM #tempRSASubAgentStat  
 WHERE  
--PN(61787)->Condition Changed by Nitesh to display all transaction in the report(28-Jan-2010)  
--isnull(GrossAmount,0) <> 0 OR isnull(CommissionAmount,0) <> 0  
(isnull(GrossAmount,0) <> 0 or isnull(CommissionAmount,0) <> 0)  or isnull(Settled,0)<>0  
    --RC-- 15 Jun 2006  
AND AccountName IN (  
    select trading_name from party_agent where linked_account_group = (  
    select  party_cnt from party where shortname = @AgentGroupCode) )  
    --RC-- 15 Jun 2006  
  
END  
  
DROP TABLE #tempRSASubAgentStat  
GO