SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Party_Net_Client_Login_Sel'
GO


CREATE PROCEDURE spu_Party_Net_Client_Login_Sel
    @userid varchar(40),
    @party_cnt INTEGER OUTPUT
AS


SELECT @party_cnt = ( SELECT party_cnt
   FROM Party
   WHERE shortname = @userid)
GO


