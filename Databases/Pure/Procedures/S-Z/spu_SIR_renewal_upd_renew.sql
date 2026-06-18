SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_upd_renew'
GO


CREATE PROCEDURE spu_SIR_renewal_upd_renew
    @insurance_folder_cnt Int,
    @renewal_gis_scheme_id int,
    @renewal_insurance_file_cnt int,
    @renewal_status_type char(10)
AS

/* AK 170501 To update the status of a renewal control record with renewal-scheme-id and insurance file count */
BEGIN
    UPDATE Renewal_Control
        SET renewal_gis_scheme_id = @renewal_gis_scheme_id,
            renewal_insurance_file_cnt = @renewal_insurance_file_cnt,
            renewal_status_type_id = s.renewal_status_type_id
        FROM renewal_control r, renewal_status_type s
        WHERE r.insurance_folder_cnt = @insurance_folder_cnt
        AND s.code = @renewal_status_type
END
GO


