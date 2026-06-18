SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Public_Text_add'
GO

CREATE PROCEDURE spe_Party_Public_Text_add
    @party_cnt int,
    @party_public_text_id int,
    @text_line varchar(255)
AS
BEGIN
INSERT INTO Party_Public_Text (
    party_cnt ,
    party_public_text_id ,
    text_line )
VALUES (
    @party_cnt,
    @party_public_text_id,
    @text_line)
END

GO

