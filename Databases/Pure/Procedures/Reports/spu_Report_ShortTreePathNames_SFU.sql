EXECUTE DDLDropProcedure 'spu_Report_ShortTreePathNames_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


CREATE PROCEDURE spu_Report_ShortTreePathNames_SFU
    @node_id int
AS

-- Prevent output
SET NOCOUNT ON

EXEC spu_Report_TreePath_SFU @node_id

-- Get the names and the sort keys for top element in the path
INSERT INTO Report_TreePathNames
SELECT  RTP.account_id,
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
FROM    Report_TreePath RTP,
    Element E,
    ElementExtras EE
WHERE   RTP.id1 = E.element_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)
UNION
SELECT  RTP.account_id,
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
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
FROM    Report_TreePath RTP,
    ElementExtras EE
WHERE   RTP.id1 = EE.element_id
AND EE.Totalling_id = 3

-- Work along the path
UPDATE  Report_TreePathNames
SET element_name2 = E.element_name,
    Report_Map_Id2 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id2 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name2 = EE.description,
    Report_Map_Id2 = EE.Report_Map_Id,
    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id2 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

UPDATE  Report_TreePathNames
SET element_name3 = E.element_name,
    Report_Map_Id3 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id3 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name3 = EE.description,
    Report_Map_Id3 = EE.Report_Map_Id,
    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id3 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

UPDATE  Report_TreePathNames
SET element_name4 = E.element_name,
    Report_Map_Id4 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id4 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name4 = EE.description,
    Report_Map_Id4 = EE.Report_Map_Id,
    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id4 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

UPDATE  Report_TreePathNames
SET element_name5 = E.element_name,
    Report_Map_Id5 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id5 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name5 = EE.description,
    Report_Map_Id5 = EE.Report_Map_Id,
    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id5 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

UPDATE  Report_TreePathNames
SET element_name6 = E.element_name,
    Report_Map_Id6 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id6 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name6 = EE.description,
    Report_Map_Id6 = EE.Report_Map_Id,
    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id6 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

UPDATE  Report_TreePathNames
SET element_name7 = E.element_name,
    Report_Map_Id7 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id7 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name7 = EE.description,
    Report_Map_Id7 = EE.Report_Map_Id,
    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id7 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

UPDATE  Report_TreePathNames
SET element_name8 = E.element_name,
    Report_Map_Id8 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id8 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name8 = EE.description,
    Report_Map_Id8 = EE.Report_Map_Id,
    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id8 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

UPDATE  Report_TreePathNames
SET element_name9 = E.element_name,
    Report_Map_Id9 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id9 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name9 = EE.description,
    Report_Map_Id9 = EE.Report_Map_Id,    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id9 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

UPDATE  Report_TreePathNames
SET element_name10 = E.element_name,
    Report_Map_Id10 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,
    ElementExtras EE
WHERE   RTP.id10 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id
AND EE.Totalling_id IN (1, 2)

UPDATE  Report_TreePathNames
SET element_name10 = EE.description,
    Report_Map_Id10 = EE.Report_Map_Id,
    cost_centre = EE.description    --TF040500
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    ElementExtras EE
WHERE   RTP.id10 = EE.element_id
AND RTP.account_id = RTPN.account_id
AND EE.Totalling_id = 3

DELETE FROM Report_TreePath

SET NOCOUNT OFF



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

