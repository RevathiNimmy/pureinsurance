SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Setup_folder'
GO


CREATE PROCEDURE spu_ACT_Setup_folder
    @folder_name varchar(255),
    @parent_name varchar(255),
    @company_id int  
AS


/****************************************************************************************************/
/* Oversees adding and mapping of folders for given new codes. */
/* Called by 'sp_Setup_Trans_Mapping_RSA'. */
/****************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 04/05/2001 RWH */
/***************************************************************************************************/
DECLARE 
    @temp_mapping_id int,
    @temp_node_id int

-- Does folder already exist ?
SELECT @temp_mapping_id = 0

SELECT @temp_mapping_id = mapping_id
FROM   Mapping
WHERE  description =  @folder_name
AND    company_id = @company_id

IF @temp_mapping_id = 0
    -- If it doesn't already exist, add it in.
    exec spu_ACT_Setup_add_folder 
        @folder_name, 
        @parent_name, 
        1,
        @company_id
ELSE
BEGIN
    -- If it does exist, does it map to anything.
    SELECT @temp_node_id = 0

    SELECT @temp_node_id = node_id
    FROM   StructureTree
    WHERE  mapping_id = @temp_mapping_id

    IF @temp_node_id = 0
    BEGIN
        -- If it's not mapped to anything then delete it from Mapping table and add from scratch.
        DELETE Mapping
        WHERE  mapping_id = @temp_mapping_id

        exec spu_ACT_Setup_add_folder 
            @folder_name, 
            @parent_name, 
            1,
            @company_id
    END
    ELSE
        -- If it is mapped then re-map to required parent to preserve existing accounts it may contain.
        exec spu_ACT_Setup_map_folder 
            @folder_name, 
            @parent_name,
            @company_id

END
GO


