SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_PendingRI_Report'
GO

CREATE PROCEDURE [dbo].[spu_PendingRI_Report]

AS


	CREATE TABLE #PendingRIList --Quote or Policy
	(
		PolicyNumber				VARCHAR(30),
		NameOfInsured				VARCHAR(255),
		RiskCnt						INT,
		InsuranceFileCnt			INT,
		Month						INT,
		Year						INT,
		IsDeleted					TINYINT DEFAULT 0,
		PermiumClass				CHAR(10),
		UserName					VARCHAR(255)
	)


DECLARE @PendingRI INT 
DECLARE @RiskCnt INT 
DECLARE @InsuranceFileCnt INT
DECLARE @Month INT
DECLARE @Year INT
DECLARE @PeriodID INT
DECLARE @PermiumClass CHAR(10)
DECLARE @OperatorID INT
DECLARE @UserName AS VARCHAR(255)
SELECT @PendingRI = risk_status_id FROM Risk_Status WHERE (code) = 'PENDINGRI'

INSERT INTO #PendingRIList(PolicyNumber,NameOfInsured,RiskCnt,InsuranceFileCnt)
SELECT 
IFL.Insurance_Ref AS 'PolicyNumber',
IFL.insured_name AS 'NameOfInsured',
R.risk_cnt,
IFL.insurance_file_cnt
FROM Risk R
JOIN Insurance_File_risk_Link IFRL ON IFRL.insurance_file_cnt = R.risk_cnt
JOIN Insurance_File IFL ON IFL.insurance_file_cnt = IFRL.insurance_file_cnt
WHERE R.risk_status_id = @PendingRI

DECLARE PendingRICursor CURSOR FOR
SELECT RiskCnt,InsuranceFileCnt FROM #PendingRIList

OPEN PendingRICursor

FETCH NEXT FROM PendingRICursor
INTO @RiskCnt,@InsuranceFileCnt

	WHILE @@FETCH_STATUS = 0
	BEGIN
			
			SELECT TOP 1 @PeriodID = period_id, @OperatorID = operator_id FROM TransDetail TD
			JOIN Document D ON D.document_id = TD.document_id
			WHERE D.insurance_file_cnt = @InsuranceFileCnt

			IF ISNULL(@PeriodID,0) = 0 BEGIN			
				UPDATE #PendingRIList SET IsDeleted = 1 
				WHERE InsuranceFileCnt = @InsuranceFileCnt
				AND RiskCnt = @RiskCnt
			END
			ELSE BEGIN
				SELECT @Year = DATEPART(YYYY,period_end_date),@Month = DATEPART(M,period_end_date) 
				FROM Period WHERE period_id = @PeriodID

				SELECT TOP 1 @PermiumClass = code FROM Class_Of_Business COB
				JOIN Peril P ON P.class_of_business_id = COB.class_of_business_id
				WHERE P.risk_cnt = @RiskCnt
				
				SELECT @UserName = RTRIM(username) FROM PMUser WHERE user_id = @OperatorID

				UPDATE #PendingRIList 
				SET Month = @Month,
				Year = @Year,
				PermiumClass = @PermiumClass,
				UserName = @UserName
				WHERE InsuranceFileCnt = @InsuranceFileCnt
				AND RiskCnt = @RiskCnt
			END
		FETCH NEXT FROM PendingRICursor
		INTO @RiskCnt, @InsuranceFileCnt
	END
CLOSE PendingRICursor
DEALLOCATE PendingRICursor


SELECT PolicyNumber,NameOfInsured, PermiumClass, Month, Year, UserName
FROM #PendingRIList
WHERE IsDeleted = 0



