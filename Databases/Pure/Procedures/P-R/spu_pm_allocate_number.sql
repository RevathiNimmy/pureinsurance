SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pm_allocate_number'
GO


CREATE PROCEDURE spu_pm_allocate_number
    @pmallocatednumber int OUTPUT,
    @pmnumber_range_id int,
    @user_id smallint,
    @range_prefix char(20) OUTPUT,
    @range_suffix char(20) OUTPUT,
    @new_range_code char(10) OUTPUT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 20/08/1997 JW */
/* 1.1 Number ranges and end dates added 20/07/2000 DAK */
/********************************************************************************************************/
BEGIN
    DECLARE @pmmaxnumber int,
    @pmoldmax int,
    @range_id int,
    @range_code char(10),
    @is_deleted tinyint,
    @effective_date datetime,
    @end_date datetime,
    @range_start int,
    @range_end int,
    @next_range_id int,
    @prefix char(20),
    @suffix char(20)

    SELECT @range_id = @pmnumber_range_id
    SELECT @pmmaxnumber = 0

    WHILE (@pmmaxnumber = 0)
    BEGIN

        if @range_id is null
            break

-- Get the PMNumber_Range details
        SELECT @range_code = code,
            @is_deleted = is_deleted,
            @effective_date = effective_date,
            @end_date = end_date,
            @range_start = range_start,
            @range_end = range_end,
            @next_range_id = next_range_id,
            @prefix = prefix,
            @suffix = suffix
        FROM PMNumber_Range
        WHERE pmnumber_range_id = @range_id

-- If it is deleted get the next one
        if @is_deleted = 1
        BEGIN
            SELECT @range_id = @next_range_id
            CONTINUE
        END

-- If this range is not effective yet, exit loop
        if @effective_date > GetDate()
            break

-- If the this range is no longer effective, get the next range record
        if @end_date is not null
        BEGIN
            if @end_date < GetDate()
            BEGIN
                SELECT @range_id = @next_range_id
                CONTINUE
            END
        END

-- Get the maximum number stored for this range
        SELECT @pmmaxnumber = max(pmnumber)
        FROM PMNumber
        WHERE PMNumber_range_id = @range_id

        /* if a counter id does not already exist,
                   give it a value of 1 otherwise add 1 to it*/
        If @pmmaxnumber is Null
            SELECT @pmmaxnumber = 1
        else
            SELECT @pmmaxnumber = @pmmaxnumber + 1

-- If the start is null or 0 then we have the correct range
        if (@range_start is null OR
            @range_start = 0)
            break

-- Check if next number is in range
        if @pmmaxnumber < @range_start
            SELECT @pmmaxnumber = @range_start

-- If the end is null or 0 then we have the correct range
        if (@range_end is null OR
            @range_end = 0)
            break

        if @pmmaxnumber > @range_end
        BEGIN
            SELECT @pmmaxnumber = 0
            SELECT @range_id = @next_range_id
            CONTINUE
        END

    END

    if @pmmaxnumber > 0
    BEGIN
        /* insert into table */
        insert into PMNumber(pmnumber_range_id,
                     pmnumber,
                     user_id)
        Values (@range_id,
            @pmmaxnumber,
            @user_id)
    END

/*
    If @pmmaxnumber is 1 greater than the maximum on the database, the new number cannot be there.
    -- if row is not inserted due to duplication, try again with incremented id (up to a maximum of 20 tries)
    while @@rowcount = 0 and (@pmmaxnumber - @pmoldmax) < 21
        begin
            -- try inserting row again
            SELECT @pmmaxnumber = @pmmaxnumber + 1
            insert into PMNumber(pmnumber_range_id,
                             pmnumber,
                         user_id)
            Values (@pmnumber_range_id,
                @pmmaxnumber,
                @user_id)
        end

    -- get the numeric id to pass back if insert was selected, else return FALSE
    if (@pmmaxnumber-@pmoldmax) < 21
        SELECT @pmallocatednumber = @pmmaxnumber
    else
        SELECT @pmallocatednumber = 0
*/
    if @@rowcount = 0
    BEGIN
        SELECT @pmallocatednumber = null
        SELECT @range_prefix = null
        SELECT @range_suffix = null
        SELECT @new_range_code = null
    END
    else
    BEGIN
        SELECT @pmallocatednumber = @pmmaxnumber
        SELECT @range_prefix = @prefix
        SELECT @range_suffix = @suffix
        SELECT @new_range_code = @range_code
    END

END
GO


