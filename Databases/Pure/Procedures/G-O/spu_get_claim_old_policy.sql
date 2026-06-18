SET QUOTED_IDENTIFIER OFF	SET ANSI_NULLS ON	SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_get_claim_old_policy'
GO

CREATE PROCEDURE spu_get_claim_old_policy
    @claim_id int,
	@insurance_folder_cnt int,
	@insurance_file_cnt int,
	@claim_old_policy_ref varchar(50) OUTPUT

AS

BEGIN

DECLARE @claim_insurance_folder_cnt int
DECLARE @claim_insurance_file_cnt int


SELECT TOP 1 @claim_insurance_folder_cnt=insurance_folder_cnt, @claim_insurance_file_cnt=insurance_file_cnt 
FROM event_log
WHERE claim_cnt IN 
		(SELECT	claim_id 
			FROM Claim
			WHERE base_claim_id =(SELECT base_claim_id 
									FROM Claim 
									WHERE claim_id= @claim_id)
			AND claim_cnt<>@claim_id)
			 
ORDER BY event_date

IF @claim_insurance_folder_cnt<>@insurance_folder_cnt
BEGIN
	SELECT @claim_old_policy_ref=insurance_ref FROM insurance_file 
		WHERE insurance_file_cnt=@claim_insurance_file_cnt
END
ELSE
SELECT NULL

END
GO

SET QUOTED_IDENTIFIER OFF	SET ANSI_NULLS ON	SET NOCOUNT OFF
GO
