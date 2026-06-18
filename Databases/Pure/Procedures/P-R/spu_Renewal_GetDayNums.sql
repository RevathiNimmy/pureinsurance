SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Renewal_GetDayNums'
GO

CREATE PROCEDURE spu_Renewal_GetDayNums
AS
/*
    Description : Stored procedure to retrieve Renewal settings based on Scheme, Risk Code and Default type
                  from gis_scheme, insurance_file and renewal settings.
    History : Originally by SSL
    030401 - CTAF - Changed to populate actual dates
    030401 - SSL - Modified to populate number of days and now based on
                    Scheme, Risk Code and Default. This process iterate all GIS_Scheme record for each
                    Risk_code record.
*/
BEGIN
    DECLARE @g_pre_selection_day int
    DECLARE @g_selection_day int
    DECLARE @g_invite_day int
    DECLARE @g_confirm_day int
    DECLARE @g_lapse_day int
    DECLARE @g_quote_day int
    DECLARE @g_reminder_day int
    DECLARE @g_renew_day int
    DECLARE @g_housekeep_day int

    DECLARE @product_id int
    DECLARE @gis_scheme_id int

    DECLARE @l_pre_selection_day int
    DECLARE @l_selection_day int
    DECLARE @l_invite_day int
    DECLARE @l_confirm_day int
    DECLARE @l_lapse_day int
    DECLARE @l_quote_day int
    DECLARE @l_reminder_day int
    DECLARE @l_renew_day int
    DECLARE @l_housekeep_day int

    DECLARE @pre_selection_day_num int
    DECLARE @selection_day_num int
    DECLARE @invite_day_num int
    DECLARE @confirm_day_num int
    DECLARE @lapse_day_num int
    DECLARE @quote_day_num int
    DECLARE @reminder_day_num int
    DECLARE @renew_day_num int
    DECLARE @housekeep_day_num int

    SET NOCOUNT ON

    DELETE FROM Renewal_Scheme_Risk

    /* Get the global defaults */
    SELECT
        @g_pre_selection_day = rs.pre_selection_day_num,
        @g_selection_day = rs.selection_day_num,
        @g_invite_day = rs.invite_day_num,
        @g_confirm_day = rs.confirm_day_num,
        @g_lapse_day = rs.lapse_day_num,
        @g_quote_day = rs.quote_day_num,
        @g_reminder_day = rs.reminder_day_num,
        @g_renew_day = rs.renew_day_num,
        @g_housekeep_day = rs.housekeep_day_num
        FROM renewal_settings AS rs
        WHERE rs.product_id = -1

    /* Iterate through the risk_groups first based on Risk_Code table */
    DECLARE cSettingsCursor CURSOR FAST_FORWARD FOR
        SELECT product_id,
        pre_selection_day_num,
        selection_day_num,
        invite_day_num,
        confirm_day_num,
        lapse_day_num,
        quote_day_num,
        reminder_day_num,
        renew_day_num,
        housekeep_day_num
        FROM Renewal_Settings
        INNER JOIN Risk_Code ON Renewal_Settings.product_id = Risk_Code.risk_code_id

    /* Open the cursor */
    OPEN cSettingsCursor

    /* Get the first record from Renewal Settings cursor */
    FETCH NEXT FROM cSettingsCursor INTO
        @product_id, @pre_selection_day_num, @selection_day_num, @invite_day_num,
        @confirm_day_num, @lapse_day_num, @quote_day_num, @reminder_day_num,
        @renew_day_num, @housekeep_day_num

    WHILE @@FETCH_STATUS = 0 BEGIN
        /* For each day, if it's NULL then we use the global setting, otherwise we take the days */

        /* Pre Selection */
        SELECT @l_pre_selection_day =
        CASE
            WHEN (@pre_selection_day_num IS NULL) THEN
                @g_pre_selection_day
            ELSE
                @pre_selection_day_num
        END

        /* Selection */
        SELECT @l_selection_day =
        CASE
            WHEN (@selection_day_num IS NULL) THEN
                @g_selection_day
            ELSE
                @selection_day_num
        END

        /* Invite */
        SELECT @l_invite_day =
        CASE
            WHEN (@invite_day_num IS NULL) THEN
                @g_invite_day
            ELSE
                @invite_day_num
        END

        /* Confirm */
        SELECT @l_confirm_day =
        CASE
            WHEN (@confirm_day_num IS NULL) THEN
                @g_confirm_day
            ELSE
                @confirm_day_num
        END

        /* Lapse */
        SELECT @l_lapse_day =
        CASE
            WHEN (@lapse_day_num IS NULL) THEN
                @g_lapse_day
            ELSE
                @lapse_day_num
        END

        /* Quote */
        SELECT @l_quote_day =
        CASE
            WHEN (@quote_day_num IS NULL) THEN
                @g_quote_day
            ELSE
                @quote_day_num
        END

        /* Reminder */
        SELECT @l_reminder_day =
        CASE
            WHEN (@reminder_day_num IS NULL) then
                @g_reminder_day
            ELSE
                @reminder_day_num
        END

        /* Renew */
        SELECT @l_renew_day =
        CASE
            WHEN (@renew_day_num IS NULL) then
                @g_renew_day
            ELSE
                @renew_day_num
        END

        /* Housekeep */
        SELECT @l_housekeep_day =
        CASE
            WHEN (@housekeep_day_num IS NULL) then
                @g_housekeep_day
            ELSE
                @housekeep_day_num
        END

        /* Now iterrate through the gis_scheme table */
        DECLARE cGISCursor CURSOR FAST_FORWARD FOR
            SELECT gis_scheme_id,
            pre_selection_day_num,
            selection_day_num,
            invite_day_num,
            confirm_day_num,
            lapse_day_num,
            quote_day_num,
            reminder_day_num,
            renew_day_num,
            housekeep_day_num
            FROM GIS_Scheme

        /* Open the cursor */
        OPEN cGISCursor

        /* Get the first record from GIS_Scheme cursor */
        FETCH NEXT FROM cGISCursor INTO
            @gis_scheme_id, @pre_selection_day_num, @selection_day_num, @invite_day_num,
            @confirm_day_num, @lapse_day_num, @quote_day_num, @reminder_day_num,
            @renew_day_num, @housekeep_day_num

        WHILE @@FETCH_STATUS = 0 BEGIN

            /* Pre Selection */
            IF (@pre_selection_day_num IS NOT NULL) BEGIN
                SELECT @l_pre_selection_day = @pre_selection_day_num
            END

            /* Selection */
            IF (@selection_day_num IS NOT NULL) BEGIN
                SELECT @l_selection_day = @selection_day_num
            END

            /* Invite */
            IF (@invite_day_num IS NOT NULL) BEGIN
                SELECT @l_invite_day = @invite_day_num
            END

            /* Confirm */
            IF (@confirm_day_num IS NOT NULL) BEGIN
                SELECT @l_confirm_day = @confirm_day_num
            END

            /* Lapse */
            IF (@lapse_day_num IS NOT NULL) BEGIN
                SELECT @l_lapse_day = @lapse_day_num
            END

            /* Quote */
            IF (@quote_day_num IS NOT NULL) BEGIN
                SELECT @l_quote_day = @quote_day_num
            END

            /* Reminder */
            IF (@reminder_day_num IS NOT NULL) BEGIN
                SELECT @l_reminder_day = @reminder_day_num
            END

            /* Renew */
            IF (@renew_day_num IS NOT NULL) BEGIN
               SELECT @l_renew_day = @renew_day_num
            END

            /* Housekeep */
            IF (@housekeep_day_num IS NOT NULL) BEGIN
                SELECT @l_housekeep_day = @housekeep_day_num
            END

            /* Insert it into the temporary table */
            INSERT INTO Renewal_Scheme_Risk
            ( gis_scheme_id, risk_code_id, pre_selection_day, selection_day, invite_day, confirm_day, lapse_day,
              quote_day, reminder_day, renew_day, housekeep_day )
            VALUES
            ( @gis_scheme_id, @product_id, @l_pre_selection_day, @l_selection_day, @l_invite_day, @l_confirm_day, @l_lapse_day,
              @l_quote_day, @l_reminder_day, @l_renew_day, @l_housekeep_day )

            /* Get the next record GIS_Scheme record*/
            FETCH NEXT FROM cGISCursor INTO
                @gis_scheme_id, @pre_selection_day_num, @selection_day_num, @invite_day_num,
                @confirm_day_num, @lapse_day_num, @quote_day_num, @reminder_day_num,
                @renew_day_num, @housekeep_day_num
        END

        /* Close the cursor */
        CLOSE cGISCursor

        /* Remove it from memory */
        DEALLOCATE cGISCursor

        /* Get the next record from Renewal Settings*/
        FETCH NEXT FROM cSettingsCursor INTO
            @product_id, @pre_selection_day_num, @selection_day_num, @invite_day_num,
            @confirm_day_num, @lapse_day_num, @quote_day_num, @reminder_day_num,
            @renew_day_num, @housekeep_day_num

    END

    CLOSE cSettingsCursor

    DEALLOCATE cSettingsCursor

    SET NOCOUNT OFF

    /* Return the contents of the table */
    SELECT * FROM Renewal_Scheme_Risk

END
GO

