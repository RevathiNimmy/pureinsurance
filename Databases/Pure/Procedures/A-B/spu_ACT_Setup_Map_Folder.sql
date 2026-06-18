SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Setup_Map_Folder'
GO


CREATE PROCEDURE spu_ACT_Setup_Map_Folder
    @folder_name varchar(20),
    @parent_name varchar(20),
    @company_id int
AS

/****************************************************************************************************/
/* Maps a given folder to be the child of a given */
/* parent folder in Account Explorer. This was originally written for use in setting up a   */
/* base Charter of Accounts. */
/* Called by 'sp_ACT_Setup_folder'. */
/****************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/05/2001 RWH */
/***************************************************************************************************/
DECLARE
    @child_node int,
    @parent_node int

/* DD 23/08/2002 */
/* Get the Product Option for multi-tree accounting */
DECLARE @Value VARCHAR(20)
SELECT
    @Value=Value
FROM
    Hidden_options
WHERE
    option_number=16

/*
    If Null/0 then there is only one tree.
    Hardcoded for performance reasons
*/
IF @Value IS NULL OR @Value=0
    SELECT @company_id=1

-- Get node_id of child.
SELECT @child_node = st.node_id
FROM   Mapping m
JOIN   StructureTree st ON st.mapping_id = m.mapping_id
WHERE  m.description = @folder_name
AND    st.company_id = @company_id

-- Get node_id of parent.
SELECT @parent_node = st.node_id
FROM   Mapping m
JOIN   StructureTree st ON st.mapping_id = m.mapping_id
WHERE  m.description = @parent_name
AND    st.company_id = @company_id

IF @parent_node is null
    SELECT @parent_node = 0

-- make folder child of parent.
UPDATE StructureTree
SET    parent_node_id = @parent_node
WHERE  node_id = @child_node

GO


