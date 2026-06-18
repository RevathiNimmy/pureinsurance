SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Range_sel'
GO
/*************************************************************************/
/* ERWIN generated select all fields for a given key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Range_sel
    @actnumber_range_id int
AS

SELECT
    actnumber_range_id,
    actnumber_group_id,
    code,
    description
FROM ACTNumber_Range

WHERE actnumber_range_id = @actnumber_range_id

GO

