SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_settings_risk_code_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_settings_risk_code_sel
    @risk_code_id int
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
    FROM renewal_settings rs,
         risk_code rc
    WHERE rc.risk_group_id = rs.product_id
    AND rc.risk_code_id = @risk_code_id

END
GO


