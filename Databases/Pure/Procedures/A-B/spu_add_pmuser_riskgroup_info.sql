SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_add_pmuser_riskgroup_info'
GO

CREATE PROCEDURE spu_add_pmuser_riskgroup_info
    @user_id integer,
    @risk_group_id integer,
    @fsa_user_status_id integer,
    @passed_exam bit,
    @date_passed_exam datetime,
    @fsa_user_competency_id int OUTPUT
AS

BEGIN

	insert into fsa_user_competency
	(user_id, risk_group_id, fsa_user_status_id, effective_date, date_last_amended, passed_exam, date_passed_exam)
	values (@user_id, @risk_group_id, @fsa_user_status_id, getdate(), getdate(), @passed_exam, @date_passed_exam)

	SET @fsa_user_competency_id=@@IDENTITY

END
GO
