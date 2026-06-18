SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Range_saa'
GO
/*************************************************************************/
/* ERWIN generated select all records ascending */
/*************************************************************************/
/*************************************************************************/
/* 1.0 03/02/98 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Range_saa
AS

SELECT
    actnumber_range_id,
    actnumber_group_id,
    code,
    description
FROM ACTNumber_Range

ORDER BY actnumber_range_id ASC

GO

