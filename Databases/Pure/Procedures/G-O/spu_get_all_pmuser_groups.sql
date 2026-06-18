SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_all_pmuser_groups'
GO

CREATE PROCEDURE spu_get_all_pmuser_groups
    @effective_date datetime
AS

BEGIN
    SELECT ug.pmuser_group_id,
        ug.[code],
        ug.[description]
    FROM PMUser_Group ug
    WHERE ug.effective_date <= @effective_date
    AND ug.is_deleted = 0
END
GO
