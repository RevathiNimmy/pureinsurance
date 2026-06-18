SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Period_Start_Date'
GO


CREATE PROCEDURE spu_ACT_Get_Period_Start_Date
    @period_id int,
    @period_start_date datetime OUTPUT
AS

/*****************************************************************************/
/* Description: Given a period_id, it will return the start date for that */
/* period. */
/* History: CTAF 040299 - Created */
/* DD 31/07/2002: Alter for updated spu_ACT_Get_Previous_Period_Id */
/*****************************************************************************/
BEGIN
    DECLARE @previous_period_id int
    DECLARE @next_period_id int
    DECLARE @period_end_date datetime
    DECLARE @next_period_end_date datetime
    DECLARE @period_size int
    /* Get the end date of the period passed */
    SELECT @period_end_date = (SELECT period_end_date FROM period WHERE period_id = @period_id)
    IF (@period_id = 1)
    BEGIN
        /* Cant use the next method if the period is the first period. Instead calculate
           size of periods and subtract from the end date.
        */
        /* Get the next period */
        EXEC spu_ACT_Get_Next_Period_Id @period_id = @period_id,
                        @next_period_id = @next_period_id OUTPUT,
                        @period_end_date = @period_end_date
        /* Get the period end date for the next period */
        SELECT @next_period_end_date = (SELECT period_end_date FROM period WHERE period_id = @next_period_id)
        /* Find the size of a period */
        SELECT @period_size = DATEDIFF(day, @period_end_date, @next_period_end_date)
        /* Work out the start date of the period */
        SELECT @period_start_date = DATEADD(day, @period_size * -1, @period_end_date)

    END
    ELSE
    BEGIN
        /* Get the previous period */
        EXEC spu_ACT_Get_Previous_Period_Id @period_id = @period_id,
                            @period_end_date = @period_end_date,
                            @previous_period_id = @previous_period_id OUTPUT
        /* Get the end date of the previous period */
        SELECT @period_start_date = (SELECT period_end_date FROM period WHERE period_id = @previous_period_id)
        /* Add one and thats the start date of the period passed */
        SELECT @period_start_date = DATEADD(day, 1, @period_start_date)
    END
END
GO


