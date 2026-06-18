EXEC DDLDropProcedure 'spu_GetCurrentInsuranceFile'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_GetCurrentInsuranceFile

    @InsuranceFolderCnt INT
    
AS

DECLARE @MaxVersion INT
DECLARE @MaxLatestVersion INT
DECLARE @LatestInsuranceFileTypeId INT
DECLARE @LatestInsuranceFileCnt INT

SELECT 
    @MaxVersion = MAX(policy_version)
FROM insurance_file
WHERE insurance_folder_cnt=@InsuranceFolderCnt
AND (
        insurance_file_status_id IS NULL 
        OR 
        insurance_file_status_id = 1
    )
AND (
        insurance_file_type_id = 2 
        OR 
        insurance_file_type_id = 5 
        OR 
        insurance_file_type_id = 10
    )

SELECT 
    @MaxLatestVersion = MAX(policy_version)
FROM insurance_file
WHERE insurance_folder_cnt = @InsuranceFolderCnt

SELECT 
    @LatestInsuranceFileTypeId = insurance_file_type_id,
    @LatestInsuranceFileCnt = insurance_file_cnt
FROM insurance_file
WHERE policy_version = @MaxLatestVersion
AND insurance_folder_cnt = @insurancefoldercnt

SELECT  
    i.insurance_file_cnt, 
    l.gis_policy_link_id, 
    l.gis_scheme_id,
    @LatestInsuranceFileTypeId, 
    @LatestInsuranceFileCnt,
    GS.backdated_mta_days_allowed
FROM insurance_file i
JOIN gis_policy_link l
    ON l.insurance_file_cnt = i.insurance_file_cnt
JOIN gis_scheme gs
    ON gs.gis_scheme_id = l.gis_scheme_id
WHERE i.policy_version = @Maxversion
AND i.insurance_folder_cnt = @insurancefoldercnt


GO
