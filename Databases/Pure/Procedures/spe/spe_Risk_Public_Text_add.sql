SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_Public_Text_add'
GO

CREATE PROCEDURE spe_Risk_Public_Text_add
    @insurance_file_cnt int,
    @risk_id int,
    @risk_public_text_id int,
    @text_line varchar(255)
AS
BEGIN
INSERT INTO Risk_Public_Text (
    risk_cnt ,
    risk_public_text_id ,
    text_line )
VALUES (
    @risk_id,
    @risk_public_text_id,
    @text_line)
END

GO

