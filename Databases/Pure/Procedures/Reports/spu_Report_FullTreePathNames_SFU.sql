EXECUTE DDLDropProcedure 'spu_Report_FullTreePathNames_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO







/****** Object:  Stored Procedure dbo.sp_Report_FullTreePathNames    Script Date: 16/10/00 12:03:12 ******/





CREATE PROCEDURE spu_Report_FullTreePathNames_SFU
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
    null    --TF170500
FROM    Report_TreePath RTP,
    Element E,
    ElementExtras EE
WHERE   RTP.id1 = E.element_id
AND EE.element_id = E.element_id

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

UPDATE  Report_TreePathNames
SET element_name8 = E.element_name,
    Report_Map_Id8 = EE.Report_Map_Id
FROM    Report_TreePath RTP,
    Report_TreePathNames RTPN,
    Element E,    ElementExtras EE
WHERE   RTP.id8 = E.element_id
AND RTP.account_id = RTPN.account_id
AND EE.element_id = E.element_id

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

DELETE FROM Report_TreePath

SET NOCOUNT OFF









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

