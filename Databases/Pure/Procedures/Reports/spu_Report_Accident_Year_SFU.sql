SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_Accident_Year_SFU'
GO


/**********************************************************************************************************************************
** Created by Kerry Butler
** 17/09/2001
** Agency Reports -  Accident_Year.rpt
**
**********************************************************************************************************************************
** 1.1      28/11/2001   JMK     remove Reinsurer parameter and UW_Type
**                               get year from loss_date
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Accident_Year_SFU
    @start_date datetime,
    @end_date datetime,
    @BranchID	int,
    @reins_code	varchar (20),
    @TypeOfCurrency Varchar(15),
    @GroupByCode Varchar(15)
AS

SET NOCOUNT ON

-- test
/*DECLARE 
	@start_date datetime,
	@end_date datetime,
	@reins_code	varchar (20)

SELECT 	@start_date = '2004-01-01',
    	@end_date = convert(datetime, '07-31-2004'),
    	@reins_code = 'H04' --'ALL'
*/

DECLARE	@sReinsCode	varchar (20)
DECLARE	@iso_code Varchar(15)
DECLARE @description Varchar(255)
DECLARE @iBranchid	Varchar(15)

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

Select @iBranchid=Isnull(@branchid,0)

IF @reins_code = 'ALL' SELECT @reins_code = NULL
SELECT @sReinsCode = ISNULL(@reins_code, '')

CREATE TABLE #tempAccident_Year
        (LossYear               varchar (10)    NULL,
        PeriodNumber            int             NULL,
        ReinsurerName           varchar (20)    NULL,
        
        REinsurerResolvedName   varchar (100)   NULL,
        LossFromDate            datetime        NULL,
        PaidClaim               decimal (19,4)  NULL,
        Description             varchar (255)   NULL,
        StatsDetailType         varchar (3)     NULL,
        CompanyCode				Varchar(15)		Null,
        CompanyDesc				Varchar(255)	Null,
        CurencyCode				Varchar(15)		NULL,
        CurencyDesc				Varchar(255)	NULL
        )

INSERT into #tempAccident_Year
    SELECT datepart(year,c.loss_from_date),
        sf.posting_period_number,
        ri_shortname,
        CASE sd.stats_detail_type
            WHEN 'TTY' THEN
                (SELECT description FROM Treaty WHERE ri_shortname = code)
            ELSE
                (SELECT resolved_name FROM Party WHERE ri_shortname = shortname)
            END,
        c.loss_from_date,
        CASE @TypeOfCurrency 
        	When 'Base'  THEN this_premium_home
        	When 'System' THEN	this_premium_system
        END,
        LEFT(c.description,255),
        sd.stats_detail_type,s.Code CompanyCode,s.Description CompanyDesc,
        Case @TypeOfCurrency 	
        	When 'System' then @systemcurrencycode
        	When 'Base' then cu.Code
        	
        End CurrencyCode,
        Case @TypeOfCurrency 	
		     When 'System' then @systemcurrencydesc
		     When 'Base' then cu.Description
        End CurrencyDesc
    FROM stats_detail sd
    JOIN stats_folder sf ON sf.stats_folder_cnt = sd.stats_folder_cnt
    JOIN claim c ON c.claim_id = sf.loss_id
    JOIN Source S On S.source_id= sf.source_id
    JOIN Currency Cu ON cu.Currency_id= s.Base_currency_id
    
    WHERE   sf.transaction_type_code = 'C_CP'
    AND sd.stats_detail_type IN ('TTY','COI')
    AND sf.transaction_date BETWEEN @start_date AND @end_date
    AND (@sReinsCode = '' OR
    		(@sReinsCode <> '' AND sd.ri_shortname = @sReinsCode))
  -- And sf.source_id= @iBranchid

-- In case of duff data...
UPDATE #tempAccident_Year
SET  ReinsurerResolvedName = ReinsurerName
WHERE ReinsurerResolvedName IS NULL

SELECT *,
Case @GroupBycode
	When 'Branch' Then CompanyCode
	When 'Branch and Currency' Then CompanyCode
else	''
End GroupByCode
	
FROM #tempAccident_Year
DROP TABLE #tempAccident_Year


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

