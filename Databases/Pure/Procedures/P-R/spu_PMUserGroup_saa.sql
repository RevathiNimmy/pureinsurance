SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PMUserGroup_saa'
GO


CREATE PROCEDURE spu_PMUserGroup_saa
AS


/*********************************************************************************************************
** STORED PROCEDURE     : spu_PMUserGroup_saa
** PARAMETERS           : -
** DESCRIPTION          : select list of effective PMUser Groups
**
**********************************************************************************************************
** Revision             Description of Modification                             Date            Who
**
** 1                    Created for bSIRReportGroup                             17/01/2001      JMK
**********************************************************************************************************/
SELECT  pmuser_group_id,
        code,
        description
FROM    PMUser_Group
WHERE   is_deleted = 0
AND     datediff(day, effective_date, getdate()) >= 0
GO


