SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Full_Path'
GO


CREATE PROCEDURE spu_ACT_Select_Full_Path
    @account_id int,
    @full_path varchar(255) OUTPUT
AS

/***************************************************************************************************************

Description: Returns the path to an account in the structure tree

Parameters: account_id = Account_id for the account to be used
           full_path = OUTPUT parameter. Returns the path to the account.

History: CTAF 080101 - Created - version 1.0

***************************************************************************************************************/
BEGIN

DECLARE @parent_node_id int
DECLARE @node_id int
DECLARE @element_name varchar(255)
DECLARE @element_id int

    SELECT @full_path = ""

    /* Start node */
    SELECT @parent_node_id = parent_node_id,
           @node_id = node_id,
           @element_id = element_id
    FROM   StructureTree
    WHERE  account_id = @account_id

    /* Loop while we have a parent node */
    WHILE (@parent_node_id > 0)
    BEGIN

        /* Get the name of the element */
        SELECT @element_name = element_name
        FROM Element
        WHERE element_id = @element_id

        /* We dont want any trailing slashes */
        IF (@full_path <> "")
        BEGIN
            SELECT @full_path = RTRIM(@element_name) + "\" + @full_path
        END
        ELSE
        BEGIN
            SELECT @full_path = RTRIM(@element_name)
        END

        /* Move to the parent node */
        SELECT @parent_node_id = parent_node_id,
            @node_id = node_id,
            @element_id = element_id
        FROM StructureTree
        WHERE node_id = @parent_node_id

    END

    SELECT @element_name = element_name
    FROM Element
    WHERE element_id = @element_id

    SELECT @full_path = "\" + RTRIM(@element_name) + "\" + @full_path

END
GO


