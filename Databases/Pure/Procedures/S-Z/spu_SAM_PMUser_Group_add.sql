SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_PMUser_Group_add'
GO

-- This procedure is intended for adding data in upgrade scripts, not for calling from normal code.
CREATE PROCEDURE spu_SAM_PMUser_Group_add
    @language_id smallint,
    @code char(10),
    @description varchar(255),
    @is_sys_admin_group tinyint = 0
AS BEGIN
    SET NOCOUNT ON

    -- Re-runnable check.
    IF EXISTS (SELECT NULL FROM PMUser_Group WHERE code = @code) BEGIN
        RETURN
    END

    -- Generate the caption ID.
    DECLARE @caption_id integer
    EXECUTE spu_pm_caption_id_return @language_id, @description, @caption_id OUTPUT

    -- Generate the user group ID.
    DECLARE @pmuser_group_id integer

    -- Insert the row.
    INSERT INTO PMUser_Group (
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        is_sys_admin_group
    ) VALUES (
        @caption_id,
        @code,
        @description,
        0,
        GETDATE(),
        @is_sys_admin_group
    )
	SELECT @pmuser_group_id =@@IDENTITY
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
