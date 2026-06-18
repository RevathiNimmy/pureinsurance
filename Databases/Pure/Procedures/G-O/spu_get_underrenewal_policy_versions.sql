SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_get_underrenewal_policy_versions
GO

CREATE PROCEDURE spu_get_underrenewal_policy_versions  
		@InsuranceFileCnt INT
AS  
     CREATE TABLE #UnderRenewalTemp
    (
        insurance_file_cnt            INT,
        renewal_status_cnt            INT,
        renewal_insurance_file_cnt    INT,
        anniversary_copy			TINYINT NULL   
	)
	INSERT INTO #UnderRenewalTemp
	SELECT ifi2.insurance_file_cnt, rs.renewal_status_cnt, 
		rs.renewal_insurance_file_cnt, IFLRen.anniversary_copy
	FROM Insurance_File ifi JOIN Insurance_file ifi2 
	ON ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt
	JOIN Renewal_Status rs ON ifi2.insurance_file_cnt = rs.insurance_file_cnt
	JOIN Insurance_File IFLRen ON RS.renewal_insurance_file_cnt = IFLRen.insurance_file_cnt
	WHERE ifi.insurance_file_cnt = @InsuranceFileCnt
 
	-- Add Replaced - Deferred Reinsurance and under renewal versions
	INSERT INTO #UnderRenewalTemp
	SELECT ifi.insurance_file_cnt, 0 as renewal_status_cnt, 
	ifiRen.insurance_file_cnt, ifiRen.anniversary_copy
	FROM Insurance_File ifi (NOLOCK) JOIN Insurance_file ifiRen (NOLOCK)
	ON ifi.insurance_folder_cnt = ifiRen.insurance_folder_cnt
	WHERE 
		ifi.insurance_file_cnt = @InsuranceFileCnt   
		AND ifiRen.insurance_file_type_id = 3		-- Policy under Renewal
		AND ifiRen.insurance_file_status_id = 5		-- Replaced - Deferred Reinsurance

	SELECT * FROM #UnderRenewalTemp

	DROP TABLE #UnderRenewalTemp
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO