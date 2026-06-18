
EXECUTE DDLDropProcedure 'spu_Report_Payments_By_Party_Type_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

/**********************************************************************************************************************************
**
**********************************************************************************************************************************
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.00                         Created as sp_Report_Payments_By_Repairer
**
** 1.01     17/07/2002  JMK     Renamed sp_Report_Payments_By_Party_Type
**                              drop old procedure
**                              Change Repairer parameter to Party_Type
**                               Add PartyType to output
**                              Limit by Party_Type in first select and replace update (for speed)
**                              Take out SumInsuredSingle subquery - returning zeros, use SumInsured instead (tested OK on 7 random claims)
**                              Take out redundant Duties
** 1.02		09/09/2004	JT		Multi currency changes
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Payments_By_Party_Type_SFU
        (
        @Party_Type varchar(20),
        @start_date datetime,
        @end_date datetime,
        @Report_period varchar (20),
        @TypeOfCurrency	Varchar(30),
        @GroupBycode	Varchar(30)
        
        )
AS

SET NOCOUNT ON
/*
-- For testing
-- 'Specify Dates'
-- 'Yesterday'
-- 'Today'
-- 'Last Full Week'
-- 'This Week'
-- 'Last Full Month'
-- 'This Month '
declare @start_date datetime,
         @end_date datetime,
         @Report_period varchar (20),
         @Party_Type varchar(20)

select @start_date = dateadd(month, -7, getdate()),
         @end_date = getdate(),
         @Report_period = 'specify dates',
         @Party_Type = 'lawyer'
*/
/*Get System Currency Details--jitendra*/
	declare @SystemCurrencyCode varchar(10)
	declare @SystemCurrencyDesc varchar(255)
    SELECT
    	@SystemCurrencyCode = c.iso_code,
    	@SystemCurrencyDesc = c.description
    FROM PMSystem pms
    JOIN currency c
    	ON c.currency_id = pms.currency_id
    WHERE pms.system_id = 1
/*end  Get System Currency*/
SET NOCOUNT ON

CREATE TABLE #TempPaymentsByPartyTypeCode
(
        PolNum varchar (30) NULL,
        LossCode varchar (30) NULL,
        InsFileCnt int,
        TransTypeID int NULL,
        TransType varchar (10) NULL,
        ProductCode varchar (10) NULL,
        TransDate datetime NULL,
        DocRef varchar (25) NULL,
        FromDate datetime NULL,
        ToDate datetime NULL,
        Client varchar (100) NULL,
        PartyType varchar (255) NULL,                   -- Added
        Agency varchar(100) NULL,
        Product varchar (255) NULL,
        RiskType varchar (255) NULL,
        StatsFolderCnt int,
        StatsDetailID int,
        StatsDetailType char(3),
        RiskTypeID int NULL,
        PerilID int NULL,
        SumInsured decimal (19,4) NULL,
        --SumInsuredSingle decimal (19,4) NULL,         -- Removed
        --Duties decimal (19,4) NULL,                   -- Removed
        Amount1 decimal (19,4) NULL,
        CompanyCode	Varchar(10),
        CompanyDesc	Varchar(255),
        CurrencyCode	Varchar(10),
        CurrencyDesc	Varchar(255),
        GroupByCode		Varchar(30)
)

CREATE TABLE #tempStatsFolder
    (
        StatsFolderCnt int
    )

INSERT INTO #tempStatsFolder
    SELECT stats_folder_cnt
    FROM Stats_Folder
    WHERE document_ref like ('c%')
    AND (
    @Report_period = 'specify dates' AND
        (
        datediff(day, @start_date, transaction_date) >=0
        AND datediff(day, transaction_date, @end_date) >=0
        )
    OR
    @Report_period = 'yesterday' AND
    datediff (day, transaction_date, getdate())= 1
    OR
    @Report_period = 'today' AND
    datediff (day, transaction_date, getdate())= 0
    OR
    @Report_period = 'last full week' AND
    datediff (week, transaction_date, getdate())= 1
    OR
    @Report_period = 'this week' AND
    datediff (week, transaction_date, getdate())= 0
    OR
    @Report_period = 'last full month' AND
    datediff (month, transaction_date, getdate())= 1
    OR
    @Report_period = 'this month' AND
    datediff (month, transaction_date, getdate())= 0
    )

INSERT INTO #TempPaymentsByPartyTypeCode
    SELECT sf.insurance_ref,
    sf.loss_code,
    sf.insurance_file_cnt,
    sf.transaction_type_id,
    sf.transaction_type_code,
    sf.product_code,
    sf.transaction_date,
    sf.document_ref,
    sf.cover_start_date,
    sf.expiry_date,
    pt.resolved_Name,
    pty.description,
    (SELECT resolved_name FROM Party WHERE shortname = sf.agent_shortname),
    p.description,
    rt.description,
    sd.stats_folder_cnt,
    sd.stats_detail_id,
    sd.stats_detail_type,
    sd.risk_type_id,
    sd.peril_id,
    Case @TypeOfcurrency 
    	WHEN 'System' THEN sd.sum_insured_system
		WHEN 'Base' THEN sd.sum_insured_home
		WHEN 'Transaction' THEN ROUND(ISNULL(sd.sum_insured_home,0)/ISNULL(sd.currency_rate,1),2)
	END,
	Case @TypeOfcurrency 
	   	WHEN 'System' THEN sd.this_premium_system
		WHEN 'Base' THEN sd.this_premium_home
		WHEN 'Transaction' THEN sd.this_premium_original
	END,
	S.Code,S.description,
	Case @TypeOfCurrency 
		WHEN 'Base' THEN CB.Code
		WHEN 'system' THEN @SystemCurrencyCode
		WHEN 'Transaction' THEN CT.Code
	END,
    Case @TypeOfCurrency 
		WHEN 'Base' THEN CB.description
		WHEN 'system' THEN @SystemCurrencyDesc
		WHEN 'Transaction' THEN CT.Description
	END,
	Case @GroupByCode 
		WHEN 'Branch' THEN S.Code
		WHEN 'Branch And Currency' THEN S.Code
		WHEN 'Currency' THEN CT.Code
	END
		

FROM Stats_Detail sd
JOIN party pt on sd.ri_party_cnt = pt.party_cnt
left join party_type pty on pt.party_type_id = pty.party_type_id
JOIN Stats_Folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
JOIN #tempStatsFolder sft ON sd.stats_folder_cnt = sft.StatsFolderCnt
LEFT OUTER JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
JOIN SOURCE S ON s.source_id = sf.source_id
JOIN Currency CB ON CB.Currency_id = S.Base_currency_id
JOIN currency CT ON CT.Iso_code= sf.currency_code

WHERE sd.stats_detail_type = 'GRS'
AND pty.code like 'ot%'
AND (pty.description = @Party_Type OR @Party_Type = 'ALL')

SET NOCOUNT OFF

select * from #TempPaymentsByPartyTypeCode

DROP TABLE #tempStatsFolder
DROP TABLE #TempPaymentsByPartyTypeCode


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

