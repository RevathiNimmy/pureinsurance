SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_reportgroup_ins'
GO


CREATE PROCEDURE spu_reportgroup_ins
    @report_group_id int,
    @report_id int,
	@UserId INT = NULL,
	@UniqueId varchar(50) = NULL,
	@ScreenHierarchy varchar(500) = NULL
AS


/*********************************************************************************************************
** STORED PROCEDURE     : spu_reportgroup_ins
** PARAMETERS           : @report_group_id int, @report_id int
** DESCRIPTION          : insert new records with the given report_group_id and report_id
**
**********************************************************************************************************
** Revision             Description of Modification                             Date            Who
**
** 1                    Created for bSIRReportGroup                             17/01/2001      JMK
**********************************************************************************************************/

INSERT INTO Report_Group_Contents 
		(report_group_id, report_id,UserId,UniqueId,ScreenHierarchy)
SELECT  report_group_id = @report_group_id,
        report_id = @report_id,
		UserId = @UserId,
		UniqueId = @UniqueId,
		ScreenHierarchy = @ScreenHierarchy
GO


