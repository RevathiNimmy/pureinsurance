SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_default_policy_xml_add'
GO

CREATE PROCEDURE spe_default_policy_xml_add
    @default_policy_id int,
    @sequence_no int,
    @xml_string varchar(255)
AS
BEGIN
INSERT INTO default_policy_xml (
    default_policy_id,
    sequence_no,
    xml_string)
VALUES (
    @default_policy_id,
    @sequence_no,
    @xml_string)
END

GO

