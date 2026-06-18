SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_reportgroup_usergroups_saa'
GO


CREATE PROCEDURE spu_reportgroup_usergroups_saa
    @report_group_id int
AS


/*********************************************************************************************************
** STORED PROCEDURE     : spu_reportgroup_usergroups_saa
** PARAMETERS           : @report_group_id int
** DESCRIPTION          : select list of pmuser_group_id with the given report_group_id.
**
**********************************************************************************************************
** Revision             Description of Modification                             Date            Who
**
** 1                    Created for bSIRReportGroup                             18/01/2001      JMK
**********************************************************************************************************/
SELECT  pmuser_group_id
FROM    Report_Group_User_Groups
WHERE   report_group_id = @report_group_id
GO


