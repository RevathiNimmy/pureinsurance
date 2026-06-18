EXECUTE DDLDropProcedure 'Spu_wp_insurancefileassociates_get_keys'
GO
CREATE PROCEDURE Spu_wp_insurancefileassociates_get_keys

    @PartyCnt INT,
	@InsuranceFileCnt INT,
	@RiskId INT,
	@ClaimCnt INT,
	@DocumentRef VARCHAR(25),
	@Instance1 INT,
	@Instance2 INT,
	@Instance3 INT

AS
SELECT	Insurance_file_associates_cnt
FROM	Insurance_File_Associates IFA
Left join Insurance_File INF on INF.insurance_file_cnt=IFA.Insurance_file_cnt
WHERE	IFA.Insurance_file_cnt = @InsuranceFileCnt
AND date_attached<=INF.cover_start_date   -- Date Attached is less than or equal to the Cover From Date



GO


