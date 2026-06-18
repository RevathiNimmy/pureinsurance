ddldropprocedure 'spu_wp_ClmInfoChkLstServiceCount'
go

CREATE PROCEDURE spu_wp_ClmInfoChkLstServiceCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
    SELECT count(*) 'how_many'
    FROM Claim_Expert_Service ces
    JOIN party p
		ON p.party_cnt = ces.Party_Claim_id
	JOIN party_type pt
    	ON pt.party_type_id = p.party_type_id
    LEFT OUTER JOIN party_address_usage pau
    	ON pau.party_cnt = ces.Party_Claim_id
    LEFT OUTER JOIN address_usage_type aut
    	ON aut.address_usage_type_id = pau.address_usage_type_id
    LEFT OUTER JOIN address a
    	ON a.address_cnt = pau.address_cnt
    WHERE service_type_id=2
    	AND ISNULL(aut.code,'3131 XCO') = '3131 XCO'
    	AND Claim_id = @ClaimCnt