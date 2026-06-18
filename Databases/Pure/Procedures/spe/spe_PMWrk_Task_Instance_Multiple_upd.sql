SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Instance_Multiple_upd'
GO

CREATE PROCEDURE spe_PMWrk_Task_Instance_Multiple_upd
	@pmwrk_task_instance_cnt INTEGER,
    	@pmuser_group_id INTEGER,
    	@user_id SMALLINT,
    	@last_modified DATETIME,
    	@modified_by_id SMALLINT
AS
BEGIN
Declare @Party_cnt INT

SELECT @party_cnt=ISNULL(p.party_cnt,0) from PMUSER u JOIN party p
on u.party_cnt=p.party_cnt
WHERE user_id=@user_id and party_type_id=3
    UPDATE PMWrk_Task_Instance SET
        pmuser_group_id = @pmuser_group_id,
        [user_id] = @user_id,
        last_modified = @last_modified,
        modified_by_id = @modified_by_id,
        party_cnt=@party_cnt
    WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt

END
GO
