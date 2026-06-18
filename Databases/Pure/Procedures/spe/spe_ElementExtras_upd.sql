SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ElementExtras_upd'
GO
/*************************************************************************/
/* ERWIN generated update a record based on the key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/*************************************************************************/
CREATE PROCEDURE spe_ElementExtras_upd
    @element_id int,
    @Totalling_Id int,
    @Description varchar(70),
    @Report_Map_Id int,
    @Account_Map_Id int,
    @Is_deletable tinyint,
    @group_for_gl_export_ind tinyint = null
AS
BEGIN

UPDATE ElementExtras
    SET
    Totalling_Id=@Totalling_Id,
    Description=@Description,
    Report_Map_Id=@Report_Map_Id,
    Account_Map_Id=@Account_Map_Id,
    Spare_Number=0,
    Spare_Text='',
    Is_deletable=@Is_deletable,
    group_for_gl_export_ind = @group_for_gl_export_ind
WHERE element_id = @element_id

END

GO
