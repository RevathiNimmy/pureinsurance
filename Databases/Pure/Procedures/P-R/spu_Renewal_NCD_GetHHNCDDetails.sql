SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Renewal_NCD_GetHHNCDDetails'
GO


CREATE PROCEDURE spu_Renewal_NCD_GetHHNCDDetails
    @insurance_file_cnt INT
AS


BEGIN

    SELECT
        insurance_file.insurance_folder_cnt,
        insurance_file.insurance_ref,
        qh_buildings.pol_build_ncd_yrs,
        qh_contents.pol_contents_ncd_yrs,
        giihgempolicy.giihgempolicy_id,
        ISNULL(qh_policy.pol_buildings_cover_yn, ''),
        ISNULL(qh_policy.pol_contents_cover_yn, '')
    FROM qh_buildings, qh_contents, giihgempolicy, insurance_file, qh_policy
    WHERE qh_buildings.giihgempolicy_id = giihgempolicy.giihgempolicy_id
    AND qh_contents.giihgempolicy_id = giihgempolicy.giihgempolicy_id
    AND qh_policy.giihgempolicy_id = giihgempolicy.giihgempolicy_id
    AND insurance_file.insurance_file_cnt = @insurance_file_cnt
    AND giihgempolicy.gis_policy_link_id IN
            (SELECT gis_policy_link_id
         FROM gis_policy_link
         WHERE insurance_file_cnt = @insurance_file_cnt)
END
GO


