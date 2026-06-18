SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_event_private_motor_sel'
GO

CREATE PROCEDURE spe_event_private_motor_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    driving_restriction,
    usage,
    cover,
    excess,
    no_claims_discount_years,
    is_ncd_protected
 FROM event_private_motor
WHERE insurance_file_cnt = @insurance_file_cnt

GO

