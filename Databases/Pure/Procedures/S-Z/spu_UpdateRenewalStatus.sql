SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

-- during renewal amendment user can change policy details and so we need to update the renewal status table with these info

EXECUTE DDLDropProcedure 'spu_UpdateRenewalStatus'
GO

CREATE PROCEDURE spu_UpdateRenewalStatus
					@RenewalStatusCnt int,
					@RenewalStatusTypeId int=0	
AS

IF @RenewalStatusTypeId =0
BEGIN
	UPDATE Renewal_Status
	SET product_id = ifi.product_id,
		insurance_holder_cnt = ifi.insured_cnt,
		lead_agent_cnt = ifi.lead_agent_cnt
	FROM Renewal_Status rs JOIN Insurance_file ifi ON rs.renewal_insurance_file_cnt = ifi.insurance_file_cnt
	WHERE rs.renewal_status_cnt = @RenewalStatusCnt
END
ELSE
BEGIN
	UPDATE Renewal_Status
	SET product_id = ifi.product_id,
		insurance_holder_cnt = ifi.insured_cnt,
		lead_agent_cnt = ifi.lead_agent_cnt,
		renewal_status_type_id=@RenewalStatusTypeId
	FROM Renewal_Status rs JOIN Insurance_file ifi ON rs.renewal_insurance_file_cnt = ifi.insurance_file_cnt
	WHERE rs.renewal_status_cnt = @RenewalStatusCnt

END


GO

