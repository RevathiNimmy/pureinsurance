SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Group_upd'
GO
/*************************************************************************/
/* ERWIN generated update a record based on the key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Group_upd
    @actnumber_group_id int,
    @code char(10),
    @caption_id int,
    @description varchar(255),
    @is_reset_yearly tinyint,
    @is_deleted tinyint,
    @effective_date datetime
AS
BEGIN

UPDATE ACTNumber_Group
    SET
    code=@code,
    caption_id=@caption_id,
    description=@description,
    is_reset_yearly=@is_reset_yearly,
    is_deleted=@is_deleted,
    effective_date=@effective_date

WHERE actnumber_group_id = @actnumber_group_id

END

GO

