SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_Coinsurer_Party_Key_Sel'
GO
CREATE  PROCEDURE  spu_ACT_Coinsurer_Party_Key_Sel
   @sShortCode VARCHAR(200)
AS    
SELECT p.party_cnt FROM Party p
JOIN Party_Insurer pin ON p.party_cnt=pin.party_cnt
WHERE p.shortname=@sShortCode
AND p.is_deleted = 0
   
