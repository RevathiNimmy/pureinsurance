SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_New_Business_Summary_Report'
GO

CREATE PROCEDURE [dbo].[spu_New_Business_Summary_Report]
	@BranchName VARCHAR(MAX),
	@PeriodDateTo DATE,
	@PeriodDateFrom DATE
AS


		CREATE TABLE #FirstVersionOfPolicy --Quote or Policy
	(
		InsuranceFileKey				INT NOT NULL PRIMARY KEY
		
	)
INSERT INTO #FirstVersionOfPolicy(InsuranceFileKey)
SELECT MIN(insurance_file_cnt) FROM Insurance_File IFL 
WHERE IFL.cover_start_date BETWEEN @PeriodDateFrom AND @PeriodDateTo
GROUP BY IFL.insurance_folder_cnt


SELECT CLB.code AS 'COBCode',CLB.description AS 'COBName' ,SUM(P.this_premium) AS 'Premium', Count(P.Peril_id) AS 'PolicyCount'
FROM Insurance_File IFL
JOIN insurance_file_risk_link IFRL ON IFL.insurance_file_cnt = IFRL.insurance_file_cnt
JOIN Peril P ON P.risk_cnt = IFRL.risk_cnt
JOIN Class_Of_Business CLB ON CLB.class_of_business_id = P.class_of_business_id
JOIN #FirstVersionOfPolicy FVoP ON FVoP.InsuranceFileKey = IFL.insurance_file_cnt
GROUP BY CLB.code, CLB.description
HAVING CLB.code <> 'STAMPDUTY'



