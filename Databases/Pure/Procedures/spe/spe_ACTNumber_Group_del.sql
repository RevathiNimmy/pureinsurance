SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Group_del'
GO
/*************************************************************************/
/* ERWIN generated delete a record for a given key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Group_del
    @actnumber_group_id int
AS

DELETE FROM ACTNumber_Group

WHERE actnumber_group_id = @actnumber_group_id

GO

