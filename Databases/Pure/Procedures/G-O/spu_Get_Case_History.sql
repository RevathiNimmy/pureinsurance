SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Get_Case_History
GO

CREATE PROCEDURE spu_Get_Case_History
	@basecaseid Int
AS
SELECT
    c.case_id,
    (SELECT Top 1 event_date FROM event_log WHERE case_id=c.case_id ORDER BY event_cnt ASC) AS date_of_change,
    (SELECT Top 1 description FROM event_log WHERE case_id=c.case_id ORDER BY event_cnt ASC) AS description,
    cp.description AS status,
    u.username
FROM [case] c
JOIN case_progress cp
    ON c.case_progress_id = cp.case_progress_id
JOIN PMUser u
    ON u.user_id =c.user_id
WHERE
    c.base_case_id =  @basecaseid
	AND is_dirty_case=0
order by
    date_of_change

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO