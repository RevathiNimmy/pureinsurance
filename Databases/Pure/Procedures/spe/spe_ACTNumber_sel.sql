SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_sel'
GO
/*************************************************************************/
/* ERWIN generated select all fields for a given key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_sel
    @actnumber_id int,
    @actnumber_range_id int
AS

SELECT
    actnumber_id,
    actnumber_range_id,
    allocated_at,
    user_id

FROM ACTNumber

WHERE actnumber_id = @actnumber_id AND actnumber_range_id = @actnumber_range_id

GO

