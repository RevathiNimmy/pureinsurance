SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Status_sel'
GO

CREATE PROCEDURE spe_Renewal_Status_sel
    @renewal_status_cnt int
AS

SELECT
    renewal_status_cnt,
    product_id,
    renewal_status_type_id,
    insurance_holder_cnt,
    insurance_file_cnt,
    lead_agent_cnt,
    created_by_id,
    date_created,
    critical_date,
    renewal_insurance_file_cnt,
    is_invite_printed

 FROM Renewal_Status

WHERE renewal_status_cnt = @renewal_status_cnt

GO

