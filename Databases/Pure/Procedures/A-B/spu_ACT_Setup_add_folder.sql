SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Setup_add_folder'
GO


CREATE PROCEDURE spu_ACT_Setup_add_folder
    @description varchar(255),
    @parent_name varchar(20),
    @account_type int,
    @company_id int
AS

/****************************************************************************************************/
/* Adds records into the table required to add a new */
/* folder to Account Explorer. This was originally written for use in setting up a  */
/* base Charter of Accounts. */
/* Called by 'sp_ACT_Setup_folder'. */
/****************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/05/2001 RWH */
/***************************************************************************************************/
/*
SELECT @element_name = 'RikScriptTest',
    @parent_name = 'Accruals',
    @account_type = 1 -- Heading Account
*/

DECLARE
    @element_id int,
    @parent_id int,
    @mapping_id int,
    @parent_node int,
    @test_node int

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

SELECT @element_id = 0

-- Does required folder already exist.
SELECT @element_id = e.element_id
FROM   Mapping m
JOIN   StructureTree st ON st.mapping_id = m.mapping_id
JOIN   Element e ON e.element_id = st.element_id
WHERE  m.description = @description
AND    m.company_id = @company_id


-- If folder doesn't exist then insert it.
if @element_id = 0
BEGIN
    INSERT INTO Element (element_name, parent_id)
    VALUES (@description, Null)

    SELECT @element_id = @@IDENTITY

    INSERT INTO ElementExtras (element_id, description, spare_text, report_map_id, account_map_id, is_deletable, spare_number, group_for_gl_export_ind)
    VALUES (@element_id,@account_type,@description,0,0,0,Null,0)

    INSERT INTO Mapping
    VALUES (
        1,
        1,
        @description)

    SELECT @mapping_id = @@IDENTITY
END
/*
print @element_id

select * from Element
where element_id = @element_id
*/
SELECT @parent_id = 0

SELECT @parent_id = e.element_id,
       @parent_node = st.node_id
FROM   Mapping m
JOIN   StructureTree st ON st.mapping_id = m.mapping_id
JOIN   Element e ON e.element_id = st.element_id
WHERE  m.description = @parent_name
AND    m.company_id = @company_id

SELECT @test_node = 0

-- Check this element is not already child of this parent.
SELECT @test_node = node_id
FROM   StructureTree
WHERE  element_id = @element_id
AND    parent_node_id = @parent_node

IF @test_node = 0
BEGIN
    IF @parent_node is null
        SELECT @parent_node = 0

    INSERT INTO StructureTree
        (company_id, mapping_id, element_id, parent_node_id)
    VALUES (
        @company_id,
        @mapping_id,
        @element_id,
        @parent_node)

END
GO


