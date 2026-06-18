SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pm_lookup_table_sel_all'
GO


CREATE PROCEDURE spu_pm_lookup_table_sel_all
    @pmproduct_id INT,
    @table_name VARCHAR(30)
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original version 29/11/1999 DAK */
/********************************************************************************************************/
SELECT edit_privilege_level edit_privilege_level,
    linked_caption_id,
    linked_object_name,
    linked_class_name,
    interface_Component	
FROM PMProduct_Lookup
WHERE pmproduct_id = @pmproduct_id
AND lookup_table_name = @table_name
GO


