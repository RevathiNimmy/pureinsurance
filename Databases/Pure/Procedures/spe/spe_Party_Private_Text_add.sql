SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Private_Text_add'
GO

CREATE PROCEDURE spe_Party_Private_Text_add
    @party_cnt int,
    @party_private_text_id int,
    @text_line varchar(255)
AS
BEGIN
INSERT INTO Party_Private_Text (
    party_cnt ,
    party_private_text_id ,
    text_line )
VALUES (
    @party_cnt,
    @party_private_text_id,
    @text_line)
END

GO

