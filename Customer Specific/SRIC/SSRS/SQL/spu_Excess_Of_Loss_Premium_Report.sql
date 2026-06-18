SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_Excess_Of_Loss_Premium_Report'
GO

CREATE PROCEDURE [dbo].[spu_Excess_Of_Loss_Premium_Report]
	@Year INT,
	@Month INT,
	@Frequency CHAR(1) --Y: Year, M: Month
AS

DECLARE @sSQL				NVARCHAR(MAX)
DECLARE @LossDateTo			DATE
DECLARE @LossDateFrom		DATE
DECLARE @InsuranceFileCnt	INT
DECLARE @RiskCnt			INT

		CREATE TABLE #InsuranceFile
	(
		InsuranceFileCnt	INT ,
		RiskCnt				INT
		
	)

		CREATE TABLE #Result
	(
		COBCode				CHAR(10),
		PerilTypeCode		CHAR(10),
		TreatyCode			CHAR(10),
		Premium				INT,
		RiskCnt				INT
		
	)

	IF UPPER(LTRIM(RTRIM(@Frequency))) = 'M'	BEGIN
		SELECT @LossDateFrom = CONVERT(DATE, CONVERT(VARCHAR,@Year) + '/'+ CONVERT(VARCHAR,@Month) +'/01', 120)
	END
	ELSE BEGIN -- Y: Year
		SELECT @LossDateFrom = CONVERT(DATE, CONVERT(VARCHAR,@Year) + '/01/01', 120)
	END
	SELECT @LossDateTo = CONVERT(DATE, CONVERT(VARCHAR,@Year) + '/'+ CONVERT(VARCHAR,@Month) +'/01', 120)

	SELECT @sSQL =		   'INSERT INTO #InsuranceFile(InsuranceFileCnt,RiskCnt) '
	SELECT @sSQL = @sSQL + 'SELECT IFL.insurance_file_cnt,R.risk_cnt '
	SELECT @sSQL = @sSQL + 'FROM Insurance_File IFL '
	SELECT @sSQL = @sSQL + 'JOIN insurance_file_risk_link IFRL ON IFRL.insurance_file_cnt = IFL.insurance_file_cnt '
	SELECT @sSQL = @sSQL + 'JOIN Risk R ON R.risk_cnt = IFRL.risk_cnt '
	SELECT @sSQL = @sSQL + 'AND IFL.cover_start_date BETWEEN ' + '''' + CONVERT(VARCHAR,@LossDateFrom) + '''' +' AND EOMONTH(' + ''''+ CONVERT(VARCHAR,@LossDateTo) + '''' + ') '
	
	EXEC SP_EXECUTESQL @sSQL

	DECLARE PolicyCursor Cursor FOR
	SELECT * FROM #InsuranceFile

	OPEN PolicyCursor

	FETCH NEXT FROM PolicyCursor
	INTO @InsuranceFileCnt, @RiskCnt

	WHILE @@FETCH_STATUS = 0
	BEGIN

		INSERT INTO #Result
		SELECT	COB.code AS 'COBCode',PT.code AS 'PerilTypeCode',T.code AS 'TreatyCode',
				Sum(RIL.premium_value) AS Premium,@RiskCnt AS RiskCnt FROM 
		Risk R
		JOIN Peril P ON P.risk_cnt = R.risk_cnt
		JOIN Peril_Type PT ON PT.peril_type_id = P.peril_type_id
		JOIN Class_Of_Business COB ON COB.class_of_business_id = P.class_of_business_id
		JOIN RI_Arrangement RI ON RI.risk_cnt = R.risk_cnt
		JOIN RI_Arrangement_Line RIL ON RIL.ri_arrangement_id = RI.ri_arrangement_id
		JOIN Treaty T ON T.treaty_id = RIL.treaty_id
		WHERE RI.original_flag = 0 
		AND RIL.type = 'TX'
		AND P.ri_band = RI.ri_band_id
		AND R.Risk_cnt  = @RiskCnt
		GROUP BY COB.code,PT.code,T.code
		
		FETCH NEXT FROM PolicyCursor
		INTO @InsuranceFileCnt, @RiskCnt
	END
	CLOSE PolicyCursor
	DEALLOCATE PolicyCursor

	SELECT COBCode,PerilTypeCode,TreatyCode,Sum(Premium) AS 'TotalPremium' FROM #Result
	GROUP BY COBCode,PerilTypeCode,TreatyCode


