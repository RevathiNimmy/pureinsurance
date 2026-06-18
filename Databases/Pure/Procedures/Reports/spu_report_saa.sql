SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_report_saa'
GO


CREATE PROCEDURE spu_report_saa
AS


/*********************************************************************************************************
** STORED PROCEDURE     : spu_report_saa
** PARAMETERS           : -
** DESCRIPTION          : select list of all effective maintained reports.
**
**********************************************************************************************************
** Revision             Description of Modification                             Date            Who
**
** 1                    Created for bSIRReportGroup                             17/01/2001      JMK
**********************************************************************************************************/
SELECT  report_id,
        code,
        report_name,
        description
FROM    Report
WHERE   is_deleted = 0
AND     datediff(day, effective_date, getdate()) >= 0
ORDER BY code

GO


