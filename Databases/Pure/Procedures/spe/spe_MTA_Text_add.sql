SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_MTA_Text_add'
GO

CREATE PROCEDURE spe_MTA_Text_add
    @mta_cnt int,
    @mta_text_id int,
    @text_line varchar(255)
AS
BEGIN
INSERT INTO MTA_Text (
    mta_cnt ,
    mta_text_id ,
    text_line )
VALUES (
    @mta_cnt,
    @mta_text_id,
    @text_line)
END

GO

