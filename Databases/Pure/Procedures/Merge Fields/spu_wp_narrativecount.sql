ddldropprocedure 'spu_wp_narrativecount'
go
CREATE PROCEDURE spu_wp_narrativecount	@PartyCnt INT,
					@InsuranceFileCnt INT,
    @RiskId INT,
					@ClaimCnt INT,
					@DocumentRef VARCHAR(25),
					@Instance1 INT,
					@Instance2 INT,
					@Instance3 INT
AS

	select 	count(n.code) "how_many"
	from 	narrative_code n, policy_narrative p 
	where 	p.insurance_file_cnt = @InsuranceFileCnt and
		n.narrative_code_id = p.narrative_code_id
GO