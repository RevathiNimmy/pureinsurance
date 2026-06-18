SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Corporate_Client_updpassword'
GO

CREATE PROCEDURE spe_Party_Corporate_Client_updpassword
    @party_cnt int,
    @tp_password VARCHAR(255)
AS
BEGIN

    UPDATE 
        Party_Corporate_Client
    SET
        tp_password = @tp_password
    WHERE 
        party_cnt = @party_cnt
    
END

GO

