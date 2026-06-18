SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Period_Dates'
GO


CREATE PROCEDURE spu_ACT_Get_Period_Dates
    @period_id_1 int,
    @period_id_2 int,
    @period_id_3 int
AS

/********************************************************************************************************/
/* Gets three period_end_date 's for the three passed period_id's */
/* */
/* Author: Christopher Field */
/* Date : 11 SEPT 1998 */
/********************************************************************************************************/
SELECT period_id, period_end_date
FROM period
WHERE period_id = @period_id_1
OR period_id = @period_id_2
OR period_id = @period_id_3
ORDER BY period_end_date ASC
GO


