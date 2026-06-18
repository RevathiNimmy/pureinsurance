SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Range_add'
GO
/*************************************************************************/
/* ERWIN generated add record and generate ID column if required. */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Range_add
    @actnumber_range_id int OUTPUT ,
    @actnumber_group_id int ,
    @code char(10) ,
    @description varchar(255)
AS
BEGIN

IF @ACTNumber_Range_id = 0
                SELECT @ACTNumber_Range_id = NULL

IF @ACTNumber_Range_id = NULL
                SELECT @ACTNumber_Range_id = MAX(ACTNumber_Range_id) + 1
    FROM ACTNumber_Range

IF @ACTNumber_Range_id = NULL
    SELECT @ACTNumber_Range_id = 1

INSERT INTO ACTNumber_Range (
    actnumber_range_id ,
    actnumber_group_id ,
    code ,
    description )
VALUES (
    @actnumber_range_id,
    @actnumber_group_id,
    @code,
    @description)
END

GO

