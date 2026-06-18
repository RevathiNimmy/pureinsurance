SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_check_in_renewal'
GO


CREATE PROCEDURE spu_check_in_renewal
    @insurance_file_cnt INT
AS


SELECT  rs.renewal_status_type_id
FROM    renewal_status rs,
    insurance_file if1,
    insurance_file if2
WHERE   if1.insurance_file_cnt = @insurance_file_cnt
AND if2.insurance_folder_cnt = if1.insurance_folder_cnt
AND rs.insurance_file_cnt = if2.insurance_file_cnt
GO


