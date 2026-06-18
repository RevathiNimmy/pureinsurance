SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ElementExtras_del'
GO
/*************************************************************************/
/* ERWIN generated delete a record for a given key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ElementExtras_del
    @element_id int
AS

DELETE FROM ElementExtras

WHERE element_id = @element_id

GO

