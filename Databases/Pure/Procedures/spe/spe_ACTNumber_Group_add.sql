SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Group_add'
GO
/*************************************************************************/
/* ERWIN generated add record and generate ID column if required. */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Group_add
    @actnumber_group_id int OUTPUT ,
    @code char(10) ,
    @caption_id int ,
    @description varchar(255) ,
    @is_reset_yearly tinyint ,
    @is_deleted tinyint ,
    @effective_date datetime
AS
BEGIN

IF @ACTNumber_Group_id = 0
                SELECT @ACTNumber_Group_id = NULL

IF @ACTNumber_Group_id = NULL
                SELECT @ACTNumber_Group_id = MAX(ACTNumber_Group_id) + 1
    FROM ACTNumber_Group

IF @ACTNumber_Group_id = NULL
    SELECT @ACTNumber_Group_id = 1

INSERT INTO ACTNumber_Group (
    actnumber_group_id ,
    code ,
    caption_id ,
    description ,
    is_reset_yearly ,
    is_deleted ,
    effective_date )
VALUES (
    @actnumber_group_id,
    @code,
    @caption_id,
    @description,
    @is_reset_yearly,
    @is_deleted,
    @effective_date)
END

GO

