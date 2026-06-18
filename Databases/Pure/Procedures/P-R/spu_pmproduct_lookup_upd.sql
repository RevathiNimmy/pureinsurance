SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmproduct_lookup_upd'
GO


CREATE PROCEDURE spu_pmproduct_lookup_upd
    @pmproduct_id SMALLINT,
    @table_name VARCHAR(30),
    @privilege_level SMALLINT,
    @linked_caption_id INT,
    @linked_object_name VARCHAR(30),
    @linked_class_name VARCHAR(30),
    @is_generic_maintenance SMALLINT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 10/11/1999 DAK */
/********************************************************************************************************/
UPDATE pmproduct_lookup
    SET edit_privilege_level = @privilege_level,
        linked_caption_id = @linked_caption_id,
        linked_object_name = @linked_object_name,
        linked_class_name = @linked_class_name,
        is_generic_maintenance = @is_generic_maintenance
    WHERE pmproduct_id = @pmproduct_id
    AND lookup_table_name = @table_name
GO


