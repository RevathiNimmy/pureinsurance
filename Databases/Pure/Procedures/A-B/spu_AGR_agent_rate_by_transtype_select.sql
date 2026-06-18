SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_AGR_agent_rate_by_transtype_select'
GO


CREATE PROCEDURE spu_AGR_agent_rate_by_transtype_select
(
@party_id int,
@risk_code_id int,
@risk_group_id int,
@effective_date datetime,
@transaction_type int,
@agent_rate numeric(19,4) OUTPUT,
@agent_value numeric(19,4) OUTPUT,
@agent_minimum numeric(19,4) OUTPUT,
@rate_type_ind tinyint OUTPUT,
@tax_group_id int OUTPUT
)
AS

DECLARE @found bit
DECLARE @rate1 numeric(19,4)
DECLARE @value1 numeric(19,4)
DECLARE @minimum1 numeric(19,4)
DECLARE @rate2 numeric(19,4)
DECLARE @value2 numeric(19,4)
DECLARE @minimum2 numeric(19,4)
DECLARE @rate3 numeric(19,4)
DECLARE @value3 numeric(19,4)
DECLARE @minimum3 numeric(19,4)

IF EXISTS(SELECT NULL FROM agent_rate WHERE party_cnt = @party_id AND risk_code_id = @risk_code_id AND effective_date<=@effective_date)
BEGIN

	SET @found=1

	SELECT TOP 1
	@rate1=ar.agent_rate1,
	@value1=ar.agent_value1,
	@minimum1=ar.minimum_total1,
	@rate2=ar.agent_rate2,
	@value2=ar.agent_value2,
	@minimum2=ar.minimum_total2,
	@rate2=ar.agent_rate3,
	@value2=ar.agent_value3,
	@minimum3=ar.minimum_total3,
	@rate_type_ind=ar.rate_type_ind,
	@tax_group_id=ar.tax_group_id
	FROM agent_rate ar
	WHERE 
	ar.party_cnt = @party_id
	AND
	ar.risk_code_id = @risk_code_id
	AND
	ar.effective_date<=@effective_date
	ORDER BY ar.effective_date DESC, ar.rate_type_ind ASC

END

ELSE

BEGIN

	SET @found=1
	
	SELECT TOP 1
	@rate1=agr.agent_rate1,
	@value1=agr.agent_value1,
	@minimum1=agr.minimum_total1,
	@rate2=agr.agent_rate2,
	@value2=agr.agent_value2,
	@minimum2=agr.minimum_total2,
	@rate3=agr.agent_rate3,
	@value3=agr.agent_value3,
	@minimum3=agr.minimum_total3,
	@rate_type_ind=agr.rate_type_ind,
	@tax_group_id=agr.tax_group_id
	FROM agent_group_rate agr
	WHERE 
	agr.party_cnt = @party_id
	AND
	agr.risk_group_id = @risk_group_id
	AND
	agr.effective_date<=@effective_date
	ORDER BY agr.effective_date DESC, agr.rate_type_ind ASC

END

IF @found=1
BEGIN
	IF @transaction_type=0
		SELECT @agent_rate=@rate1, @agent_value=@value1, @agent_minimum=@minimum1
	ELSE
		IF (@transaction_type=1 OR @transaction_type=2)
			SELECT @agent_rate=@rate3, @agent_value=@value3, @agent_minimum=@minimum3
		ELSE
			SELECT @agent_rate=@rate2, @agent_value=@value2, @agent_minimum=@minimum2
END

GO