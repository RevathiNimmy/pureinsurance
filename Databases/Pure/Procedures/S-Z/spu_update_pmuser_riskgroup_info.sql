SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_update_pmuser_riskgroup_info'
GO

CREATE PROCEDURE spu_update_pmuser_riskgroup_info
    @fsa_user_competency_id integer,
    @fsa_user_status_id integer,
    @passed_exam bit,
    @date_passed_exam datetime
AS

BEGIN
	IF @fsa_user_status_id = 0
	BEGIN
	    DELETE FROM fsa_user_competency
	    WHERE fsa_user_competency_id = @fsa_user_competency_id
	END
	ELSE
	BEGIN
		UPDATE fsa_user_competency
		SET fsa_user_status_id = @fsa_user_status_id,
		date_last_amended = getdate(),
		passed_exam = @passed_exam,
		date_passed_exam = @date_passed_exam
		WHERE fsa_user_competency_id = @fsa_user_competency_id
	END
END
GO
