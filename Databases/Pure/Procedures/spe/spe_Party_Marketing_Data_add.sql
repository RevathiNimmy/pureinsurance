SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Marketing_Data_add'
GO

CREATE PROCEDURE spe_Party_Marketing_Data_add
    @party_cnt int,
    @marketing_code_id int,
    @value varchar(255)
AS
BEGIN
INSERT INTO Party_Marketing_Data (
    party_cnt ,
    marketing_code_id ,
    value )
VALUES (
    @party_cnt,
    @marketing_code_id,
    @value)
END

GO

