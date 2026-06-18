

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_TrainingAndCompetency'
GO

CREATE PROCEDURE spu_Report_TrainingAndCompetency
	@branch_id int,
	@last_reporting_date datetime,
	@end_date datetime
AS

SET NOCOUNT ON

DECLARE
@BranchDescription varchar(255),
@TotalStaff int,
@TotalStaffAdvice int,
@TotalStaffAdviceFT numeric(19,4),
@TotalStaffSupervise int,
@TotalStaffCompetent int,
@TotalStaffExam int,
@TotalStaffLeft int

IF NOT EXISTS(SELECT NULL FROM hidden_options WHERE option_number = 61 AND value = '1')
BEGIN
    SELECT
	NULL as branch_id,
	NULL as branch_description,
	NULL as user_id,
	NULL as username,
	NULL as job_basis,
	NULL as percent_hours_worked,
	NULL as risk_group_id,
	NULL as risk_description,
	NULL as fsa_user_status_id,
	NULL as code,
	NULL as fsa_status_description,
	NULL as passed_exam,
	NULL as date_passed_exam,
	NULL as total_staff,
	NULL as total_staff_advice,
	NULL as total_staff_advice_ft,
	NULL as total_staff_supervise,
	NULL as total_staff_competent,
	NULL as total_staff_exam,
	NULL as total_staff_left,
	1 as fsa_disabled
    RETURN
END

IF ISNULL(@branch_id,0)=0
BEGIN
	SET @branch_id=NULL
	SET @BranchDescription='All Branches'
END
ELSE
	SELECT @BranchDescription=S.description FROM Source S WHERE S.source_id=@branch_id

SELECT @TotalStaff=COUNT(*)
FROM
PMUser U
WHERE
ISNULL(U.sirius_user,0)=0
AND ISNULL(U.is_deleted,0)=0
AND @end_date>=U.effective_date
AND (@branch_id IS NULL OR (@branch_id IS NOT NULL AND NOT EXISTS(SELECT NULL FROM PMUser_Source WHERE user_id=U.user_id AND source_id=@branch_id)))

CREATE TABLE #tmpTraining
(
	user_id int,
	percent_hours_worked numeric(12,8)
)

INSERT INTO #tmpTraining
SELECT DISTINCT U.user_id, U.percent_hours_worked
FROM
PMUser U
INNER JOIN
fsa_user_competency FUC ON U.user_id=FUC.user_id
INNER JOIN
fsa_user_status FUS ON FUC.fsa_user_status_id=FUS.fsa_user_status_id
WHERE
ISNULL(U.sirius_user,0)=0
AND ISNULL(U.is_deleted,0)=0
AND @end_date>=U.effective_date
AND (@branch_id IS NULL OR (@branch_id IS NOT NULL AND NOT EXISTS(SELECT NULL FROM PMUser_Source WHERE user_id=U.user_id AND source_id=@branch_id)))
AND FUS.code IN ('CPT','SPR','CMOF','GADV')

SELECT @TotalStaffAdvice=COUNT(*) FROM #tmpTraining
SELECT @TotalStaffAdviceFT=SUM(percent_hours_worked/100) FROM #tmpTraining

DROP TABLE #tmpTraining

SELECT @TotalStaffSupervise=COUNT(DISTINCT U.user_id)
FROM
PMUser U
INNER JOIN
fsa_user_competency FUC ON U.user_id=FUC.user_id
INNER JOIN
fsa_user_status FUS ON FUC.fsa_user_status_id=FUS.fsa_user_status_id
WHERE
ISNULL(U.sirius_user,0)=0
AND ISNULL(U.is_deleted,0)=0
AND @end_date>=U.effective_date
AND (@branch_id IS NULL OR (@branch_id IS NOT NULL AND NOT EXISTS(SELECT NULL FROM PMUser_Source WHERE user_id=U.user_id AND source_id=@branch_id)))
AND FUS.code='SPR'

SELECT @TotalStaffCompetent=COUNT(DISTINCT U.user_id)
FROM
PMUser U
INNER JOIN
fsa_user_competency FUC ON U.user_id=FUC.user_id
INNER JOIN
fsa_user_status FUS ON FUC.fsa_user_status_id=FUS.fsa_user_status_id
WHERE
ISNULL(U.sirius_user,0)=0
AND ISNULL(U.is_deleted,0)=0
AND @end_date>=U.effective_date
AND (@branch_id IS NULL OR (@branch_id IS NOT NULL AND NOT EXISTS(SELECT NULL FROM PMUser_Source WHERE user_id=U.user_id AND source_id=@branch_id)))
AND FUS.code In ('SPR','CPT')

SELECT @TotalStaffExam=COUNT(DISTINCT U.user_id)
FROM
PMUser U
INNER JOIN
fsa_user_competency FUC ON U.user_id=FUC.user_id
WHERE
ISNULL(U.sirius_user,0)=0
AND ISNULL(U.is_deleted,0)=0
AND @end_date>=U.effective_date
AND (@branch_id IS NULL OR (@branch_id IS NOT NULL AND NOT EXISTS(SELECT NULL FROM PMUser_Source WHERE user_id=U.user_id AND source_id=@branch_id)))
AND FUC.passed_exam=1

SELECT @TotalStaffLeft=COUNT(DISTINCT u.user_id)
FROM
PMUser U
INNER JOIN
fsa_user_competency FUC ON U.user_id=FUC.user_id
INNER JOIN
fsa_user_status FUS ON FUC.fsa_user_status_id=FUS.fsa_user_status_id
WHERE
ISNULL(U.sirius_user,0)=0
AND ISNULL(U.is_deleted,0)=1
AND U.date_deleted>=@last_reporting_date
AND @end_date>=U.effective_date
AND (@branch_id IS NULL OR (@branch_id IS NOT NULL AND NOT EXISTS(SELECT NULL FROM PMUser_Source WHERE user_id=U.user_id AND source_id=@branch_id)))
AND FUS.code In ('CPT','SPR','CMOF','GADV')

SELECT
@branch_id as branch_id,
@BranchDescription as branch_description,
U.user_id,
U.username,
CASE U.job_basis
WHEN 1 THEN 'Full Time'
WHEN 2 THEN 'Part Time'
ELSE '' END As job_basis,
U.percent_hours_worked,
RG.risk_group_id,
RG.description as risk_description,
FUS.fsa_user_status_id,
FUS.code,
FUS.description as fsa_status_description,
FUC.passed_exam,
FUC.date_passed_exam,
@TotalStaff as total_staff,
@TotalStaffAdvice as total_staff_advice,
@TotalStaffAdviceFT as total_staff_advice_ft,
@TotalStaffSupervise as total_staff_supervise,
@TotalStaffCompetent as total_staff_competent,
@TotalStaffExam as total_staff_exam,
@TotalStaffLeft as total_staff_left,
0 as fsa_disabled
FROM
PMUser U
LEFT OUTER JOIN
fsa_user_competency FUC ON U.user_id=FUC.user_id
LEFT OUTER JOIN
risk_group RG ON FUC.risk_group_id=RG.risk_group_id
LEFT OUTER JOIN
fsa_user_status FUS ON FUC.fsa_user_status_id=FUS.fsa_user_status_id
WHERE
ISNULL(U.sirius_user,0)=0
AND ISNULL(U.is_deleted,0)=0
AND @end_date>=U.effective_date
AND (@branch_id IS NULL OR (@branch_id IS NOT NULL AND NOT EXISTS(SELECT NULL FROM PMUser_Source WHERE user_id=U.user_id AND source_id=@branch_id)))

GO
