SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_EffectiveDate_PRE'
GO

CREATE PROCEDURE spu_get_EffectiveDate_PRE
   
    @gis_policy_link_id INT,
	@PREEffectiveDateOption VARCHAR(50)
AS

SELECT 
	CASE @PREEffectiveDateOption 
		 WHEN 'COVERDATE' THEN insf.Cover_Start_Date
		 WHEN 'INCEPTIONTPI' THEN insf.Inception_Date_TPI
		 WHEN 'TRANSACTIONDATE' THEN insf.Date_issued
		 WHEN 'INCEPTIONTPI(MONTHLY)' THEN DATEADD(year, -1, insf.anniversary_date)
		 ELSE insf.Date_issued
	END
   FROM gis_policy_link gpl (nolock)
   INNER JOIN  risk r (nolock) ON gpl.risk_id = r.risk_cnt 
   INNER JOIN  Insurance_File_risk_link Irs   ON Irs.risk_cnt = r.risk_cnt 
   INNER JOIN Insurance_File insf ON insf.insurance_file_cnt = Irs.insurance_file_cnt 
   WHERE gpl.gis_policy_link_id = CAST(@gis_policy_link_id AS VARCHAR) 


