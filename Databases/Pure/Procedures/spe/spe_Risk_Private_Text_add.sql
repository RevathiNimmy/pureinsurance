SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_Private_Text_add'
GO

CREATE PROCEDURE spe_Risk_Private_Text_add
    @insurance_file_cnt int,
    @risk_id int,
    @risk_private_text_id int,
    @text_line varchar(255)
AS
BEGIN
INSERT INTO Risk_Private_Text (
    risk_cnt ,
    risk_private_text_id ,
    text_line )
VALUES (
    @risk_id,
    @risk_private_text_id,
    @text_line)
END

GO

