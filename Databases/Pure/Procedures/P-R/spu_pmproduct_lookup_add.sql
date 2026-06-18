SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmproduct_lookup_add'
GO


CREATE PROCEDURE spu_pmproduct_lookup_add
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
INSERT INTO pmproduct_lookup( pmproduct_id,
                    lookup_table_name,
                    edit_privilege_level,
                    linked_caption_id,
                    linked_object_name,
                    linked_class_name,
                    is_generic_maintenance)
    VALUES (@pmproduct_id,
        @table_name,
        @privilege_level,
        @linked_caption_id,
        @linked_object_name,
        @linked_class_name,
        @is_generic_maintenance)
GO


