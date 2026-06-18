SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_do_get_node_mapping'
GO


CREATE PROCEDURE spu_ACT_do_get_node_mapping
    @node_id integer,
    @mapping_id integer OUTPUT
AS

/*******************************************************************************************/
/* Returns the Mapping ID for a Node in the Structure Tree. */
/*******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 22/01/1999 RFC */
/*******************************************************************************************/
BEGIN
DECLARE @parent_node_id integer
    SELECT
        @parent_node_id = parent_node_id ,
        @mapping_id = mapping_id
    FROM structuretree
    WHERE node_id = @node_id
    WHILE (@mapping_id IS NULL)
    BEGIN
        IF (@parent_node_id = @node_id)
        BEGIN
            SELECT @mapping_id = NULL
            RETURN
        END
        ELSE
        BEGIN
            SELECT
                @node_id = node_id ,
                @parent_node_id = parent_node_id ,
                @mapping_id = mapping_id
            FROM structuretree
            WHERE node_id = @parent_node_id
        END
    END
END
GO


