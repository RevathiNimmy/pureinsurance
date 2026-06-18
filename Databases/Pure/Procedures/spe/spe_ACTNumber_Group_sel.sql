SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Group_sel'
GO
/*************************************************************************/
/* ERWIN generated select all fields for a given key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Group_sel
    @actnumber_group_id int
AS

SELECT
    actnumber_group_id,
    code,
    caption_id,
    description,
    is_reset_yearly,
    is_deleted,
    effective_date
FROM ACTNumber_Group

WHERE actnumber_group_id = @actnumber_group_id

GO

