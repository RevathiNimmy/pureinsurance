SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_reportgroup_contents_saa'
GO


CREATE PROCEDURE spu_reportgroup_contents_saa
    @report_group_id int
AS


/*********************************************************************************************************
** STORED PROCEDURE     : spu_reportgroup_contents_saa
** PARAMETERS           : @report_group_id int
** DESCRIPTION          : select list of report ids with the given report_group_id.
**
**********************************************************************************************************
** Revision             Description of Modification                             Date            Who
**
** 1                    Created for bSIRReportGroup                             17/01/2001      JMK
**********************************************************************************************************/
SELECT  report_id
FROM    Report_Group_Contents
WHERE   report_group_id = @report_group_id
GO


