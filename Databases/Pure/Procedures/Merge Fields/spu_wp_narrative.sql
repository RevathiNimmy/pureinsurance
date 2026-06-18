ddldropprocedure 'spu_wp_narrative'
go

CREATE PROCEDURE spu_wp_narrative	@PartyCnt INT,
					@InsuranceFileCnt INT,
					@RiskID INT,
					@ClaimCnt INT,
					@DocumentRef VARCHAR(25),
					@Instance1 INT,
					@Instance2 INT,
					@Instance3 INT
AS
	DECLARE @code VARCHAR(10)
	DECLARE @description VARCHAR(255)

	DECLARE narrative_result  SCROLL CURSOR FOR
	select 	n.code, n.description
	from 	narrative_code n, policy_narrative p
	where 	p.insurance_file_cnt = @InsuranceFileCnt and
		n.narrative_code_id = p.narrative_code_id
	OPEN narrative_result


--DC290605 PN22029 use instance1 not instance2
	FETCH ABSOLUTE  @Instance1 FROM narrative_result INTO @code, @description
	CLOSE narrative_result

	SELECT @code 'code', @description 'description'

	DEALLOCATE narrative_result
