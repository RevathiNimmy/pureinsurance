SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Setup_Map_Ledger'
GO


CREATE PROCEDURE spu_ACT_Setup_Map_Ledger
    @ledger_code varchar(2),
    @folder_name varchar(255),
    @sub_branch_id int
AS

/****************************************************************************************/
/* Maps a given ledger to a given folder. If there are accounts in the */
/* currently mapped folder then these are moved to the new area. This was originally */
/* written for use in setting up a base Charter of Accounts. */
/* Called by 'sp_ACT_Setup_RSA_ChartOfAcc'. */
/****************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/05/2001 RWH */
/* 1.1 Update new ledger with mapping_id. 23/07/2001 RWH */
/****************************************************************************************/
DECLARE
    @company_id int,
    @temp_element_id int,
    @temp_mapping_id int,
    @temp_node_id int,
    @ledger_id int,
    @ledger_mapping_id int,
    @ledger_element_id int,
    @ledger_node_id int

-- Does folder already exist
SELECT
    @temp_element_id = 0,
    @temp_mapping_id = 0,
    @temp_node_id = 0

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
BEGIN
    SELECT @company_id=1
    SELECT @sub_branch_id=1
END
ELSE
BEGIN
    SELECT @company_id = source_id
    FROM   sub_branch
    WHERE  sub_branch_id = @sub_branch_id
END

SELECT @temp_mapping_id = m.mapping_id,
       @temp_element_id = st.element_id ,
       @temp_node_id = st.node_id
FROM   Mapping m
JOIN   StructureTree st ON st.mapping_id = m.mapping_id
WHERE  m.description =  @folder_name
AND    st.company_id = @company_id

-- Get folder ledger is mapped to.
SELECT @ledger_id = l.ledger_id,
       @ledger_mapping_id = l.mapping_id,
       @ledger_element_id = st.element_id,
       @ledger_node_id = st.node_id
FROM   Ledger l
JOIN   StructureTree st ON st.mapping_id = l.mapping_id
WHERE  l.ledger_short_name =@ledger_code
AND    l.sub_branch_id = @sub_branch_id

BEGIN TRANSACTION

IF @temp_mapping_id <> 0
BEGIN
    IF @ledger_mapping_id is null
        -- Update new ledger with correct mapping_id.
        UPDATE Ledger
        SET    mapping_id = @temp_mapping_id
        WHERE  ledger_id = @ledger_id
    ELSE
    BEGIN
        -- Is existing folder mapped to ledger.
        IF @ledger_mapping_id <> @temp_mapping_id
        BEGIN
            -- Rename exisitng folder to Temp.
            UPDATE Mapping
            SET    Description = 'Temp'
            WHERE  mapping_id = @temp_mapping_id

            UPDATE Element
            SET    element_name = 'Temp'
            WHERE  element_id = @temp_element_id

            -- Copy all elements in existing folder into client.
            UPDATE StructureTree
            SET    parent_node_id = @ledger_node_id
            WHERE  parent_node_id = @temp_node_id

            -- Delete old folder.
            DELETE StructureTree
            WHERE  node_id = @temp_node_id

            DELETE ElementExtras
            WHERE  element_id = @temp_element_id

            DELETE Element
            WHERE  element_id = @temp_element_id

            DELETE Mapping
            WHERE  mapping_id = @temp_mapping_id

            -- Rename Client ledger to be Direct
            UPDATE Mapping
            SET    Description = @folder_name
            WHERE  mapping_id = @ledger_mapping_id

            UPDATE Element
            SET    element_name = @folder_name
            WHERE  element_id = @ledger_element_id
        END
    END
END
ELSE
BEGIN
    -- Rename Client ledger to be Direct
    UPDATE Mapping
    SET    Description = @folder_name
    WHERE  mapping_id = @ledger_mapping_id

    UPDATE Element
    SET    element_name = @folder_name
    WHERE  element_id = @ledger_element_id
END

COMMIT TRANSACTION
GO

