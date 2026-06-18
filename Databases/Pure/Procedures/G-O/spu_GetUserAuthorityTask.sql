SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GetUserAuthorityTask'
GO

CREATE PROCEDURE spu_GetUserAuthorityTask
    @user_id int,
    @Task_code varchar(10)
As

SELECT DISTINCT T.Code
FROM 
    PMUSER U
    JOIN PMUser_Group_User UG 
    ON U.User_ID= UG.USer_ID
    JOIN pmuser_group_activity UGA 
    ON UG.PMUSER_Group_ID= UGA.PMUSER_Group_ID
    JOIN pmwrk_task_group_task TG
    ON UGA.pmwrk_task_group_id = TG.pmwrk_task_group_id
    JOIN PMWrk_Task T ON 
    T.PmWrk_task_id= TG.PmWrk_task_id
    WHERE U.USer_ID=@user_id
    AND T.code=@Task_code 





