SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmproduct_lookup_all'
GO


CREATE PROCEDURE spu_pmproduct_lookup_all
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 21/12/1999 DAK */
/********************************************************************************************************/
SELECT pmproduct_id,
    lookup_table_name,
    edit_privilege_level,
    linked_caption_id,
    linked_object_name,
    linked_class_name,
    is_generic_maintenance
FROM PMProduct_Lookup
GO


