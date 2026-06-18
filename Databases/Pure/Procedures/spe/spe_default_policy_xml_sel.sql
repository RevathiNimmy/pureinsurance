SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_default_policy_xml_sel'
GO

CREATE PROCEDURE spe_default_policy_xml_sel
    @default_policy_id int,
    @sequence_no int
AS
SELECT
    default_policy_id,
    sequence_no,
    xml_string
 FROM default_policy_xml
WHERE default_policy_id = @default_policy_id AND sequence_no = @sequence_no

GO

