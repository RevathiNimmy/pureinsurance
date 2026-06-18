SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_saa'
GO
/*************************************************************************/
/* ERWIN generated select all records ascending */
/*************************************************************************/
/*************************************************************************/
/* 1.0 03/02/98 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_saa
    @actnumber_range_id int
AS

SELECT
    actnumber_id,
    actnumber_range_id,
    allocated_at,
    user_id
FROM ACTNumber

WHERE actnumber_range_id = @actnumber_range_id
ORDER BY actnumber_id ASC

GO

