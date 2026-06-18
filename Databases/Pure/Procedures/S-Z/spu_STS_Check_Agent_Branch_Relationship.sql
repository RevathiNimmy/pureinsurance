ddldropprocedure 'spu_STS_Check_Agent_Branch_Relationship'
go

CREATE PROCEDURE spu_STS_Check_Agent_Branch_Relationship
	@Username VARCHAR(255),
	@PrimaryKey INT,
	@Permission INT output

AS

SELECT @Permission = -1

SELECT
	@Permission = PMUser.Party_Cnt
FROM
	pmuser INNER JOIN Party_Agent_Branch ON
		pmuser.Party_Cnt = Party_Agent_Branch.PARTY_cnt
WHERE
	PMUser.username = @Username
    AND Party_Agent_Branch.SOURCE_ID = @PrimaryKey