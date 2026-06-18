SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_Claims_Paid_By_Major_Class_Report'
GO

CREATE PROCEDURE [dbo].[spu_Claims_Paid_By_Major_Class_Report]
	@LossDateTo DATE
AS

	
	DECLARE @sSQL				NVARCHAR(MAX)
	DECLARE @enumClosedClaim	INT = 3
	DECLARE @enumReClosedClaim	INT = 5
	DECLARE @ClaimVersionID		INT
	DECLARE @ClaimID			INT
	DECLARE @ClaimNumber		VARCHAR(255)
	DECLARE @TotalPaidYTD		INT
	DECLARE @TotalPaidCM		INT
	DECLARE @ProportionalYTD	INT
	DECLARE @ProportionalCM		INT
	DECLARE @NonProportionalYTD	INT
	DECLARE @NonProportionalCM	INT
	DECLARE @ClassOfBusinessID	INT
	DECLARE @COBDescription		VARCHAR(255)
	DECLARE @PerilTypeID	    INT
	DECLARE @PerilDescription	VARCHAR(255)
	DECLARE @LossDateFrom		DATE

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

		CREATE TABLE #ClaimPaidByClass
	(
		ClaimID					INT NOT NULL PRIMARY KEY,
		ClaimNumber				VARCHAR(30),
		ClaimVersionNumber		INT,
		TotalPaidYTD			DECIMAL,
		TotalPaidCM				DECIMAL,
		ProportionalYTD			DECIMAL,
		ProportionalCM			DECIMAL,
		NonProportionalYTD		DECIMAL,
		NonProportionalCM		DECIMAL,
		ClassOfBusinessID		INT,
		COBDescription			VARCHAR(255),
		PerilTypeID				INT,
		PerilDescription		VARCHAR(255),
		NetYTD					DECIMAL,
		NetCM					DECIMAL
	)


		CREATE TABLE #LatestVersionOfClaim
	(
		MaxVersion				INT,
		BaseClaimID				INT NOT NULL PRIMARY KEY
		
	)

	SELECT @LossDateFrom = CONVERT(DATE, CONVERT(VARCHAR,DATEPART(YYYY,@LossDateTo)) + '/01/01', 120)

	SELECT @sSQL =		   'INSERT INTO #LatestVersionOfClaim(MaxVersion,BaseClaimID) '
	SELECT @sSQL = @sSQL + 'SELECT MAX(version_id) AS MaxVersion,base_claim_id '
	SELECT @sSQL = @sSQL + 'FROM Claim C '
	SELECT @sSQL = @sSQL + 'JOIN Insurance_File IFL ON IFL.insurance_file_cnt = C.Policy_id '
	SELECT @sSQL = @sSQL + 'WHERE C.is_dirty <> 1 '
	SELECT @sSQL = @sSQL + 'AND C.transaction_type_id = 3 '
	SELECT @sSQL = @sSQL + 'AND C.last_modified_date BETWEEN ' + '''' + CONVERT(VARCHAR,@LossDateFrom) + '''' +' AND EOMONTH(' + ''''+ CONVERT(VARCHAR,@LossDateTo) + '''' + ') '
	SELECT @sSQL = @sSQL + 'GROUP BY base_claim_id '
	
	EXEC SP_EXECUTESQL @sSQL
	--Print (@sSQL)
	--Return
	
	DECLARE PayClaimCursor Cursor FOR
	SELECT * FROM #LatestVersionOfClaim

	OPEN PayClaimCursor

	FETCH NEXT FROM PayClaimCursor
	INTO @ClaimVersionID, @ClaimID

	WHILE @@FETCH_STATUS = 0
	BEGIN


		SET @ClaimNumber = ''
		SET @TotalPaidYTD = 0
		SET @TotalPaidCM = 0
		SET @ProportionalYTD = 0
		SET @ProportionalCM = 0	
		SET @NonProportionalYTD = 0
		SET @NonProportionalCM = 0
		SET @ClassOfBusinessID = 0
		SET @COBDescription = ''
		SET @PerilTypeID = 0
		SET @PerilDescription = ''

		
		SELECT  C.Claim_id,
		CASE WHEN DATEPART(MONTH,C.Last_modified_date) = DATEPART(MONTH,@LossDateTo) THEN 1
		ELSE 0 END AS 'CurrentMonth'
		INTO #ClaimIDs
		FROM claim C 
		WHERE C.base_claim_id IN (SELECT base_claim_id FROM claim WHERE claim_id =@ClaimID)
		AND CONVERT(date,C.Last_modified_date) BETWEEN @LossDateFrom AND EOMONTH(@LossDateTo)
		AND C.is_dirty=0				   

		SELECT  
		@TotalPaidYTD = SUM(R.this_payment)
		FROM reserve R
		JOIN claim_peril CP ON  R.claim_peril_id = CP.claim_peril_id  
		JOIN #ClaimIDs C ON C.Claim_id = CP.Claim_id

		SELECT  
		@TotalPaidCM = SUM(R.this_payment)
		FROM reserve R
		JOIN claim_peril CP ON  R.claim_peril_id = CP.claim_peril_id  
		JOIN #ClaimIDs C ON C.Claim_id = CP.Claim_id
		WHERE C.CurrentMonth = 1	
		
		SELECT @ProportionalYTD = SUM(CRAL.this_payment)
		FROM Claim_RI_Arrangement_Line CRAL	
		JOIN #ClaimIDs C ON C.Claim_id = CRAL.claim_id
		JOIN Treaty T ON CRAL.treaty_id = T.treaty_id
		JOIN Reinsurance_type RT ON RT.reinsurance_type_id = T.reinsurance_type_id
		WHERE RT.reinsurance_type_id IN (@ReinsuranceTypeEnumFAC,@ReinsuranceTypeEnum001,@ReinsuranceTypeEnum003,
										 @ReinsuranceTypeEnumQUO,@ReinsuranceTypeEnumFAP)

		SELECT @ProportionalCM = SUM(CRAL.this_payment)
		FROM Claim_RI_Arrangement_Line CRAL	
		JOIN #ClaimIDs C ON C.Claim_id = CRAL.claim_id
		JOIN Treaty T ON CRAL.treaty_id = T.treaty_id
		JOIN Reinsurance_type RT ON RT.reinsurance_type_id = T.reinsurance_type_id
		WHERE RT.reinsurance_type_id IN (@ReinsuranceTypeEnumFAC,@ReinsuranceTypeEnum001,@ReinsuranceTypeEnum003,
										 @ReinsuranceTypeEnumQUO,@ReinsuranceTypeEnumFAP)
		AND C.CurrentMonth = 1


		SELECT @NonProportionalYTD = SUM(CRAL.this_payment)
		FROM Claim_RI_Arrangement_Line CRAL	
		JOIN #ClaimIDs C ON C.Claim_id = CRAL.claim_id
		JOIN Treaty T ON CRAL.treaty_id = T.treaty_id
		JOIN Reinsurance_type RT ON RT.reinsurance_type_id = T.reinsurance_type_id
		WHERE RT.reinsurance_type_id IN (@ReinsuranceTypeEnumXOL,@ReinsuranceTypeEnumCAT,@ReinsuranceTypeEnumFAX)

		SELECT @NonProportionalCM = SUM(CRAL.this_payment)
		FROM Claim_RI_Arrangement_Line CRAL	
		JOIN #ClaimIDs C ON C.Claim_id = CRAL.claim_id
		JOIN Treaty T ON CRAL.treaty_id = T.treaty_id
		JOIN Reinsurance_type RT ON RT.reinsurance_type_id = T.reinsurance_type_id
		WHERE RT.reinsurance_type_id IN (@ReinsuranceTypeEnumXOL,@ReinsuranceTypeEnumCAT,@ReinsuranceTypeEnumFAX)
		AND C.CurrentMonth = 1


		SELECT 
		@ClassOfBusinessID=COB.class_of_business_id, 
		@COBDescription=COB.description,
		@PerilTypeID = PT.peril_type_id,
		@PerilDescription = PT.description
		FROM Claim_Peril CP 
		JOIN Peril_Type PT ON PT.peril_type_id = CP.Peril_type_id
		JOIN Class_Of_Business COB ON COB.class_of_business_id =  PT.class_of_business_id
		WHERE CP.Claim_id = @ClaimID

		INSERT INTO #ClaimPaidByClass(	ClaimID,ClaimNumber,ClaimVersionNumber,
										TotalPaidYTD,TotalPaidCM,
										ProportionalYTD,ProportionalCM,
										NonProportionalYTD,NonProportionalCM,
										NetYTD, NetCM,
										ClassOfBusinessID,COBDescription,
										PerilTypeID,PerilDescription)
											
		SELECT 
		@ClaimID,
		@ClaimNumber,
		@ClaimVersionID,
		ISNULL(@TotalPaidYTD,0),
		ISNULL(@TotalPaidCM,0),
		ISNULL(@ProportionalYTD,0),
		ISNULL(@ProportionalCM,0),
		ISNULL(@NonProportionalYTD,0),
		ISNULL(@NonProportionalCM,0),
		ISNULL(@TotalPaidYTD,0) - ISNULL(@ProportionalYTD,0) - ISNULL(@NonProportionalYTD,0),
		ISNULL(@TotalPaidCM,0) - ISNULL(@ProportionalCM,0) - ISNULL(@NonProportionalCM,0),
		ISNULL(@ClassOfBusinessID,0),
		@COBDescription,
		@PerilTypeID,
		@PerilDescription

		Drop Table #ClaimIDs

		FETCH NEXT FROM PayClaimCursor
		INTO @ClaimVersionID, @ClaimID
	END
CLOSE PayClaimCursor
DEALLOCATE PayClaimCursor

SELECT 
SUM(TotalPaidYTD) AS 'TotalPaidYTD', SUM(TotalPaidCM) AS 'TotalPaidCM', 
Sum(ProportionalYTD) AS 'ProportionalYTD', SUM(ProportionalCM) AS 'ProportionalCM',
SUM(NonProportionalYTD) AS 'NonProportionalYTD', SUM(NonProportionalCM) AS 'NonProportionalCM',
SUM(NetYTD) AS 'NetYTD', SUM(NetCM) 'NetCM',
ClassOfBusinessID, COBDescription,
PerilTypeID, PerilDescription
FROM #ClaimPaidByClass
GROUP BY PerilTypeID, PerilDescription, ClassOfBusinessID, COBDescription
DROP TABLE #ClaimPaidByClass



