SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_Case_Claim_Link'
GO

CREATE PROCEDURE [dbo].[spu_Get_Case_Claim_Link]
	@basecaseid Int
AS
/*
	Created By:		???
	Creation Date:  ???
	Descriptoin:	Returns claims and case information dependant on the passed base case id

	Amendments

	Amended By		Date		Description
	-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	George Harris	12 Dec 2018	Changes the select statement to first insert the details into a temp table for perfrmance reasons				
*/

SELECT
	MAX(Version_id) as version_id,
	MAX(Claim_Id) as claim_id,
	base_claim_id
INTO #t
FROM claim C
WHERE base_case_id = @basecaseid
GROUP BY base_claim_id;

SELECT
    C.claim_id,
    C.claim_number,
    C.loss_from_date 'loss_date',
	h.description as 'Claim Handler',
	r.description as 'Risk Type',
    CS.description 'status',
    ISNULL(
		(SELECT
			SUM((R.initial_reserve + R.revised_reserve) - paid_to_date)
		FROM claim_peril CP
		JOIN reserve R ON CP.claim_peril_id = R.claim_peril_id
		JOIN reserve_type RT ON R.reserve_type_id = RT.reserve_type_id
				AND RT.is_indemnity=1
		WHERE CP.claim_id=c.claim_id)
		,0) 'total_indemnity',
    ISNULL(
		(SELECT
			SUM((R.initial_reserve + R.revised_reserve) - paid_to_date)
		FROM claim_peril CP
		JOIN reserve R ON CP.claim_peril_id = R.claim_peril_id
		JOIN reserve_type RT ON R.reserve_type_id = RT.reserve_type_id
				AND RT.is_expense=1
		WHERE CP.claim_id=c.claim_id)
		,0) 'total_expense',
    ISNULL(
		(SELECT
			SUM((R.initial_reserve + R.revised_reserve) - paid_to_date )
		FROM claim_peril CP
		JOIN reserve R ON CP.claim_peril_id = R.claim_peril_id
		JOIN reserve_type RT
				ON R.reserve_type_id = RT.reserve_type_id
				AND RT.is_excess=1
	    WHERE
			CP.claim_id=c.claim_id)
		,0) 'total_excess',
    c.policy_id ,
    c.Claim_Status_id
FROM Claim C
	JOIN Claim_status CS ON C.claim_status_id = CS.claim_status_id
	INNER JOIN #t claim_version	ON c.claim_id = claim_version.claim_id
	INNER JOIN Handler h ON c.handler_id=h.handler_id
	INNER JOIN Risk r ON r.risk_cnt=c.risk_type_id
WHERE C.base_case_id =  @basecaseid;
GO
