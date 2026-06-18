SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Pool_del'
GO
/*************************************************************************/
/* ERWIN generated delete a record for a given key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Pool_del
    @actnumber_pool_id int,
    @actnumber_range_id int,
    @company_id smallint
AS

DELETE FROM ACTNumber_Pool

WHERE actnumber_pool_id = @actnumber_pool_id AND actnumber_range_id = @actnumber_range_id
      AND company_id = @company_id

GO

