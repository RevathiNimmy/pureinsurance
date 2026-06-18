SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_update_agent_common_renewal_date'
GO


CREATE PROCEDURE spu_update_agent_common_renewal_date 
	@insurance_file_cnt INT
AS
BEGIN
	DECLARE @renewal_date DATETIME
	DECLARE @common_renewal_date DATETIME
	DECLARE @single_instalment_agent TINYINT
	DECLARE @lead_agent_cnt INT

	SELECT @lead_agent_cnt = ISNULL(lead_agent_cnt, 0)
	FROM   insurance_file
	WHERE  insurance_file_cnt = @insurance_file_cnt

	IF @lead_agent_cnt <> 0
	BEGIN
		SELECT @common_renewal_date = common_renewal_date,
			   @single_instalment_agent = ISNULL(is_single_instalment_plan, 0)
		FROM   Party_agent
		WHERE  party_cnt = @lead_agent_cnt

		IF @single_instalment_agent = 1
		BEGIN
			SELECT @renewal_date = MIN(renewal_date)
			FROM   (SELECT insurance_ref,
						   MAX(renewal_date) renewal_date
					FROM   insurance_file
					WHERE  lead_agent_cnt = @lead_agent_cnt
						   AND insurance_file_type_id NOT IN ( 3, 8 )
						   AND ISNULL(insurance_file_status_id, 0) NOT IN ( 1, 2 )
					GROUP  BY insurance_ref) TmpTable

			IF @renewal_date > @common_renewal_date
			BEGIN
				UPDATE party_agent
				SET    common_renewal_date = DATEADD(YYYY, 1, common_renewal_date)
				WHERE  party_cnt = @lead_agent_cnt
			END
		END
	END
END 
