SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_control_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_control_sel
AS


BEGIN
    SELECT rc.insurance_folder_cnt,
           gs.scheme_desc,
           gisins.description as 'holding_insurer',
           rc.renewal_insurance_file_cnt,
           rtrim(rst.code) as 'renewal_status',
           rst.description as 'renewal_status_description',
           isnull(rc.suspension_level, 0), /* AK 090501 to return 0 in case of null */
           rc.renewal_edi_audit_id,
           rc.renewal_gis_scheme_id,
           rc.product_id,
           rc.renewal_date as 'due_date',
           insfile.insurance_ref
    FROM renewal_control rc
    INNER JOIN renewal_status_type rst ON rc.renewal_status_type_id = rst.renewal_status_type_id
    INNER JOIN GIS_Scheme gs ON rc.gis_scheme_id = gs.gis_scheme_id
    INNER JOIN GIS_Insurer gisins ON gs.gis_insurer_id = gisins.gis_insurer_id
    INNER JOIN Insurance_Folder insfold ON rc.insurance_folder_cnt = insfold.insurance_folder_cnt
    INNER JOIN Insurance_File insfile ON insfile.insurance_folder_cnt = insfold.insurance_folder_cnt
    WHERE insfile.insurance_file_cnt = (SELECT MIN(insurance_file_cnt)
                                        FROM Insurance_File
                                        WHERE Insurance_File.insurance_folder_cnt = rc.insurance_folder_cnt)
END
GO


