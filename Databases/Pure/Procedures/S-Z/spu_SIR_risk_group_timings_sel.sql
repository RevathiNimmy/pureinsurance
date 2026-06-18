SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_risk_group_timings_sel'
GO


CREATE PROCEDURE spu_SIR_risk_group_timings_sel
AS


BEGIN
    /*
        If the default global values aren't there, then this is the first time it's
        being run so slap some other defaults in the database too
    */
    IF NOT EXISTS (SELECT * FROM Renewal_Settings WHERE product_id = -1)
    BEGIN

        INSERT INTO Renewal_Settings
        ( product_id, pre_selection_day_num, invite_day_num, lapse_day_num, confirm_day_num, selection_day_num, reminder_day_num, quote_day_num )
        SELECT rg.risk_group_id, 45, 14, 21, 1, 30, 7, 21
        FROM risk_group rg

        INSERT INTO Renewal_Settings
        ( product_id, pre_selection_day_num, invite_day_num, lapse_day_num, confirm_day_num, selection_day_num, reminder_day_num, quote_day_num )
        VALUES
        ( -1, 45, 14, 21, 1, 30, 7, 21 )

    END

    SELECT rs.setting_id,
              rg.risk_group_id,
              rg.code as Code,
              rg.description as Description,
              ISNULL(rs.pre_selection_day_num, -1),
              ISNULL(rs.invite_day_num, -1),
              ISNULL(rs.lapse_day_num, -1),
              ISNULL(rs.confirm_day_num, -1),
              ISNULL(rs.selection_day_num, -1),
              ISNULL(rs.reminder_day_num, -1),
              ISNULL(rs.quote_day_num, -1)
    FROM Risk_Group rg
    LEFT OUTER JOIN Renewal_Settings rs ON rg.risk_group_id = rs.product_id
    UNION
    SELECT rs.setting_id,
              rs.product_id,
              'GLOBAL' as Code,
              '(Global)' as Description,
              rs.pre_selection_day_num,
              rs.invite_day_num,
              rs.lapse_day_num,
              rs.confirm_day_num,
              rs.selection_day_num,
              rs.reminder_day_num,
              rs.quote_day_num
    FROM Renewal_Settings rs
    WHERE product_id = -1
    ORDER BY rg.description, rg.code, rg.risk_group_id

END
GO


