SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_PartyAgentByAgentCnt'
GO

CREATE PROCEDURE spu_Get_PartyAgentByAgentCnt  
    @Agent_Cnt int  
AS  
SELECT p.party_cnt, p.shortname,p.resolved_name,A.Address1,A.Address2,A.Address3,A.Address4,
	A.Postal_Code,A.Country_id,C.Description,PA.is_single_instalment_plan
FROM
	Party P INNER JOIN Party_Agent PA ON P.Party_Cnt=PA.Party_Cnt
	LEFT JOIN Party_Address_Usage ON Party_Address_Usage.party_cnt = P.party_cnt
	LEFT JOIN Address A ON Party_Address_Usage.address_cnt = A.address_cnt
	LEFT JOIN Country C ON A.Country_id=C.Country_Id
WHERE 
	P.is_deleted = 0
	AND P.party_cnt = @Agent_Cnt    

GO

