SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_TreePath'
GO
-- CDH 01/04/2003 Optimised heavily
CREATE PROCEDURE spu_Report_TreePath
    @node_id int
AS
    DECLARE @count1 int
    DECLARE @count2 int
    SET NOCOUNT ON

    -- Empty tables
-- DELETE FROM Report_Nodes
-- DELETE FROM Report_TreePath
-- DELETE FROM Report_TreePathNames

-- + BSJ PN1982/PN13765 -- Truncate is much faster than delete
    TRUNCATE TABLE Report_Nodes
    TRUNCATE TABLE Report_TreePath
    TRUNCATE TABLE Report_TreePathNames

    -- Add top level requested into table
    INSERT INTO Report_Nodes
        SELECT node_id
        FROM StructureTree
        WHERE node_id = @node_id
        AND node_id not in (SELECT node_id FROM Report_Nodes WITH (NOLOCK)) -- BSJ PN1982/PN13765 No need for lock on this table

    -- Initialise the counts
    SELECT @count2 = 0
    SELECT @count1 = COUNT(*) FROM Report_Nodes 

    -- Get the folders & accounts
    WHILE @count1 <> @count2 BEGIN
        INSERT INTO Report_Nodes
            SELECT ST.node_id
            FROM StructureTree ST
            INNER JOIN Report_Nodes RN WITH (NOLOCK) ON ST.parent_node_id = RN.node_id
            WHERE ST.node_id not in (SELECT node_id FROM Report_Nodes WITH (NOLOCK)) -- BSJ PN1982/PN13765 No need for lock on this table

        SELECT @count2 = @count1
        SELECT @count1 = COUNT(*) FROM Report_Nodes
    END

    -- Get all account references
    INSERT INTO Report_TreePath (
        account_id,
        id1
    ) SELECT
        ST.account_id,
        ST.node_id
        FROM StructureTree ST
        INNER JOIN Report_Nodes RN WITH (NOLOCK) ON ST.node_id = RN.node_id -- BSJ PN1982/PN13765 No need for lock on this table
        WHERE ST.account_id is not null

    -- Work way up tree structure
    UPDATE Report_TreePath
        SET id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    UPDATE Report_TreePath
        SET id3 = RTP.id2,
        id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    UPDATE Report_TreePath
        SET id4 = RTP.id3,
        id3 = RTP.id2,
        id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    UPDATE Report_TreePath
        SET id5 = RTP.id4,
        id4 = RTP.id3,
        id3 = RTP.id2,
        id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    UPDATE Report_TreePath
        SET id6 = RTP.id5,
        id5 = RTP.id4,
        id4 = RTP.id3,
        id3 = RTP.id2,
        id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    UPDATE Report_TreePath
        SET id7 = RTP.id6,
        id6 = RTP.id5,
        id5 = RTP.id4,
        id4 = RTP.id3,
        id3 = RTP.id2,
        id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    UPDATE Report_TreePath
        SET id8 = RTP.id7,
        id7 = RTP.id6,
        id6 = RTP.id5,
        id5 = RTP.id4,
        id4 = RTP.id3,
        id3 = RTP.id2,
        id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    UPDATE Report_TreePath
        SET id9 = RTP.id8,
        id8 = RTP.id7,
        id7 = RTP.id6,
        id6 = RTP.id5,
        id5 = RTP.id4,
        id4 = RTP.id3,
        id3 = RTP.id2,
        id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    UPDATE Report_TreePath
        SET id10 = RTP.id9,
        id9 = RTP.id8,
        id8 = RTP.id7,
        id7 = RTP.id6,
        id6 = RTP.id5,
        id5 = RTP.id4,
        id4 = RTP.id3,
        id3 = RTP.id2,
        id2 = RTP.id1,
        id1 = ST.parent_node_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1
        WHERE RTP.id1 > 1 and ST.parent_node_id > 0   --MKW300503 PN3365 Retricted parent_node_id to not zero. BSJ Faster to search on > 0 than <> 0

    -- Get element id for each folder
    UPDATE Report_TreePath
        SET id1 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id1

    UPDATE Report_TreePath
        SET id2 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id2

    UPDATE Report_TreePath
        SET id3 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id3

    UPDATE Report_TreePath
        SET id4 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id4

    UPDATE Report_TreePath
        SET id5 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id5

    UPDATE Report_TreePath
        SET id6 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id6

    UPDATE Report_TreePath
        SET id7 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id7

    UPDATE Report_TreePath
        SET id8 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id8

    UPDATE Report_TreePath
        SET id9 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id9

    UPDATE Report_TreePath
        SET id10 = ST.element_id
        FROM StructureTree ST
        INNER JOIN Report_TreePath RTP WITH (NOLOCK) ON ST.node_id = RTP.id10

    TRUNCATE TABLE Report_Nodes
GO
