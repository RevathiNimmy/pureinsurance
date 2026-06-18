SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Report_FAC_Ceeded_Register_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 21/08/2000
** RSA Reports - Fac_Ceeded_Register.rpt
**
**********************************************************************************************************************************
** 22/11/2000   Jude Killip     Real Data
**
** 07/12/2000   Jude Killip     bug 297 - stats_detail_type
**
** 21/03/2001   Jude Killip     bug 374 selection criteria
**                              amend commission values
**                              limit to last 12 months
**
** 25/06/2001   Jude Killip     For current period only
**                              add document_ref (for transaction code)
**                              use insurance_holder_name (not shortname)
**                              change @Reinsurer parameter to resolved name (shortname always null)
**
** 04/07/2001   Jude Killip     filters for Claims/nonClaims details
**                              default null parameter to 'ALL'
**                              allow for wildcard searches
**
** 12/11/2001   JMK             get UWType (display Insurer/Reinsurer)
**
** 19/11/2001   JMK             get UWType using sp_Report_GetUnderwritingType
**
** Released separately from now on - start versioning
***********************************************************************************************************************************
** VER  DATE        WHO     DESC
**
** 1.01 09/01/2002  JMK     use new lookup parameter "Period" - user's selection from list of
**                          current and previous period_end_dates (as a string)
**
** 1.02	27/08/2003	JMK		PN6417: Add join to Document
**
** 		06/10/2003	JMK		PN6421: Change group from Risk type to Class of Business (to go with Premium Gross To Net report)
**									also: 	add standard parameters (@branch_id and @sBasis)
**											change @Reinsurer to match party lookup (party.shortname)
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_FAC_Ceeded_Register_SFU
				@branch_id int,
                @Reinsurer varchar (20),
                @PeriodDate varchar (20),
                @sBasis varchar(50)

AS

SET NOCOUNT ON

/*
--for testing
declare @branch_id int,
		@Reinsurer varchar (100),
        @PeriodDate varchar (20),
        @sBasis varchar(50)

SELECT  @branch_id = 0, @PeriodDate = 'Jul 31 2003',
@sBasis = 'Transaction Date'
--'Transaction Period'

SELECT @Reinsurer  = 'ALL'
*/

DECLARE @period_end_date datetime, @dtSelectedPeriodEnd datetime, @dtPrevPeriodEnd datetime
DECLARE @SelectedPeriodID int, @iBranchPeriod int
-- Always use Branch 1 period table
-- if anyone sets up different periods for different branches this will
-- need to be revisited and worked out for the branch
SELECT @iBranchPeriod = 1

-- Convert selected period string to datetime
SELECT @PeriodDate = @PeriodDate + ' 23:59:59'
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)

-- Selected period values
SELECT @dtSelectedPeriodEnd = max(period_end_date)
FROM period
WHERE period_end_Date <= @period_end_date
AND company_id = @iBranchPeriod

SELECT @SelectedPeriodID = period_id
FROM period
WHERE period_end_Date  = @dtSelectedPeriodEnd
AND company_id = @iBranchPeriod

-- Previous period values
SELECT @dtPrevPeriodEnd = max(period_end_date)
FROM Period
WHERE period_end_date < @period_end_date
AND company_id = @iBranchPeriod

-- *If current period is the first period set up
IF @dtPrevPeriodEnd IS NULL
BEGIN
    SELECT  @dtPrevPeriodEnd = dateadd(month, -1, @dtSelectedPeriodEnd)
END

-- get UWType
DECLARE @UWType char(1)
EXECUTE spu_Report_GetUnderwritingType_SFU @UWType OUTPUT

CREATE TABLE #tempRSAFACCeedReg
(
        StatsFolderCnt int,
        DocRef varchar (25) NULL,
        PolNum varchar (30) NULL,
        InsFileCnt int,
        Client varchar (60) NULL,
        TransTypeID int NULL,
        TransTypeCode varchar (10) NULL,
        ProductCode varchar (10) NULL,
        TransDate datetime NULL,
        Product varchar (255) NULL,
        StatsDetailID int,
        COBCode varchar(10) NULL,
        ReinsurerCnt int NULL,
        ReinsurerShort varchar (20) NULL,
        Reinsurer varchar (100) NULL,
        AmountPremium decimal (19,4) NULL,
        AmountCommission decimal (19,4) NULL,
        PercCommission decimal (19,4) NULL,
        COB varchar (255) NULL,
        dtSelectedPeriodEnd datetime,
        UWType char (1),
		AvgPercComm decimal (19,4) NULL 
)


IF isnull(@Reinsurer,'') = ''
SELECT @Reinsurer = 'ALL'

IF @branch_id = 0
	IF @Reinsurer = 'ALL'
	BEGIN
    INSERT INTO #tempRSAFACCeedReg
        SELECT sf.stats_folder_cnt,
            sf.document_ref,
            sf.insurance_ref,
            sf.insurance_file_cnt,
            sf.insurance_holder_name,
            sf.transaction_type_id,
            sf.transaction_type_code,
            sf.product_code,
            sf.transaction_date,
            p.description,
            sd.stats_detail_id,
            sd.class_of_business_code,
            sd.ri_party_cnt,
            sd.ri_shortname,
            py.resolved_name,
            sd.this_premium_home,
            isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0),
            sd.commission_percent,
            cob.description,
            @dtSelectedPeriodEnd,
            @UWType,0
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type = 'FAC'
            AND (isnull(sd.this_premium_home,0) <> 0
                OR isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0) <> 0)
		JOIN Class_Of_Business cob ON sd.class_of_business_id = cob.class_of_business_id
        LEFT OUTER JOIN Party py ON sd.ri_party_cnt = py.party_cnt
        LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
        JOIN Document d ON d.document_ref = sf.document_ref
        WHERE
		   	(
			@sBasis = 'Transaction Date' AND  (sf.document_date > @dtPrevPeriodEnd AND sf.document_date <= @dtSelectedPeriodEnd)
			OR
			@sBasis = 'Transaction Period' AND (sf.posting_period_number = @SelectedPeriodID)
			)
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

	END
	ELSE
	BEGIN

		INSERT INTO #tempRSAFACCeedReg
			SELECT sf.stats_folder_cnt,
				sf.document_ref,
				sf.insurance_ref,
				sf.insurance_file_cnt,
				sf.insurance_holder_name,
				sf.transaction_type_id,
				sf.transaction_type_code,
				sf.product_code,
				sf.transaction_date,
				p.description,
				sd.stats_detail_id,
				sd.class_of_business_code,
				sd.ri_party_cnt,
				sd.ri_shortname,
				py.resolved_name,
				sd.this_premium_home,
				isnull(sd.lead_commission_value_home,0) +
						isnull(sd.sub_commission_value_home,0),
				sd.commission_percent,
				cob.description,
				@dtSelectedPeriodEnd,
				@UWType,0
			FROM Stats_Folder sf
			JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
				AND sd.stats_detail_type = 'FAC'
				AND sd.ri_party_cnt IN
					(SELECT party_cnt
					FROM Party
					WHERE shortname = @Reinsurer)
				AND (isnull(sd.this_premium_home,0) <> 0
					OR isnull(sd.lead_commission_value_home,0) +
						isnull(sd.sub_commission_value_home,0) <> 0)
			JOIN Class_Of_Business cob ON sd.class_of_business_id = cob.class_of_business_id
			LEFT OUTER JOIN Party py ON sd.ri_party_cnt = py.party_cnt
			LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
			JOIN Document d ON d.document_ref = sf.document_ref
			WHERE
        		(
				@sBasis = 'Transaction Date' AND  (sf.document_date > @dtPrevPeriodEnd AND sf.document_date <= @dtSelectedPeriodEnd)
				OR
	    		@sBasis = 'Transaction Period' AND (sf.posting_period_number = @SelectedPeriodID)
				)
			AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

	END
ELSE
	IF @Reinsurer = 'ALL'
	BEGIN
		INSERT INTO #tempRSAFACCeedReg
			SELECT sf.stats_folder_cnt,
				sf.document_ref,
				sf.insurance_ref,
				sf.insurance_file_cnt,
				sf.insurance_holder_name,
				sf.transaction_type_id,
				sf.transaction_type_code,
				sf.product_code,
				sf.transaction_date,
				p.description,
				sd.stats_detail_id,
				sd.class_of_business_code,
				sd.ri_party_cnt,
				sd.ri_shortname,
				py.resolved_name,
				sd.this_premium_home,
				isnull(sd.lead_commission_value_home,0) +
						isnull(sd.sub_commission_value_home,0),
				sd.commission_percent,
				cob.description,
				@dtSelectedPeriodEnd,
				@UWType,0
			FROM Stats_Folder sf
			JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
				AND sd.stats_detail_type = 'FAC'
				AND (isnull(sd.this_premium_home,0) <> 0
					OR isnull(sd.lead_commission_value_home,0) +
						isnull(sd.sub_commission_value_home,0) <> 0)
			JOIN Class_Of_Business cob ON sd.class_of_business_id = cob.class_of_business_id
			LEFT OUTER JOIN Party py ON sd.ri_party_cnt = py.party_cnt
			LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
			JOIN Document d ON d.document_ref = sf.document_ref
			WHERE
		   		(
				@sBasis = 'Transaction Date' AND  (sf.document_date > @dtPrevPeriodEnd AND sf.document_date <= @dtSelectedPeriodEnd)
				OR
				@sBasis = 'Transaction Period' AND (sf.posting_period_number = @SelectedPeriodID)
				)
			AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
			AND sf.source_id = @branch_id

	END
	ELSE
	BEGIN

    INSERT INTO #tempRSAFACCeedReg
        SELECT sf.stats_folder_cnt,
            sf.document_ref,
            sf.insurance_ref,
            sf.insurance_file_cnt,
            sf.insurance_holder_name,
            sf.transaction_type_id,
            sf.transaction_type_code,
            sf.product_code,
            sf.transaction_date,
            p.description,
            sd.stats_detail_id,
            sd.class_of_business_code,
            sd.ri_party_cnt,
            sd.ri_shortname,
            py.resolved_name,
            sd.this_premium_home,
            isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0),
            sd.commission_percent,
            cob.description,
            @dtSelectedPeriodEnd,
            @UWType,0

        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type = 'FAC'
            AND sd.ri_party_cnt IN
                (SELECT party_cnt
                FROM Party
                WHERE shortname = @Reinsurer)
            AND (isnull(sd.this_premium_home,0) <> 0
                OR isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0) <> 0)
		JOIN Class_Of_Business cob ON sd.class_of_business_id = cob.class_of_business_id
        LEFT OUTER JOIN Party py ON sd.ri_party_cnt = py.party_cnt
        LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
        JOIN Document d ON d.document_ref = sf.document_ref
        WHERE
        	(
			@sBasis = 'Transaction Date' AND  (sf.document_date > @dtPrevPeriodEnd AND sf.document_date <= @dtSelectedPeriodEnd)
			OR
	    	@sBasis = 'Transaction Period' AND (sf.posting_period_number = @SelectedPeriodID)
			)
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
			AND sf.source_id = @branch_id

	END

SET NOCOUNT OFF
update a set a.AvgPercComm = b.AvgPer from #tempRSAFACCeedReg a inner join  
(select sum(AmountCommission)/sum(AmountPremium) * 100 as AvgPer, cob from  #tempRSAFACCeedReg group by cob) as b 
on a.cob = b.cob where a.cob = b.cob

SELECT  StatsFolderCnt,
        DocRef,
        PolNum,
        InsFileCnt,
        Client,
        TransTypeID,
        TransTypeCode,
        ProductCode,
        TransDate,
        Product,
        StatsDetailID,
        COBCode,
        ReinsurerCnt,
        ReinsurerShort,
        Reinsurer,
        AmountPremium,
        AmountCommission,
        PercCommission,
        COB,
        dtSelectedPeriodEnd,
        UWType,
		AvgPercComm
FROM #tempRSAFACCeedReg
WHERE Isnull(AmountPremium,0) <> 0 OR Isnull(AmountCommission,0) <> 0
DROP TABLE  #tempRSAFACCeedReg
GO