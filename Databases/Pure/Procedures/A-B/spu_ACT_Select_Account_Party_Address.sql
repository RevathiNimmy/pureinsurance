SET QUOTED_IDENTIFIER ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Account_Party_Address'
GO

CREATE PROCEDURE spu_ACT_Select_Account_Party_Address
(
	@account_key INT
)
AS 

SELECT 
    p.party_cnt, 
    p.party_type_id,
    p.[name],
    ppc.initials
FROM 
    Account AS acc 
INNER JOIN
    Party AS p ON p.party_cnt = acc.account_key
LEFT JOIN
    Party_Personal_Client AS ppc ON ppc.party_cnt = p.party_cnt
WHERE
    acc.account_key = @account_key
   
SET QUOTED_IDENTIFIER OFF 
GO
