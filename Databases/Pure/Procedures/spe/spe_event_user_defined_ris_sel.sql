SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_user_defined_ris_sel'
GO

CREATE PROCEDURE spe_event_user_defined_ris_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    defined_risk_data_id,
    instance,
    value
 FROM event_user_defined_risk_data
WHERE insurance_file_cnt = @insurance_file_cnt

GO

