SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_AGR_agent_rate_risk_group_add'
GO


CREATE PROCEDURE spu_AGR_agent_rate_risk_group_add
(
@party_id int,
@risk_group_id int,
@effective_date datetime,
@agent_rate1 numeric(7,4),
@agent_value1 numeric(19,4),
@minimum_total1 numeric(19,4),
@agent_rate2 numeric(7,4),
@agent_value2 numeric(19,4),
@minimum_total2 numeric(19,4),
@agent_rate3 numeric(7,4),
@agent_value3 numeric(19,4),
@minimum_total3 numeric(19,4),
@rate_type_ind int,
@tax_group_id int,
@apply_to_all bit
)
AS

DECLARE @loop_risk_group_id int

INSERT INTO agent_group_rate
(party_cnt,risk_group_id,effective_date,agent_rate1,agent_value1,minimum_total1,agent_rate2,agent_value2,minimum_total2,agent_rate3,agent_value3,minimum_total3,rate_type_ind,tax_group_id)
VALUES
(@party_id,@risk_group_id,@effective_date,@agent_rate1,@agent_value1,@minimum_total1,@agent_rate2,@agent_value2,@minimum_total2,@agent_rate3,@agent_value3,@minimum_total3,@rate_type_ind,@tax_group_id)

IF @apply_to_all=1
BEGIN

	DECLARE Risk_Cursor CURSOR FAST_FORWARD FOR
	SELECT risk_group_id FROM risk_group
	WHERE
	risk_group_id<>@risk_group_id
	AND is_deleted=0
	AND effective_date<=getdate()

	OPEN Risk_Cursor

	FETCH NEXT FROM Risk_Cursor INTO @loop_risk_group_id

	WHILE @@FETCH_STATUS=0
	BEGIN
		IF NOT EXISTS(SELECT NULL FROM agent_group_rate WHERE party_cnt=@party_id AND risk_group_id=@loop_risk_group_id AND effective_date=@effective_date AND rate_type_ind=@rate_type_ind)
		BEGIN
			INSERT INTO agent_group_rate
			(party_cnt,risk_group_id,effective_date,agent_rate1,agent_value1,minimum_total1,agent_rate2,agent_value2,minimum_total2,agent_rate3,agent_value3,minimum_total3,rate_type_ind,tax_group_id)
			VALUES
			(@party_id,@loop_risk_group_id,@effective_date,@agent_rate1,@agent_value1,@minimum_total1,@agent_rate2,@agent_value2,@minimum_total2,@agent_rate3,@agent_value3,@minimum_total3,@rate_type_ind,@tax_group_id)
		END

		FETCH NEXT FROM Risk_Cursor INTO @loop_risk_group_id
	END

	CLOSE Risk_Cursor
	DEALLOCATE Risk_Cursor

END

GO