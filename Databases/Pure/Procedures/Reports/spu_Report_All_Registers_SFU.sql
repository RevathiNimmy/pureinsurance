EXECUTE DDLDropProcedure 'spu_Report_All_Registers_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 18/11/2000
** Reports  - Register reports
.
**                (Premium & Commission Register; Claim Register; Average Premium Register)
**
**********************************************************************************************************************************
** 08/08/2004	JT				Added Multicurrency Feature	
** 28/05/2003 Jon Kemp		    Added Branch Parameter to report
**
** 04/08/2003 Andrew Bibby      Replaced use of 'transaction_export_folder' with 'Document' table
**                              Reason: the transaction_export_table should not be used, it only holds data temporarily in SFU
**
** 20/08/2003 Andrew Bibby      Use of FAST_FORWARD on cursor
**
** 09/10/2003 Jude Killip		Update Agency = " Direct" if there is no Agent
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_All_Registers_SFU
    (
    @register_type     varchar(20),
    @start_date 		nvarchar(50),    
    @end_date 			nvarchar(50),
    @register_period   varchar(20),
    @branch_id         int,
    @TypeOfCurrency		Varchar(15),
    @GroupByCode		Varchar(30)
    )
AS

SET NOCOUNT ON
/*
--for testing
declare @register_type varchar (20),
    @start_date datetime,
    @end_date datetime,
    @register_period varchar (20)

--pre
--com
--cla
--ave
select @register_type = 'com',
    @start_date = dateadd(day,-55,getdate()),
    @end_date = getdate(),
    @register_period = 'This Month'
--Specify Dates
--Today
--Yesterday
--This Week
--Last Full Week
--This Month
--Last Full Month
*/
SELECT @start_date= CONVERT(DATETIME, @start_date, 103),    
    @end_date = CONVERT(DATETIME, @end_date, 103) 
	
DECLARE @sAmountType varchar (20),
	@ibranchID int

IF @branch_id IS NULL
		SELECT @iBranchID = 0
	ELSE
		SELECT @iBranchID = @branch_id

/* Amount values according to Register type
        'ThisPremium'           this_premium_home
        'Commission'            lead_commission_home + sub_commission_home
        'SumInsured'            sum_insured_home
        'AveragePremium'        (sd.this_premium_home/sd.sum_insured_home)*100
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
IF @register_type LIKE 'Com%'
    SELECT @sAmountType = 'Commission'
ELSE
    SELECT @sAmountType = 'ThisPremium'

CREATE TABLE #tempRegisters
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
        Client varchar (255) NULL,
        Agency varchar (255) NULL,
        Product varchar (255) NULL,
        RiskType varchar (255) NULL,
        StatsFolderCnt int,
        StatsDetailID int,
        StatsDetailType char(3),
        RiskTypeID int NULL,
        PerilID int NULL,
        SumInsured decimal (19,4) NULL,
        SumInsuredSingle decimal (19,4) NULL,
        Duties decimal (19,4) NULL,
        Amount1 decimal (19,4) NULL,
        Amount2 decimal (19,4) NULL,
        Amount3 decimal (19,4) NULL,
        Amount4 decimal (19,4) NULL,
        Amount5 decimal (19,4) NULL,
        SourceID	Int			NULL ,--added col for multicurrency
        CurrencyCode	 VarChar(10),  
        originalflag varchar(10) NULL,
		BaseCurrencyRate decimal (19,8) NULL,
        SystemCurrencyRate decimal (19,8) NULL    
)

-- GET Stats_Folders
-- if Claims Register, select only Claims transaction_type_codes

CREATE TABLE #tempStatsFolder
    (
        StatsFolderCnt int
    )

IF @register_type LIKE 'CLA%'
BEGIN
    --print 'start of claim insert'

    INSERT INTO #tempStatsFolder
        SELECT sf.stats_folder_cnt
        FROM Stats_Folder sf
        -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
        INNER JOIN Document AS doc 
            ON doc.document_ref = sf.document_ref
        WHERE sf.transaction_type_code LIKE ('C_%')
        AND (
        @register_period = 'specify dates' AND
            (
            datediff(day, @start_date, transaction_date) >=0
            AND datediff(day, transaction_date, @end_date) >=0
            )
        OR
        @register_period = 'yesterday' AND
        datediff (day, transaction_date, getdate())= 1
        OR
        @register_period = 'today' AND
        datediff (day, transaction_date, getdate())= 0
        OR
        @register_period = 'last full week' AND
        datediff (week, transaction_date, getdate())= 1
        OR
        @register_period = 'this week' AND
        datediff (week, transaction_date, getdate())= 0
        OR
        @register_period = 'last full month' AND
        datediff (month, transaction_date, getdate())= 1
        OR
        @register_period = 'this month' AND
        datediff (month, transaction_date, getdate())= 0
        )
	AND ( @iBranchID= 0
              or    (   @iBranchID <> 0 and sf.source_id  = @iBranchID ))
END

-- if NOT a Claims Register, only NON Claims transaction_type_codes
ELSE
BEGIN
    --print 'start of NON claim insert'

    INSERT INTO #tempStatsFolder
        SELECT sf.stats_folder_cnt
        FROM Stats_Folder sf
        -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
        INNER JOIN Document AS doc 
            ON doc.document_ref = sf.document_ref
        WHERE sf.transaction_type_code NOT LIKE ('C_%')
        AND (
        @register_period = 'specify dates' AND
            (
            datediff(day, @start_date, transaction_date) >=0
            AND datediff(day, transaction_date, @end_date) >=0
            )
        OR
        @register_period = 'yesterday' AND
        datediff (day, transaction_date, getdate())= 1
        OR
        @register_period = 'today' AND
        datediff (day, transaction_date, getdate())= 0
        OR
        @register_period = 'last full week' AND
        datediff (week, transaction_date, getdate())= 1
        OR
        @register_period = 'this week' AND
        datediff (week, transaction_date, getdate())= 0
        OR
        @register_period = 'last full month' AND
        datediff (month, transaction_date, getdate())= 1
        OR
        @register_period = 'this month' AND
        datediff (month, transaction_date, getdate())= 0
        )
	AND ( @iBranchID= 0
              or    (   @iBranchID <> 0 and sf.source_id = @iBranchID ))
END

INSERT INTO #tempRegisters
    SELECT 
        sf.insurance_ref,
        sf.loss_code,
        sf.insurance_file_cnt,
        sf.transaction_type_id,
        sf.transaction_type_code,
        sf.product_code,
        sf.transaction_date,
        sf.document_ref,
        sf.cover_start_date,
        sf.expiry_date,
        (SELECT resolved_name FROM Party WHERE shortname = sf.insurance_holder_shortname),
        (SELECT ISNULL(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
        p.description,
        rt.description,
        sd.stats_folder_cnt,
        sd.stats_detail_id,
        sd.stats_detail_type,
        sd.risk_type_id,
        sd.peril_id,
        sd.sum_insured_total,
        (SELECT
           Case @TypeOfCurrency
            WHEN 'Base' THEN          sd.sum_insured_home    
            WHEN 'System' THEN sd.sum_insured_system    
            WHEN 'Transaction' THEN ROUND(ISNULL(sd.sum_insured_home,0)/ISNULL(sd.currency_rate,1),2)    
           	END
         WHERE ((ISNULL(sd.original_flag,0)=0 AND sf.transaction_type_code <> 'MTC') OR (ISNULL(sd.original_flag,0)=1 AND sf.transaction_type_code = 'MTC'))  AND  
            sd.peril_id =
            (SELECT 
                 min(sd2.peril_id)
             FROM 
                 stats_detail sd2
             WHERE 
                 sd2.stats_folder_cnt = sd.stats_folder_cnt
             AND 
                 sd.stats_detail_type <> 'TTY' and sd.stats_detail_type <> 'FAC')),
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,sf.source_id,sd.Currency_code,sd.original_flag,
			ifi.currency_base_xrate,ifi.system_base_xrate  
    FROM 
        Stats_Detail sd
    INNER JOIN 
        Stats_Folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
    INNER JOIN 
        #tempStatsFolder sft ON sd.stats_folder_cnt = sft.StatsFolderCnt
	INNER JOIN
        insurance_file ifi ON sf.insurance_file_cnt = ifi.insurance_file_cnt
    LEFT OUTER JOIN 
        Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
    LEFT OUTER JOIN 
        Product p ON sf.product_id = p.product_id
    WHERE 
        sd.stats_detail_type IN ( 'GRS', 'TTY', 'FAC', 'COI', 'XOL', 'FAX', 'SUB','TYX','TFS')	--PN: 72519
    OR  
        (sd.stats_detail_type IN ('TAG', 'TAT', 'TAF', 'TAC')
    AND 
        (SELECT ISNULL(tt.is_not_applied_to_client,0) FROM tax_type tt WHERE tt.tax_type_id in (select tax_type_id from tax_band where tax_band_id = sd.tax_type_id)) <> 1)    

IF @register_type LIKE 'CLA%'
BEGIN
--print 'Update Sum Insured for Claims'
-- Use cursor
-- Cursor variables
DECLARE @StatsFolderCnt int,
        @StatsDetailID int,
        @SumInsured decimal (19,4)

-- Additional variables
DECLARE @CountPerilTypeId int,
        @SumInsuredSingle decimal (19,4)

DECLARE Stats_cursor CURSOR FAST_FORWARD FOR
    SELECT 
        sd.stats_folder_cnt,
        sd.stats_detail_id,
        sd.sum_insured_total
    FROM 
        Stats_Detail sd
    INNER JOIN 
        #tempStatsFolder sft ON sd.stats_folder_cnt = sft.StatsFolderCnt
    WHERE 
        sd.stats_detail_type IN ('GRS', 'TTY', 'FAC', 'COI','XOL','FAX','TYX','TFS')	--PN: 72519
    OR 
        (sd.stats_detail_type IN ('TAG', 'TAT', 'TAF', 'TAC')
    AND
        (SELECT ISNULL(tt.is_not_applied_to_client,0) FROM tax_type tt WHERE tt.tax_type_id = sd.tax_type_id) <> 1)


OPEN Stats_cursor

    FETCH NEXT FROM Stats_cursor
    INTO @StatsFolderCnt,
        @StatsDetailID,
        @SumInsured

    WHILE @@FETCH_STATUS = 0
    BEGIN

        SELECT @CountPerilTypeId = (SELECT Count(DISTINCT peril_type_id)
                        FROM stats_detail
                        WHERE stats_folder_cnt = @StatsFolderCnt
                        GROUP BY stats_folder_cnt)

        IF ISNULL(@CountPerilTypeId,0) = 0 SELECT @CountPerilTypeId = 1

        SELECT @SumInsuredSingle = @SumInsured/@CountPerilTypeId

        UPDATE #tempRegisters
        SET SumInsuredSingle = @SumInsuredSingle
        WHERE StatsFolderCnt = @StatsFolderCnt
        AND StatsDetailID = @StatsDetailID

        --print 'debug'
        --SELECT @StatsFolderCnt 'StatsFolderCnt', @SumInsured 'SumInsured', @CountPerilTypeId '@CountPerilTypeId', @SumInsuredSingle 'SumInsuredSingle'

        FETCH NEXT FROM Stats_cursor
        INTO @StatsFolderCnt,
            @StatsDetailID,
            @SumInsured

    END

CLOSE Stats_cursor
DEALLOCATE Stats_cursor
END

DROP TABLE #tempStatsFolder

--print 'Update Amount1, Amount2, Amount3, Amount4 '
-- This Premium: non claims change the sign of TTY and FAC
-- This Premium: claims leave the sign alone

IF @sAmountType = 'ThisPremium' AND @register_type NOT LIKE 'Cla%'
BEGIN
    --print 'Update Premium Records - ' + @sAmountType
    UPDATE #tempRegisters
    --Changed  col for multicurrency
        SET Amount1 = (SELECT Case @TypeOfCurrency 
        						When 'Base' Then sd.this_premium_home 
        						When 'System' Then sd.this_premium_system
        						WHEN 'Transaction' THEN sd.this_premium_original
        					  END
        				WHERE sd.stats_detail_type = 'GRS'),
            Amount2 = (SELECT Case @TypeOfCurrency 
        						When 'Base' Then -sd.this_premium_home 
        						When 'System' Then -sd.this_premium_system
        						WHEN 'Transaction' THEN -sd.this_premium_original
        					  END WHERE sd.stats_detail_type = 'TTY'),
            Amount3 = (SELECT Case @TypeOfCurrency 
        						When 'Base' Then -sd.this_premium_home 
        						When 'System' Then -sd.this_premium_system
        						WHEN 'Transaction' THEN -sd.this_premium_original
        					  END WHERE sd.stats_detail_type = 'FAC'),
            Amount4 = (SELECT Case @TypeOfCurrency 
        						When 'Base' Then sd.this_premium_home 
        						When 'System' Then sd.this_premium_system
        						WHEN 'Transaction' THEN sd.this_premium_original
        					  END WHERE sd.stats_detail_type = 'COI'),
           Amount5 = (SELECT Case @TypeOfCurrency						          						
								When 'Base' Then sd.this_premium_home
						  		When 'System' Then sd.this_premium_system
						        WHEN 'Transaction' THEN sd.this_premium_original
        					  END WHERE sd.stats_detail_type = 'XOL' OR sd.stats_detail_type = 'FAX') --PN: 72519
        --END ---added col for multicurrency
        FROM  #tempRegisters
        JOIN Stats_Detail sd ON (sd.stats_folder_cnt = StatsFolderCnt AND sd.stats_detail_id = StatsDetailID)

END
ELSE IF @sAmountType = 'ThisPremium' AND @register_type LIKE 'Cla%'
BEGIN
    --print 'Update Premium Records - ' + @sAmountType
    UPDATE #tempRegisters
    --Changed  col for multicurrency
        SET Amount1 = (SELECT CASE WHEN sf.transaction_type_code IN ('C_SA', 'C_RV') 
                            THEN
                            Case @TypeOfCurrency 
        						When 'Base' Then -sd.this_premium_home 
        						When 'System' Then -sd.this_premium_system
        						WHEN 'Transaction' THEN -sd.this_premium_original
        					  END 
        					 ELSE 
        					  Case @TypeOfCurrency 
        						When 'Base' Then sd.this_premium_home 
        						When 'System' Then sd.this_premium_system
        						WHEN 'Transaction' THEN sd.this_premium_original
        					  END 
        					 END WHERE sd.stats_detail_type = 'GRS'),
--For Claims reserves use the figure as it comes from the database 
--For Claims Payments & Salvages use -1 * the figure as it comes from the database 
            Amount2 = (SELECT CASE WHEN sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP') 
                            THEN  
                            Case @TypeOfCurrency 
        						When 'Base' Then sd.this_premium_home 
        						When 'System' Then sd.this_premium_system
        						WHEN 'Transaction' THEN sd.this_premium_original
        					 END
							ELSE 
								CASE WHEN sd.stats_version < 13 THEN
									Case @TypeOfCurrency 
										When 'Base' Then -sd.this_premium_home 
										When 'System' Then -sd.this_premium_system
										WHEN 'Transaction' THEN -sd.this_premium_original
        					  		END
								ELSE
									Case @TypeOfCurrency 
										When 'Base' Then -sd.this_premium_home 
										When 'System' Then -sd.this_premium_system
										WHEN 'Transaction' THEN -sd.this_premium_original
        					  		END
								END
							END 
                WHERE (sd.stats_detail_type = 'TTY' OR sd.stats_detail_type = 'TFS') AND sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP', 'C_RV', 'C_SA')),

            Amount3 = (SELECT CASE WHEN sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP') 
                            THEN 
                            Case @TypeOfCurrency 
								When 'Base' Then sd.this_premium_home 
								When 'System' Then sd.this_premium_system
								WHEN 'Transaction' THEN sd.this_premium_original
        					END
							ELSE
								CASE WHEN sd.stats_version < 13 THEN
									Case @TypeOfCurrency 
										When 'Base' Then -sd.this_premium_home 
										When 'System' Then -sd.this_premium_system
										WHEN 'Transaction' THEN -sd.this_premium_original
        					  		END
								ELSE
									Case @TypeOfCurrency 
										When 'Base' Then -sd.this_premium_home 
										When 'System' Then -sd.this_premium_system
										WHEN 'Transaction' THEN -sd.this_premium_original
        					  		END
								END 
							END
                WHERE sd.stats_detail_type = 'FAC' AND sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP', 'C_RV', 'C_SA')),

            Amount4 = (SELECT CASE WHEN sf.transaction_type_code IN ('C_CO', 'C_CR') 
                            THEN sd.this_premium_home
							ELSE
								CASE WHEN sd.stats_version < 13 THEN
									Case @TypeOfCurrency 
										When 'Base' Then -sd.this_premium_home 
										When 'System' Then -sd.this_premium_system
										WHEN 'Transaction' THEN -sd.this_premium_original
        					  		END 
								ELSE
									Case @TypeOfCurrency 
										When 'Base' Then sd.this_premium_home 
										When 'System' Then sd.this_premium_system
										WHEN 'Transaction' THEN sd.this_premium_original
        					  		END
								END
							END 
		--END---Changed col for multicurrency
                WHERE sd.stats_detail_type = 'COI' AND sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP', 'C_RV', 'C_SA')),
                Amount5 = (SELECT CASE WHEN sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP') 
		                            THEN
										CASE WHEN sd.stats_version < 13 THEN
											Case @TypeOfCurrency 
												When 'Base' Then -sd.this_premium_home 
												When 'System' Then -sd.this_premium_system
												WHEN 'Transaction' THEN -sd.this_premium_original
		        					  		END 
										ELSE
											Case @TypeOfCurrency 
												When 'Base' Then sd.this_premium_home 
												When 'System' Then sd.this_premium_system
												WHEN 'Transaction' THEN sd.this_premium_original
		        					  		END
										END
									END 
				--END---Changed col for multicurrency
				--PN: 72519
                WHERE (sd.stats_detail_type = 'XOL' OR sd.stats_detail_type = 'FAX' OR sd.stats_detail_type = 'TYX') AND sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP', 'C_RV', 'C_SA'))
        FROM  #tempRegisters
        JOIN Stats_Detail sd ON (sd.stats_folder_cnt = StatsFolderCnt AND sd.stats_detail_id = StatsDetailID)
        JOIN Stats_Folder sf ON sf.Stats_Folder_cnt = sd.Stats_Folder_cnt
END
ELSE IF @sAmountType = 'Commission'
BEGIN

    --print 'Update Gross Records - com - ' + @sAmountType
    UPDATE #tempRegisters
     --Changed  col for multicurrency
        SET Amount1 = (SELECT CASE @TypeOfCurrency WHEN 'Base'
        				THEN ISNULL(sd.lead_commission_value_home,0)
        				WHEN 'System' 
        				THEN ISNULL(sd.lead_commission_value_system,0) 
        				WHEN 'Transaction' THEN ROUND(ISNULL(lead_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
        				END
        				+
        				CASE @TypeOfCurrency 
        					WHEN 'Base' THEN ISNULL(sd.sub_commission_value_home,0)
                        	WHEN 'System' THEN ISNULL(sd.sub_commission_value_system,0)
                        	WHEN 'Transaction' THEN ROUND(ISNULL(sub_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
                        END
                        WHERE sd.stats_detail_type = 'GRS' or sd.stats_detail_type = 'SUB'),
            Amount2 = (SELECT(
                            CASE @TypeOfCurrency 
                            	WHEN 'Base' THEN ISNULL(sd.lead_commission_value_home,0) 
                            	WHEN 'System' THEN ISNULL(sd.lead_commission_value_system,0) 
                            	WHEN 'Transaction' THEN ROUND(ISNULL(lead_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
                            END
                            +
                            CASE @TypeOfCurrency 
                            	WHEN 'Base' THEN ISNULL(sd.sub_commission_value_home,0)
                            	WHEN 'System' THEN ISNULL(sd.sub_commission_value_system,0)
                            	WHEN 'Transaction' THEN ROUND(ISNULL(sub_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
                            END
                            )* -1
                    WHERE sd.stats_detail_type = 'TTY'),
            Amount3 = (SELECT(
            				CASE @TypeOfCurrency WHEN 'Base' THEN
                            	ISNULL(sd.lead_commission_value_home,0)
                            	WHEN 'System' THEN ISNULL(sd.lead_commission_value_system,0)
                            	WHEN 'Transaction' THEN ROUND(ISNULL(lead_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
                            END
                            +
                            CASE @TypeOfCurrency 
                            	WHEN 'Base' THEN ISNULL(sd.sub_commission_value_home,0)
								WHEN 'System' THEN ISNULL(sd.sub_commission_value_system,0)
							    WHEN 'Transaction' THEN ROUND(ISNULL(sub_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
                            END
                            ) * -1
                    WHERE sd.stats_detail_type = 'FAC'),
            Amount4 = (SELECT 
            		CASE @TypeOfCurrency 
            			WHEN  'Base' 	THEN ISNULL(sd.lead_commission_value_home,0)
            			WHEN  'System' 	THEN ISNULL(sd.lead_commission_value_system,0)
            			WHEN 'Transaction' THEN ROUND(ISNULL(lead_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
            		END
            		+
            		CASE @TypeOfCurrency 
                    	WHEN  'Base' THEN	ISNULL(sd.sub_commission_value_home,0)
                    	WHEN  'System' 	THEN ISNULL(sd.sub_commission_value_SYSTEM,0)
                    	WHEN 'Transaction' THEN ROUND(ISNULL(sub_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
                    END
                    	WHERE sd.stats_detail_type = 'COI'),
            Amount5 = (SELECT
	                		CASE @TypeOfCurrency
	                			WHEN  'Base' 	THEN ISNULL(sd.lead_commission_value_home,0)
	                			WHEN  'System' 	THEN ISNULL(sd.lead_commission_value_system,0)
	                			WHEN 'Transaction' THEN ROUND(ISNULL(lead_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
	                		END
	                		+
	                		CASE @TypeOfCurrency
	                        	WHEN  'Base' THEN	ISNULL(sd.sub_commission_value_home,0)
	                        	WHEN  'System' 	THEN ISNULL(sd.sub_commission_value_SYSTEM,0)
	                        	WHEN 'Transaction' THEN ROUND(ISNULL(sub_commission_value_home,0)/ISNULL(sd.currency_rate,1),2)
	                        END
                    	WHERE sd.stats_detail_type = 'XOL' OR sd.stats_detail_type = 'FAX' )	--PN: 72519
         --END--------Changed  col for multicurrency
        FROM  
            #tempRegisters
        INNER JOIN 
            Stats_Detail sd ON (sd.stats_folder_cnt = StatsFolderCnt AND sd.stats_detail_id = StatsDetailID)

END

--print 'Update Tax values - but not for Commission Register'
IF @register_type NOT LIKE 'Com%'
BEGIN
    --print 'update TAX ' + @sAmountType
    
    --Now Tax is being picked up from Tax_Calculation table
    
    	UPDATE #tempRegisters
         SET Duties = CASE @TypeOfCurrency
                     WHEN 'Base' THEN
                              (SELECT SUM(value) FROM tax_calculation tc  
                              WHERE tc.insurance_file_cnt = tr.InsFileCnt AND transtype <> 'TTAC') * BaseCurrencyRate 
                     WHEN 'System' THEN
                              ((SELECT SUM(value) FROM tax_calculation tc  
                              WHERE tc.insurance_file_cnt = tr.InsFileCnt AND transtype <> 'TTAC') * BaseCurrencyRate) / SystemCurrencyRate
                     WHEN 'Transaction' THEN
                              (SELECT SUM(value) FROM tax_calculation tc  
                              WHERE tc.insurance_file_cnt = tr.InsFileCnt AND transtype <> 'TTAC')  
                     END 
        FROM  
            #tempRegisters tr
        WHERE 
            StatsDetailType='TAG' AND
    		StatsDetailID = (SELECT MIN(tr1.StatsDetailID) 
    			 FROM #tempRegisters tr1 
    			 WHERE tr1.InsFileCnt=tr.InsFileCnt 
    			 AND tr1.StatsDetailType='TAG') 
            
            UPDATE #tempRegisters
                SET RiskType= (SELECT TOP 1 TR.RiskType 
            			FROM #tempRegisters TR
            			WHERE TR.StatsFolderCnt=temp_r.StatsFolderCnt AND TR.RiskType IS NOT NULL
            			GROUP BY TR.RiskType),
            	RiskTypeID= (SELECT TOP 1 TR.RiskTypeID 
            			FROM #tempRegisters TR
            			WHERE TR.StatsFolderCnt=temp_r.StatsFolderCnt AND TR.RiskTypeID IS NOT NULL
            			GROUP BY TR.RiskTypeID)
            FROM #tempRegisters temp_r
        WHERE RiskType IS NULL
        
        
END

UPDATE #tempRegisters
	SET Agency = ' Direct'
	WHERE isnull(Agency,'') = ''

UPDATE #tempRegisters    
   SET Amount1= ISNULL(Amount1,0),  
    Amount2= ISNULL(Amount2,0),  
    Amount3= ISNULL(Amount3,0),  
    Amount4= ISNULL(Amount4,0),  
    Amount5= ISNULL(Amount5,0),  
    Duties= ISNULL(Duties,0),  
    SumInsuredSingle = ISNULL(SumInsuredSingle,0),  
 SumInsured = ISNULL(SumInsured,0),  
 BaseCurrencyRate = ISNULL(BaseCurrencyRate,0),  
 SystemCurrencyRate = ISNULL(SystemCurrencyRate,0)  

SET NOCOUNT OFF


--print 'filter out zero values'
SELECT *,S.Code CompanyCode,S.description CompanyDesc
 	--Changed  col for multicurrency
    ,Case @typeOfCurrency 
    	When 'Base'  THEN CB.Code 
    	When 'System'  THEN @SystemCurrencycode 
    	WHEN 'Transaction' THEN CT.Code
     END CurrencyCode1,
    Case @TypeOfCurrency 
    	WHEN 'Base' THEN CB.description 
    	WHEN 'System' THEN @systemCurrencyDesc
    	WHEN 'Transaction' THEN CT.Description
    END CurrecnyDesc,
    Case @GroupByCode 
    	When 'Branch' THEN S.Code
    	When 'Branch And Currency' THEN S.Code
    	WHEN 'Currency' THEN CT.Code
    else ''
    END GroupBycode
FROM 
    #tempRegisters TR
    Inner join Source S ON S.Source_id = TR.SourceId
    inner join Currency CB ON S.base_Currency_id = CB.Currency_id
    Inner Join Currency CT ON CT.iso_code = TR.CurrencyCode /*For Transaction currency*/
WHERE 
    (
    ISNULL(Amount1,0) <> 0
OR  ISNULL(Amount2,0) <> 0
OR  ISNULL(Amount3,0) <> 0
OR  ISNULL(Amount4,0) <> 0
OR  ISNULL(Amount5,0) <> 0
OR  ISNULL(Duties,0) <> 0
    )

DROP TABLE #tempRegisters
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
