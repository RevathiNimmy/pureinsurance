SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_default_policy_xml_del'
GO

CREATE PROCEDURE spe_default_policy_xml_del
    @default_policy_id int,
    @sequence_no int
AS
DELETE FROM default_policy_xml
WHERE default_policy_id = @default_policy_id AND sequence_no = @sequence_no

GO

