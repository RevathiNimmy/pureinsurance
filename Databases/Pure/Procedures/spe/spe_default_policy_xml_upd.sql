SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_default_policy_xml_upd'
GO

CREATE PROCEDURE spe_default_policy_xml_upd
    @default_policy_id int,
    @sequence_no int,
    @xml_string varchar(255)
AS
BEGIN
UPDATE default_policy_xml
    SET
    xml_string=@xml_string
WHERE default_policy_id = @default_policy_id AND sequence_no = @sequence_no
END

GO

