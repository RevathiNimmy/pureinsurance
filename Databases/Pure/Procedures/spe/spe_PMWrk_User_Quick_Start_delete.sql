SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_User_Quick_Start_delete'
GO
CREATE PROCEDURE spe_PMWrk_User_Quick_Start_delete  
    @pmwrk_task_group_id int,  
    @pmwrk_task_id int,  
    @user_id smallint  
AS  
BEGIN  
    DELETE FROM PMWrk_User_Quick_Start  
    WHERE pmwrk_task_group_id = @pmwrk_task_group_id  
    AND pmwrk_task_id = @pmwrk_task_id  
    AND user_id = @user_id;  
END
GO
