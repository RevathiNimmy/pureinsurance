SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_renewal_for_policy'
GO


CREATE PROCEDURE spu_get_renewal_for_policy
    @insurance_file_cnt INT
AS


SELECT rs.renewal_insurance_file_cnt, rst.code, rs.renewal_status_cnt
FROM    renewal_status rs
JOIN Renewal_Status_Type rst ON rs.renewal_status_type_id = rst.renewal_status_type_id
WHERE   rs.insurance_file_cnt = @insurance_file_cnt
GO



