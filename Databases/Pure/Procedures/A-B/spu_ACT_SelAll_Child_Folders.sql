SET QUOTED_IDENTIFIER OFF  SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Child_Folders'
GO

CREATE PROCEDURE spu_ACT_SelAll_Child_Folders

@node_id int,
@session_id int = NULL OUTPUT
AS

DECLARE @child_node_id int,
        @element_name varchar(255),
        @element_id int,
        @parent_node_id int


--Get a new session_id
IF (@session_id IS NULL)
BEGIN
      EXECUTE spu_pm_session_id_alloc @session_id OUTPUT
END

--Create cursor containing next layer of child nodes
DECLARE c_node CURSOR FAST_FORWARD FOR
SELECT  node_id,
        element_name,
        T.element_id,
        T.parent_node_id
FROM    StructureTree T
JOIN    Element E ON E.element_id = T.element_id
WHERE   T.parent_node_id = @node_id
AND     T.account_id IS NULL

OPEN c_node
FETCH NEXT FROM c_node INTO @child_node_id, @element_name, @element_id, @parent_node_id
WHILE @@FETCH_STATUS = 0 
BEGIN
        --Call this procedure recursively, passing the current session id
        EXEC spu_ACT_SelAll_Child_Folders @child_node_id, @session_id

        INSERT INTO Temp_child_folders
        VALUES
        (
        @session_id,
        @child_node_id,
        @parent_node_id,
        @element_id,
        @element_name
        )

        FETCH NEXT FROM c_node INTO @child_node_id, @element_name, @element_id, @parent_node_id
END
CLOSE c_node
DEALLOCATE c_node
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO
