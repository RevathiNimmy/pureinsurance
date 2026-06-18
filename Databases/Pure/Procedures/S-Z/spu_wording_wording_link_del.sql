SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wording_wording_link_del'
GO


CREATE PROCEDURE spu_wording_wording_link_del
    @document_template_id int
AS


BEGIN
DELETE
    wording_wording_link
WHERE
    document_template_id = @document_template_id

END
GO


