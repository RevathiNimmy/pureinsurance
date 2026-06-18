SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_reportgroup_del'
GO


CREATE PROCEDURE spu_reportgroup_del
    @report_group_id int,
	@UserId INT = NULL,
	@UniqueId varchar(50) = NULL,
	@ScreenHierarchy varchar(500) = NULL
AS


/*********************************************************************************************************
** STORED PROCEDURE     : spu_reportgroup_del
** PARAMETERS           : @report_group_id int
** DESCRIPTION          : delete all records with the given report_group_id.
**
**********************************************************************************************************
** Revision             Description of Modification                             Date            Who
**
** 1                    Created for bSIRReportGroup                             17/01/2001      JMK
**********************************************************************************************************/

UPDATE Report_Group_Contents SET UserId = @UserId,UniqueId = @UniqueId,ScreenHierarchy = @ScreenHierarchy
WHERE   report_group_id = @report_group_id

DELETE FROM Report_Group_Contents
WHERE   report_group_id = @report_group_id
GO


