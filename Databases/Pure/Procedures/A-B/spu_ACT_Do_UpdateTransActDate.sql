SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_UpdateTransActDate'
GO


CREATE PROCEDURE spu_ACT_Do_UpdateTransActDate
    @transdetail_id int,
    @current_period_id int,
    @first_date datetime
AS

/**************************************************************************/
/* Description: Updates the period and accounting date on a transdetail */
/* if theyre posted to a closed period */
/* */
/* History: CTAF 050299 Created */
/**************************************************************************/
BEGIN
    DECLARE @transdetail_period_id int
    DECLARE @transact_date datetime
-- SELECT @transdetail_period_id = period_id
-- FROM transdetail
-- WHERE transdetail_id = @transdetail_id
    SELECT @transact_date = accounting_date
    FROM transdetail
    WHERE transdetail_id = @transdetail_id
    IF (@transact_date < @first_date)
    BEGIN
        UPDATE transdetail
        SET period_id = @current_period_id,
            accounting_date = @first_date
        WHERE transdetail_id = @transdetail_id
    END
END
GO


