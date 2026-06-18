SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Claim_Public_Text_add'
GO

CREATE PROCEDURE spe_Claim_Public_Text_add
    @claim_cnt int,
    @claim_public_text_id int,
    @text_line varchar(255)
AS
BEGIN

UPDATE Claim 
SET Last_modified_date = Getdate()
WHERE Claim_id = @claim_cnt

INSERT INTO Claim_Public_Text (
    claim_cnt ,
    claim_public_text_id ,
    text_line )
VALUES (
    @claim_cnt,
    @claim_public_text_id,
    @text_line)
END

GO

