
EXECUTE DDLDropProcedure 'spu_ACT_Get_Account_ShortCode_From_PartyCnt'
GO

CREATE PROCEDURE spu_ACT_Get_Account_ShortCode_From_PartyCnt  
@party_cnt INT  
AS  
SELECT ac.short_code  FROM account ac  
  INNER JOIN party p ON p.party_cnt = ac.account_key  
WHERE p.party_cnt = @party_cnt  