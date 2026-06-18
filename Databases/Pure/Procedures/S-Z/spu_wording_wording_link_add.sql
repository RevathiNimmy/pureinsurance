SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wording_wording_link_add'
GO


CREATE PROCEDURE spu_wording_wording_link_add
    @document_template_id int,
    @calls_template_id int
AS


BEGIN
INSERT INTO wording_wording_link (
    document_template_id,
    calls_template_id)
VALUES (
    @document_template_id,
    @calls_template_id)

END
GO


