SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmproduct_lookup_sel_all'
GO


CREATE PROCEDURE spu_pmproduct_lookup_sel_all
    @pmproduct_id SMALLINT,
    @table_name VARCHAR(30)
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 10/11/1999 DAK */
/********************************************************************************************************/
SELECT pmproduct_id pmproduct_id,
    lookup_table_name lookup_table_name,
    edit_privilege_level edit_privilege_level,
    linked_caption_id linked_caption_id,
    linked_object_name linked_object_name,
    linked_class_name linked_class_name,
    is_generic_maintenance is_generic_maintenance
FROM PMProduct_Lookup
WHERE pmproduct_id = @pmproduct_id
AND lookup_table_name = @table_name
GO


