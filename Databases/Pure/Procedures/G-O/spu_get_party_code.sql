SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_party_code'
GO

CREATE PROCEDURE spu_get_party_code
    @party_cnt INT
   
AS
BEGIN
     SELECT pt.code 
        FROM party p,party_type pt
        WHERE p.party_type_id=pt.party_type_id
        AND p.party_cnt=@party_cnt

END


GO