SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_navigator_details_select'
GO


CREATE PROCEDURE spu_SIR_renewal_navigator_details_select
    @InsuranceFolderCnt int
AS


BEGIN
    SELECT gdm.code,
        insfile.insured_cnt
    FROM Renewal_Control rc,
        GIS_Data_Model gdm,
        Insurance_Folder insfold,
        Insurance_File insfile
    WHERE rc.renewal_gis_scheme_id = gdm.gis_data_model_id
    AND insfile.insurance_folder_cnt = insfold.insurance_folder_cnt
    AND rc.insurance_folder_cnt = @InsuranceFolderCnt
END
GO


