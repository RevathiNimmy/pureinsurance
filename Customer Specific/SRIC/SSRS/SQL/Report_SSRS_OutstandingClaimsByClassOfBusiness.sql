EXEC DDLDropProcedure spu_rptSSRS_OutstandingClaimsByClassOfBusiness
GO
CREATE PROCEDURE [dbo].[spu_rptSSRS_OutstandingClaimsByClassOfBusiness]
(
	@DateFrom DATETIME
	,@DateTo DATETIME
)AS
/*
	EXEC spu_rptSSRS_OutstandingClaimsByClassOfBusiness '01 Jan 2018','30 Jan 2019'

	sp_helptext spu_rptSSRS_OutstandingClaimsByClassOfBusiness

	select * from Claim_RI_Arrangement_Line
	select * from Claim_RI_Arrangement
	select * from transaction_type


*/
BEGIN

	DECLARE @sSQL						NVARCHAR(MAX)
	DECLARE @ClaimID					INT
	DECLARE @ClaimVersionID				INT

	DECLARE @ReinsuranceTypeEnumFAC		INT 
	DECLARE @ReinsuranceTypeEnumQUO     INT 
	DECLARE @ReinsuranceTypeEnumCOM     INT
	DECLARE @ReinsuranceTypeEnumCOI     INT
	DECLARE @ReinsuranceTypeEnumXOL     INT
	DECLARE @ReinsuranceTypeEnum001     INT 
	DECLARE @ReinsuranceTypeEnum002     INT 
	DECLARE @ReinsuranceTypeEnum003     INT 
	DECLARE @ReinsuranceTypeEnumRET     INT
	DECLARE @ReinsuranceTypeEnumFAX     INT
	DECLARE @ReinsuranceTypeEnumFAP     INT 
	DECLARE @ReinsuranceTypeEnumCAT     INT

	--Proportional--
	SELECT @ReinsuranceTypeEnumFAC = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'FAC'
	SELECT @ReinsuranceTypeEnum001 = reinsurance_type_id FROM Reinsurance_type WHERE CODE = '001'
	SELECT @ReinsuranceTypeEnum002 = reinsurance_type_id FROM Reinsurance_type WHERE CODE = '002'
	SELECT @ReinsuranceTypeEnum003 = reinsurance_type_id FROM Reinsurance_type WHERE CODE = '003'
	SELECT @ReinsuranceTypeEnumQUO = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'QUO'
	SELECT @ReinsuranceTypeEnumFAP = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'FAP'

	--Non Proportional--
	SELECT @ReinsuranceTypeEnumXOL = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'XOL'
	SELECT @ReinsuranceTypeEnumCAT = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'CAT'
	SELECT @ReinsuranceTypeEnumFAX = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'FAX'

	CREATE TABLE #LatestVersionOfClaim
	(
		MaxVersion				INT,
		BaseClaimID				INT NOT NULL PRIMARY KEY
	)
	
	CREATE TABLE  #tmpFinalREsults
	(
		Major_Class								VARCHAR(255)
		,Premum_Class							VARCHAR(255)
		,Claim_Number							VARCHAR(255)
		,Gross_Outstanding						DECIMAL(15,2)
		,Proportional_Gross_Outstanding			DECIMAL(15,2)
		,Non_Proportional_Gross_Outstanding		DECIMAL(15,2)
		,Net_Net								DECIMAL(15,2)
		,Age_Of_Claim							INT
		,Claim_Handler							VARCHAR(255)
	)

	SELECT @sSQL =		   'INSERT INTO #LatestVersionOfClaim(MaxVersion,BaseClaimID) '
	SELECT @sSQL = @sSQL + 'SELECT MAX(version_id) AS MaxVersion,base_claim_id '
	SELECT @sSQL = @sSQL + 'FROM Claim C '
	SELECT @sSQL = @sSQL + 'JOIN Insurance_File IFL ON IFL.insurance_file_cnt = C.Policy_id '
	SELECT @sSQL = @sSQL + 'WHERE C.is_dirty <> 1 '
	SELECT @sSQL = @sSQL + 'AND C.transaction_type_id = 3 '
	SELECT @sSQL = @sSQL + 'AND claim_status_ID  in (1,2,4) '
	SELECT @sSQL = @sSQL + 'AND C.last_modified_date BETWEEN ' + '''' + CONVERT(VARCHAR,@DateFrom) + '''' +' AND EOMONTH(' + ''''+ CONVERT(VARCHAR,@DateTo) + '''' + ') '
	SELECT @sSQL = @sSQL + 'GROUP BY base_claim_id '
	
	EXEC SP_EXECUTESQL @sSQL
	--Print (@sSQL)
	--Return
	
	DECLARE PayClaimCursor CURSOR FOR
	SELECT * FROM #LatestVersionOfClaim

	OPEN PayClaimCursor

	FETCH NEXT FROM PayClaimCursor
	INTO @ClaimVersionID, @ClaimID

	WHILE @@FETCH_STATUS = 0
	BEGIN

		SELECT  C.Claim_id,
		CASE WHEN DATEPART(MONTH,C.Last_modified_date) = DATEPART(MONTH,@DateTo) THEN 1
		ELSE 0 END AS 'CurrentMonth'
		INTO #ClaimIDs
		FROM claim C 
		WHERE C.base_claim_id IN (SELECT base_claim_id FROM claim WHERE claim_id =@ClaimID)
		AND CONVERT(date,C.Last_modified_date) BETWEEN @DateFrom AND EOMONTH(@DateTo)
		AND C.is_dirty=0	

		INSERT INTO #tmpFinalREsults
		SELECT COB.description
			,PT.description
			,Claim_Number
			,SUM(res.initial_reserve) + SUM(res.revised_reserve)
			,SUM(CRAL.Reserve_to_date)
			,SUM(CRAL2.Reserve_to_date)
			,SUM(res.initial_reserve) + SUM(res.revised_reserve) - SUM(res.initial_reserve) - SUM(res.revised_reserve)
			,DATEDIFF(MM,Reported_date,GetDATE())
			,PMU.full_name
		FROM Claim C
		INNER JOIN PMUser PMU ON PMU.user_id = C.Handler_id
		INNER JOIN risk R on R.risk_cnt = C.Risk_type_id
		INNER JOIN Peril P ON P.risk_cnt = R.risk_cnt
		INNER JOIN Claim_Peril CP on CP.Claim_id = c.Claim_id
		INNER JOIN Peril_Type PT on PT.peril_type_id = CP.Peril_type_id
		INNER JOIN Reserve Res on Res.claim_Peril_id = cp.Claim_Peril_id
		INNER Join Class_Of_Business COB on COB.class_of_business_id = P.class_of_business_id
		INNER JOIN Claim_RI_Arrangement CA ON C.Claim_id = CA.Claim_id

		INNER JOIN Claim_RI_Arrangement_Line CRAL ON CRAL.Claim_id = CA.Claim_id
			INNER JOIN Treaty T ON CRAL.treaty_id = T.treaty_id
			AND T.reinsurance_type_id IN(@ReinsuranceTypeEnumFAC,@ReinsuranceTypeEnum001,@ReinsuranceTypeEnum002,@ReinsuranceTypeEnum003,@ReinsuranceTypeEnumQUO,@ReinsuranceTypeEnumFAP) 

		INNER JOIN Claim_RI_Arrangement_Line CRAL2 ON CA.ri_arrangement_id = CRAL2.ri_arrangement_id
		INNER JOIN Treaty T2 ON CRAL2.treaty_id = T2.treaty_id
			AND T2.reinsurance_type_id IN(@ReinsuranceTypeEnumXOL,@ReinsuranceTypeEnumCAT,@ReinsuranceTypeEnumFAX) 

		JOIN #ClaimIDs TEMPC ON C.Claim_id = TEMPC.claim_id
		GROUP BY COB.description,PT.description,Claim_Number,PMU.full_name,Reported_date

		Drop Table #ClaimIDs

		FETCH NEXT FROM PayClaimCursor
		INTO @ClaimVersionID, @ClaimID

	END
	CLOSE PayClaimCursor
	DEALLOCATE PayClaimCursor

	SELECT * FROM #tmpFinalREsults
END
	
