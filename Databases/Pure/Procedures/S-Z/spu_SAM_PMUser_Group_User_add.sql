SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_PMUser_Group_User_add'
GO

-- This procedure is intended for adding data in upgrade scripts, not for calling from normal code.
CREATE PROCEDURE spu_SAM_PMUser_Group_User_add
    @pmuser_group_code char(10),
    @user_name varchar(255),
    @is_supervisor tinyint = 0
AS BEGIN
    SET NOCOUNT ON

    -- Generate the user group ID.
    DECLARE @pmuser_group_id integer
    SELECT @pmuser_group_id = pmuser_group_id FROM PMUser_Group WHERE code = @pmuser_group_code

    -- Generate the user ID.
    DECLARE @user_id integer
    SELECT @user_id = user_id FROM PMUser WHERE username = @user_name

    -- Re-runnable check.
    IF EXISTS (SELECT NULL
        FROM PMUser_Group_User
        WHERE pmuser_group_id = @pmuser_group_id
        AND user_id = @user_id) BEGIN
        RETURN
    END

    -- Generate the display sequence number.
    DECLARE @display_sequence_num integer
    SELECT @display_sequence_num = ISNULL(MAX(display_sequence_num), 0) + 1 FROM PMUser_Group_User
        WHERE pmuser_group_id = @pmuser_group_id
        AND user_id = @user_id

    -- Insert the row.
    INSERT INTO PMUser_Group_User (
        pmuser_group_id,
        user_id,
        display_sequence_num,
        is_supervisor
    ) VALUES (
        @pmuser_group_id,
        @user_id,
        @display_sequence_num,
        @is_supervisor
    )
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
