SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Details_For_Completion_Intimation'
GO

CREATE PROCEDURE spu_Get_Details_For_Completion_Intimation
    @claim_id int,
	@TransactionType varchar(10)
AS
BEGIN

DECLARE @ReserveEntered money

IF @TransactionType = 'C_CO'
BEGIN
	select @ReserveEntered = sum(initial_reserve) from Reserve R
	inner join Claim_Peril CP on CP.Claim_Peril_id = R.claim_Peril_id
	where Cp.Claim_id = @claim_id
END
ELSE IF @TransactionType = 'C_CR'
BEGIN
	select @ReserveEntered = sum(initial_reserve) + sum(revised_reserve) from Reserve R
	inner join Claim_Peril CP on CP.Claim_Peril_id = R.claim_Peril_id
	where Cp.Claim_id = @claim_id
END

Select pti.created_by_id, pu.username, @ReserveEntered As ReserveEntered from PMWrk_Task_Instance pti
	inner join PMUser pu on pti.created_by_id = pu.user_id
	inner join claim_link cl on cl.link_id = pti.pmwrk_task_instance_cnt
	where cl.claim_id = @claim_id and cl.link_type_id = 2 and cl.processed = 0

END
GO