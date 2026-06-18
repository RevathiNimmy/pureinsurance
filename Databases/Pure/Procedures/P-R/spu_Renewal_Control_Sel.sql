SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Renewal_Control_Sel'
GO


CREATE PROCEDURE spu_Renewal_Control_Sel
    
    @insurance_folder_cnt INT
    
AS

SELECT 
    rc.insurance_folder_cnt,
    rc.product_id,
    rc.renewal_insurance_file_cnt,
    rc.renewal_status_type_id,
    rc.suspension_level,
    rc.renewal_edi_audit_id,
    rc.renewal_gis_scheme_id,
    rc.renewal_date,
    rc.gis_scheme_id,
    rst.code,
    rc.party_cnt,
    rc.risk_code_id,   
    rc.old_insurance_file_cnt
FROM renewal_control rc
JOIN renewal_status_type rst
    ON rst.renewal_status_type_id = rc.renewal_status_type_id
WHERE rc.insurance_folder_cnt = @insurance_folder_cnt

GO


