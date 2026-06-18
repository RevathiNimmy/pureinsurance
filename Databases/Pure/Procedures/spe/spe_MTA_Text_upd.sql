SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_MTA_Text_upd'
GO

CREATE PROCEDURE spe_MTA_Text_upd
    @mta_cnt int,
    @mta_text_id int,
    @text_line varchar(255)
AS
BEGIN
UPDATE MTA_Text
    SET
    text_line=@text_line
WHERE mta_cnt = @mta_cnt AND mta_text_id = @mta_text_id
END

GO

