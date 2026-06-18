SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Pool_saa'
GO
/*************************************************************************/
/* ERWIN generated select all records ascending */
/*************************************************************************/
/*************************************************************************/
/* 1.0 03/02/98 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Pool_saa
AS

SELECT
    actnumber_pool_id,
    actnumber_range_id,
    allocated_at,
    user_id
FROM ACTNumber_Pool

ORDER BY actnumber_pool_id ASC, actnumber_range_id ASC

GO

