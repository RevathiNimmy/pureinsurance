SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_pmuser_riskgroup_info'
GO

CREATE PROCEDURE spu_get_pmuser_riskgroup_info
    @user_id integer,
    @effective_date datetime
AS

BEGIN
	select 	rg.risk_group_id, 
		rg.code, 
		rg.description, 
		isnull(fuc.fsa_user_competency_id, 0),
		isnull(fuc.user_id, 0),
		isnull(fuc.fsa_user_status_id, 0),
		isnull(fus.description, ''),
		fuc.passed_exam,
		fuc.date_passed_exam
	from risk_group rg
	left outer join fsa_user_competency fuc
	on fuc.risk_group_id = rg.risk_group_id
	and fuc.user_id = @user_id
	and fuc.effective_date <= @effective_date
	left outer join fsa_user_status fus
	on fus.fsa_user_status_id = fuc.fsa_user_status_id
	where rg.effective_date <= @effective_date
	and rg.is_deleted = 0
END
GO
