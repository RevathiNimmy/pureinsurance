SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ElementExtras_add'
GO
/*************************************************************************/
/* ERWIN generated add record */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/*************************************************************************/
CREATE PROCEDURE spe_ElementExtras_add
    @element_id int,
    @Totalling_Id int,
    @Description varchar(70),
    @Report_Map_Id int,
    @Account_Map_Id int,
    @is_deletable tinyint
AS

BEGIN
INSERT INTO ElementExtras (
    element_id ,
    Totalling_Id ,
    Description ,
    Report_Map_Id ,
    Account_Map_Id ,
    Spare_Number ,
    Spare_Text,
    Is_Deletable)
VALUES (
    @element_id,
    @Totalling_Id,
    @Description,
    @Report_Map_Id,
    @Account_Map_Id,
    0,
    '',
    @is_deletable)
END

GO

