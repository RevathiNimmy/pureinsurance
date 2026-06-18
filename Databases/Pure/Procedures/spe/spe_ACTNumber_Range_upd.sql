SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Range_upd'
GO
/*************************************************************************/
/* ERWIN generated update a record based on the key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Range_upd
    @actnumber_range_id int,
    @actnumber_group_id int,
    @code char(10),
    @description varchar(255)
AS
BEGIN

UPDATE ACTNumber_Range
    SET
    actnumber_group_id=@actnumber_group_id,
    code=@code,
    description=@description

WHERE actnumber_range_id = @actnumber_range_id

END

GO

