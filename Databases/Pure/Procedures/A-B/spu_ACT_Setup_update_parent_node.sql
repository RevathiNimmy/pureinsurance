SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Setup_update_parent_node'
GO


CREATE PROCEDURE spu_ACT_Setup_update_parent_node
    @previous_map_folder_name varchar(255),
    @new_map_folder_name varchar(255)
AS
/****************************************************************************************************/
/* Moves all accounts mapped to a given folder into a new given folder.                             */
/* This was originally written for use in setting up a base Charter of Accounts.                    */
/* Called by 'sp_Setup_Export_Map' on broking/underwriting database.                                */
/****************************************************************************************************/
/* Revision Description of Modification Date        Who                                             */
/* -------- --------------------------- ----        ---                                             */
/* 1.0      Original                    08/05/2001  RWH                                             */
/****************************************************************************************************/
DECLARE @previous_map_node int,
    @new_map_node int

    -- Get previous parent node.
    SELECT @previous_map_node = st.node_id
    FROM Mapping m,
            StructureTree st
    WHERE m.description = @previous_map_folder_name
    AND st.mapping_id = m.mapping_id

    -- Get new parent node.
    SELECT @new_map_node = st.node_id
    FROM Mapping m,
            StructureTree st
    WHERE m.description = @new_map_folder_name
    AND st.mapping_id = m.mapping_id

    -- Update parent node of existing accounts.
    UPDATE StructureTree
    SET parent_node_id = @new_map_node
    WHERE parent_node_id = @previous_map_node
    AND mapping_id is null
GO


