SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Claim_Private_Text_add'
GO

CREATE PROCEDURE spe_Claim_Private_Text_add
    @claim_cnt int,
    @claim_private_text_id int,
    @text_line varchar(255)
AS
BEGIN
INSERT INTO Claim_Private_Text (
    claim_cnt ,
    claim_private_text_id ,
    text_line )
VALUES (
    @claim_cnt,
    @claim_private_text_id,
    @text_line)
END

GO

