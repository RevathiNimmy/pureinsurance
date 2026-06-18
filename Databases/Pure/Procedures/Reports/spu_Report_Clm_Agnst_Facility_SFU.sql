SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Clm_Agnst_Facility_SFU'
GO


/**********************************************************************************************************************************
** Created by Jude Killip
** 06/11/2001
** Reports - Claims_Incurred_against_Facility_Attachments.rpt
**
**********************************************************************************************************************************
** 1.01 JMK 12/11/2001  Store current Treaty information against all previous Treaties (so that they can be grouped together)
**
** 1.02 JMK 03/12/2001  Amend Treaty information selection
**
** 1.03 KB  07/01/2002  Remove check of ri_shortname against treaty description
**                      This was added to cater for incorrect data but is no longer appropriate
** 1.04 KB  09/01/2002  Analyse by product rather than by class of business
**
** 1.05 JMK 13/03/2002  get all insurance_file_cnt with claims transactions to limit selection
** 10.6	JT	10/08/2004	MultiCurrency feature
** 10.7 RC  14Jun2006	Filter by Agent Group
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Clm_Agnst_Facility_SFU
	@ClassOfBus		varchar(10),
	@AgentCode		varchar(20),
	@TypeOfCurrency	Varchar(30),
	@GroupByCode	Varchar(30),
	@AgentGroupCode Varchar(30)
AS

SET NOCOUNT ON

-- AGS 290604 Start
DECLARE @sClassOfBus		varchar(10),
	@iAgtCnt		int,
	@sAgentCode		varchar(20)	
--290604 End	

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

-- get current year values
DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
EXECUTE spu_Report_GetCurrentYear_SFU @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT

IF @ClassOfBus = 'ALL' SELECT @ClassOfBus = NULL
SELECT @sClassOfBus = ISNULL(@ClassOfBus, '')


IF @AgentCode = 'ALL' SELECT @AgentCode = NULL
SELECT @sAgentCode = ISNULL(@AgentCode, '')
IF @sAgentCode <> ''
BEGIN 
	SELECT @iAgtCnt = Party_Cnt
		FROM Party
		WHERE shortname = @sAgentCode
END
ELSE SELECT @iAgtCnt = 0
/*Get System Currency Details*/
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
-- get all insurance_file_cnt with claims transactions to limit selection
CREATE TABLE #tmpClaims
(
    InsuranceFileCnt int
)

INSERT INTO #tmpClaims
    SELECT insurance_file_cnt
    FROM stats_folder
    WHERE transaction_type_code LIKE ('C_%')

CREATE TABLE #tmpStats
(
    StatsFolderCnt int,
    CobID int NULL,
    AgentCnt int NULL,
    CurrentTreaty varchar (20) NULL,
    Treaty varchar (20) NULL,
    RecordTypeId int,
    RecordType varchar (30),
    Amount money NULL,
    Product varchar (255)    NULL,
    SourceID	INT	--**added for multi currency 11/08/2004
)

--print 'Gross Premium Records'
INSERT INTO #tmpStats
    SELECT sd.stats_folder_cnt,
        sd.class_of_business_id,
        sf.agent_cnt,
        NULL,
        sd.ri_shortname,
        1,
        'GPI Transacted',
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN sd.this_premium_home * -1
        	WHEN 'System' THEN	sd.this_premium_system * -1
        END,
    sf.product_code,sf.source_id
        FROM  stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
        WHERE stats_detail_type = ('TTY')
        AND sf.transaction_type_code NOT LIKE ('C_%')           -- Not claims
        AND 
		(
			(@TypeOfCurrency = 'Base' and isnull(sd.this_premium_home,0) <> 0) 
			or 
			(@TypeOfCurrency = 'System' and isnull(sd.this_premium_system,0) <> 0)
		)
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
        AND sf.stats_folder_cnt IN
                                    (
                                    SELECT stats_folder_cnt
                                    FROM stats_detail
                                    -- WHERE ri_shortname IN (SELECT code FROM treaty)
                                    -- OR ri_shortname IN (SELECT description FROM treaty))
                                    WHERE ri_shortname IN (SELECT code FROM treaty)
                                    )
        AND sf.insurance_file_cnt IN
                                    (
                                    SELECT InsuranceFileCnt
                                    FROM #tmpClaims
                                    )

--print 'Commission Records'
INSERT INTO #tmpStats
    SELECT sd.stats_folder_cnt,
        sd.class_of_business_id,
        sf.agent_cnt,
        NULL,
        sd.ri_shortname,
        2,
        'Commission',
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN	isnull(sd.lead_commission_value_home,0) 
        	WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
        END
        	+
        Case @TypeOfCurrency 
			WHEN 'Base' THEN	isnull(sd.sub_commission_value_home,0) * -1
			WHEN 'System' THEN isnull(sd.sub_commission_value_system,0) * -1
		END,
    sf.product_code,sf.source_id
        FROM  stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
        WHERE stats_detail_type = ('TTY')
        AND sf.transaction_type_code NOT LIKE ('C_%')           -- Not claims
        AND(
			(@TypeOfCurrency = 'Base' and isnull(sd.sub_commission_value_home,0) <> 0) 
			or 
			(@TypeOfCurrency = 'System' and isnull(sd.sub_commission_value_system,0) <> 0)
		   )
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
        AND sf.stats_folder_cnt IN
                                    (
                                    SELECT stats_folder_cnt
                                    FROM stats_detail
                                    -- WHERE ri_shortname IN (SELECT code FROM treaty)
                                    -- OR ri_shortname IN (SELECT description FROM treaty))
                                    WHERE ri_shortname IN (SELECT code FROM treaty)
                                    )
        AND sf.insurance_file_cnt IN
                                    (
                                    SELECT InsuranceFileCnt
                                    FROM #tmpClaims
                                    )

--print 'Claims Records'
INSERT INTO #tmpStats
    SELECT sd.stats_folder_cnt,
        sd.class_of_business_id,
        sf.agent_cnt,
        NULL,
        sd.ri_shortname,
        3,
        'Claims Paid',
        Case @TypeOfCurrency 
		        	WHEN 'Base' THEN sd.this_premium_home * -1
		        	WHEN 'System' THEN	sd.this_premium_system * -1
        END,
        --sd.this_premium_home * -1,
    sf.product_code,sf.source_id
        FROM  stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
        WHERE stats_detail_type = ('TTY')
        AND sf.transaction_type_code = ('C_CP')           -- claims payments
        AND 
        (
			(@TypeOfCurrency = 'Base' and isnull(sd.this_premium_home,0) <> 0) 
			or 
			(@TypeOfCurrency = 'System' and isnull(sd.this_premium_system,0) <> 0)
		)
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
        AND sf.stats_folder_cnt IN
                                    (
                                    SELECT stats_folder_cnt
                                    FROM stats_detail
                                    --WHERE ri_shortname IN (SELECT code FROM treaty)
                                    --OR ri_shortname IN (SELECT description FROM treaty))
                                    WHERE ri_shortname IN (SELECT code FROM treaty)
                                    )
--print 'OS Reserve Records'
INSERT INTO #tmpStats
    SELECT sd.stats_folder_cnt,
        sd.class_of_business_id,
        sf.agent_cnt,
        NULL,
        sd.ri_shortname,
        4,
        'O/S Loss Reserve',
--        sd.this_premium_home * -1,
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN sd.this_premium_home * -1
        	WHEN 'System' THEN	sd.this_premium_system * -1
        END,
    sf.product_code,sf.source_id
        FROM  stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
	JOIN CLAIM CLM ON CLM.Claim_Number = sf.loss_code and CLM.Claim_Status_id IN(2,4)
        WHERE stats_detail_type = ('TTY')
        AND sf.transaction_type_code in ('C_CO', 'C_CR')           -- claims opened and maintained
		AND (
			(@TypeOfCurrency = 'Base' and isnull(sd.this_premium_home ,0) <> 0) 
			or 
			(@TypeOfCurrency = 'System' and isnull(sd.this_premium_system ,0) <> 0)
		)
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
        AND sf.stats_folder_cnt IN
                                    (
                                    SELECT stats_folder_cnt
                                    FROM stats_detail
                                    --WHERE ri_shortname IN (SELECT code FROM treaty)
                                    --OR ri_shortname IN (SELECT description FROM treaty))
                                    WHERE ri_shortname IN (SELECT code FROM treaty)
                                    )
        --AND (SELECT Claim_Status_id FROM claim WHERE Claim_Number = sf.loss_code) IN (2,4) -- open or reopened


--print 'Unearned Premium Records'
-- Use cursor to calculate unearned premium
-- Cursor variables
DECLARE @StatsFolderCnt int,
        @CobID int,
        @AgentCnt int,
        @Treaty varchar (20),
        @Amount decimal (19,8),
        @TotalDays int,
        @DaysEarned int,
        @MidnightRenew int,
    	@Product    varchar (255),
    	@SourceID	Int

-- Additional variable for calculating
DECLARE @DailyRate decimal (19,8),
        @UnearnedAmount decimal (19,8)

DECLARE Stats_cursor CURSOR FOR
    SELECT sd.stats_folder_cnt,
        sd.class_of_business_id,
        sf.agent_cnt,
        sd.ri_shortname,
--        sd.this_premium_home,
		Case @TypeOfCurrency 
			WHEN 'Base' THEN sd.this_premium_home * -1
			WHEN 'System' THEN	sd.this_premium_system * -1
		END,
        datediff(day, sf.cover_start_date, sf.expiry_date),
        datediff(day, sf.cover_start_date, @dtCurrentPeriodEnd),
        (SELECT isnull(p.is_midnight_renewal,0) FROM Product p WHERE sf.product_id = p.product_id),
    sf.product_code,sf.source_id
        FROM  stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
        WHERE stats_detail_type = ('TTY')
        AND sf.transaction_type_code NOT LIKE ('C_%')           -- Not claims
        AND 
        (
			(@TypeOfCurrency = 'Base' and isnull(sd.this_premium_home,0) <> 0) 
			or 
			(@TypeOfCurrency = 'System' and isnull(sd.this_premium_system ,0) <> 0)
		)

--        AND isnull(sd.this_premium_home,0) <> 0
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
        AND sf.stats_folder_cnt IN
                                    (
                                    SELECT stats_folder_cnt
                                    FROM stats_detail
                                    WHERE ri_shortname IN (SELECT code FROM treaty)
                                    )
        AND sf.insurance_file_cnt IN
                                    (
                                    SELECT InsuranceFileCnt
                                    FROM #tmpClaims
                                    )


OPEN Stats_cursor

    FETCH NEXT FROM Stats_cursor
    INTO @StatsFolderCnt,
        @CobID,
        @AgentCnt,
        @Treaty,
        @Amount,
        @TotalDays,
        @DaysEarned,
        @MidnightRenew,
    	@Product,
    	@SourceID

    WHILE @@FETCH_STATUS = 0
    BEGIN

        IF @DaysEarned >=  @TotalDays + @MidnightRenew          -- all earned
            BEGIN
                SELECT @UnearnedAmount = 0
            END
            ELSE
            BEGIN
                IF @DaysEarned <=0                              -- in the future therefore all unearned
                    BEGIN
                        SELECT @UnearnedAmount = @Amount
                    END
                    ELSE                                        -- part earned
                    BEGIN
                        SELECT @DailyRate = @Amount/(@TotalDays + @MidnightRenew)
                        SELECT @UnearnedAmount = @DailyRate*(@TotalDays + @MidnightRenew - @DaysEarned)
                    END
            END

        -- debug
        -- SELECT @Amount 'Amount', @TotalDays+@MidnightRenew 'TotalDays + MidnightRenew', @DaysEarned 'DaysEarned', @DailyRate 'DailyRate', @UnearnedAmount 'UnearnedAmount'

        INSERT INTO #tmpStats
            SELECT StatsFolderCnt = @StatsFolderCnt,
                CobID = @CobID,
                AgentCnt = @AgentCnt,
                NULL,
                Treaty = @Treaty,
                RecordTypeId = 5,
                RecordType = 'Net Unearned Prm',
                Amount = @UnearnedAmount,
        		Product = @Product,
        		SourceID = @SourceId

        FETCH NEXT FROM Stats_cursor
        INTO @StatsFolderCnt,
            @CobID,
            @AgentCnt,
            @Treaty,
            @Amount,
            @TotalDays,
            @DaysEarned,
            @MidnightRenew,
        	@Product,
        	@SourceID

    END
CLOSE Stats_cursor
DEALLOCATE Stats_cursor

--*********************************************************************************************************************************
-- START    Populate temporary table #tmpTreaty
--          Treaty details going back 6 Treaties
--*********************************************************************************************************************************
CREATE TABLE #tmpTreaty
    (
        tempID int IDENTITY,
        code1 varchar (10),
        replaces_id1 int,
        code2 varchar (10),
        replaces_id2 int,
        code3 varchar (10),
        replaces_id3 int,
        code4 varchar (10),
        replaces_id4 int,
        code5 varchar (10),
        replaces_id5 int,
        code6 varchar (10),
        replaces_id6 int,
    )

INSERT INTO #tmpTreaty
    SELECT t.code,
        t.replaces_treaty_id,
        NULL, NULL,
        NULL, NULL,
        NULL, NULL,
        NULL, NULL,
        NULL, NULL
    FROM treaty t
    WHERE t.treaty_id NOT IN (SELECT isnull(t1.replaces_treaty_id,'')
                        FROM treaty t1
                        WHERE t1.replaces_treaty_id = t.treaty_id
                        AND datediff(day, @dtCurrentPeriodEnd, t1.expiry_date) >=0
                        AND datediff(day, @dtCurrentPeriodEnd, t1.effective_date) <=0
                        AND t1.is_deleted = 0)
    AND datediff(day, @dtCurrentPeriodEnd, t.expiry_date) >=0
    AND datediff(day, @dtCurrentPeriodEnd, t.effective_date) <=0
    AND t.is_deleted = 0

--print 'current treaty -1'
UPDATE  #tmpTreaty
    SET code2 = t.code,
        replaces_id2 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id1
    WHERE t.is_deleted = 0

--print 'current treaty -2'
UPDATE  #tmpTreaty
    SET code3 = t.code,
        replaces_id3 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id2
    WHERE t.is_deleted = 0

--print 'current treaty -3'
UPDATE  #tmpTreaty
    SET code4 = t.code,
        replaces_id4 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id3
    WHERE t.is_deleted = 0

--print 'current treaty -4'
UPDATE  #tmpTreaty
    SET code5 = t.code,
        replaces_id5 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id4
    WHERE t.is_deleted = 0

--print 'current treaty -5'
UPDATE  #tmpTreaty
    SET code6 = t.code,
        replaces_id6 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id5
    WHERE t.is_deleted = 0

--*********************************************************************************************************************************
-- END    Populate temporary table #tmpTreaty
--*********************************************************************************************************************************
--*********************************************************************************************************************************
-- START    Populate temporary table #tmpTreatyCurrent
--          Store Current Treaty against all previous Treaties
--*********************************************************************************************************************************
CREATE TABLE #tmpTreatyCurrent
    (
        CurrentTreaty varchar (20),
        Treaty varchar (20) NULL
    )

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code1
    FROM #tmpTreaty tt
    WHERE tt.code1 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code2
    FROM #tmpTreaty tt
    WHERE tt.code2 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code3
    FROM #tmpTreaty tt
    WHERE tt.code3 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code4
    FROM #tmpTreaty tt
    WHERE tt.code4 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code5
    FROM #tmpTreaty tt
    WHERE tt.code5 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code6
    FROM #tmpTreaty tt
    WHERE tt.code6 IS NOT NULL

--*********************************************************************************************************************************
-- END    Populate temporary table #tmpTreatyCurrent
--*********************************************************************************************************************************

UPDATE #tmpStats
    SET CurrentTreaty = tc.CurrentTreaty
    FROM #tmpTreatyCurrent tc
    WHERE tc.Treaty = #tmpStats.Treaty

DROP TABLE #tmpTreatyCurrent

CREATE TABLE #tmpClmAgFacility
    (
        ClassOfBusiness varchar (255) NULL,
        Agent varchar (255) NULL,
        Treaty varchar (20) NULL,
        CurrentTreaty varchar (10),
        RecordTypeId int,
        RecordType varchar (30),
        dtCurrentPeriodEnd datetime,
        TreatyName1 varchar (20) NULL,
        TreatyName2 varchar (20) NULL,
        TreatyName3 varchar (20) NULL,
        TreatyName4 varchar (20) NULL,
        TreatyName5 varchar (20) NULL,
        TreatyName6 varchar (20) NULL,
        CurrentYear1 money NULL,
        PreviousYear2 money NULL,
        PreviousYear3 money NULL,
        PreviousYear4 money NULL,
        PreviousYear5 money NULL,
        PreviousYear6 money NULL,
        SourceID	INT	NULL
     
    )
 --print 'get details together'
INSERT INTO #tmpClmAgFacility
    SELECT --(SELECT description FROM Class_Of_Business WHERE class_of_business_id = ts.CobID),
    ts.Product,
    (SELECT p.resolved_name FROM Party p WHERE p.party_cnt = ts.AgentCnt),
    ts.Treaty,
    ts.CurrentTreaty,
    ts.RecordTypeId,
    ts.RecordType,
    @dtCurrentPeriodEnd,
    tt.code1,
    tt.code2,
    tt.code3,
    tt.code4,
    tt.code5,
    tt.code6,
    CASE WHEN tt.code1 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code2 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code3 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code4 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code5 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code6 = ts.Treaty THEN
        ts.Amount
    END, ts.sourceid
    FROM #tmpStats ts
    JOIN #tmpTreaty tt ON tt.code1 = ts.CurrentTreaty
    WHERE (@sClassOfBus = '' OR (@sClassOfBus <> '' AND ts.Product = @sClassOfBus))
    	AND (@iAgtCnt = 0 OR (@iAgtCnt <> 0 AND @iAgtCnt = ts.AgentCnt)) 


DROP TABLE #tmpStats
DROP TABLE #tmpTreaty
DROP TABLE #tmpClaims

SET NOCOUNT OFF

IF LOWER(@AgentGroupCode) = 'all'
BEGIN

PRINT 'ENTER1'

	SELECT *,
	    S.Code ComapnyCode, S.Description CompanyDesc,
	    Case @TypeOfCurrency 
	    	When 'Base' THEN C.Code 
	    	WHEN 'System' THEN @SystemcurrencyCode  
	    END CurrencyCode,
	    Case @TypeOfCurrency 
	    	WHEN 'Base' THEN C.Description
	    	WHEN 'System' THEN @Systemcurrencydesc
	    END CurrencyDesc,
	    Case @GroupbyCode 
	    	WHEN 'Branch' THEN S.Code
	    	WHEN 'Branch And Currency' THEN S.code
	    ELSe ''
	    END 'GroupByCode'
	     FROM #tmpClmAgFacility TF
	     JOIN SOURCE S ON S.source_id= tf.sourceID
		 JOIN Currency C ON C.currency_id= s.base_currency_id

END

IF LOWER(@AgentGroupCode) <> 'all'
BEGIN

PRINT 'ENTER2'

	SELECT *,
	    S.Code ComapnyCode, S.Description CompanyDesc,
	    Case @TypeOfCurrency 
	    	When 'Base' THEN C.Code 
	    	WHEN 'System' THEN @SystemcurrencyCode  
	    END CurrencyCode,
	    Case @TypeOfCurrency 
	    	WHEN 'Base' THEN C.Description
	    	WHEN 'System' THEN @Systemcurrencydesc
	    END CurrencyDesc,
	    Case @GroupbyCode 
	    	WHEN 'Branch' THEN S.Code
	    	WHEN 'Branch And Currency' THEN S.code
	    ELSe ''
	    END 'GroupByCode'
	     FROM #tmpClmAgFacility TF
	     JOIN SOURCE S ON S.source_id= tf.sourceID
		 JOIN Currency C ON C.currency_id= s.base_currency_id
        --RC-- 14 Jun 2006
        WHERE Agent IN(
        select trading_name from party_agent where linked_account_group = (
        select  party_cnt from party where shortname = @AgentGroupCode) )
        --RC-- 14 Jun 2006
END    

DROP TABLE #tmpClmAgFacility

GO