SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_BS_TreePathNames'
GO
CREATE PROCEDURE spu_Report_BS_TreePathNames
    @node_id int
AS

-- Prevent output
SET NOCOUNT ON

DECLARE @description varchar(70),
        @heading_id int

SELECT @heading_id = 200

EXECUTE spu_Report_TreePath @node_id

-- Populate Heading table
-- - DELETE FROM Report_Headings
-- BSJ PN1982/13765 Truncate is much faster than Delete
TRUNCATE TABLE Report_Headings

DECLARE c_Headings CURSOR FAST_FORWARD FOR

    SELECT DISTINCT description
    FROM ElementExtras
    WHERE totalling_id = 3

OPEN c_Headings

FETCH NEXT FROM c_Headings INTO @description

WHILE @@FETCH_STATUS = 0 BEGIN
    SELECT @heading_id = @heading_id + 1

    INSERT INTO Report_Headings
    VALUES
    (
    @heading_id,
    @description
    )

    FETCH NEXT FROM c_Headings INTO @description
END
CLOSE c_Headings
DEALLOCATE c_Headings

-- -DELETE FROM Report_TreePathNames
-- + BSJ PN1982/13765 Truncate is much faster than Delete
TRUNCATE TABLE Report_TreePathNames

-- Get the names and the sort keys for top element in the path
INSERT INTO Report_TreePathNames
SELECT RTP.account_id,
    E.element_name,
    EE.Report_Map_Id,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Element E,
    ElementExtras EE WHERE RTP.id1 = E.element_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)
UNION
SELECT RTP.account_id,
    EE.description,
    EE.Report_Map_Id,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null, null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    ElementExtras EE
WHERE RTP.id1 = EE.element_id
AND ISNULL(EE.Totalling_id, 0) = 3

-- Work backwards along the path
UPDATE Report_TreePathNames
SET element_name10 = E.element_name,
    Report_Map_Id10 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK),
    Element E,
    ElementExtras EE
WHERE RTP.id10 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET element_name10 = EE.description,
    Report_Map_Id10 = EE.Report_Map_Id,
    cost_centre = EE.description --TF040500
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE
WHERE RTP.id10 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3

UPDATE Report_TreePathNames
SET element_name9 = E.element_name,
    Report_Map_Id9 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    Element E,
    ElementExtras EE
WHERE RTP.id9 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET
-- element_name10 = element_name9,
-- Report_Map_Id10 = Report_Map_Id9,
    element_name9= RH.description,
    Report_Map_Id9 = RH.heading_id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE,
    Report_Headings RH
WHERE RTP.id9 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3
AND RH.description = EE.description

UPDATE Report_TreePathNames
SET element_name8 = E.element_name,
    Report_Map_Id8 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    Element E,
    ElementExtras EE
WHERE RTP.id8 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET
-- element_name9 = element_name8,
-- Report_Map_Id9 = Report_Map_Id8,
    element_name8 = RH.description,
    Report_Map_Id8 = RH.heading_id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE,
    Report_Headings RH
WHERE RTP.id8 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3
AND RH.description = EE.description

UPDATE Report_TreePathNames
SET element_name7 = E.element_name,
    Report_Map_Id7 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    Element E,

    ElementExtras EE
WHERE RTP.id7 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET
-- element_name8 = element_name7,
-- Report_Map_Id8 = Report_Map_Id7,
    element_name7 = RH.description,
    Report_Map_Id7 = RH.heading_id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE,
    Report_Headings RH
WHERE RTP.id7 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3
AND RH.description = EE.description

UPDATE Report_TreePathNames
SET element_name6 = E.element_name,
    Report_Map_Id6 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    Element E,
    ElementExtras EE
WHERE RTP.id6 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET
-- element_name7 = element_name6,
-- Report_Map_Id7 = Report_Map_Id6,
    element_name6 = RH.description,
    Report_Map_Id6 = RH.heading_id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE,
    Report_Headings RH
WHERE RTP.id6 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3
AND RH.description = EE.description

UPDATE Report_TreePathNames
SET element_name5 = E.element_name,
    Report_Map_Id5 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    Element E,
    ElementExtras EE
WHERE RTP.id5 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET
-- element_name6 = element_name5,
-- Report_Map_Id6 = Report_Map_Id5,
    element_name5 = RH.description,
    Report_Map_Id5 = RH.heading_id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE,
    Report_Headings RH
WHERE RTP.id5 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3
AND RH.description = EE.description

UPDATE Report_TreePathNames
SET element_name4 = E.element_name,
    Report_Map_Id4 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    Element E,
    ElementExtras EE
WHERE RTP.id4 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET
-- element_name5 = element_name4,
-- Report_Map_Id5 = Report_Map_Id4,
    element_name4 = RH.description,
    Report_Map_Id4 = RH.heading_id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE,
    Report_Headings RH
WHERE RTP.id4 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3
AND RH.description = EE.description

UPDATE Report_TreePathNames
SET element_name3 = E.element_name,
    Report_Map_Id3 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    Element E,
    ElementExtras EE
WHERE RTP.id3 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET
-- element_name4 = element_name3,
-- Report_Map_Id4 = Report_Map_Id3,
    element_name3 = RH.description,
    Report_Map_Id3 = RH.heading_id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE,
    Report_Headings RH
WHERE RTP.id3 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3
AND RH.description = EE.description

UPDATE Report_TreePathNames
SET element_name2 = E.element_name,
    Report_Map_Id2 = EE.Report_Map_Id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    Element E,
    ElementExtras EE
WHERE RTP.id2 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND ISNULL(EE.Totalling_id, 0) IN (0, 1, 2)

UPDATE Report_TreePathNames
SET
-- element_name3 = element_name2,
-- Report_Map_Id3 = Report_Map_Id2,
    element_name2 = RH.description,

    Report_Map_Id2 = RH.heading_id
FROM Report_TreePath RTP WITH (NOLOCK), -- BSJ PN1982/13765 No need to lock this table
    Report_TreePathNames RTPN WITH (NOLOCK), 
    ElementExtras EE,
    Report_Headings RH
WHERE RTP.id2 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND ISNULL(EE.Totalling_id, 0) = 3
AND RH.description = EE.description

-- - DELETE FROM Report_TreePath
-- BSJ PN1982/13765 Truncate is much faster than Delete
TRUNCATE TABLE Report_TreePath

SET NOCOUNT OFF

GO

