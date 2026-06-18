SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_User_Quick_Start_add'
GO
CREATE PROCEDURE spe_PMWrk_User_Quick_Start_add
    @pmwrk_task_group_id int,
    @pmwrk_task_id int,
    @user_id smallint,
    @display_sequence_num int
AS
BEGIN
INSERT INTO PMWrk_User_Quick_Start (
    pmwrk_task_group_id,
    pmwrk_task_id,
    user_id,
    display_sequence_num )
VALUES (
    @pmwrk_task_group_id,
    @pmwrk_task_id,
    @user_id,
    @display_sequence_num)
END
GO

