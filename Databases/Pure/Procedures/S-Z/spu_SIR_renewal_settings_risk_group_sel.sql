SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_settings_risk_group_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_settings_risk_group_sel
    @risk_group_id int
AS


BEGIN
    SELECT confirm_day_num,
       housekeep_day_num,
       invite_day_num,
       lapse_day_num,
       pre_selection_day_num,
       product_id,
       quote_day_num,
       reminder_day_num,
       renew_day_num,
       selection_day_num,
       setting_id
    FROM renewal_settings
    WHERE @risk_group_id = product_id

END
GO


