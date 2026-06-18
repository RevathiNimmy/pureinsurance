SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ElementExtras_sel'
GO
/*************************************************************************/
/* ERWIN generated select all fields for a given key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 RFC Original (Based on Original by SP) */
/*************************************************************************/
CREATE PROCEDURE spe_ElementExtras_sel
    @element_id int
AS

SELECT
    element_id,
    Totalling_Id,
    Description,
    Report_Map_Id,
    Account_Map_Id,
    Spare_Number,
    Spare_Text,
    Is_deletable,
    group_for_gl_export_ind
FROM ElementExtras

WHERE element_id = @element_id

GO

