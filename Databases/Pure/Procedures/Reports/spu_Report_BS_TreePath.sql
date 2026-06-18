SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_BS_TreePath'
GO
CREATE PROCEDURE spu_Report_BS_TreePath
    @node_id int
AS

DECLARE @count1 int,
    @count2 int

-- Prevent output
SET NOCOUNT ON

-- Empty tables
DELETE FROM Report_Nodes
DELETE FROM Report_TreePath
DELETE FROM Report_TreePathNames

-- Add top level requested into table
INSERT INTO Report_Nodes
SELECT node_id
FROM StructureTree
WHERE node_id = @node_id
AND node_id not in (SELECT node_id FROM Report_Nodes)

-- initialise the counts
SELECT @count2 = 0
SELECT @count1 = (SELECT COUNT(node_id) FROM Report_Nodes)

-- Get the folders & accounts
WHILE @count1 <> @count2
BEGIN

    INSERT INTO Report_Nodes
    SELECT ST.node_id
    FROM StructureTree ST,
        Report_Nodes RN
    WHERE ST.parent_node_id = RN.node_id
    AND ST.node_id not in (SELECT node_id FROM Report_Nodes)

    SELECT @count2 = @count1
    SELECT @count1 = (SELECT COUNT(node_id) FROM Report_Nodes)

END

-- Get all account references
INSERT INTO Report_TreePath
SELECT ST.account_id,
    ST.node_id,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
FROM Report_Nodes RN,
    StructureTree ST
WHERE ST.node_id = RN.node_id
AND ST.account_id is not null

-- Work way up TreeStructure
UPDATE Report_TreePath
SET id2 = RTP.id1,
    id1 = ST.parent_node_id
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

UPDATE Report_TreePath
SET id3 = RTP.id2,
    id2 = RTP.id1,
    id1 = ST.parent_node_id
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

UPDATE Report_TreePath
SET id4 = RTP.id3,
    id3 = RTP.id2,
    id2 = RTP.id1,
    id1 = ST.parent_node_id
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

UPDATE Report_TreePath
SET id5 = RTP.id4,
    id4 = RTP.id3,
    id3 = RTP.id2,
    id2 = RTP.id1,
    id1 = ST.parent_node_id
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

UPDATE Report_TreePath
SET id6 = RTP.id5,
    id5 = RTP.id4,
    id4 = RTP.id3,
    id3 = RTP.id2,

    id2 = RTP.id1,
    id1 = ST.parent_node_id
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

UPDATE Report_TreePath
SET id7 = RTP.id6,
    id6 = RTP.id5,
    id5 = RTP.id4,
    id4 = RTP.id3,
    id3 = RTP.id2,
    id2 = RTP.id1,
    id1 = ST.parent_node_id
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

UPDATE Report_TreePath
SET id8 = RTP.id7,
    id7 = RTP.id6,
    id6 = RTP.id5,
    id5 = RTP.id4,
    id4 = RTP.id3,
    id3 = RTP.id2,
    id2 = RTP.id1,
    id1 = ST.parent_node_id
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

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
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

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
FROM StructureTree ST,
    Report_TreePath RTP
WHERE RTP.id1 > 1
AND ST.node_id = RTP.id1

-- Get element id for each folder

UPDATE Report_TreePath
SET id1 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id1 = ST.node_id

UPDATE Report_TreePath
SET id2 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id2 = ST.node_id

UPDATE Report_TreePath
SET id3 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id3 = ST.node_id

UPDATE Report_TreePath
SET id4 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id4 = ST.node_id

UPDATE Report_TreePath
SET id5 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id5 = ST.node_id

UPDATE Report_TreePath
SET id6 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id6 = ST.node_id

UPDATE Report_TreePath
SET id7 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id7 = ST.node_id

UPDATE Report_TreePath
SET id8 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id8 = ST.node_id

UPDATE Report_TreePath
SET id9 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id9 = ST.node_id

UPDATE Report_TreePath
SET id10 = ST.element_id
FROM Report_TreePath RTP,
    StructureTree ST
WHERE RTP.id10 = ST.node_id

DELETE FROM Report_Nodes

SET NOCOUNT OFF

GO

