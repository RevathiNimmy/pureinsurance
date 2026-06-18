SET QUOTED_IDENTIFIER OFF	SET ANSI_NULLS ON	SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_event_log_claim_policy_upd'
GO

CREATE PROCEDURE spu_event_log_claim_policy_upd
    @claim_cnt int,
    @party_cnt int ,
    @insurance_folder_cnt int ,
    @insurance_file_cnt int 
AS

BEGIN

UPDATE event_log
SET party_cnt = @party_cnt,
    insurance_folder_cnt = @insurance_folder_cnt,
    insurance_file_cnt = @insurance_file_cnt
FROM (SELECT claim_id FROM Claim
		WHERE base_claim_id =(SELECT base_claim_id FROM Claim
								WHERE claim_id= @claim_cnt)) AS cl
WHERE cl.claim_id= event_log.claim_cnt

END
GO

SET QUOTED_IDENTIFIER OFF	SET ANSI_NULLS ON	SET NOCOUNT OFF
GO
