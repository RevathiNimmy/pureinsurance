SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_reports_limit_by_user'
GO


CREATE PROCEDURE spu_get_reports_limit_by_user
    @UserID int
AS


/*********************************************************************************************************
** STORED PROCEDURE     : spu_get_reports_limit_by_user
** PARAMETERS           : @g_iUserID
** DESCRIPTION          : get list of reports available to current user,
**                              based on PMUser_Group for given g_iUserID
**
**********************************************************************************************************
** Revision             Description of Modification                             Date            Who
**
** 1                    Created for bSIRReportPrint                             30/01/2001      JMK
**********************************************************************************************************/

SELECT DISTINCT r.report_name, r.description
FROM report r
JOIN report_group_contents rgc ON r.report_id = rgc.report_id
JOIN report_group_user_groups rgug ON rgc.report_group_id = rgug.report_group_id
JOIN report_group rg ON rg.report_group_id = rgc.report_group_id
WHERE rgug.pmuser_group_id in
        (SELECT ugu.pmuser_group_id
        FROM pmuser_group_user ugu
        JOIN pmuser_group ug ON ug.pmuser_group_id = ugu.pmuser_group_id
        WHERE ugu.user_id = @UserID                                           -- current user
        AND ug.is_deleted = 0                                                   -- PMUser Group not deleted
        AND datediff(day, ug.effective_date, getdate()) >= 0)                   -- PMUser Group not future use
AND r.is_deleted = 0                                                            -- Report not deleted
AND datediff(day, r.effective_date, getdate()) >= 0                             -- Report not future use
AND rg.is_deleted = 0                                                           -- Report Group not deleted
AND datediff(day, rg.effective_date, getdate()) >= 0                            -- Report Group not future use
GO


