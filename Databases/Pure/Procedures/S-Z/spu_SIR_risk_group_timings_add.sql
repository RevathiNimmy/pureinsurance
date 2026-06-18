SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_risk_group_timings_add'
GO


CREATE PROCEDURE spu_SIR_risk_group_timings_add
    @product_id int,
    @selection_day_num int,
    @invite_day_num int,
    @confirm_day_num int,
    @lapse_day_num int,
    @pre_selection_day_num int,
    @quote_day_num int,
    @reminder_day_num int,
    @setting_id int OUTPUT
AS


BEGIN
    INSERT INTO Renewal_Settings
    ( product_id, selection_day_num, invite_day_num, confirm_day_num, lapse_day_num, pre_selection_day_num, quote_day_num, reminder_day_num )
    VALUES
    ( @product_id, @selection_day_num, @invite_day_num, @confirm_day_num, @lapse_day_num, @pre_selection_day_num, @quote_day_num, @reminder_day_num )

    SELECT @setting_id = @@IDENTITY
END
GO


