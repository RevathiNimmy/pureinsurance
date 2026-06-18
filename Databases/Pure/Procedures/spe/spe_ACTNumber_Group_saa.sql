SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Group_saa'
GO
/*************************************************************************/
/* ERWIN generated select all records ascending */
/*************************************************************************/
/*************************************************************************/
/* 1.0 03/02/98 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Group_saa
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

ORDER BY actnumber_group_id ASC

GO

