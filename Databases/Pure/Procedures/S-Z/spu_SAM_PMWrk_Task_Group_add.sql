SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_PMWrk_Task_Group_add'
GO

-- This procedure is intended for adding data in upgrade scripts, not for calling from normal code.
CREATE PROCEDURE spu_SAM_PMWrk_Task_Group_add
    @language_id smallint,
    @code char(10),
    @description varchar(255),
    @display_icon integer = 8
AS BEGIN
    SET NOCOUNT ON

    -- Re-runnable check.
    IF EXISTS (SELECT NULL FROM PMWrk_Task_Group WHERE code = @code) BEGIN
        RETURN
    END

    -- Generate the caption ID.
    DECLARE @caption_id integer
    EXECUTE spu_pm_caption_id_return @language_id, @description, @caption_id OUTPUT

    -- Insert the row.
    INSERT INTO PMWrk_Task_Group (
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        display_icon
    ) VALUES (
        @caption_id,
        @code,
        @description,
        0,
        GETDATE(),
        @display_icon
    )
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
