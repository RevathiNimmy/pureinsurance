SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_task_user_groups'
GO

CREATE PROCEDURE spu_get_task_user_groups
    @TaskID INT
AS
BEGIN
    SELECT	
        DISTINCT(uga.pmuser_group_id),
        ug.[description]
    FROM 
        pmwrk_task_group_task tgt, 
        pmuser_group_activity uga,
        pmuser_group ug
    WHERE
        tgt.pmwrk_task_group_id = uga.pmwrk_task_group_id
    AND uga.pmuser_group_id = ug.pmuser_group_id
    AND tgt.pmwrk_task_id = @TaskID
END
GO



