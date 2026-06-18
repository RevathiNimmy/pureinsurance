DDLDropProcedure 'spu_ICCS_1_GetAnniversaryDate'
GO

CREATE PROCEDURE spu_ICCS_1_GetAnniversaryDate
        @lPartyCnt INT,
        @lPolicyLinkId INT,
        @lPolicyBinderId INT,
        @lInsuranceFileCnt INT,
        @lInsuranceFolderCnt INT
 
AS
    SELECT anniversary_date 
	FROM Insurance_File
	WHERE insurance_file_cnt = @lInsuranceFileCnt

GO

