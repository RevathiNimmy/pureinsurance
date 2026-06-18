SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Pool_upd'
GO
/*************************************************************************/
/* ERWIN generated update a record based on the key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Pool_upd
    @actnumber_pool_id int,
    @actnumber_range_id int,
    @allocated_at datetime,
    @user_id int
AS
BEGIN

UPDATE ACTNumber_Pool
    SET
    allocated_at=@allocated_at,
    user_id=@user_id

WHERE actnumber_pool_id = @actnumber_pool_id AND actnumber_range_id = @actnumber_range_id

END

GO

